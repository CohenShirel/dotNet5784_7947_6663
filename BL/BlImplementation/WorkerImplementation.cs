namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;
using System.Xml.Linq;
using static BO.Exceptions;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    // private BlApi.IBl _dal1 = BlApi.Factory.Get;
    //private static Assignments lstAss = 

    // private static Assignments lstAss = AssignmentsImplementation.ReadAll();

    public int Create(BO.Worker boWorker)
    {
        if (boWorker.currentAssignment.status==Status.Unscheduled || boWorker.currentAssignment.status == Status.Scheduled)
        {
            Tools.CheckId(boWorker.Id);
            Tools.IsName(boWorker.Name!);
            Tools.checkCost(boWorker.HourSalary);
            Tools.IsMail(boWorker.Email!);
            DO.Worker doWorker = new DO.Worker
            (boWorker.Id, boWorker.Experience, boWorker.HourSalary, boWorker.Name, boWorker.Email);

            try
            {
                DO.Worker dWorker = new DO.Worker
                {
                    Experience = boWorker.Experience,
                    HourSalary = boWorker.HourSalary,
                    Name = boWorker.Name,
                    Email = boWorker.Email,
                };
                int idWor = _dal.Worker.Create(dWorker);
                return idWor;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlAlreadyExistsException($"Worker with ID={boWorker.Id} already exists", ex);
            }
            catch (BlInvalidOperationException ex)
            {
                throw new Exceptions.BlInvalidOperationException($"Failed to creat currentAssignment of Worker with ID={boWorker.Id} ", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create new worker", ex);
            }

        }
        throw new Exceptions.BlException("Failed to create new worker");

    }

    public IEnumerable<BO.AssignmentsInList> ConvertLstAssDOToBO()
    {
        return (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
                let ass = new BO.AssignmentsInList
                {
                    Id = doAssignments.IdAssignments,
                    AssignmentName = doAssignments.Name!,
                    LevelAssignments = doAssignments.LevelAssignments,
                    status = Tools.calaStatus(doAssignments),
                }
                select ass);
    }
    public void Delete(int id)
    {
        BO.Worker wrk = Read(id)!;
        if (wrk.currentAssignment.status == Status.Unscheduled || wrk.currentAssignment.status == Status.Scheduled)
        {
            //to check if the worker  in the middle of ass or he finished ass
            //0??
            bool hasCompletedTask = ConvertLstAssDOToBO().Any(ass => ass.IdWorker != null && ass.IdWorker.Equals(id) &&
            (ass.status == Status.Done || ass.status == Status.OnTrack));
            if (hasCompletedTask)
                throw new Exceptions.BlInvalidOperationException($"Cannot delete worker with ID={id} because he has link to assignments");
            try
            {
                _dal.Worker.Delete(id);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to delete task", ex);
            }
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
        try
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
                HourSalary = doWorker.HourSalary,
                currentAssignment= checkCurrentAssignment()::??
                //currentAssignment = Console.ReadLine() ?? throw new FormatException("Wrong input")
            };
            if(checkCurrentAssignment())
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
        }
        catch (BlInvalidOperationException ex)
        {
            throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
        }
        catch (Exception ex)
        {
            // טיפול בחריגות אחרות כפי שנדרש
            throw new Exceptions.BlException("Failed to read worker", ex);
        }


    }
    public IEnumerable<BO.WorkerInList> ConvertLstWrkDOToBO()
    {
        return (from DO.Worker doWorker in _dal.Worker.ReadAll()
                let wrk = new BO.WorkerInList
                {
                    Id = doWorker.IdWorker,
                    Name = doWorker.Name!,
                    currentAssignment=
                }
                select wrk);
    }
    public IEnumerable<WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null)
    {
        if (filter==null)
            return _dal.Worker.ReadAll().Select(doWorker => new BO.WorkerInList
            {
                Id = doWorker.IdWorker,
                Name = doWorker.Name!,

            });
        return (from DO.Worker doWorker in _dal.Worker.ReadAll()
                let boCIL = new BO.WorkerInList
                {
                    Id = doWorker.IdWorker,
                    Name = doWorker.Name!,
                    //currentAssignment = checkCurrentAssignment(doWorker) ? doWorker.currentAssignment : throw new BlInvalidOperationException("Failed to read currentAssignment"),
                    currentAssignment = doWorker.currentAssignment
                }
                where filter(boCIL)
                select boCIL) ;


        //{
        //    Experience = boWorker.Experience,
        //            HourSalary = boWorker.HourSalary,
        //            Name = boWorker.Name,
        //            Email = boWorker.Email,
        //            currentAssignment = checkCurrentAssignment(boWorker) ? boWorker.currentAssignment : throw new BlInvalidOperationException("Failed to update currentAssignment"),
        //        };
        //DO.Worker wrk = Tools.ConvertWrkBOToDO(bWorker);
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

    public bool checkCurrentAssignment(BO.Worker boWorker)
    {
        // Check if the assignment is allocated to another worker
        if (boWorker.currentAssignment == null|| boWorker.currentAssignment.IdWorker!= boWorker.Id)
            throw new Exceptions.BlInvalidOperationException("The assignment is allocated to another worker");

        // Check if all dependent assignments are completed
        if (boWorker.currentAssignment.links != null && boWorker.currentAssignment.links.All(link => link.status == Status.Done) == false)
            throw new Exceptions.BlInvalidOperationException("Not all dependent assignments are completed");

        // Check if the assignment's level is not higher than the worker's experience level
        if ((int)boWorker.currentAssignment.LevelAssignments > (int)boWorker.Experience)
            throw new Exceptions.BlInvalidOperationException("The assignment's level ishigher than the worker's experience level");
        return true;
}
    public void Update(BO.Worker boWorker)
    {
        if (boWorker.currentAssignment.status == Status.Unscheduled || boWorker.currentAssignment.status == Status.Scheduled)
        {
            Tools.CheckId(boWorker.Id);
            Tools.IsName(boWorker.Name!);
            Tools.checkCost(boWorker.HourSalary);
            Tools.IsMail(boWorker.Email!);
            try
            {
                BO.Worker bWorker = new BO.Worker
                {
                    Experience = boWorker.Experience,
                    HourSalary = boWorker.HourSalary,
                    Name = boWorker.Name,
                    Email = boWorker.Email,
                    currentAssignment = checkCurrentAssignment(boWorker) ? boWorker.currentAssignment : throw new BlInvalidOperationException("Failed to update currentAssignment"),
                };
                DO.Worker wrk = Tools.ConvertWrkBOToDO(bWorker);
                _dal.Worker.Update(ref wrk);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Worker with ID={boWorker.Id} does Not exists", ex);
            }
            catch (BlInvalidOperationException ex)
            {
                throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to update worker", ex);
            }
        }
        if (boWorker.currentAssignment.status == Status.OnTrack)
        {
            Tools.CheckId(boWorker.Id);
            Tools.IsName(boWorker.Name!);
            Tools.checkCost(boWorker.HourSalary);
            Tools.IsMail(boWorker.Email!);
            try
            {
                BO.Worker bWorker = new BO.Worker
                {
                    HourSalary = boWorker.HourSalary,
                    Name = boWorker.Name,
                    Email = boWorker.Email,
                    currentAssignment = checkCurrentAssignment(boWorker) ? boWorker.currentAssignment : throw new BlInvalidOperationException("Failed to update currentAssignment"),
                };
                DO.Worker wrk = Tools.ConvertWrkBOToDO(bWorker);
                _dal.Worker.Update(ref wrk);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Worker with ID={boWorker.Id} does Not exists", ex);
            }
            catch (BlInvalidOperationException ex)
            {
                throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to update worker", ex);
            }
        }
    }
}






//IEnumerable<BO.AssignmentsInList> lstA = (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
//                                          let ass = new BO.AssignmentsInList
//                                          {
//                                              Id = doAssignments.IdAssignments,
//                                              AssignmentName = doAssignments.Name!,
//                                              LevelAssignments = doAssignments.LevelAssignments,
//                                              status = Tools.calaStatus(doAssignments),
//                                          }
//                                          select ass);