using DalApi;
namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IWorker Worker => new WorkerImplementation();
    public IAssignments Assignment => new AssignmentsImplementation();
    public ILink Link => new LinkImplementation();
    //public DateTime? StartProjectTime
    //{

    //    get { return Config.StartProjectTime; }

    //    set { StartProjectTime = value; }

    //}
    public IClock Clock => new ClockImplemetation();

    //public DateTime Clock
    //{

    //    get { return Config.Clock; }

    //    set { Clock = value; }

    //}
}

