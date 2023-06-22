using NUnit.Framework;
using TourPlanner.Configuration;

namespace TourPlanner.Testing;

[TestFixture]
public class ConfigurationTests
{
    [Test]
    public void TestConfigFileReading()
    {
        var dbConnection = AppConfigManager.Settings.DbConnection;
        
        Assert.That(dbConnection, Is.EqualTo(AppConfigManager.Settings.DbConnection));
    }

    [Test]
    public void TestSettingChange()
    {
        AppConfigManager.Settings.DbConnection = "ANewConnection";
        
        Assert.That(AppConfigManager.Settings.DbConnection, Is.EqualTo("ANewConnection"));
    }

    [Test]
    public void TestLogfileLayoutIsNotEmpty()
    {
        Assert.That(AppConfigManager.Settings.LogLayout, Is.Not.EqualTo(""));
    }
}