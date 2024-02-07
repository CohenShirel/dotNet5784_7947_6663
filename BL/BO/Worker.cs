using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
public class Worker
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DO.Level Experience { get; set;}
    public int HourSalary { get; set;}

    //אתגררררררררררררר
    public override string ToString()
    {
       return Tools.ToStringProperty(this);
    }

    //החזרה של משימה נוכחית עי שאילתא
    public Assignments currentAssignment { get; set; }

    //public Tuple<int, string>? currentAssignment { get; set; }
}

//public bool IsActive { get; set; }
//DateTime? BirthDate { get; set; }
//public DateTime RegistrationDate { get; init; }
//Year CurrentYear { get; init; } // (Year)(DateTime.Now.Year - RegistrationDate.Year);

//we had it on DO
//DateTime? BirthDate { get; set; }
// public DateTime RegistrationDate { get; init; }