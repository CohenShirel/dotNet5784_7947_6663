namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class AssignmentsImplementation : IAssignments
{
    public int Create(Assignment ass)
    {
        Assignment newAssignments = ass with { IdAssignment = DataSource.Config.idPAssignments };
        DataSource.Assignmentss.Add(newAssignments);
        return newAssignments.IdAssignment;
    }
    public void Delete(int id)
    {
        if (Read(a => a.IdAssignment == id) is null)
            throw new DalDoesNotExistException($"Assignment with ID={id} not exists");
        DataSource.Assignmentss.Remove(Read(a => a.IdAssignment == id)!);
    }
    public void DeleteAll()
    {
        Assignment ass = Read(a => true)!;
        DataSource.Assignmentss.Remove(ass);
    }
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignmentss.FirstOrDefault(filter);
    }
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) //stage 2
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

    public void Update(Assignment ass)
    {
        if (Read(a => a.IdAssignment == ass.IdAssignment) is null)
            throw new DalDoesNotExistException($"Assignment with ID={ass.IdAssignment} not exists");
        DataSource.Assignmentss.Remove(Read(a => a.IdAssignment == ass.IdAssignment)!);
        DataSource.Assignmentss.Add(ass);
    }
}
