namespace DalApi;
public interface IClock
{
    public DateTime? SetStartProject(DateTime startProject);
    public DateTime? GetStartProject();
    public void FormatClockOneHour();
    public void FormatClockOneDay();
    public void FormatClockOneMonth();
    public void FormatClockOneYear();
    public void resetClock();
}

