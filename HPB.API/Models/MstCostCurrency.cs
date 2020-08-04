using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstCostCurrency
    {
        public MstCostCurrency()
        {
            Project = new HashSet<Project>();
            ProjectMaintenance = new HashSet<ProjectMaintenance>();
        }

        public int Id { get; set; }
        public string CostCurrency { get; set; }
        public string CostCurrencySymboy { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectMaintenance> ProjectMaintenance { get; set; }
    }
}
