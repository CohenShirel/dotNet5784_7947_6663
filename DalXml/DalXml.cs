using DalApi;
using System.Data.Common;
using System.Diagnostics;
namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IWorker Worker => new WorkerImplementation();
    public IAssignments Assignments => new AssignmentsImplementation();
    public ILink Link => new LinkImplementation();
    public DateTime? StartProjectTime
    {

        get { return Config.StartProjectTime; }

        set { StartProjectTime = value; }

    }
}

