using DO;
namespace BO;
public class WorkerInAssignment
{
    public int WorkerId { get; init; }
    public required int AssignmentNumber { get; init; }
    public required string AssignmentName { get; init; }
    public override string ToString() => this.ToStringProperty();
}


