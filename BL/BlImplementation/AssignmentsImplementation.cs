﻿namespace BlImplementation;
using BlApi;
using BO;
using DO;
//namespace Implementation
using System.Collections.Generic;
using static BO.Exceptions;

internal class AssignmentsImplementation : IAssignments
{
   
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


    //return (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
    //        select new BO.AssignmentsInList
    //        {
    //            Id = doAssignments.IdAssignments,
    //            AssignmentName = doAssignments.Name,
    //            LevelAssignments = doAssignments.LevelAssignments,
    //            status = calaStatus(doAssignments),
    //        });
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
            //    DO.Assignments doAssignments = new DO.Assignments
            //    (ass.IdAssignments, ass.DurationAssignments, ass.LevelAssignments, ass.IdWorker, ass.dateSrart,ass.DateBegin,
            //    ass.DeadLine, ass.DateFinish, boAss.Name, boAss.Description, boAss.Remarks, boAss.ResultProduct);
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

        if (boAss.status == Status.Unscheduled || boAss.status == Status.Scheduled)
        {
            //check the name and the id
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignments);
            //try
            //{
            //    DO.Assignments doAssignments = new DO.Assignments
            //    (boAss.IdAssignments, boAss.DurationAssignments, boAss.LevelAssignments, boAss.IdWorker, boAss.dateSrart, boAss.DateBegin,
            //    boAss.DeadLine, boAss.DateFinish, boAss.Name, boAss.Description, boAss.Remarks, boAss.ResultProduct);

            //    _dal.Assignments.Update(ref doAssignments);
            //}
            try
            {
                //    DO.Assignments doAssignments = new DO.Assignments
                //    (ass.IdAssignments, ass.DurationAssignments, ass.LevelAssignments, ass.IdWorker, ass.dateSrart,ass.DateBegin,
                //    ass.DeadLine, ass.DateFinish, boAss.Name, boAss.Description, boAss.Remarks, boAss.ResultProduct);
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
