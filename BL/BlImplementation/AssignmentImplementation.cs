namespace BlImplementation;
using BO;
using DalApi;
//using BlTest;
//namespace Implementation
using System.Collections.Generic;

internal class AssignmentImplementation : BlApi.IAssignment
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    //private static readonly DalApi.IDal s_dal = Bl.s_dal;
    //  private static readonly Random s_rand = new();
    public int Create(BO.Assignment boAssignment)
    {
        Status s = Tools.GetProjectStatus();
        if (s == BO.Status.Unscheduled || s == Status.Scheduled)
        {
            //check the name and the id
            Tools.IsName(boAssignment.Description!);
            //Tools.CheckId(boAssignment.IdAssignment);
            //Add dependencies for previous tasks from the existing ta
            //sk list
            DO.Assignment doAss = new DO.Assignment
             (boAssignment.IdAssignment, boAssignment.DurationAssignment, boAssignment.LevelAssignment, boAssignment.IdWorker, boAssignment.dateSrart, boAssignment.DateBegin,
                boAssignment.DeadLine, boAssignment.DateFinish, boAssignment.Name, boAssignment.Description, boAssignment.Remarks, boAssignment.ResultProduct);
            try
            {
                int idAss = _dal.Assignment.Create(doAss);

                if (boAssignment.Links != null && boAssignment.Links.Any())
                {
                    for (int i = 0; i < boAssignment.Links.Count; i++)
                    {
                        //doAss.IdAssignment

                        _dal.Link!.Create(new DO.Link(i, idAss, boAssignment.Links[i].Id));
                    }
                }
                return idAss;

            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlAlreadyExistsException($"Assignment with ID={boAssignment.IdAssignment} already exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }

        }
        throw new Exceptions.BlException("Failed to create task");

    }

    public void Delete(int id)
    {
        BO.Assignment ass1 = Read(id)!;
        Status s = Tools.GetProjectStatus();
        if (s == Status.Unscheduled || s == Status.Scheduled)
        {
            Tools.CheckId(id);
            
            //BO.Assignment ass = Read(id)!;
            // Check if the assignment is linked to other assignments
            if (ass1.Links != null && ass1.Links.Any())
            {
                for (int i = 0; i < ass1.Links!.Count; i++)
                    //if there is ass that wasnt finished && the ass will finish after the current ass??????????
                    if (ass1.Links[i].DeadLine != null && ass1.Links[i].DeadLine > ass1.DateBegin)
                        throw new Exceptions.BlInvalidOperationException($"Cannot delete assignment with ID={id} as it is linked to other assignments");
            }
            try
            {
                _dal.Assignment.Delete(id);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={id} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to delete task", ex);
            }
        }
        throw new Exceptions.BlException("YOU MUSTN'T DELEDTE THIS ASSIGNMENT");

        //שימו לב: אי אפשר למחוק משימות לאחר יצירת לו"ז הפרויקט.
    }

    private static Tuple<int, string> getWorker(int id)
    {
        if (id == 0)
            return new Tuple<int, string>(0, " ");
        var worker = _dal.Worker.Read(worker => worker.Id == id);
        if (worker == null)
            return new Tuple<int, string>(0, " ");
        string name = worker.Name!;
        return new Tuple<int, string>(id, name);
    }

    public BO.Assignment? Read(int id)
    {
        try
        {
            DO.Assignment doAssignment = _dal.Assignment.Read(assignments => assignments.IdAssignment
            == id)
                ?? throw new Exceptions.BlDoesNotExistException($"Assigment with ID={id} does Not exist");
            return new BO.Assignment
            {
                IdAssignment = doAssignment.IdAssignment,
                Name = doAssignment.Name,
                Description = doAssignment.Description,
                status = Tools.GetProjectStatus(),
                //status = Tools.GetProjectStatus(doAssignment),
                dateSrart = doAssignment.DateSrart,
                DateBegin = doAssignment.DateBegin,
                DateFinish = doAssignment.DateFinish,
                LevelAssignment = doAssignment.LevelAssignment,//???
                DeadLine = doAssignment.DeadLine,
                DurationAssignment = doAssignment.DurationAssignment,
                //endProject = doAssignment.endProject,  
                IdWorker = getWorker(doAssignment.WorkerId).Item1,
                /////////Links = Bl.GetLink(IdAssignment).ToList,
                // = doAssignment.LevelAssignment,
                ResultProduct = doAssignment.ResultProduct,
                Remarks = doAssignment?.Remarks,
            };
        }
        catch (Exception ex)
        {
            // טיפול בחריגות אחרות כפי שנדרש
            throw new Exceptions.BlException("Failed to read task", ex);
        }
    }
    public IEnumerable<BO.AssignmentInList> ReadAll(Func<BO.AssignmentInList, bool>? filter = null) =>
        from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
        let ass = new BO.AssignmentInList
        {
            Id = doAssignment.IdAssignment,
            AssignmentName = doAssignment.Name!,
            IdWorker = doAssignment.WorkerId,
            DeadLine = doAssignment.DeadLine,
            DateBegin = doAssignment.DateBegin,
            //LevelAssignment = doAssignment.LevelAssignment,
            status = Tools.GetProjectStatus(),
            //status = Tools.GetProjectStatus(doAssignment)

        }
        where filter is null ? true : filter!(ass)
        select ass;
    public IEnumerable<BO.Assignment> ReadAllAss(Func<BO.Assignment, bool>? filter = null)
    {
        return (from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
                let ass = new BO.Assignment
                {
                    IdAssignment = doAssignment.IdAssignment,
                    DurationAssignment = doAssignment.DurationAssignment,
                    IdWorker = doAssignment.WorkerId,
                    Name = doAssignment.Name!,
                    Description = doAssignment.Description,
                    Remarks = doAssignment.Remarks,
                    ResultProduct = doAssignment.ResultProduct,
                    LevelAssignment = doAssignment.LevelAssignment,
                    dateSrart = doAssignment.DateSrart,
                    DateBegin = doAssignment.DateBegin,
                    DeadLine = doAssignment.DeadLine,
                    DateFinish = doAssignment.DateFinish,
                    status = Tools.GetProjectStatus()
                    //status = Tools.GetProjectStatus(doAssignment)

                }
                where filter!(ass)
                select ass);
    }
    public void Update(BO.Assignment boAss)
    {
        BO.Assignment ass = Read(boAss.IdAssignment)!;
        Status s = Tools.GetProjectStatus();
        //var lst = _dal.Link.ReadAll(l => l.IdAssignment == boAss.IdAssignment);
        //var lstP = _dal.Link.ReadAll(l => l.IdPAssignment == boAss.IdAssignment);
        DO.Assignment doAss;
       // int idOfAss = ass.IdAssignment;
        if (s == Status.OnTrack)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignment);
            try
            {
                doAss = new DO.Assignment
                {
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct
                };
                _dal.Assignment.Update(doAss);
                //Tools.ConvertAssDOToBO(doAss).Links = boAss.Links;

               
                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, doAss.IdAssignment, l.IdPAssignment));
                //}
                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, l.IdAssignment, doAss.IdAssignment));
                //}

            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        if (s == Status.Unscheduled)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignment);
            try
            {
                doAss = new DO.Assignment
                {
                    DurationAssignment = boAss.DurationAssignment,
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct
                };
                _dal.Assignment.Update(doAss);
                //Tools.ConvertAssDOToBO(doAss).Links = boAss.Links;

                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, doAss.IdAssignment, l.IdPAssignment));
                //}
                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, l.IdAssignment, doAss.IdAssignment));
                //}
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        // if (((boAss.status== Status.Scheduled) && (ass.status == Status.UnScheduled)) || boAss.status == Status.Scheduled)
        if (s == Status.Scheduled)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignment);
            try
            {
                doAss = new DO.Assignment
                {
                    IdAssignment = boAss.IdAssignment,
                    DurationAssignment = boAss.DurationAssignment,
                    LevelAssignment = boAss.LevelAssignment,
                    WorkerId = boAss.IdWorker,//להוסיף בדיקה אם 
                    Name = boAss.Name,
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct,
                    DateBegin = boAss.DateBegin,
                    DeadLine = boAss.DeadLine,
                };
                _dal.Assignment.Update(doAss);
                //Tools.ConvertAssDOToBO(doAss).Links = boAss.Links;

                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, doAss.IdAssignment, l.IdPAssignment));
                //}
                //foreach (var l in lst)
                //{
                //    _dal.Link!.Create(new DO.Link(1, l.IdAssignment, doAss.IdAssignment));
                //}
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }

    }
}


