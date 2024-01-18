using DalApi;
namespace Dal;

//stage 3
sealed public class DalXml : IDal
{
    public IWorker Worker => new WorkerImplementation();
    public IAssignments Assignments => new AssignmentsImplementation();
    public ILink Link => new LinkImplementation();

    //IWorker IDal.Worker => throw new NotImplementedException();
    //IAssignments IDal.Assignments => throw new NotImplementedException();
    //ILink IDal.Link => throw new NotImplementedException();
}

