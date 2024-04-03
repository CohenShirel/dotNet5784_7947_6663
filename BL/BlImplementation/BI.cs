namespace BlImplementation;
using BlApi;
using System;

public /*internal */class Bl : IBl
{
    private static DalApi.IDal s_dal = DalApi.Factory.Get;


    public BlApi.IWorker Worker => new WorkerImplementation(this);

    public BlApi.IAssignment Assignment => new AssignmentImplementation(this);

    public static DateTime? StartProjectTime => s_dal!.StartProjectTime;//לעשות פונקציה שתבקש מהמשתמש תאריך התחלה לפרויקט

    /* => DalTest.Initialization.Do();*/
    public static void InitializeDB()
    {
        DalTest.Initialization.Initialize();
    }
    public static void reset()
    {
        //מנקה את כל הרשומות
        BlApi.Factory.Get();
        s_dal = DalApi.Factory.Get;
        //DalTest.Initialization.Do();
        //המס הרצים חוזרים למס ההתחלתיים
        s_dal!.Worker!.DeleteAll();
        s_dal.Assignment!.DeleteAll();
        s_dal.Link!.DeleteAll();
    }
    private DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; }  set { s_Clock = value; } }
    public void FormatClockOneHour()=> Clock=Clock.AddHours(1);
    public void FormatClockOneDay() => Clock = Clock.AddDays(1);
    public void FormatClockOneMonth() => Clock = Clock.AddMonths(1);
    public void FormatClockOneYear()=> Clock= Clock.AddYears(1);
    public void ResetClock()=>Clock=DateTime.Now.Date;
}


















//we want it willl be uniq so it= static