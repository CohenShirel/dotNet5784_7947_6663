using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Gannt;

internal class AssgnmentsSchedule
{
    public int AssignmentId { get; set; }
    public string? AssignmentName { get; set; }
    public int WorkerId { get; set; }
    public string? WorkerName { get; set;}
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public BO.Status AssignmentStatus { get; set; }
    public List<int>? DependentAssignment {  get; set; }
}
