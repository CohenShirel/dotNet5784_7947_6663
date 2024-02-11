using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
public interface IAssignments
{
    public int Create(BO.Assignments item);
    public BO.Assignments? Read(int id);
    public IEnumerable<BO.AssignmentsInList> ReadAll(Func<BO.AssignmentsInList, bool>? filter = null);
    public IEnumerable<BO.Assignments> ReadAllAss(Func<BO.Assignments, bool>? filter = null);

    public void Update(ref BO.Assignments item);
    public void Delete(int id);
   // public BO.WorkerInAssignments GetDetailedCourseForStudent(int WorkerId, int AssignmentsId);

}