//int idAss = _dal.Assignment.Create(doAss);
//for (int i = 0; i < boAssignment.Links!.Count; i++)
//{
//    if (boAssignment.Links is not null)
//    {
//        _dal.Link!.Create(new DO.Link(i, doAss.IdAssignment, boAssignment.Links[i].Id));

//        //boAssignment.Links!.Select(Links => _dal.Link.Create(new DO.Link(i, doAss.IdAssignment, boAssignment.Links[i].Id)));
//    }
//}
////for (int i = 0; i < boAssignment.Links!.Count; i++)
////{
////    _dal.Link!.Create(new DO.Link(i, doAss.IdAssignment, boAssignment.Links[i].Id));
////}
//return idAss;






//public WorkerInAssignment GetDetailedCourseForStudent(int WorkerId, int AssignmentId)
//{
//    throw new NotImplementedException();
//}






//private static DateTime? a (DO.Assignment assignments)
//{
//    return a > 0 ? assignments.Date : null; 
//}
//schudledate->  DateBegin  העבודה לתחילת מתוכנן תאריך   
//completedate   DateFinish  בפועל סיום תארי  
//startdate      DateStart   המשימה על העבודה תחילת תאריך
//daedtimedate              לסיום מתוכנן תאריך="DeadLine
// Unscheduled, Scheduled,OnTrack,InJeopardy,Done
//private static Status GetProjectStatus(DO.Assignment assignments)
//{
//    if (assignments.DateBegin is null)
//        return BO.Status.Unscheduled;
//    if (assignments.DateFinish is not null)
//        return BO.Status.Done;
//    return BO.Status.Done;//לא צריך את השורה הזאת
//    //if(assignments.dateSrart is null)
//    //    return BI.s_Clock= assignments.requidifferent :assignments.DeadLine
//    //    ?Status.InJeopardy: Status.Scheduled;
//    ////assignments has started but hasnt finished
//    //return BI.s_Clock = (+assignments.requidifferent - (BI.s_Clock - assignments.dateSrart))
//    //    > assignments.DeadLine ? Status.InJeopardy : Status.OnTrack;
//}
//תאריך משוער לסיום,לעשות פונקציה כזאת כמו של הסטטוס
//לעשות את ריד עפ פילטר
//    try
//        {
//            return from DO.Worker doWrk in _dal.Worker.ReadAll()
//                   let ass = _dal.Assignment.Read(t => t.IdWorker == doWrk.IdWorker && t.dateSrart is not null && t.DateFinish is null)
//                   let worker = new BO.Worker
//                   {
//                       Id = doWrk.IdWorker,
//                       Name = doWrk.Name,
//                       Email = doWrk.Email,
//                       Experience = doWrk.Experience,
//                       HourSalary = doWrk.HourSalary,
//                       currentAssignment = ass is not null ? new BO.WorkerInAssignment { AssignmentNumber = ass.IdAssignment!, AssignmentName = ass.Name } : null,
//                   }
//                   where filter is null ? true : filter(worker)
//                   select worker;
//}
//          catch (DO.DalAlreadyExistsException ex)
//        {
//            throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
//        }
//        catch (BlInvalidOperationException ex)
//        {
//            throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
//        }
//        catch (Exception ex)
//        {
//            // טיפול בחריגות אחרות כפי שנדרש
//            throw new Exceptions.BlException("Failed to read worker", ex);
//        }
//public BO.Assignment? Read(Func<BO.Assignment, bool>? filter = null)
//{
//    try
//    {
//        return from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
//               let ass = new BO.Assignment
//               {
//                   IdAssignment = doAssignment.IdAssignment,
//                   Name = doAssignment.Name,
//                   Description = doAssignment.Description,
//                   status = Tools.GetProjectStatus(doAssignment),
//                   dateSrart = doAssignment.dateSrart,
//                   DateBegin = doAssignment.DateBegin,
//                   DateFinish = doAssignment.DateFinish,
//                   LevelAssignment = doAssignment.LevelAssignment,//???
//                   DeadLine = doAssignment.DeadLine,
//                   DurationAssignment = doAssignment.DurationAssignment,
//                   //endProject = doAssignment.endProject,  
//                   Worker = getWorker(doAssignment.IdWorker),
//                   /////////Links = Bl.GetLink(IdAssignment).ToList,
//                   // = doAssignment.LevelAssignment,
//                   ResultProduct = doAssignment.ResultProduct,
//                   Remarks = doAssignment?.Remarks,
//               }
//               where filter is null ? true : filter(ass)
//               select ass;
//    }
//    catch (Exception ex)
//    {
//        // טיפול בחריגות אחרות כפי שנדרש
//        throw new Exceptions.BlException("Failed to delete task", ex);
//    }
//}