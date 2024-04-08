namespace BO;
public class Worker
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DO.Level Experience { get; set; }
    public int HourSalary { get; set; }
    public override string ToString()
    {
        return Tools.ToStringProperty(this);
    }
    public WorkerInAssignment currentAssignment { get; set; }
}