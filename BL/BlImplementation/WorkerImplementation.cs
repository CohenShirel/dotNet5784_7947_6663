namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(Worker item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public WorkerInAssignments GetDetailedCourseForStudent(int WorkerId, int AssignmentsId)
    {
        throw new NotImplementedException();
    }

    public Worker? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<WorkerInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Worker item)
    {
        throw new NotImplementedException();
    }
}
