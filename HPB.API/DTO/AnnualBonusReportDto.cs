using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualBonusReportDto
    {
        public IEnumerable<AnnualBonusMEMBERDtoDetailDto> AnnualBonusMEMBERDtoDetailDto { get; set; }
        public AnnualBonusMEMBERDtoTotalDto AnnualBonusMEMBERDtoTotalDto { get; set; }

        public IEnumerable<AnnualBonusLEADERDetailDto> AnnualBonusLEADERDetailDto { get; set; }
        public AnnualBonusLEADERTotalDto AnnualBonusLEADERTotalDto { get; set; }
    }
    public class AnnualBonusMEMBERDtoDetailDto
    {
        public int? No { get; set; }
        public string ProjectName { get; set; }
        public decimal? Day { get; set; }
        public decimal? Bonus { get; set; }
        public int? EmpId { get; set; }
        public int year { get; set; }

    }
    public class AnnualBonusMEMBERDtoTotalDto
    {
        public int? DayWorkInMonth { get; set; }
        public string RatingFactor { get; set; }
        public decimal? Salary { get; set; }
        public decimal? TotalBonus { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int year { get; set; }

    }

    public class AnnualBonusLEADERDetailDto
    {
        public int? No { get; set; }
        public string ProjectName { get; set; }
        public decimal? Estimate { get; set; }
        public int? EstimateCostCurrencyId { get; set; }
        public decimal? Bonus { get; set; }

        public int? EmpId { get; set; }
        public int year { get; set; }

    }
    public class AnnualBonusLEADERTotalDto
    {
        public decimal? RateYen { get; set; }
        public decimal? RateUSD { get; set; }
        public string RatingFactorLeader { get; set; }
      
        
        public decimal? TotalBonus { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int year { get; set; }

    }

    public partial class MemberBonusDto
    {

        public int? EmpId { get; set; }
        public string EmpName { get; set; }



    }
}
