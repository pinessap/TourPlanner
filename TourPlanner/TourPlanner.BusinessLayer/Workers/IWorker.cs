using TourPlanner.DataAccessLayer.DataAccessObjects;

namespace TourPlanner.BusinessLayer.Workers;

public abstract class IWorker
{
    /// <summary>
    /// The object containing all Tour Data information
    /// </summary>
    protected readonly TourDao TourDao = new ();
}