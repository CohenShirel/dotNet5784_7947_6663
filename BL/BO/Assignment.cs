namespace BO;
public class Assignment
{
    public int IdAssignment { get; set; }
    public int DurationAssignment { get; set; }
    public DO.Level LevelAssignment { get; set; }
    public int IdWorker { get; set; }
    public DateTime? dateSrart { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DeadLine { get; set; }
    public DateTime? DateFinish { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Remarks { get; set; }
    public string? ResultProduct { get; set; }
    public override string ToString()
    {
        return Tools.ToStringProperty(this);
    }
    public Status status { get; set; }
    public List<AssignmentInList>? Links { get; set; }
    public Tuple<int, string>? Worker { get; set; }
}