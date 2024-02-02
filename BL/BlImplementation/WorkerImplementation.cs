namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Worker boWorker)
    {
        DO.Worker doWorker = new DO.Worker
        (boWorker.Id, boWorker.Experience, boWorker.HourSalary, boWorker.Name, boWorker.Email);
        try
        {
            int idWor = _dal.Worker.Create(doWorker);
            return idWor;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlAlreadyExistsException($"Worker with ID={boWorker.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Worker.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
        }
    }

    //public WorkerInAssignments GetDetailedCourseForStudent(int WorkerId, int AssignmentsId)
    //{
    //    throw new NotImplementedException();
    //}
    //public Worker Read(int id)
    //{
    //    DO.Worker? doWorker = _dal.Worker.Read(id); //??  throw new BO


    //    //if (doWorker == null)
    //    //    //throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");

    //    return new BO.Worker()
    //    {
    //        Id = doWorker.IdWorker,
    //        Name = doWorker.Name,
    //        Email = doWorker.Email,
    //        Experience = doWorker.Experience,
    //        HourSalary = doWorker.HourSalary
    //    };
    //}
    public Worker Read(int id)
    {
        // קוד הבא מבצע קריאה לפונקציה Read בשכבת ה DO ומצפה לקבל את המופע של Worker המתאים למזהה id
        DO.Worker? doWorker = _dal.Worker.Read(w => w.IdWorker == id) ?? throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exist");

        // אם המופע הוא null, כלומר לא נמצא Worker עם המזהה הנתון, נזרוק חריגת BO.BlDoesNotExistException
        //if (doWorker == null)
        //{
        //    throw new BO.BlDoesNotExistException($"Worker with ID={id} does Not exist");
        //}

        // אחרת, נבצע המרה מתוך מופע Worker בפורמט DO למופע Worker בפורמט BO ונחזיר אותו
        return new BO.Worker()
        {
            Id = doWorker.IdWorker,
            Name = doWorker.Name,
            Email = doWorker.Email,
            Experience = doWorker.Experience,
            HourSalary = doWorker.HourSalary
        };
    }

    IEnumerable<BO.WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null)
    {
        if (null == filter)
            //return (from DO.Course doCourse in _dal.Course.ReadAll()
            //        select new BO.CourseInList
            //        {
            //            Id = doCourse.Id,
            //            CourseNumber = doCourse.CourseNumber,
            //            CourseName = doCourse.CourseName,
            //            InYear = (BO.Year)doCourse.InYear!,
            //            InSemester = (BO.SemesterNames)doCourse?.InSemester!
            //        });

            return _dal.Worker.ReadAll().Select(doWorker => new BO.WorkerInList
            {
                IdWorker = doWorker.IdWorker,
                Name = doWorker.Name,
                Email = doWorker.Email,
                Experience = doWorker.Experience,
                HourSalary = doWorker.HourSalary
            });

        //else
        return (from DO.Worker doCourse in _dal.Worker.ReadAll()
                let boCIL = new BO.WorkerInList
                {
                    IdWorker = doWorker.IdWorker,
                    Name = doWorker.Name,
                    Email = doWorker.Email,
                    Experience = doWorker.Experience,
                    HourSalary = doWorker.HourSalary
                }
                where filter(boCIL)
                select boCIL);
    }
    //public IEnumerable<Worker> ReadAll(Func<BO.Worker, bool>? filter = null) =>
    //    from doWorker in _dal.Worker.ReadAll()

    //    let b = new Worker
    //    {
    //        IdWorker = doWorker.IdWorker,
    //        Name = doWorker.Name,
    //        Email = doWorker.Email,
    //        Experience = doWorker.Experience,
    //        HourSalary = doWorker.HourSalary
    //    }
    //    where filter?.Invoke(b) ?? true
    //    select b;

    //public IEnumerable<WorkerInList> ReadAll()
    //{
    //    throw new NotImplementedException();
    //}

    public void Update(BO.Worker boWorker)
    {
        try
        {
            DO.Worker doWorker = new DO.Worker
            (boWorker.Id, boWorker.Experience, boWorker.HourSalary, boWorker.Name, boWorker.Email);
            _dal.Worker.Update(ref doWorker);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={boWorker.Id} does Not exists", ex);
        }
    }

   
}


