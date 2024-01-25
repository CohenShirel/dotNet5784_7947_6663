using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

internal class Schedule
{
    public int WorkertId { get; init; }
    public string? WorkerName { get; init; }
    public List<WorkerInAssignments>? Courses { get; init; } = null;
    public override string ToString() => this.ToStringProperty();
}
