

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

    public Assignments? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Assignments> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Assignments item)
    {
        throw new NotImplementedException();
    }
}
