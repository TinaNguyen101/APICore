using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualLeavePaidDto
    {

        public int EmpId { get; set; }
        public int DayCurrentYear { get; set; }
        public int DayRemainLastYear { get; set; }

        public int year { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
