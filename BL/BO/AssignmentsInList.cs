﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class AssignmentsInList
{
    public int Id { get; init; }
    //public required string AssignmentNumber { get; set;}//??האם צריך גם את התז שלו או מספיק רק שם
    public required string AssignmentName { get; set; }
    public Level LevelAssignments { get; set;}
    public Status status { get; set; }
    //public DateOnly? dateSrart {get; init;}
    //public DateOnly? DateBegin {get; init;}
    public override string ToString() => this.ToStringProperty();


}
