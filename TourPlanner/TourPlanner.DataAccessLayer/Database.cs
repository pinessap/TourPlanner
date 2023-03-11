using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class Database : IDataAccess
    {
        private string connectionString;

        public Database()
        {
            //connectionString = "";
            //establish connection with db
        }
        public List<Tour> GetTours()
        {
            //select with ORM whatever

            //ZUM TESTEN
            return new List<Tour>() //ZUM TESTEN
            {
                new Tour() { Name = "Tour1" },
                new Tour() { Name = "Tour2"},
                new Tour() { Name = "Tour3" }
            };
        }
    }
}
