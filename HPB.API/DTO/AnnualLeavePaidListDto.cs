using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualLeavePaidListDto
    {
        public int? No { get; set; }
        public int? EmpId { get; set; }

        public string EmpName { get; set; }
        public int year { get; set; }
        public decimal DayCurrentYear { get; set; }
        public decimal DayRemainLastYear { get; set; }

        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }

        public decimal TotalDay { get; set; }

        public DateTime? EmpStartDate { get; set; }
        public DateTime? EmpEndDate { get; set; }

    }
}
