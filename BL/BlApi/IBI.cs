using BO;
using BlImplementation;
using static BO.Exceptions;
using DalApi;
namespace BlApi;
public interface IBl 
{

    //just for read
    public IWorker Worker { get; }
    public IAssignments Assignments { get; }
    public static void reset() { }
    //  public IWorkerInAssignments WorkerInAssignments { get; }
    // public ISchedule Schedule { get; }
    //לעשות כאן לוז אוטומטי;
    public static DateTime? StartProjectTime { get; }//??

}

