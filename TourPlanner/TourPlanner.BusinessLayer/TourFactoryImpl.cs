using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {
        private TourDAO tourDao = new TourDAO();
        public IEnumerable<Tour> GetTours()
        {
            return tourDao.GetTours();
        }

        public IEnumerable<Tour> Search(string name, bool caseSensitive = false)
        {
            IEnumerable<Tour> items = GetTours();
            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(name));
            }
            return items.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
