using BO;
namespace BlApi;
public interface IBl 
{
    //just for read
    public IWorker Worker { get; }
    public IAssignments Assignments { get; }
  //  public IWorkerInAssignments WorkerInAssignments { get; }
   // public ISchedule Schedule { get; }
}

