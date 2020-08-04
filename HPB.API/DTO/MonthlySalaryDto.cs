using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlySalaryDto
    {

        public int? SalaryId { get; set; }
        public decimal? BonusOt { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Deduction { get; set; }
        public int YearMonth { get; set; }
        public int EmpId { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
