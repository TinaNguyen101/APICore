using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class ProjectMemberDto
    {

        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? EmpId { get; set; }
        public int? ProjectPositionId { get; set; }
        public string EmpName { get; set; }
        public string ProjectPositionName { get; set; }
        public string StyleCss { get; set; }

        public string EmpMobile1 { get; set; }
        public string EmpEmail1 { get; set; }
        public string EmpImage { get; set; }

        public int? EmpGender { get; set; }
    }
}
