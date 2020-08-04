using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectMemberInsertDto
    {

        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public int ProjectPositionId { get; set; }


    }
}
