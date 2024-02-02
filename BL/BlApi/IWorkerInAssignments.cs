using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IWorkerInAssignments
    {
        //כמה משימות נשאר לעובד?
        public BO.WorkerInAssignments GetSchedulePerWorker(int WorkerId);
     //  public bool GetTotalAverageOfStudent(int StudentId);
      //  public bool GetAveragePerYearOfStudent(int StudentId, BO.Year year);
      //  public bool UpdateGrade(int StudentId, int CourseId, double grade);

    }
}


