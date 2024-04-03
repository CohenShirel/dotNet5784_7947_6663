using DalApi;

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

    public IClock Clock { get; }










    public List<BO.Assignment> lstAssignments { get; }


    //public static DateTime? StartProjectTime { get; }//??
    //clock
    #region Clock HandLing
    //public DateTime Clock { get; }
    //public void FormatClockOneHour() { }
    //public void FormatClockOneDay() { }
    //public void FormatClockOneMonth() { }
    //public void FormatClockOneYear() { }
    //public void ResetClock() { }
    #endregion

}

