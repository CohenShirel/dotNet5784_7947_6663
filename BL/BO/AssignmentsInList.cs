namespace BO;

public class AssignmentInList
{
    //    העבודה לתחילת מתוכנן תאריך = "DateBegin" ></ param >
    ///// <param לסיום מתוכנן תאריך="DeadLine"></param>
    public int Id { get; init; }
    public required string AssignmentName { get; set; }
    public DateTime? DeadLine { get; set; }
    public DateTime? DateBegin { get; set; }
    public Status status { get; set; }
    public int IdWorker { get; set; }
    public DO.Level LevelAssignment { get; set; }
    public override string ToString() => this.ToStringProperty();
}
