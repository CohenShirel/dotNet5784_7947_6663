namespace BlImplementation;
using BlApi;
using BO;
//namespace Implementation
using System.Collections.Generic;

internal class AssignmentsImplementation : IAssignments
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    //private static readonly DalApi.IDal s_dal = Bl.s_dal;
    //  private static readonly Random s_rand = new();
    public int Create(BO.Assignments boAssignments)
    {
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
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Assignments.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Assignments with ID={id} does Not exists", ex);
        }
    }

    //public WorkerInAssignments GetDetailedCourseForStudent(int WorkerId, int AssignmentsId)
    //{
    //    throw new NotImplementedException();
    //}
    private static Tuple<int,string> getWorker(int id)
    {
        if (id == null)
            return new Tuple<int, string>(0, " ");
        var worker = _dal.Worker.Read(worker => worker.IdWorker == id);
        if (worker == null)
            return new Tuple<int,string>(0, " ");
        string name=worker.Name!;
        return new Tuple <int, string>(id, name);
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
    private static Status calaStatus(DO.Assignments assignments)
    {
        if (assignments.DateBegin is null)
            return BO.Status.Unscheduled;
        if (assignments.DateFinish is not null)
            return BO.Status.Done;
        return BO.Status.Done;//לא צריך את השורה הזאת
        //if(assignments.dateSrart is null)
        //    return BI.s_Clock= assignments.requidifferent :assignments.DeadLine
        //    ?Status.InJeopardy: Status.Scheduled;
        ////assignments has started but hasnt finished
        //return BI.s_Clock = (+assignments.requidifferent - (BI.s_Clock - assignments.dateSrart))
        //    > assignments.DeadLine ? Status.InJeopardy : Status.OnTrack;
    }
    public Assignments? Read(int id)
    {
        DO.Assignments doAssignments = _dal.Assignments.Read(assignments => assignments.IdAssignments == id)?? throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exist");
        return new Assignments
        {
        
            IdAssignments = doAssignments.IdAssignments,
            Name = doAssignments.Name,
            Description = doAssignments.Description,
            status = calaStatus(doAssignments),
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

    public IEnumerable<BO.AssignmentsInList> ReadAll()
    {
        return (from DO.Assignments doAssignments in _dal.Assignments.ReadAll()
                select new BO.AssignmentsInList
                {
                    Id = doAssignments.IdAssignments,
                    AssignmentName = doAssignments.Name,
                    LevelAssignments = doAssignments.LevelAssignments,
                    status = calaStatus(doAssignments),
                });
    }

    public void Update(BO.Assignments boAss)
    {
        try
        {
            DO.Assignments doAssignments = new DO.Assignments
            (boAss.IdAssignments, boAss.DurationAssignments, boAss.LevelAssignments,boAss.IdWorker, boAss.dateSrart, boAss.DateBegin,
            boAss.DeadLine, boAss.DateFinish, boAss.Name, boAss.Description, boAss.Remarks, boAss.ResultProduct);

            _dal.Assignments.Update(ref doAssignments);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Assignments with ID={boAss.IdAssignments} does Not exists", ex);
        }
    }
}
