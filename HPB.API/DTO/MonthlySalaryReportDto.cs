using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlySalaryReportDto
    {
        public IEnumerable<MonthlySalaryDetailReportDto> detail { get; set; }
        public MonthlySalaryTotalReportDto total { get; set; }

    }

    public class MonthlySalaryDetailReportDto
    {

        public int No { get; set; }
        public int? TotalDayOff { get; set; }
        public string EmpName { get; set; }
        public string PositionName { get; set; }
        public decimal? Salary { get; set; }
        public decimal? BonusOt { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? ActualSalary { get; set; }
        public int YearMonth { get; set; }
        public string Comment { get; set; }
    }

    public class MonthlySalaryTotalReportDto
    {

        public decimal? TotalSalary { get; set; }
        public decimal? TotalBonusOt { get; set; }
        public decimal? TotalAllowance { get; set; }
        public decimal? TotalDeduction { get; set; }
        public decimal? TotalActualSalary { get; set; }
    }
}
