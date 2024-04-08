using DalApi;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IWorker Worker => new WorkerImplementation();
    public IAssignments Assignment => new AssignmentsImplementation();
    public ILink Link => new LinkImplementation();
    public IClock Clock => new ClockImplemetation();
}

