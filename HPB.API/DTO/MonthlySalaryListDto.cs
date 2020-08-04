using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlySalaryListDto
    {

        public int Id { get; set; }
        public int? SalaryId { get; set; }
        public decimal? Salary { get; set; }
        public decimal? BonusOt { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Deduction { get; set; }
        public int YearMonth { get; set; }
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
