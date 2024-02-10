using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class WorkerInAssignments
{
    public int WorkerId { get; init; }
    public required int AssignmentsNumber { get; init; }
   // public required string AssignmentsName { get; init; }
  //  public DateTime? dateSrart { get; init; }
   // public DateTime? DateBegin { get; init; }
    public override string ToString() => this.ToStringProperty();
}
