using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IClock
{
    public DateTime? SetStartProject(DateTime startProject);
    public DateTime? GetStartProject();
    public void resetClock();
    public void FormatClockOneHour();
    public void FormatClockOneDay();
    public void FormatClockOneMonth();
    public void FormatClockOneYear();
    //public DateTime? SetEndProject(DateTime endProject);
    //public DateTime? GetEndProject();
}
