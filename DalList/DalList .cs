using DalApi;

namespace Dal;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IWorker Worker =>new WorkerImplementation();

    public IAssignments Assignments => new AssignmentsImplementation();

    public ILink Link => new LinkImplementation();
}
