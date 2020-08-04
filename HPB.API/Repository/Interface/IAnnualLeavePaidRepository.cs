using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IAnnualLeavePaidRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<int> checkYearAnnualLeavePaid(int year);
        Task<IEnumerable<AnnualLeavePaidListDto>> AllAnnualLeavePaidAsync(int year);
        Task<int> InsertAnnualLeavePaidAsync(int year);
        Task<int> GetMaxYear(int year, int empId);
        Task<int> UpdateDayRemainAsync(int year, int empId, int maxYear);
        Task<IEnumerable<DayOffDto>> GetAllDayOffAsync(string yearMonth);
        Task<DayOffDto> GetDayOffByIdAsync(int id);
        Task<int> InsertDayOffAsync(DayOffDto dto);
        Task<int> UpdateDayOffAsync(DayOffDto dto);
        Task<int> DeleteDayOffAsync(int id);

        Task<AnnualLeavePaidReportDto> GetAnnualLeavePaidReportAsync(string yearMonth);

    }
}
