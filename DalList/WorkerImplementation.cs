namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class WorkerImplementation:IWorker
{
    public int Create(Worker w)
    {
        //for entities with normal id (not auto id)
        if (Read(w.IdWorker) is not null)
            throw new Exception($"Worker with ID={w.IdWorker} already exists");
        DataSource.Workers.Add(w);
        return w.IdWorker;
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Worker with ID={id} not exists");
        Worker w = Read(id);
        DataSource.Workers.Remove(w);
    }
    public Worker? Read(int id)
    {
       if (DataSource.Workers.Find(IdW => IdW.IdWorker == id) != null)
            return DataSource.Workers.Find(IdW => IdW.IdWorker == id);
        return null;
    }
    public List<Worker> ReadAll()
    {
        return new List<Worker>(DataSource.Workers.ToList());
    }
    public void Update(ref Worker w)
    {
        Delete(w.IdWorker);
        Create(w);
    }
}
