namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;
//using BlTest;
//namespace Implementation
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BO.Exceptions;

internal class AssignmentImplementation : BlApi.IAssignment
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private readonly IBl _bl;
    internal AssignmentImplementation(IBl bl) => _bl = bl;
    public int Create(BO.Assignment boAssignment)
    {
        Status s = Tools.GetProjectStatus();
        if (s == BO.Status.Unscheduled || s == Status.Scheduled)
        {
            Tools.IsName(boAssignment.Description!);
            DO.Assignment doAss = new DO.Assignment
             (boAssignment.IdAssignment, boAssignment.DurationAssignment, boAssignment.LevelAssignment, boAssignment.IdWorker, boAssignment.dateSrart, boAssignment.DateBegin,
                boAssignment.DeadLine, boAssignment.DateFinish, boAssignment.Name, boAssignment.Description, boAssignment.Remarks, boAssignment.ResultProduct);
            try
            {
                int idAss = _dal.Assignment.Create(doAss);
                boAssignment.IdAssignment = idAss;
                updateDependincies(boAssignment);
                return idAss;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlAlreadyExistsException($"Assignment with ID={boAssignment.IdAssignment} already exists", ex);

            }
            catch (Exception ex)
            {
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        throw new Exceptions.BlException("Failed to create task");
    }

    private void updateDependincies(BO.Assignment boAssignment)
    {
        if (boAssignment.Links != null && boAssignment.Links.Any())
        {
            for (int i = 0; i < boAssignment.Links.Count; i++)
            {
                _dal.Link!.Create(new DO.Link(i, boAssignment.IdAssignment, boAssignment.Links[i].Id));
            }
        }
    }

    public void Delete(int id)
    {
        BO.Assignment ass1 = Read(id)!;
        Status s = Tools.GetProjectStatus();
        if (s == Status.Unscheduled || s == Status.Scheduled)
        {
            Tools.CheckId(id);
            if (ass1.Links != null && ass1.Links.Any())
            {
                for (int i = 0; i < ass1.Links!.Count; i++)
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
                throw new Exceptions.BlException("Failed to delete task", ex);
            }
        }
        else
            throw new Exceptions.BlException("YOU MUSTN'T DELEDTE THIS ASSIGNMENT");
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

    public IEnumerable<AssignmentInList> GetLinkedAssignments(BO.Assignment currentAssignment)
    {
        List<AssignmentInList> links = new List<AssignmentInList>();

        if (currentAssignment == null)
            return Enumerable.Empty<AssignmentInList>();
        if (currentAssignment.Links != null && currentAssignment.Links.Any())
        {
            for (int i = 0; i < currentAssignment.Links.Count; i++)
            {
                links.Add(currentAssignment.Links[i]);         
            }
        }
        if (links == null)
            return Enumerable.Empty<AssignmentInList>();
        return links;
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
                dateSrart = doAssignment.DateSrart,
                DateBegin = doAssignment.DateBegin,
                DateFinish = doAssignment.DateFinish,
                LevelAssignment = doAssignment.LevelAssignment,
                DeadLine = doAssignment.DeadLine,
                DurationAssignment = doAssignment.DurationAssignment,
                IdWorker = getWorker(doAssignment.WorkerId).Item1,
                ResultProduct = doAssignment.ResultProduct,
                Remarks = doAssignment?.Remarks,
            };
        }
        catch (Exception ex)
        {
            throw new Exceptions.BlException("Failed to read task", ex);
        }
    }
    public BO.Assignment? Read(Func<DO.Assignment, bool> filter)
    {
        DO.Assignment doAssignment = _dal.Assignment.Read(filter) ?? null;
        if (doAssignment == null)
            return null;
        return new BO.Assignment
        {
            IdAssignment = doAssignment.IdAssignment,
            Name = doAssignment.Name,
            Description = doAssignment.Description,
            status = Tools.GetProjectStatus(),
            dateSrart = doAssignment.DateSrart,
            DateBegin = doAssignment.DateBegin,
            DateFinish = doAssignment.DateFinish,
            LevelAssignment = doAssignment.LevelAssignment,
            DeadLine = doAssignment.DeadLine,
            DurationAssignment = doAssignment.DurationAssignment,
            IdWorker = getWorker(doAssignment.WorkerId).Item1,
            ResultProduct = doAssignment.ResultProduct,
            Remarks = doAssignment?.Remarks,
        };
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
            LevelAssignment = doAssignment.LevelAssignment,
            status = Tools.GetProjectStatus(),
        }
        where filter is null ? true : filter!(ass)
        select ass;

    public IEnumerable<BO.Assignment> ReadAllAss(Func<BO.Assignment, bool>? filter = null) =>
         from DO.Assignment doAssignment in _dal.Assignment.ReadAll()
         let ass = new BO.Assignment
         {
             IdAssignment = doAssignment.IdAssignment,
             Name = doAssignment.Name!,
             IdWorker = doAssignment.WorkerId,
             DeadLine = doAssignment.DeadLine,
             DateBegin = doAssignment.DateBegin,
             LevelAssignment = doAssignment.LevelAssignment,
             status = Tools.GetProjectStatus(),
             dateSrart=doAssignment.DateSrart,
             DateFinish=doAssignment.DateFinish,
             Remarks=doAssignment.Remarks,
             Description = doAssignment.Description,
             ResultProduct=doAssignment.ResultProduct,
             DurationAssignment=doAssignment.DurationAssignment
         }
         where filter is null ? true : filter!(ass)
         select ass;

    public void Update(BO.Assignment boAss)
    {
        BO.Assignment ass = Read(boAss.IdAssignment)!;
        Status s = Tools.GetProjectStatus();
        DO.Assignment doAss;
        if (s == Status.OnTrack)
        {
            Tools.IsName(boAss.Description!);
            Tools.CheckId(boAss.IdAssignment);
            try
            {
                doAss = new DO.Assignment
                {
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct,
                    DateSrart=boAss.dateSrart,
                    DateFinish=boAss.DateFinish,
                    IdAssignment=boAss.IdAssignment,
                    WorkerId=boAss.IdWorker,
                };
                _dal.Assignment.Update(doAss);
                updateDependincies(boAss);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
        if (s == Status.Unscheduled)
        {
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
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
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
                    WorkerId = boAss.IdWorker,
                    Name = boAss.Name,
                    Description = boAss.Description,
                    Remarks = boAss.Remarks,
                    ResultProduct = boAss.ResultProduct,
                    DateBegin = boAss.DateBegin,
                    DeadLine = boAss.DeadLine,
                };
                updateDependincies(boAss);

                _dal.Assignment.Update(doAss);
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new Exceptions.BlDoesNotExistException($"Assignment with ID={boAss.IdAssignment} does Not exists", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.BlException("Failed to create task", ex);
            }
        }
    }

    private static DO.Assignment updateBasicAssigmentDetails(BO.Assignment boAss, DO.Assignment doAss)
    {
        Tools.IsName(boAss.Description!);
        Tools.CheckId(boAss.IdAssignment);
        doAss = doAss with
        {
            Description = boAss.Description,
            Remarks = boAss.Remarks,
            ResultProduct = boAss.ResultProduct
        };
        return doAss;
    }
    private static DO.Assignment updateAssigmentWorkerId(BO.Assignment boAss, Status s, DO.Assignment doAss)
    {
        if (s == Status.Scheduled)
        {
            doAss = doAss with
            {
                WorkerId = boAss.IdWorker,
                DateSrart = s_bl.Clock.GetStartProject()
            };
        }
        return doAss;
    }
}