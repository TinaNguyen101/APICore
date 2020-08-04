using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class SalaryFilterDto
    {
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public DateTime? ApprovalDateStart { get; set; }
        public DateTime? ApprovalDateEnd { get; set; }
        public string EmpName { get; set; }

    }
}
