namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    public IWorker Worker => new WorkerImplementation();

    public IAssignments Assignments => new AssignmentsImplementation();

   // public IWorkerInAssignments WorkerInAssignments => new WorkerInAssignmentsImplementation();

  //  public ISchedule Schedule =>new ScheduleImplementation();
}
