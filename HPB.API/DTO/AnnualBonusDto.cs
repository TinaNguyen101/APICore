using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualBonusDto
    {

        public decimal? Bonus { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectMaintenanceId { get; set; }
        public int? RatingFactorId { get; set; }
        public decimal? RatingFactor { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int? Year { get; set; }
        public int? Day { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }



    }
}
