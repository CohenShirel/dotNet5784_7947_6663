

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class WorkerImplementation :IWorker
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
        throw new NotImplementedException();
    }

    public Worker? Read(int id)
    {
        if (DataSource.Workers.Find(IdW->Idw.WorkerId == id) = !null)
            return DataSource.Workers;//תחזיר הפניה לאותו עובד שנמצא
        return null;
            
    }

    public List<Worker> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Worker item)
    {
        throw new NotImplementedException();
    }
}
