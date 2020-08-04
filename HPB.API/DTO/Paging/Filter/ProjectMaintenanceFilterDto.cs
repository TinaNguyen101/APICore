using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectMaintenanceFilterDto
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MaintenanceName { get; set; }

        public string ProjectStatusName { get; set; }

    }
}
