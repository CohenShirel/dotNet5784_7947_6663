namespace BlApi;
public interface IBl
{

    //just for read
    public IWorker Worker { get; }
    public IAssignment Assignment { get; }
    public static void reset() { }
    //  public IWorkerInAssignment WorkerInAssignment { get; }
    // public ISchedule Schedule { get; }
    //לעשות כאן לוז אוטומטי;
    public static void InitializeDB() { }

    public static DateTime? StartProjectTime { get; }//??

}

