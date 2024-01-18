

using DalApi;
using DO;

namespace Dal;

internal class AssignmentsImplementation : IAssignments
{
    readonly string s_assignmentss_xml = "assignmentss";
    public int Create(Assignments item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        
    }

    public Assignments? Read(Func<Assignments, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Assignments?> ReadAll(Func<Assignments, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(ref Assignments item)
    {
        throw new NotImplementedException();
    }
}
