using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TourPlanner.Configuration;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Workers;

public class TourWorker : IWorker
{
    private static readonly HttpClient Client = new ();
    
    private const string DataRequestUrl = "https://www.mapquestapi.com/directions/v2/route";
    private const string ImageRequestUrl = "https://www.mapquestapi.com/staticmap/v5/map";

    public List<Tour> GetTours()
    {
        return TourDao.GetTours();
    }
    
    public List<Tour> Search(string searchValue, bool caseSensitive = false)
    {
        if (searchValue is null or "") AppLogger.Warn("SearchValue is empty or null");

        var items = GetTours();

        List<Tour> foundTours = new();

        try
        {
            foundTours.AddRange(items.Where(x => FullTextSearch(searchValue!, x, caseSensitive)));
        }
        catch (Exception ex)
        {
            AppLogger.ThrowError("SEARCH error:", ex);
        }

        AppLogger.Info("Found " + foundTours.Count + " tours that contain \"" + searchValue + "\"");

        return foundTours;
    }
    
    public async Task Add(Tour tourToAdd)
    {
        AppLogger.Info("Adding new tour \"" + tourToAdd.Name + "\"");
        
        TourDao.Add(tourToAdd);
        await AddApiInformation(tourToAdd);
        await Modify(tourToAdd);
        
        AppLogger.Info("Add tour \"" + tourToAdd.Name + "\" successful");
    }
    
    public void Delete(Tour tourToDelete)
    {
        AppLogger.Info("Deleting tour \"" + tourToDelete.Name + "\"");
        TourDao.Delete(tourToDelete);
        AppLogger.Info("Delete tour \"" + tourToDelete.Name + "\" successful");

    }
    
    public async Task Modify(Tour modifiedTour)
    {
        var modifiedProperties = TourDao.GetModifiedProperties(modifiedTour);
        
        AppLogger.Info("Modifying " + modifiedProperties.Count() + " properties of tour \"" + modifiedTour.Name + "\"");

        // If any property is changed that affects the calculated route, regenerate route
        if (modifiedProperties.Contains(nameof(modifiedTour.FromLocation)) ||
            modifiedProperties.Contains(nameof(modifiedTour.ToLocation)) ||
            modifiedProperties.Contains(nameof(modifiedTour.TransportType)))
        {
            await AddApiInformation(modifiedTour);
        }
        
        TourDao.Modify(modifiedTour);
        
        AppLogger.Info("Modify tour \"" + modifiedTour.Name + "\" successful");
    }
    
    /// <summary>
    /// Searches tour for given searchValue
    /// </summary>
    /// <param name="searchValue">The string to search for</param>
    /// <param name="tourToSearch">The tour in which the string gets searched</param>
    /// <param name="caseSensitive">If true the search is case sensitive, insensitive if false</param>
    /// <returns>True if searchValue was found, false if not</returns>
    private bool FullTextSearch(string searchValue, Tour tourToSearch, bool caseSensitive)
    {
        searchValue ??= "";
            
        // Fill search list with basic tour information
        var stringsToSearch = new List<string>
        {
            tourToSearch.Name,
            tourToSearch.Description ?? "",
            tourToSearch.ToLocation,
            tourToSearch.FromLocation,
            tourToSearch.TransportType.ToString() ?? "",
            (tourToSearch.ChildFriendliness == null ? tourToSearch.ChildFriendliness.ToString() : "")!,
            (tourToSearch.Popularity == null ? tourToSearch.Popularity.ToString() : "")!
        };
            
        // Fill search list with tour log information
        foreach (var log in tourToSearch.Logs)
        {
            stringsToSearch.Add(log.Comment);
        }
            
        // Check if searchValue is present in any added information
        foreach (var interestingInformation in stringsToSearch)
        {
            var searchInformation = caseSensitive ? interestingInformation : interestingInformation.ToLower();
            searchValue = caseSensitive ? searchValue : searchValue.ToLower();
                
            if (searchInformation.Contains(searchValue))
                return true;
        }
            
        // If nothing was found, return false
        return false;
    }

