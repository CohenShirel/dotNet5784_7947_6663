namespace BlImplementation;
using BlApi;
using System;

public /*internal */class Bl : IBl
{
    private static DalApi.IDal s_dal = DalApi.Factory.Get;

    public BlApi.IWorker Worker => new WorkerImplementation(this);

    public BlApi.IAssignment Assignment => new AssignmentImplementation(this);
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
    public List<BO.Assignment> lstAssignments { get; } = new();

    public IClock Clock =>new ClockImplemention();
}


















//we want it willl be uniq so it= static