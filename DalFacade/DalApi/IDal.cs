namespace DalApi;
public interface IDal
{
    IWorker Worker { get; }
    IAssignments Assignment { get; }
    ILink Link { get; }
    IClock Clock { get; }
}
