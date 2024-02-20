namespace BO;

public class WorkerInAssignment
{
    public int WorkerId { get; init; }
    public required int AssignmentNumber { get; init; }
    // public required string AssignmentName { get; init; }
    //  public DateTime? dateSrart { get; init; }
    // public DateTime? DateBegin { get; init; }
    public override string ToString() => this.ToStringProperty();
}
