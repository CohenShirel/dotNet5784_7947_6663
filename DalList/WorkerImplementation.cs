

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
    //checkkkkkkkkkkkkkkkkkkkkkkkkk
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Worker with ID={id} not exists");
        Worker w = Read(id);
        DataSource.Workers.Remove(w);
        return w.IdWorker;
    }

    public Worker? Read(int id)
    {
        return (DataSource.Workers.Find(IdW->Idw.WorkerId == id) = !null)
            //return IdW;
            // return null;
            //return DataSource.Workers;//תחזיר הפניה לאותו עובד שנמצא

   }

    public List<Worker> ReadAll()
    {
        return new List<Worker>(DataSource.Workers.ToList);
    }
    //ref?
    //אם זה לא טוב אולי נעשה מחיקה ואז הוספה מחדש...
    /*יש להסיר את ההפניה לאובייקט הקיים מרשימה
 לאחר מכן, יש להוסיף לרשימה את ההפניה לאובייקט המעודכן שהתקבל כפרמטר
 */
    public void Update(ref Worker w)
    {
        if (Read(w.IdWorker) is null)
            throw new Exception($"Worker with ID={w.IdWorker} not exists");
        Worker worker = Read(w.IdWorker);
        worker.Experience = w.Experience;
        worker.HourSalary = w.HourSalary;
        worker.Name = w.Name;
        worker.Email = w.Email;
    }
}
