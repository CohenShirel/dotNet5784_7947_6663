
//using DalList;
namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

public class AssignmentsImplementation:IAssignments
{
    public int Create(Assignments ass)
    {
        Assignments newAssignments = ass with { IdAssignments = DataSource.Config.idPAssignments };
        DataSource.Assignmentss.Add(newAssignments);
        return newAssignments.IdAssignments;


        //if (Read(ass.IdAssignments) is not null)
        //    throw new Exception($"Assignment with ID={ass.IdAssignments} already exists");
        ////for entities with auto id
        //int id = DataSource.Config.idPAssignments;
        //Assignments copy = ass with { IdAssignments = id };
        //DataSource.Assignmentss.Add(copy);
        //return id;
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Assignments with ID={id} not exists");
        DataSource.Assignmentss.Remove(Read(id));
    }
    public Assignments? Read(int id)
    {
        if (DataSource.Assignmentss.Find(IdA => IdA.IdAssignments == id) != null)
            return DataSource.Assignmentss.Find(IdA => IdA.IdAssignments == id);
        return null;
    }
    public int ReadName(string name)
    {
        if (DataSource.Assignmentss.Find(n => n.Name == name) != null)
            return DataSource.Assignmentss.Find(n => n.Name == name)!.IdAssignments;
        return 0;
    }
    public List<Assignments> ReadAll()
    {
        return new List<Assignments>(DataSource.Assignmentss.ToList());
    }
    public void Update(ref Assignments ass)
    {
        Delete(ass.IdAssignments);
        Create(ass);
    }
}