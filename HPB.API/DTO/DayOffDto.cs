using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class DayOffDto
    {

        public int Id { get; set; }
        public DateTime? DayOff1 { get; set; }
        public string Reason { get; set; }
        public int? TimeOff { get; set; }
        public int? ApprovedBy { get; set; }
        public string ApprovedName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? EmpId { get; set; }

        public string EmpName { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
