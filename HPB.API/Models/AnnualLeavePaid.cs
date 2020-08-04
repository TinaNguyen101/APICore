using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class AnnualLeavePaid
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int DayCurrentYear { get; set; }
        public int? DayRemainLastYear { get; set; }
        public int? Year { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
