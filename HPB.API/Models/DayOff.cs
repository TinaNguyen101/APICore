using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class DayOff
    {
        public int Id { get; set; }
        public DateTime? DayOff1 { get; set; }
        public string Reason { get; set; }
        public int? TimeOff { get; set; }
        public int? ApprovedBy { get; set; }
        public int? EmpId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
