using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstPosition
    {
        public MstPosition()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string PositionName { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
