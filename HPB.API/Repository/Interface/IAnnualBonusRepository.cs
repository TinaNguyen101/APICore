using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IAnnualBonusRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<decimal> GetBonusAsync(decimal day, int empId, int year);
        Task<IEnumerable<int>> GetAllYearAnnualBonusAsync();
        Task<IEnumerable<AnnualBonusListByEmpDto>> GetAllAnnualBonusAsync(int year, int empId);
        Task<int> checkExistAnnualBonusAsync(int year);
        Task<int> InsertAnnualBonusAsync(AnnualBonusDto dto);
        Task<int> UpdateAnnualBonusAsync(AnnualBonusDto dto);
        Task<int> DeleteAnnualBonusAsync(int id);


        Task<IEnumerable<int>> GetAllYearAnnualRatingFactorAsync();
        Task<IEnumerable<AnnualRatingFactorListDto>> GetAllAnnualRatingFactorAsync(int year);
        Task<int> checkExistAnnualRatingFactorAsync(int year);
        Task<int> InsertAnnualRatingFactorAsync(IEnumerable<AnnualRatingFactorListDto> dto);

        Task<int> UpdateAnnualRatingFactorAsync(IEnumerable<AnnualRatingFactorListDto> dto);

        Task<IEnumerable<AnnualBonusListDto>> GetAllAnnualBonusAsync(int year, int rateYen, int rateUSD);
        Task<IEnumerable<MemberBonusDto>> GetListMemberBonusAsync(int year);
        Task<AnnualBonusReportDto> GetBonusReportAsync(int year, int rateYen, int rateUSD, int empId);


    }
}
