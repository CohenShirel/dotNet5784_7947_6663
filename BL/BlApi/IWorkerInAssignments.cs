namespace BlApi
{
    public interface IWorkerInAssignment
    {
        //כמה משימות נשאר לעובד?
        public BO.WorkerInAssignment GetSchedulePerWorker(int WorkerId);
        //  public bool GetTotalAverageOfStudent(int StudentId);
        //  public bool GetAveragePerYearOfStudent(int StudentId, BO.Year year);
        //  public bool UpdateGrade(int StudentId, int CourseId, double grade);

    }
}


