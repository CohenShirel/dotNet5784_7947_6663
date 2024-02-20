namespace BlImplementation;
using BlApi;
using DalApi;
using BO;
using BlImplementation;
using static BO.Exceptions;
using System;

public /*internal */class Bl : IBl
{
    private static DalApi.IDal s_dal = DalApi.Factory.Get;


    public BlApi.IWorker Worker => new WorkerImplementation();

    public BlApi.IAssignments Assignments => new AssignmentsImplementation();

    public static DateTime? StartProjectTime => s_dal!.StartProjectTime;//לעשות פונקציה שתבקש מהמשתמש תאריך התחלה לפרויקט

   // DateTime? IBl.DeadLine => null;

    public static void reset() => DalTest.Initialization.Do();
    //{
    //    //מנקה את כל הרשומות
    //    BlApi.Factory.Get();
    //    s_dal=DalApi.Factory.Get;
    //    //DalTest.Initialization.Do();
    //    //המס הרצים חוזרים למס ההתחלתיים
    //    s_dal!.Worker!.DeleteAll();
    //    s_dal.Assignments!.DeleteAll();
    //    s_dal.Link!.DeleteAll();
    //}
    //לעשות כאן לוז אוטומטי??
    // public IWorkerInAssignments WorkerInAssignments => new WorkerInAssignmentsImplementation();

    //  public ISchedule Schedule =>new ScheduleImplementation();
}
