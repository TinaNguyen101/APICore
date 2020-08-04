using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Salary
    {
        public Salary()
        {
            MonthlySalary = new HashSet<MonthlySalary>();
        }

        public int Id { get; set; }
        public decimal? Salary1 { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? EmpId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual ICollection<MonthlySalary> MonthlySalary { get; set; }
    }
}
