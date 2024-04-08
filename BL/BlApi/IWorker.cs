using BO;

namespace BlApi;

public interface IWorker
{
    public int Create(BO.Worker item);
    public BO.Worker Read(int id);
    public IEnumerable<WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null);
    public void Update(BO.Worker item);
    public void Delete(int id);
}