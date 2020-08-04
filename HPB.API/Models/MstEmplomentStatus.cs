using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstEmplomentStatus
    {
        public MstEmplomentStatus()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string EmpStatus { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
