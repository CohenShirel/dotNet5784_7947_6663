using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    internal interface IWorker
    {
        public int Create(BO.Worker item);
        public BO.Worker? Read(int id);
        public IEnumerable<BO.WorkerInList> ReadAll();
        public void Update(BO.Worker item);
        public void Delete(int id);
        public BO.WorkerInAssignments GetDetailedCourseForStudent(int StudentId, int CourseId);

    }
}
