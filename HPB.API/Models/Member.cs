using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? EmpId { get; set; }
        public int? ProjectPositionId { get; set; }
        public int? ProjectMaintenanceId { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectMaintenance ProjectMaintenance { get; set; }
        public virtual MstProjectPosition ProjectPosition { get; set; }
    }
}
