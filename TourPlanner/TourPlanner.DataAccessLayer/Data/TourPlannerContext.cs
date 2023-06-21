using Microsoft.EntityFrameworkCore;
using TourPlanner.Configuration;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.Data;

/// <summary>
/// This class essentially represents our entire database
/// Each property is a table
/// Gets used to automatically create all the tables in the database
/// Gets used to automatically interact with the database via code
/// TL;DR: Dont add any logic to this
/// </summary>
public class TourPlannerContext : DbContext
{
    public DbSet<Tour> Tours { get; set; } = null!;
    public DbSet<TourLog> TourLogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(AppConfigManager.Settings.DbConnection);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Makes sure that when a tour is deleted, all associated tourLogs are deleted too
        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Logs)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Makes sure that the transportType enum is saved as a string in the database
        modelBuilder.Entity<Tour>()
            .Property(e => e.TransportType)
            .HasConversion<string>();
    }
}