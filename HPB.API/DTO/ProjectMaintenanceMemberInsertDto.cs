using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectMaintenanceMemberInsertDto
    {

        public int ProjectMaintenanceId { get; set; }
        public int EmpId { get; set; }
        public int ProjectPositionId { get; set; }


    }
}