    /// <summary>
    /// Takes tour and fills it with calculated values from the MapQuest API and saves route image.
    /// </summary>
    /// <param name="tourWithoutApiValues">A tour with a from-, to-position and transportType. EstimatedTime, TourDistance and image file get overwritten/saved.</param>
    private async Task AddApiInformation(Tour tourWithoutApiValues)
    {
        var requestParams = new Dictionary<string, string>
        {
            { "key", AppConfigManager.Settings.MapQuestApiKey },
            { "from", tourWithoutApiValues.FromLocation },
            { "to", tourWithoutApiValues.ToLocation },
            { "routeType", MapTransportTypeToRouteType(tourWithoutApiValues.TransportType) },
            { "unit", "k" }
        };

        var urlWithParams = AddGetParamsToUrl(DataRequestUrl, requestParams);

        try
        {
            AppLogger.Info("Performing Api request: " + urlWithParams);
            // Perform data request
            var responseString = await Client.GetStringAsync(urlWithParams);

            // Parse request
            var responseJson = JObject.Parse(responseString);

            // If mapQuest returns a routeError, it usually means the route is impossible or the locations were invalid
            if (responseJson["route"]!["error"] != null || responseJson["route"]!["routeError"] != null)
            {
                throw new InvalidDataException("Request: \"" + urlWithParams + "\"\nResponse: " + responseJson);
            }

            // Extract relevant information
            var estimatedTime = responseJson["route"]!["time"]!.Value<double>();
            var estimatedDistance = responseJson["route"]!["distance"]!.Value<float>();
            var sessionId = responseJson["route"]!["sessionId"]!.Value<string>()!;
            var boundingBox = responseJson["route"]!["boundingBox"]!["ul"]!["lat"]!.Value<string>() + "," +
                              responseJson["route"]!["boundingBox"]!["ul"]!["lng"]!.Value<string>() + "," +
                              responseJson["route"]!["boundingBox"]!["lr"]!["lat"]!.Value<string>() + "," +
                              responseJson["route"]!["boundingBox"]!["lr"]!["lng"]!.Value<string>();

            // Save request data into tour object
            tourWithoutApiValues.EstimatedTime = TimeSpan.FromSeconds(estimatedTime);
            tourWithoutApiValues.TourDistance = estimatedDistance;

            // Performs image request, also saving the image in the file system
            await PerformRouteImageRequest(sessionId, boundingBox, tourWithoutApiValues.PathToRouteImage);
        }
        catch (HttpRequestException ex)
        {
            AppLogger.ThrowError("API error: Unauthorized!", ex);
        }
        catch (InvalidDataException ex)
        {
            AppLogger.ThrowError("API error: Invalid data.", ex);
        }
        catch (Exception ex)
        {
            AppLogger.ThrowError("API error: ", ex);
        }
    }

    /// <summary>
    /// Adds get parameters to a given URL string
    /// </summary>
    /// <param name="url">Url in string format, e.g. https://myWebsite.com</param>
    /// <param name="requestParams">A dictionary with parameters, key represents left and value right, e.g. "key1=value1 & key2=value2"</param>
    /// <returns>Url with added get parameters</returns>
    private string AddGetParamsToUrl(string url, Dictionary<string, string> requestParams)
    {
        url += "?";

        foreach (var param in requestParams)
        {
            url += param.Key + "=" + param.Value + "&";
        }

        url = url.TrimEnd('&');

        return url;
    }

    /// <summary>
    /// Maps TransportTypes enum to corresponding request string
    /// </summary>
    /// <returns>routeType string for MapQuest Api</returns>
    private string MapTransportTypeToRouteType(Tour.TransportTypes type)
    {
        return type switch
        {
            Tour.TransportTypes.Bicycle => "bicycle",
            Tour.TransportTypes.Walking => "pedestrian",
            Tour.TransportTypes.Car => "fastest",
            _ => "fastest"
        };
    }

    /// <summary>
    /// Saves image from given MapQuest session
    /// </summary>
    /// <param name="sessionId">Session id from MapQuest data request</param>
    /// <param name="boundingBox">Bounding box of image, consisting of coordinates of top left and bottom right corner</param>
    /// <param name="fileSaveLocation">Absolute path to image file, e.g. "C:/Users/Downloads/image.png"</param>
    private async Task PerformRouteImageRequest(string sessionId, string boundingBox, string fileSaveLocation)
    {
        var requestParams = new Dictionary<string, string>
        {
            { "key", AppConfigManager.Settings.MapQuestApiKey },
            { "session", sessionId },
            { "boundingBox", boundingBox },
            { "format", "png" },
            { "defaultMarker", "none" }
        };

        var urlWithParams = AddGetParamsToUrl(ImageRequestUrl, requestParams);
        
        AppLogger.Info("Performing Api request: " + urlWithParams);
        
        var responseStream = await Client.GetStreamAsync(urlWithParams);
        
        TourDao.SaveToFile(fileSaveLocation, responseStream);
    }
}