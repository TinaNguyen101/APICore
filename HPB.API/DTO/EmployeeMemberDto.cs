using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class EmployeeMemberDto
    {
        public int Id { get; set; }

        public string EmpName { get; set; }

        public string EmpMobile1 { get; set; }
        public string EmpMobile2 { get; set; }
        public string EmpEmail1 { get; set; }
        public string EmpEmail2 { get; set; }

        public string EmpImage { get; set; }

        public int? EmpGender { get; set; }

        public int? EmpStatusId { get; set; }
        public string EmpStatus { get; set; }

        public int? PosId { get; set; }
        public string PositionName { get; set; }

    }
}
