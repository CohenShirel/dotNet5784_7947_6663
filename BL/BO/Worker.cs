using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
public class Student
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    DateTime? BirthDate { get; set; }
    public DateTime RegistrationDate { get; init; }
    Year CurrentYear { get; init; } // (Year)(DateTime.Now.Year - RegistrationDate.Year);
    //אתגררררררררררררר
    //public override string ToString() => this.ToStringProperty();
}
//int IdWorker,///////////////////////////////
//    Level Experience,
//    int HourSalary,
//    string? Name = null,////////////////////
//    string? Email = null//////////////////