namespace BO;

internal class Schedule
{
    public int WorkertId { get; init; }
    public string? WorkerName { get; init; }
    public List<WorkerInAssignment>? Assignment { get; init; } = null;
    public override string ToString() => this.ToStringProperty();
}
