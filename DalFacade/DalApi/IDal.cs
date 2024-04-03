namespace DalApi
{
    public interface IDal
    {
        IWorker Worker { get; }
        IAssignments Assignment { get; }
        ILink Link { get; }
        IClock Clock { get; }
        // ICrud crd { get; }
       // public DateTime? StartProjectTime { get; set; }
        //public DateTime? DeadLine { get; set; }
    }
}
