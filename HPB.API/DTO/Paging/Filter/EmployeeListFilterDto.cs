using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class EmployeeListFilterDto
    {

        public string EmpName { get; set; }
        public string EmpAddress { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }
        public int? EmpGender { get; set; }
        public int? EmpStatusId { get; set; }
        public int? DeptId { get; set; }
    }
}
