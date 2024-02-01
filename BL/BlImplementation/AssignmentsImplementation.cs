namespace BlImplementation;
using BlApi;
using BO;
//namespace Implementation

using System.Collections.Generic;
using System.Security.AccessControl;
internal class AssignmentsImplementation : IAssignments
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
   // private static readonly DalApi.IDal s_dal = ;
  //  private static readonly Random s_rand = new();

    public int Create(Assignments item)
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

    public Assignments? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AssignmentsInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Assignments item)
    {
        throw new NotImplementedException();
    }
}
