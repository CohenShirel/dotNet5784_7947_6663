namespace Dal;
using DO;
using System.Collections.Generic;
using DalApi;


internal class WorkerImplementation: IWorker
{
    public int Create(Worker w)
    {
        //for entities with normal id (not auto id)
        if (Read(a => a.IdWorker == w.IdWorker) is not null)
            throw new DalAlreadyExistsException($"Worker with ID={w.IdWorker} already exists");
        DataSource.Workers.Add(w);
        return w.IdWorker;
    }
    public void Delete(int id)
    {
        if (Read(a => a.IdWorker == id) is null)
            throw new DalDoesNotExistException($"Worker with ID={id} not exists");
        Worker w = Read(a => a.IdWorker == id)!;
        DataSource.Workers.Remove(w);
    }
    public void DeleteAll()
    {
        Worker w = Read(a =>true)!;
        DataSource.Workers.Remove(w);
    }
    public Worker? Read(Func<Worker, bool> filter)
    {
        return DataSource.Workers.FirstOrDefault(filter);
    }
    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Workers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Workers
               select item;
    }
    public void Update(ref Worker w)
    {
        Delete(w.IdWorker);
        Create(w);
    }
}
