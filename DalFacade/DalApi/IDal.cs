using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IWorker Worker { get; }
        IAssignments Assignments { get; }
        ILink Link { get; }
        // ICrud crd { get; }
        public DateTime? StartProjectTime { get; set; }
        //public DateTime? DeadLine { get; set; }
    }
}
