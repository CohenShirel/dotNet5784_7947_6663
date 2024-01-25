using BO;

namespace BlApi;
public interface IBl
{
    public IWorker Student { get; }
    public IAssignments Course { get; }
    public IWorkerInAssignments GradeSheet { get; }
    public ISchedule Schedule { get; }
}

