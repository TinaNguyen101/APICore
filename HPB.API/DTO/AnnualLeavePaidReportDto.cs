using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualLeavePaidReportDto
    {

        public IEnumerable<AnnualLeavePaidDayOffDto> AnnualLeavePaidDayOffDto { get; set; }

        public IEnumerable<AnnualLeavePaidResultDto> AnnualLeavePaidResultDto { get; set; }

        public IEnumerable<AnnualLeavePaidTotalResultDto> AnnualLeavePaidTotalResultDto { get; set; }

    }
    public class AnnualLeavePaidDayOffDto
    {

        public DateTime? DayOff { get; set; }
        public decimal CountDayOff { get; set; }
        public int EmpId { get; set; }
        public DateTime? Holiday { get; set; }


    }
    public class AnnualLeavePaidResultDto
    {

        public int No { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }
        public decimal DayRemainLast { get; set; }
        public decimal CountDayOff { get; set; }
        public decimal TotalDay { get; set; }


    }

    public class AnnualLeavePaidTotalResultDto
    {
        public DateTime? DayOff { get; set; }
        public decimal TotalDayOff { get; set; }
    }
}
