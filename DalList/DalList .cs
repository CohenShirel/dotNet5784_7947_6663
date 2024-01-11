using DalApi;

namespace Dal;

sealed public class DalList : IDal
{
    public IWorker Worker =>new WorkerImplementation();

    public IAssignments Assignments => new AssignmentsImplementation();

    public ILink Link => new LinkImplementation();
}
