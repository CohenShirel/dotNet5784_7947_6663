﻿namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static BO.Exceptions;

internal class WorkerImplementation : IWorker
{
    private readonly IBl _bl;
    internal WorkerImplementation(IBl bl) => _bl = bl;

    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public int Create(BO.Worker boWorker)
    {
        Tools.CheckId(boWorker.Id);
        Tools.IsName(boWorker.Name!);
        Tools.checkCost(boWorker.HourSalary);
        Tools.IsMail(boWorker.Email!);
        Tools.IsEnum((int)boWorker.Experience);
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
                }
                select ass);
    }
    public void Delete(int id)
    {
        try
        {
            BO.Worker wrk = Read(id)!;
            if (wrk.currentAssignment != null && wrk.currentAssignment.AssignmentNumber != null)
            {
                Assignment a = s_bl.Assignment.Read(wrk.currentAssignment.AssignmentNumber)!;
                if (a.status == Status.Unscheduled || a.status == Status.Scheduled)
                {
                    _dal.Worker.Delete(id);
                }
                else
                    throw new Exceptions.BlInvalidOperationException($"Cannot delete worker with ID={id} because he has link to assignments");
            }
            else
                _dal.Worker.Delete(id);

            

        }
        catch(BlDoesNotExistException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
        }
        catch (BlInvalidOperationException ex)
        {
            throw new Exceptions.BlInvalidOperationException($"Cannot delete worker with ID={id} because he has link to assignments", ex);
        }
        catch (Exception ex)
        {
            throw new Exceptions.BlException("Failed to delete task", ex);
        }

    }

    public BO.Worker Read(int id)
    {
        try
        {
            DO.Worker doWrk = _dal.Worker.Read(wrk => wrk.Id == id)!
            ?? throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exist");
            DO.Assignment ass = _dal.Assignment.Read(t => t.WorkerId == doWrk.Id)!;
            return new BO.Worker
            {
                Id = doWrk.Id,
                Name = doWrk.Name,
                Email = doWrk.Email,
                Experience = doWrk.Experience,
                HourSalary = doWrk.HourSalary,
                currentAssignment = ass is not null ? new BO.WorkerInAssignment
                {
                    WorkerId = doWrk.Id,
                    AssignmentNumber = ass.IdAssignment,
                    AssignmentName=ass.Name
                } : null!,
            };
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={id} does Not exists", ex);
        }
        catch (BlInvalidOperationException ex)
        {
            throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={id} ", ex);
        }
        catch (Exception ex)
        {
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
           Experience=doWrk.Experience,
           currentAssignment = ass is not null ? new BO.WorkerInAssignment 
           {
               AssignmentNumber = ass.IdAssignment!,
               WorkerId = ass.WorkerId!,
               AssignmentName = ass.Name!,
           } : null!,
       }
       where filter is null ? true : filter(wrkLst)
       select wrkLst;


    public bool checkCurrentAssignment(BO.Worker boWorker)
    {
        BO.Assignment lstAss = s_bl.Assignment.Read(boWorker.currentAssignment.AssignmentNumber)!;
        if (boWorker.currentAssignment == null || boWorker.currentAssignment.WorkerId != boWorker.Id)
            throw new Exceptions.BlInvalidOperationException("The assignment is allocated to another worker");

        Assignment a = s_bl.Assignment.Read(boWorker.currentAssignment.AssignmentNumber)!;
        if (a.Links != null && a.Links.All(l => l.status == Status.Done) == false)
            throw new Exceptions.BlInvalidOperationException("Not all dependent assignments are completed");

        if ((int)a.LevelAssignment > (int)boWorker.Experience)
            throw new Exceptions.BlInvalidOperationException("The assignment's level ishigher than the worker's experience level");
        lstAss.IdWorker= boWorker.Id;
        s_bl.Assignment.Update(lstAss);
        return true;
    }
    public void Update(BO.Worker boWorker)
    {
        Tools.CheckId(boWorker.Id);
        Tools.IsName(boWorker.Name!);
        Tools.checkCost(boWorker.HourSalary);
        Tools.IsMail(boWorker.Email!);
        try
        {
            BO.Worker bWorker = new BO.Worker
            {
                Id = boWorker.Id,
                Experience = boWorker.Experience,
                HourSalary = boWorker.HourSalary,
                Name = boWorker.Name,
                Email = boWorker.Email,
            };
            DO.Worker wrk = Tools.ConvertWrkBOToDO(ref bWorker);
            _dal.Worker.Update(wrk);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exceptions.BlDoesNotExistException($"Worker with ID={boWorker.Id} does Not exists", ex);
        }
        catch (BlInvalidOperationException ex)
        {
            throw new Exceptions.BlInvalidOperationException($"Failed to update currentAssignment of Worker with ID={boWorker.Id} ", ex);
        }
        catch (Exception ex)
        {
            throw new Exceptions.BlException("Failed to update worker", ex);
        }
    }
}