using Microsoft.EntityFrameworkCore;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: Get connection string from config file
        optionsBuilder.UseNpgsql("Host=localhost; Database=tourplanner; Username=postgres; Password=trust");
    }
}