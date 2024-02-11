using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class AssignmentsInList
{
//    העבודה לתחילת מתוכנן תאריך = "DateBegin" ></ param >
///// <param לסיום מתוכנן תאריך="DeadLine"></param>
    public int Id { get; init; }
    //public required string AssignmentNumber { get; set;}//??האם צריך גם את התז שלו או מספיק רק שם
    public required string AssignmentName { get; set; }
    //public int DurationAssignments { get; set; }
   // public DO.Level LevelAssignments { get; set;}
     public DateTime? DeadLine { get; set; }
    public DateTime? DateBegin { get; set; }
    public Status status { get; set; }
      public int IdWorker { get; set; }
    public override string ToString() => this.ToStringProperty();
}
