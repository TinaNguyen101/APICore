using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualBonusListDto
    {

        public int? WorkDayMonth { get; set; }
        public int? rateYen { get; set; }
        public int? rateUSD { get; set; }
        public string EmpName { get; set; }
        public decimal? RatingFactorMember { get; set; }
        public decimal? TotalProjectMember { get; set; }
        public decimal? TotalDayMember { get; set; }
        public decimal? BonusMember { get; set; }

        public decimal? RatingFactorLeader { get; set; }
        public decimal? TotalProjectLeader { get; set; }
        public decimal? TotalEstimateCostVNDLeader { get; set; }
        public decimal? BonusLeader { get; set; }

        public decimal? TotalBonus { get; set; }
       


    }
    public class AnnualBonusListByEmpDto
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public int? RatingFactorId { get; set; }
        public decimal? RatingFactor { get; set; }
        public int? Year { get; set; }
        public string ProName { get; set; }
        public int? ProId { get; set; }
        public int? Day { get; set; }
        public decimal? Bonus { get; set; }



    }
}
