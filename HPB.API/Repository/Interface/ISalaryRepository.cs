using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface ISalaryRepository
    {
        IDbTransaction Transaction { get; set; }
        SalaryListDto[] GetAllSalaryAsQuerable(PagingRequest<SalaryFilterDto> paging);
        Task<SalaryListDto> GetSalaryByIdAsync(long id);
        Task<int> DeleteSalaryAsync(int id);
        Task<int> UpdateSalaryAsync(SalaryListDto dto);
        Task<int> InsertSalaryAsync(SalaryListDto dto);

        Boolean CheckExistsMonthlySalary(string yearMonth);
        MonthlySalaryListDto[] GetAllMonthlySalaryAsQuerable(PagingRequest<MonthlySalaryFilterDto> paging);
        Task<int> InsertMonthlySalaryAsync(string YearMonth);
        Task<int> UpdateMonthlySalaryAsync(List<MonthlySalaryListDto> lst);
        Task<int> UpdateBonusOTAsync(int empId, string yearMonth);
        Task<int> DeleteMonthlySalaryAsync(string YearMonth);

        Task<decimal> GetWageBonusImage(int imageTypeId, int totalImage);
        Task<int> GetTotalImageBonus(int totalImageInMonth);
        Task<IEnumerable<MonthlyBonusDto>> GetAllMonthlyBonusAsync(string yearMonth);
        Task<MonthlyBonusDto> GetMonthlyBonusByIdAsync(int id);
        Task<int> InsertMonthlyBonusAsync(MonthlyBonusDto dto);
        Task<int> UpdateMonthlyBonusAsync(MonthlyBonusDto dto);
        Task<int> DeleteMonthlyBonusAsync(int id);


        Task<decimal> GetWageOvertime(int empId, decimal timeOT, DateTime dateOT);
        Task<IEnumerable<MonthlyOTListDto>> GetAllMonthlyOTAsync(string yearMonth);
        Task<MonthlyOTDto> GetMonthlyOTByIdAsync(int id);
        Task<int> InsertMonthlyOTAsync(MonthlyOTDto dto);
        Task<int> UpdateMonthlyOTAsync(MonthlyOTDto dto);
        Task<int> DeleteMonthlyOTAsync(int id);

        Task<MonthlySalaryReportDto> GetMonthlySalaryReportAsync(string yearMonth);

    }
}
