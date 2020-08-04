using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Duration { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectMaintenanceId { get; set; }
        public int? EmpId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectMaintenance ProjectMaintenance { get; set; }
    }
}
