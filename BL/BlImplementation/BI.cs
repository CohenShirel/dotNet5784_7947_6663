namespace BlImplementation;
using BlApi;
using DalApi;
using BO;
using BlImplementation;
using static BO.Exceptions;
using System;

internal class Bl : IBl
{
    public BlApi.IWorker Worker => new WorkerImplementation();

    public BlApi.IAssignments Assignments => new AssignmentsImplementation();

    DateOnly? IBl.DateBegin => throw new NotImplementedException();

    DateOnly? IBl.DeadLine => throw new NotImplementedException();

    private static IDal? s_dal;
    public void reset()
    {
        //מנקה את כל הרשומות
        //DalTest.Initialization.Do();
        //המס הרצים חוזרים למס ההתחלתיים
        s_dal = DalApi.Factory.Get;
        s_dal.Worker!.DeleteAll();
        s_dal.Assignments!.DeleteAll();
    }
    //לעשות כאן לוז אוטומטי??
    // public IWorkerInAssignments WorkerInAssignments => new WorkerInAssignmentsImplementation();

    //  public ISchedule Schedule =>new ScheduleImplementation();
}
