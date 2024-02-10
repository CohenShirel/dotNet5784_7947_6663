using BO;
using DalApi;
using static BO.Exceptions;

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using BlTest;
//namespace Implementation
using System.Collections.Generic;
using static BO.Exceptions;
using DalApi;

internal class AssignmentsImplementation :IAssignments
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    //private static readonly DalApi.IDal s_dal = Bl.s_dal;
    //  private static readonly Random s_rand = new();
    public int Create(BO.Assignments boAssignments)
    {
        if (boAssignments.status == Status.Unscheduled  || boAssignments.status == Status.Scheduled)
     {
            //check the name and the id
            Tools.IsName(boAssignments.Description!);
            Tools.CheckId(boAssignments.IdAssignments);
            //Add dependencies for previous tasks from the existing ta
            //sk list
            for (int i = 0; i < boAssignments.links!.Count; i++)
            {
                _dal.Link!.Create(new Link(i, boAssignments.links[i].Id, boAssignments.IdAssignments));
            }
            DO.Assignments doAss = new DO.Assignments
             (boAssignments.IdAssignments, boAssignments.DurationAssignments, boAssignments.LevelAssignments, boAssignments.IdWorker, boAssignments.dateSrart, boAssignments.DateBegin,
                boAssignments.DeadLine, boAssignments.DateFinish, boAssignments.Name, boAssignments.Description, boAssignments.Remarks, boAssignments.ResultProduct);
            try
            {
                int idAss = _dal.Assignments.Create(doAss);
                return idAss;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlAlreadyExistsException($"Assignments with ID={boAssignments.IdAssignments} already exists", ex);
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
        BO.Assignments ass1 = Read(id)!;
        if (ass1.status == Status.Unscheduled  || ass1.status == Status.Scheduled)
     {
            Tools.CheckId(id);
            //BO.Assignments ass = Read(id)!;
            // Check if the assignment is linked to other assignments
            for (int i = 0; i < ass1.links!.Count; i++)
                //if there is ass that wasnt finished && the ass will finish after the current ass??????????
                if (ass1.links[i].DateFinish != null && ass1.links[i].DateFinish > ass1.dateSrart)
                    throw new Exceptions.BlInvalidOperationException($"Cannot delete assignment with ID={id} as it is linked to other assignments");
            try
            {
                _dal.Assignments.Delete(id);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignments with ID={id} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to delete task", ex);
            }
        }

        //שימו לב: אי אפשר למחוק משימות לאחר יצירת לו"ז הפרויקט.
    }

    //public WorkerInAssignments GetDetailedCourseForStudent(int WorkerId, int AssignmentsId)
    //{
    //    throw new NotImplementedException();
    //}
    private static Tuple<int, string> getWorker(int id)
    {
        if (id == 0)
            return new Tuple<int, string>(0, " ");
        var worker = _dal.Worker.Read(worker => worker.IdWorker == id);
        if (worker == null)
            return new Tuple<int, string>(0, " ");
        string name = worker.Name!;
        return new Tuple<int, string>(id, name);
    }
   
    public BO.Assignments? Read(int id)
    {
        try
        {
            DO.Assignments doAssignments = _dal.Assignments.Read(assignments => assignments.IdAssignments == id)
                ?? throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exist");
            return new BO.Assignments
            {

                IdAssignments = doAssignments.IdAssignments,
                Name = doAssignments.Name,
                Description = doAssignments.Description,
                status = Tools.calaStatus(doAssignments),
                dateSrart = doAssignments.dateSrart,
                DateBegin = doAssignments.DateBegin,
                DateFinish = doAssignments.DateFinish,
                LevelAssignments = doAssignments.LevelAssignments,//???
                DeadLine = doAssignments.DeadLine,
                DurationAssignments = doAssignments.DurationAssignments,
                //endProject = doAssignments.endProject,  
                Worker = getWorker(doAssignments.IdWorker),
                /////////links = Bl.GetLink(IdAssignments).ToList,
                // = doAssignments.LevelAssignments,
                ResultProduct = doAssignments.ResultProduct,
                Remarks = doAssignments?.Remarks,
            };
        }
        catch (Exception ex)
        {
            // טיפול בחריגות אחרות כפי שנדרש
            throw new Exceptions.BlException("Failed to delete task", ex);
        }
    }
    public IEnumerable<BO.AssignmentsInList> ReadAll(Func<BO.AssignmentsInList, bool>? filter = null)
    {
        return (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
                let ass = new BO.AssignmentsInList
                {
                    Id = doAssignments.IdAssignments,
                    AssignmentName = doAssignments.Name!,
                    LevelAssignments = doAssignments.LevelAssignments,
                    status = Tools.calaStatus(doAssignments)
                }
                where filter!(ass)
                select ass) ;
    }
    public IEnumerable<BO.Assignments> ReadAllAss(Func<BO.Assignments, bool>? filter = null)
    {
        return (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
                let ass = new BO.Assignments
                {
                    IdAssignments = doAssignments.IdAssignments,
                    DurationAssignments = doAssignments.DurationAssignments,
                    IdWorker = doAssignments.IdWorker,
                    Name = doAssignments.Name!,
                    Description = doAssignments.Description,
                    Remarks = doAssignments.Remarks,
                    ResultProduct = doAssignments.ResultProduct,
                    LevelAssignments = doAssignments.LevelAssignments,
                    dateSrart = doAssignments.dateSrart,
                    DateBegin = doAssignments.DateBegin,
                    DeadLine = doAssignments.DeadLine,
                    DateFinish = doAssignments.DateFinish,
                    status = Tools.calaStatus(doAssignments)
                }
                where filter!(ass)
                select ass);
    }
    public void Update(BO.Assignments boAss)
    {
        BO.Assignments ass = Read(boAss.IdAssignments)!;
        if (boAss.status == Status.OnTrack)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignments);
            try
            {
                DO.Assignments doAssignments = new DO.Assignments
                {
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct
                };
                _dal.Assignments.Update(ref doAssignments);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignments with ID={boAss.IdAssignments} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        if (boAss.status == Status.Unscheduled)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignments);
            try
            {
                 DO.Assignments doAssignments = new DO.Assignments
                 {
                     DurationAssignments = boAss.DurationAssignments,    
                     Description = boAss.Description,
                     Remarks = boAss.Remarks,
                     ResultProduct = boAss.ResultProduct
                 };
                 _dal.Assignments.Update(ref doAssignments);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignments with ID={boAss.IdAssignments} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        if (boAss.status == Status.Scheduled)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignments);
            try
            {
                
                DO.Assignments doAssignments = new DO.Assignments
                {
                    IdAssignments = boAss.IdAssignments,
                    DurationAssignments = boAss.DurationAssignments,
                    LevelAssignments = boAss.LevelAssignments,
                    IdWorker = boAss.IdWorker,//להוסיף בדיקה אם 
                    Name = boAss.Name,  
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct
                };
                _dal.Assignments.Update(ref doAssignments);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignments with ID={boAss.IdAssignments} does Not exists", ex);
            }
            catch (Exception ex)
            {
                // טיפול בחריגות אחרות כפי שנדרש
                throw new Exceptions.BlException("Failed to create task", ex);
            }

        }

        /*בשלב התכנון (ראה למטה יצירת לו"ז) ניתן לעדכן את פרטי המשימה, כגון משך הזמן הנדרש, רמת קושי, הוספת/מחיקת תלות במשימה אחרת וכו'.
לאחר יצירת לו"ז ניתן לשנות רק את השדות הטקסטואליים ואת המהנדס המוקצה למשימה.
*/
    }

   
}
//private static DateTime? a (DO.Assignments assignments)
//{
//    return a > 0 ? assignments.Date : null; 
//}
//schudledate->  DateBegin  העבודה לתחילת מתוכנן תאריך   
//completedate   DateFinish  בפועל סיום תארי  
//startdate      DateStart   המשימה על העבודה תחילת תאריך
//daedtimedate              לסיום מתוכנן תאריך="DeadLine
// Unscheduled, Scheduled,OnTrack,InJeopardy,Done
//private static Status calaStatus(DO.Assignments assignments)
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
//                   let ass = _dal.Assignments.Read(t => t.IdWorker == doWrk.IdWorker && t.dateSrart is not null && t.DateFinish is null)
//                   let worker = new BO.Worker
//                   {
//                       Id = doWrk.IdWorker,
//                       Name = doWrk.Name,
//                       Email = doWrk.Email,
//                       Experience = doWrk.Experience,
//                       HourSalary = doWrk.HourSalary,
//                       currentAssignment = ass is not null ? new BO.WorkerInAssignments { AssignmentsNumber = ass.IdAssignments!, AssignmentsName = ass.Name } : null,
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
//public BO.Assignments? Read(Func<BO.Assignments, bool>? filter = null)
//{
//    try
//    {
//        return from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
//               let ass = new BO.Assignments
//               {
//                   IdAssignments = doAssignments.IdAssignments,
//                   Name = doAssignments.Name,
//                   Description = doAssignments.Description,
//                   status = Tools.calaStatus(doAssignments),
//                   dateSrart = doAssignments.dateSrart,
//                   DateBegin = doAssignments.DateBegin,
//                   DateFinish = doAssignments.DateFinish,
//                   LevelAssignments = doAssignments.LevelAssignments,//???
//                   DeadLine = doAssignments.DeadLine,
//                   DurationAssignments = doAssignments.DurationAssignments,
//                   //endProject = doAssignments.endProject,  
//                   Worker = getWorker(doAssignments.IdWorker),
//                   /////////links = Bl.GetLink(IdAssignments).ToList,
//                   // = doAssignments.LevelAssignments,
//                   ResultProduct = doAssignments.ResultProduct,
//                   Remarks = doAssignments?.Remarks,
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