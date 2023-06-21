using System.Globalization;
using NUnit.Framework;
using TourPlanner.Converters;
using TourPlanner.Models;

namespace TourPlanner.Testing;

[TestFixture]
public class ConverterTests
{
    [Test]
    public void TestTimeSpanToStringConversion()
    {
        var timeConverter = new TimeSpanConverter();
        var time = new TimeSpan(1, 0, 0, 0);

        var convertedTime = timeConverter.Convert(time, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo(time.ToString()));
    }
    
    [Test]
    public void TestStringToTimeSpanConversion()
    {
        var timeConverter = new TimeSpanConverter();
        var time = new TimeSpan(1, 0, 0, 0);

        var convertedTime = timeConverter.ConvertBack(time.ToString(), typeof(TimeSpan), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo(time));
    }
    
    [Test]
    public void TestInvalidTimeSpanConversion()
    {
        var timeConverter = new TimeSpanConverter();
        var time = new TimeSpan(1, 0, 0, 0);

        var convertedTime = timeConverter.ConvertBack("invalidString", typeof(TimeSpan), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.Not.EqualTo(time));
    }
    
    [Test]
    public void TestFriendlyChildFriendlinessToIconConversion()
    {
        var converter = new ChildFriendlinessToIconConverter();
        var friendliness = 1;

        var convertedTime = converter.Convert(friendliness, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("\uf119;"));
    }
    
    [Test]
    public void TestInvalidChildFriendlinessToIconConversion()
    {
        var converter = new ChildFriendlinessToIconConverter();
        var friendliness = 100;

        var convertedTime = converter.Convert(friendliness, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("\uf11a;"));
    }
    
    [Test]
    public void TestCarTransportTypeToPathDataConversion()
    {
        var converter = new TransportTypeToPathDataConverter();
        var transportType = Tour.TransportTypes.Car;

        var convertedTime = converter.Convert(transportType, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("M5,11L6.5,6.5H17.5L19,11M17.5,16A1.5,1.5 0 0,1 16,14.5A1.5,1.5 0 0,1 17.5,13A1.5,1.5 0 0,1 19,14.5A1.5,1.5 0 0,1 17.5,16M6.5,16A1.5,1.5 0 0,1 5,14.5A1.5,1.5 0 0,1 6.5,13A1.5,1.5 0 0,1 8,14.5A1.5,1.5 0 0,1 6.5,16M18.92,6C18.72,5.42 18.16,5 17.5,5H6.5C5.84,5 5.28,5.42 5.08,6L3,12V20A1,1 0 0,0 4,21H5A1,1 0 0,0 6,20V19H18V20A1,1 0 0,0 19,21H20A1,1 0 0,0 21,20V12L18.92,6Z"));
    }
    
    [Test]
    public void TestBicycleTransportTypeToPathDataConversion()
    {
        var converter = new TransportTypeToPathDataConverter();
        var transportType = Tour.TransportTypes.Bicycle;

        var convertedTime = converter.Convert(transportType, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("M5,20.5A3.5,3.5 0 0,1 1.5,17A3.5,3.5 0 0,1 5,13.5A3.5,3.5 0 0,1 8.5,17A3.5,3.5 0 0,1 5,20.5M5,12A5,5 0 0,0 0,17A5,5 0 0,0 5,22A5,5 0 0,0 10,17A5,5 0 0,0 5,12M14.8,10H19V8.2H15.8L13.86,4.93C13.57,4.43 13,4.1 12.4,4.1C11.93,4.1 11.5,4.29 11.2,4.6L7.5,8.29C7.19,8.6 7,9 7,9.5C7,10.13 7.33,10.66 7.85,10.97L11.2,13V18H13V11.5L10.75,9.85L13.07,7.5M19,20.5A3.5,3.5 0 0,1 15.5,17A3.5,3.5 0 0,1 19,13.5A3.5,3.5 0 0,1 22.5,17A3.5,3.5 0 0,1 19,20.5M19,12A5,5 0 0,0 14,17A5,5 0 0,0 19,22A5,5 0 0,0 24,17A5,5 0 0,0 19,12M16,4.8C17,4.8 17.8,4 17.8,3C17.8,2 17,1.2 16,1.2C15,1.2 14.2,2 14.2,3C14.2,4 15,4.8 16,4.8Z"));
    }
    
    [Test]
    public void TestWalkingTransportTypeToPathDataConversion()
    {
        var converter = new TransportTypeToPathDataConverter();
        var transportType = Tour.TransportTypes.Walking;

        var convertedTime = converter.Convert(transportType, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("M10.74,11.72C11.21,12.95 11.16,14.23 9.75,14.74C6.85,15.81 6.2,13 6.16,12.86L10.74,11.72M5.71,10.91L10.03,9.84C9.84,8.79 10.13,7.74 10.13,6.5C10.13,4.82 8.8,1.53 6.68,2.06C4.26,2.66 3.91,5.35 4,6.65C4.12,7.95 5.64,10.73 5.71,10.91M17.85,19.85C17.82,20 17.16,22.8 14.26,21.74C12.86,21.22 12.8,19.94 13.27,18.71L17.85,19.85M20,13.65C20.1,12.35 19.76,9.65 17.33,9.05C15.22,8.5 13.89,11.81 13.89,13.5C13.89,14.73 14.17,15.78 14,16.83L18.3,17.9C18.38,17.72 19.89,14.94 20,13.65Z"));
    }
    
    [Test]
    public void TestInvalidTransportTypeToPathDataConversion()
    {
        var converter = new TransportTypeToPathDataConverter();
        var transportType = "Rocket";

        var convertedTime = converter.Convert(transportType, typeof(string), "nothing", CultureInfo.CurrentCulture);
        
        Assert.That(convertedTime, Is.EqualTo("M10.74,11.72C11.21,12.95 11.16,14.23 9.75,14.74C6.85,15.81 6.2,13 6.16,12.86L10.74,11.72M5.71,10.91L10.03,9.84C9.84,8.79 10.13,7.74 10.13,6.5C10.13,4.82 8.8,1.53 6.68,2.06C4.26,2.66 3.91,5.35 4,6.65C4.12,7.95 5.64,10.73 5.71,10.91M17.85,19.85C17.82,20 17.16,22.8 14.26,21.74C12.86,21.22 12.8,19.94 13.27,18.71L17.85,19.85M20,13.65C20.1,12.35 19.76,9.65 17.33,9.05C15.22,8.5 13.89,11.81 13.89,13.5C13.89,14.73 14.17,15.78 14,16.83L18.3,17.9C18.38,17.72 19.89,14.94 20,13.65Z"));
    }
}