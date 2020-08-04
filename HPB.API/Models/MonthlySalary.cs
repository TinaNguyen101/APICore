using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MonthlySalary
    {
        public int Id { get; set; }
        public int? SalaryId { get; set; }
        public decimal? BonusOt { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Deduction { get; set; }
        public int YearMonth { get; set; }
        public int EmpId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual Salary Salary { get; set; }
    }
}
