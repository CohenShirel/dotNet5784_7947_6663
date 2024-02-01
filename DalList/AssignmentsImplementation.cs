namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class AssignmentsImplementation:IAssignments
{
    public int Create(Assignments ass)
    {
        Assignments newAssignments = ass with { IdAssignments = DataSource.Config.idPAssignments };
        DataSource.Assignmentss.Add(newAssignments);
        return newAssignments.IdAssignments;
    }
    public void Delete(int id)
    {
        if (Read(a => a.IdAssignments == id) is null)
            throw new DalDoesNotExistException($"Assignments with ID={id} not exists");
        DataSource.Assignmentss.Remove(Read(a => a.IdAssignments == id)!);
    }
    public void DeleteAll()
    {
        Assignments ass = Read(a => true)!;
        DataSource.Assignmentss.Remove(ass);
    }
    public Assignments? Read(Func <Assignments, bool> filter)
    {
        return DataSource.Assignmentss.FirstOrDefault(filter);
    }

    //public int ReadName(string name)
    //{
    //    if (DataSource.Assignmentss.Find(n => n.Name == name) != null)
    //        return DataSource.Assignmentss.Find(n => n.Name == name)!.IdAssignments;
    //    return 0;
    //}
    public IEnumerable<Assignments> ReadAll(Func<Assignments, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Assignmentss
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Assignmentss
               select item;
    }

    public void Update(ref Assignments ass)
    {
        Delete(ass.IdAssignments);
        Create(ass);
    }
}
