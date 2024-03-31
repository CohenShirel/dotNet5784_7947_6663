namespace BO;
public class Worker
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DO.Level Experience { get; set; }
    public int HourSalary { get; set; }

    //אתגרררררררררררר
    public override string ToString()
    {
        return Tools.ToStringProperty(this);
    }

    //החזרה של משימה נוכחית עי שאילתא
    public WorkerInAssignment currentAssignment { get; set; }
}

//public bool IsActive { get; set; }
//DateTime? BirthDate { get; set; }
//public DateTime RegistrationDate { get; init; }
//Year CurrentYear { get; init; } // (Year)(DateTime.Now.Year - RegistrationDate.Year);

//we had it on DO
//DateTime? BirthDate { get; set; }
// public DateTime RegistrationDate { get; init; }