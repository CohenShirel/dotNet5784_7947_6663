﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class WorkerInList
{
    public int Id { get; init; }
    public required string WorkerNumber { get; set; }
    public required string WorkerName { get; set; }
    public override string ToString() => this.ToStringProperty();
}
