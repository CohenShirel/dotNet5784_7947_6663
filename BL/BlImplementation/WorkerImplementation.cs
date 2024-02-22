namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using static BO.Exceptions;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // private BlApi.IBl _dal1 = BlApi.Factory.Get;
    //private static Assignment lstAss = 

    // private static Assignment lstAss = AssignmentImplementation.ReadAll();

    public int Create(BO.Worker boWorker)
    {
        Tools.CheckId(boWorker.Id);
        Tools.IsName(boWorker.Name!);
        Tools.checkCost(boWorker.HourSalary);
        Tools.IsMail(boWorker.Email!);
        Tools.IsEnum((int)boWorker.Experience);
        //DO.Worker doWorker = new DO.Worker
        //(boWorker.Id, boWorker.Experience, boWorker.HourSalary, boWorker.Name, boWorker.Email);
        try
        {
            DO.Worker dWorker = new DO.Worker
            {
                Id = boWorker.Id,
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
        throw new Exceptions.BlException("Failed to create new worker");
    }

    public IEnumerable<BO.AssignmentInList> ConvertLstAssDOToBO()
    {
        return (from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
                let ass = new BO.AssignmentInList
                {
                    Id = doAssignment.IdAssignment,
                    AssignmentName = doAssignment.Name!,
                    //LevelAssignment = doAssignment.LevelAssignment,
                    //status = Tools.GetProjectStatus(doAssignment),
                }
                select ass);
    }
    public void Delete(int id)
    {
        BO.Worker wrk = Read(id)!;
        BO.Assignment? a = s_bl.Assignment.Read(ass => ass.WorkerId == id &&  ass.DateBegin != null && ass.DeadLine == null)??null;
        if (a==null/*a.status == Status.Unscheduled || a.status == Status.Scheduled*/)//If you dont have currentAssignment
        {

            //to check if the worker  in the middle of ass or he finished ass
            //0??
            bool hasCompletedTask = ConvertLstAssDOToBO().Any(ass => ass.IdWorker != null && ass.IdWorker.Equals(id) &&
            (a.status == Status.Done || a.status == Status.OnTrack));
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
    public BO.Worker Read(int id)
    {
        try
        {
            DO.Worker doWrk = _dal.Worker.Read(wrk => wrk.Id == id)!
            ?? throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exist");
            DO.Assignment? ass = _dal.Assignment.Read(t => t.WorkerId == doWrk.Id && t.DateBegin is not null && t.DeadLine is null)?? null;
            return new BO.Worker
            {
                Id = doWrk.Id,
                Name = doWrk.Name,
                Email = doWrk.Email,
                Experience = doWrk.Experience,
                HourSalary = doWrk.HourSalary,
                currentAssignment = ass is not null ? new BO.WorkerInAssignment
                {
                    WorkerId = doWrk.Id, AssignmentNumber = ass.IdAssignment 
                } : null!,
            };
        }
        catch (BlDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
        }
        catch (BlInvalidOperationException ex)
        {
            throw new BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={id} ", ex);
        }
        catch (Exception ex)
        {
            // טיפול בחריגות אחרות כפי שנדרש
            throw new BlException("Failed to read worker", ex);
        }
    }

    public IEnumerable<WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null) =>
       from DO.Worker doWrk in _dal.Worker.ReadAll()
       let ass = _dal.Assignment.Read(t => t.WorkerId == doWrk.Id /*&& t.dateSrart is not null && t.DateFinish is null*/)
       let wrkLst = new BO.WorkerInList
       {
           Id = doWrk.Id,
           Name = doWrk.Name!,
           currentAssignment = ass is not null ? new BO.WorkerInAssignment { AssignmentNumber = ass.IdAssignment!,
               WorkerId = ass.WorkerId! } : null!,
       }
       where filter is null ? true : filter(wrkLst)
       select wrkLst;


    public bool checkCurrentAssignment(BO.Worker boWorker)
    {
        BO.Assignment lstAss = s_bl.Assignment.Read(boWorker.currentAssignment.AssignmentNumber)!;
        // BO.Assignment lstAss=BO.Assignment
        // Check if the assignment is allocated to another worker
        if (boWorker.currentAssignment == null || boWorker.currentAssignment.WorkerId != boWorker.Id)
            throw new Exceptions.BlInvalidOperationException("The assignment is allocated to another worker");

        // Check if all dependent assignments are completed
        Assignment a = s_bl.Assignment.Read(boWorker.currentAssignment.AssignmentNumber)!;
        if (a.Links != null && a.Links.All(l => l.status == Status.Done) == false)
            throw new Exceptions.BlInvalidOperationException("Not all dependent assignments are completed");

        // Check if the assignment's level is not higher than the worker's experience level
        if ((int)a.LevelAssignment > (int)boWorker.Experience)
            throw new Exceptions.BlInvalidOperationException("The assignment's level ishigher than the worker's experience level");
        lstAss.IdWorker= boWorker.Id;
        s_bl.Assignment.Update(lstAss);
        //update ass
        return true;
    }

    public void Update(BO.Worker boWorker)
    {
        Assignment a = s_bl.Assignment.Read(boWorker.currentAssignment.AssignmentNumber)!;
        if (a.status == Status.Unscheduled || a.status == Status.Scheduled)
        {
            Tools.CheckId(boWorker.Id);
            Tools.IsName(boWorker.Name!);
            Tools.checkCost(boWorker.HourSalary);
            Tools.IsMail(boWorker.Email!);
            try
            {
                //WorkerInAssignment currentAssignment = boWorker.currentAssignment;

                BO.Worker bWorker = new BO.Worker
                {
                    Id = boWorker.Id,
                    Experience = boWorker.Experience,
                    HourSalary = boWorker.HourSalary,
                    Name = boWorker.Name,
                    Email = boWorker.Email,
                    currentAssignment = checkCurrentAssignment(boWorker) ? boWorker.currentAssignment : throw new BlInvalidOperationException("Failed to update currentAssignment"),
                };
                DO.Worker wrk = Tools.ConvertWrkBOToDO(ref bWorker);
                _dal.Worker.Update(wrk);
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
        if (a.status == Status.OnTrack)
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
                DO.Worker wrk = Tools.ConvertWrkBOToDO(ref bWorker);
                _dal.Worker.Update(wrk);
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

//public IEnumerable<BO.Worker> Read(Func<BO.Worker, bool>? filter = null)
//{
//    try
//    {
//        return from DO.Worker doWrk in _dal.Worker.ReadAll()
//               let ass = _dal.Assignment.Read(t => t.IdWorker == doWrk.IdWorker && t.dateSrart is not null && t.DateFinish is null)
//               let worker = new BO.Worker
//               {
//                   Id = doWrk.IdWorker,
//                   Name = doWrk.Name,
//                   Email = doWrk.Email,
//                   Experience = doWrk.Experience,
//                   HourSalary = doWrk.HourSalary,
//                   currentAssignment = ass is not null ? new BO.WorkerInAssignment { AssignmentNumber = ass.IdAssignment!, AssignmentName = ass.Name } : null,
//               }
//               where filter is null ? true : filter(worker)
//               select worker;
//    }
//    catch (DO.DalAlreadyExistsException ex)
//    {
//        throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
//    }
//    catch (BlInvalidOperationException ex)
//    {
//        throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
//    }
//    catch (Exception ex)
//    {
//        טיפול בחריגות אחרות כפי שנדרש
//        throw new Exceptions.BlException("Failed to read worker", ex);
//    }
//}
//public IEnumerable<WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null)
//{
//    if (filter==null)
//        return _dal.Worker.ReadAll().Select(doWorker => new BO.WorkerInList
//        {
//            Id = doWorker.IdWorker,
//            Name = doWorker.Name!,
//        });
//    return (from DO.Worker doWorker in _dal.Worker.ReadAll()
//            let boCIL = new BO.WorkerInList
//            {
//                Id = doWorker.IdWorker,
//                Name = doWorker.Name!,
//                //currentAssignment = checkCurrentAssignment(doWorker) ? doWorker.currentAssignment : throw new BlInvalidOperationException("Failed to read currentAssignment"),
//                currentAssignment = doWorker.currentAssignment
//            }
//            where filter(boCIL)
//            select boCIL) ;
//}

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


//****CURRENTASSIGMENT
//public IEnumerable<BO.WorkerInList> ConvertLstWrkDOToBO()
//{
//    return (from DO.Worker doWorker in _dal.Worker.ReadAll()
//            let wrk = new BO.WorkerInList
//            {
//                Id = doWorker.IdWorker,
//                Name = doWorker.Name!,
//                currentAssignment=
//            }
//            select wrk);
//}





//IEnumerable<BO.AssignmentInList> lstA = (from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
//                                          let ass = new BO.AssignmentInList
//                                          {
//                                              Id = doAssignment.IdAssignment,
//                                              AssignmentName = doAssignment.Name!,
//                                              LevelAssignment = doAssignment.LevelAssignment,
//                                              status = Tools.GetProjectStatus(doAssignment),
//                                          }
//                                          select ass);





//CURRENTASSIGNMENT
//CurrentTask = task is not null ? new BO.TaskInWorker
//            {
//                TaskID = task.Id,
//                TaskAlias = task.Alias!
//            } : null