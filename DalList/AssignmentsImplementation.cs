

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class AssignmentsImplementation : IAssignments
{
    public int Create(Assignments ass)
    {
        //for entities with auto id
        int id = DataSource.Config.IdPAssignments;
        Assignments copy = ass with { IdAssignments = id };
        DataSource.Assignmentss.Add(copy);
        return id;

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
    //int IdAssignments,
    //int DurationAssignments,
    //int LevelAssignments,
    //int IdWorker,
    //TimeSpan? DateBegin=null,
    //TimeSpan? DeadLine = null,
    //TimeSpan? DateStart = null,
    //TimeSpan? DateFinish = null,
    //string? Name = null,
    //string? Description = null,
    //string? Remarks = null,
    //string? ResultProduct = null,
    //bool Milestone = false
    public Assignments? Read(int id)
    {
        return (DataSource.Assignments.Find(IdA->IdA.IdAssignments == id) = !null)
    }

    public List<Assignments> ReadAll()
    {
        return new List<Assignments>(DataSource.Assignments.ToList);
    }

    public void Update(ref Assignments ass)
    {
        if (Read(ass.IdAssignments) is null)
            throw new Exception($"Assignments with ID={ass.IdAssignments} not exists");
        Assignments assignment = Read(ass.IdAssignments);
        //worker.Experience = w.Experience;
        //worker.HourSalary = w.HourSalary;
        //worker.Name = w.Name;
        //worker.Email = w.Email;
    }
}
