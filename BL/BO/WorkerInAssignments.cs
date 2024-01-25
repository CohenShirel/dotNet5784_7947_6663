using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class WorkerInAssignments
{
    public int WorkerId { get; init; }
    public required string AssignmentsNumber { get; init; }
    public required string AssignmentsName { get; init; }
    public DateOnly? dateSrart { get; init; }
    public DateOnly? DateBegin { get; init; }
    public override string ToString() => this.ToStringProperty();
}
