﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class WorkerInList
{
   // WorkerInList WorkerInList();
    //האם צריך לעשות?
    public int Id { get; init; }
    public required string Name { get; set; }
    public DO.Level Experience { get; set; }
    public required WorkerInAssignments currentAssignment { get; set; }

    public override string ToString() => this.ToStringProperty();
}
