namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


internal class WorkerImplementation : IWorker
{
    public int Create(Worker w)
    {
        //for entities with normal id (not auto id)
        if (Read(a => a.Id == w.Id) is not null)
            throw new DalAlreadyExistsException($"Worker with ID={w.Id} already exists");
        DataSource.Workers.Add(w);
        return w.Id;
    }
    public void Delete(int id)
    {
        if (Read(a => a.Id == id) is null)
            throw new DalDoesNotExistException($"Worker with ID={id} not exists");
        Worker w = Read(a => a.Id == id)!;
        DataSource.Workers.Remove(w);
    }
    public void DeleteAll()
    {
        Worker w = Read(a => true)!;
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
    public void Update(Worker w)
    {
        Delete(w.Id);
        Create(w);
    }
}
