using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class AnnualBonus
    {
        public int Id { get; set; }
        public decimal? Bonus { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectMaintenanceId { get; set; }
        public int? RatingFactorId { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public double? Day { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? SalaryId { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
