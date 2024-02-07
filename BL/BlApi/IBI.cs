using BO;
using BlImplementation;
using static BO.Exceptions;
namespace BlApi;
public interface IBl 
{
    //just for read
    public IWorker Worker { get; }
    public IAssignments Assignments { get; }
    public void reset();
    //  public IWorkerInAssignments WorkerInAssignments { get; }
    // public ISchedule Schedule { get; }
    //לעשות כאן לוז אוטומטי;
    public DateOnly? DateBegin { get; }//??
    public DateOnly? DeadLine { get; }//??
}

