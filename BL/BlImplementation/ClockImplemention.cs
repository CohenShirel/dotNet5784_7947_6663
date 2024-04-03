using BlApi;
namespace BlImplementation;

internal class ClockImplemention : IClock
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    //public DateTime? GetEndProject()=>_dal.Clock.GetEndProject();
    public DateTime? GetStartProject()=>_dal.Clock.GetStartProject();
    //public DateTime? SetEndProject(DateTime endProject) => _dal.Clock.SetEndProject(endProject);
    public DateTime? SetStartProject(DateTime startProject) => _dal.Clock.SetStartProject(startProject);
    public void resetClock()=>_dal.Clock.resetClock();
    public void FormatClockOneHour() => _dal.Clock.FormatClockOneHour();
    public void FormatClockOneDay() => _dal.Clock.FormatClockOneDay();
    public void FormatClockOneMonth() => _dal.Clock.FormatClockOneMonth();
    public void FormatClockOneYear() => _dal.Clock.FormatClockOneYear();
}
