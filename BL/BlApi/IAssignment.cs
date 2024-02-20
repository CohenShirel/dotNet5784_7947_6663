﻿namespace BlApi;
public interface IAssignment
{
    public int Create(BO.Assignment item);
    public BO.Assignment? Read(int id);
    public IEnumerable<BO.AssignmentInList> ReadAll(Func<BO.AssignmentInList, bool>? filter = null);
    public IEnumerable<BO.Assignment> ReadAllAss(Func<BO.Assignment, bool>? filter = null);

    public void Update(BO.Assignment item);
    public void Delete(int id);
    // public BO.WorkerInAssignment GetDetailedCourseForStudent(int WorkerId, int AssignmentId);

}
