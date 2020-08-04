using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualRatingFactorListDto
    {
        public decimal? RatingFactorMember { get; set; }
        public decimal? RatingFactorLeader { get; set; }
        public decimal? lastRatingFactorMember { get; set; }
        public decimal? lastRatingFactorLeader { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int year { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
