using DalApi;

namespace Dal;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IWorker Worker => new WorkerImplementation();

    public IAssignments Assignment => new AssignmentsImplementation();

    public ILink Link => new LinkImplementation();

    public IClock Clock { get; set; }
    //public DateTime?
    //
    //
    //Time
    //{

    //    get { return DataSource.Config.StartProjectTime; }

    //    set { DataSource.Config.StartProjectTime = value; }

    //}
}
