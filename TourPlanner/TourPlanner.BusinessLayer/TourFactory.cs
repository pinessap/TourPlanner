using System;

namespace TourPlanner.BusinessLayer
{
    public static class TourFactory
    {
        // Two fields to make this class a Singleton
        // For now always returns the TourFactoryImpl()
        // In the future there might be multiple implementations, which would then change this
        private static readonly Lazy<ITourFactory> Lazy = new(() => new TourFactoryImpl());
        public static ITourFactory Instance => Lazy.Value;
    }
}
