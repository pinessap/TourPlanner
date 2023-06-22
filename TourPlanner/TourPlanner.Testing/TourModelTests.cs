using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TourPlanner.Models;

namespace TourPlanner.Testing;

[TestFixture]
public class TourModelTests
{
    private Tour? _tourToTest;
    
    [SetUp]
    public void Setup()
    {
        _tourToTest = new Tour
        {
            Name = "TourName",
            Description = "TourDescription",
            FromLocation = "TourFrom",
            ToLocation = "TourTo",
            EstimatedTime = null,
            TourDistance = null,
            TransportType = Tour.TransportTypes.Car,
            Logs = new List<TourLog>()
        };
    }
    
    [Test]
    public void TestTourPopularityWithNoLogs()
    {
        Assert.That(_tourToTest?.Popularity, Is.Null);
    }
    
    [Test]
    public void TestTourAverageDifficultyWithNoLogs()
    {
        Assert.That(_tourToTest?.AverageDifficulty, Is.Null);
    }
    
    [Test]
    public void TestTourAverageRatingWithNoLogs()
    {
        Assert.That(_tourToTest?.AverageRating, Is.Null);
    }
    
    [Test]
    public void TestTourAverageTimeWithNoLogs()
    {
        Assert.That(_tourToTest?.AverageTime, Is.Null);
    }
    
    [Test]
    public void TestTourPopularityWithTwoLogs()
    {
        _tourToTest?.Logs.Add(new TourLog());
        _tourToTest?.Logs.Add(new TourLog());

        Assert.That(_tourToTest?.Popularity, Is.EqualTo(2));
    }
    
    [Test]
    public void TestAverageDifficultyWithTwoLogs()
    {
        var tourLog1 = new TourLog()
        {
            Difficulty = 10
        };
        
        var tourLog2 = new TourLog()
        {
            Difficulty = 1
        };
        
        _tourToTest?.Logs.Add(tourLog1);
        _tourToTest?.Logs.Add(tourLog2);

        Assert.That(_tourToTest?.AverageDifficulty, Is.EqualTo(5.5));
    }
    
    [Test]
    public void TestAverageRatingWithTwoLogs()
    {
        var tourLog1 = new TourLog()
        {
            Rating = 5
        };
        
        var tourLog2 = new TourLog()
        {
            Rating = 0
        };
        
        _tourToTest?.Logs.Add(tourLog1);
        _tourToTest?.Logs.Add(tourLog2);

        Assert.That(_tourToTest?.AverageRating, Is.EqualTo(2.5));
    }
    
    [Test]
    public void TestAverageTimeWithTwoLogs()
    {
        var tourLog1 = new TourLog()
        {
            Duration = new TimeSpan(1, 0, 0, 0)
        };
        
        var tourLog2 = new TourLog()
        {
            Duration = new TimeSpan(3, 0, 0, 0)
        };
        
        _tourToTest?.Logs.Add(tourLog1);
        _tourToTest?.Logs.Add(tourLog2);

        Assert.That(_tourToTest?.AverageTime, Is.EqualTo(new TimeSpan(2, 0, 0, 0)));
    }
}