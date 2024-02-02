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

    //we had it on DO
    //DateTime? BirthDate { get; set; }
    // public DateTime RegistrationDate { get; init; }

    //אתגררררררררררררר
    public override string ToString()
    {
       return Tools.ToStringProperty(this);
    }

    //החזרה של משימה נוכחית עי שאילתא
    public Tuple<int, string>? Assignment { get; set; }

    //public bool IsActive { get; set; }
    //DateTime? BirthDate { get; set; }
    //public DateTime RegistrationDate { get; init; }
    //Year CurrentYear { get; init; } // (Year)(DateTime.Now.Year - RegistrationDate.Year);

}
//int IdWorker,///////////////////////////////
//    Level Experience,
//    int HourSalary,
//    string? Name = null,////////////////////
//    string? Email = null//////////////////
