using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstProjectStatus
    {
        public MstProjectStatus()
        {
            Project = new HashSet<Project>();
            ProjectMaintenance = new HashSet<ProjectMaintenance>();
        }

        public int Id { get; set; }
        public string ProjectStatusName { get; set; }
        public string StyleCss { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectMaintenance> ProjectMaintenance { get; set; }
    }
}
