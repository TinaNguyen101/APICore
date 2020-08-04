using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class MonthlyBonusDto
    {

        public int Id { get; set; }
        public decimal? Wage { get; set; }
        public int? ImageUnitPriceId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string ImageType { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int? TotalImageBonus { get; set; }
        public int? YearMonth { get; set; }
        public string Comment { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
