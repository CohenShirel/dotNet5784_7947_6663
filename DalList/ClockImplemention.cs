﻿using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class ClockImplemention : IClock
{
    public void FormatClockOneDay() => DataSource.Config.StartDate!.Value.AddDays(1);

    public void FormatClockOneHour() => DataSource.Config.StartDate!.Value.AddHours(1);

    public void FormatClockOneMonth() => DataSource.Config.StartDate!.Value.AddMonths(1);

    public void FormatClockOneYear() => DataSource.Config.StartDate!.Value.AddYears(1);
    //public DateTime? GetEndProject()
    //{
    //    return DataSource.Config.EndDate!.Value;
    //}

    public DateTime? GetStartProject()
    { 
        return DataSource.Config.StartDate!.Value; 
    }
    public void resetClock()
    {
       // DataSource.Config.EndDate = null;
        DataSource.Config.StartDate = null;
    }

    //public DateTime? SetEndProject(DateTime? endProject)
    //{
    //    DataSource.Config.EndDate= endProject;
    //    return endProject;
    //}

    public DateTime? SetStartProject(DateTime startProject)
    {
        DataSource.Config.StartDate = startProject;
        return startProject;
    }
}
