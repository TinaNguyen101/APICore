using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstDepartment
    {
        public MstDepartment()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
