using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlySalaryFilterDto
    {
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public string EmpName { get; set; }
        public int yearMonth { get; set; }

    }
}
