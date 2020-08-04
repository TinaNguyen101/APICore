using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class SalaryListDto
    {

        public int Id { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? EmpId { get; set; }

        public string EmpName { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int isEdit { get; set; }
    }
}
