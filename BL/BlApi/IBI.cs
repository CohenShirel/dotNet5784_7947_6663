using DalApi;

namespace BlApi;
public interface IBl
{
    public IWorker Worker { get; }
    public IAssignment Assignment { get; }
    public static void reset() { }
    public static void InitializeDB() { }
    public IClock Clock { get; }
    public List<BO.Assignment> lstAssignments { get; }
}

