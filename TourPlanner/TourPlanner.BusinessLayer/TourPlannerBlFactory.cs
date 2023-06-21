using System;

namespace TourPlanner.BusinessLayer
{
    public static class TourPlannerBlFactory
    {
        private static readonly Lazy<ITourPlannerBl> Lazy = new(() => new TourPlannerBl());
        public static ITourPlannerBl Instance => Lazy.Value;
    }
}
