using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MonthlyBonus
    {
        public int Id { get; set; }
        public decimal? Wage { get; set; }
        public int? ImageUnitPriceId { get; set; }
        public int? EmpId { get; set; }
        public int? TotalImageBonus { get; set; }
        public int? YearMonth { get; set; }
        public string Comment { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual MstImageUnitPrice ImageUnitPrice { get; set; }
    }
}
