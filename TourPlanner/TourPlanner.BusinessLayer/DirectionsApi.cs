using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TourPlanner.Configuration;
using TourPlanner.DataAccessLayer.DataAccessObjects;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer;

public class DirectionsApi
{
    private static readonly Lazy<DirectionsApi> Lazy = new(() => new DirectionsApi());
    public static DirectionsApi Instance => Lazy.Value;
    
    private static readonly HttpClient _client = new ();
    
    private readonly TourDao _tourDao = new ();

    public async Task AddApiInformation(Tour tourWithoutApiValues)
    {
        const string requestUrl = "https://www.mapquestapi.com/directions/v2/route";

        var requestParams = new Dictionary<string, string>
        {
            { "key", AppConfigManager.Settings.MapQuestApiKey },
            { "from", tourWithoutApiValues.FromLocation },
            { "to", tourWithoutApiValues.ToLocation },
            { "routeType", MapTransportTypeToRouteType(tourWithoutApiValues.TransportType) },
            { "unit", "k" }
        };

        var urlWithParams = AddGetParamsToUrl(requestUrl, requestParams);

        try
        {
            // Perform data request
            var responseString = await _client.GetStringAsync(urlWithParams);

            // Parse request
            var responseJson = JObject.Parse(responseString);

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

    private string AddGetParamsToUrl(string url, Dictionary<string, string> requestParams)
    {
        url += "?";

        foreach (var param in requestParams)
        {
            url += param.Key + "=" + param.Value + "&";
        }

        url.TrimEnd('&');

        return url;
    }

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

    private async Task PerformRouteImageRequest(string sessionId, string boundingBox, string fileSaveLocation)
    {
        const string requestUrl = "https://www.mapquestapi.com/staticmap/v5/map";
        
        var requestParams = new Dictionary<string, string>
        {
            { "key", AppConfigManager.Settings.MapQuestApiKey },
            { "session", sessionId },
            { "boundingBox", boundingBox },
            { "format", "png" },
            { "defaultMarker", "none" }
        };

        var urlWithParams = AddGetParamsToUrl(requestUrl, requestParams);
        
        var responseStream = await _client.GetStreamAsync(urlWithParams);
        
        _tourDao.SaveToFile(fileSaveLocation, responseStream);
    }
}