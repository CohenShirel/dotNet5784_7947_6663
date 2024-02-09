using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
public class Assignments
{
    public int IdAssignments { get; init;}
    public int DurationAssignments { get; set; }
    public DO.Level LevelAssignments { get; set; }
    public int IdWorker { get; set; }
    public DateTime ? dateSrart { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DeadLine { get; set; }//calculate the end of the project
    public DateTime? DateFinish { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Remarks { get; set; }
    public string? ResultProduct { get; set; }
    //אתגררררררררררררר//
    public override string ToString()
    {
        return Tools.ToStringProperty(this);
    }
    public Status status { get; set; }
    //רשימת תלויות??????????????מסוג משימה ברשימה
    public List<Assignments>? links { get; set; }//link ssignmentsInList?
    //החזרת עובד נוכחי עי שאילתא
    public Tuple <int,string>? Worker { get; set; }
}


//int IdAssignments,///////////////////////////////////////////
//    int DurationAssignments,//////////////////////////////////
//  Level LevelAssignments,
//    int IdWorker,
//  DateTime? dateSrart=null,
//  DateTime? DateBegin = null,
//  DateTime? DeadLine = null,
//  DateTime? DateFinish = null,
//    string? Name = null,
//    string? Description = null,
//    string? Remarks = null,
//    string? ResultProduct = null,
//    bool Milestone = false
//public class Worker
//{
//    public int Id { get; init; }
//    public string? Name { get; set; }
//    public string? Email { get; set; }
//    Level Experience { get; set; }
//    int HourSalary { get; set; }
//    //we had it on DO
//    DateTime? BirthDate { get; set; }
//    public DateTime RegistrationDate { get; init; }

//    //אתגררררררררררררר
//    public override string ToString()
//    {
//        return " ";
//        //return ToolStringClass.ToStringProperty(this);
//    }

//    //public bool IsActive { get; set; }
//    //DateTime? BirthDate { get; set; }
//    //public DateTime RegistrationDate { get; init; }
//    //Year CurrentYear { get; init; } // (Year)(DateTime.Now.Year - RegistrationDate.Year);

//}
//int IdWorker,///////////////////////////////
//    Level Experience,
//    int HourSalary,
//    string? Name = null,////////////////////
//    string? Email = null//////////////////


//public record StudentA
//{
//    Id { get; init; }
//  Name { get; init; }
//    Alias { get; init; } = null;
//   IsActive { get; init; } = false;
//   CurrentYear { get; init; } = Year.FirstYear;
//}

