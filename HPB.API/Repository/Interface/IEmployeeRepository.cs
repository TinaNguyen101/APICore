using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<IEnumerable<ApprovedDto>> GetApprovedAsync();
        EmployeeListDto[] AllAsQuerable(PagingRequest<EmployeeListFilterDto> paging);
        Task<IEnumerable<EmployeeMemberDto>> GetByDeptAsync(int deptId);
        Task<EmployeeDto> GetById(long id, string pathFolderImage);
        Task<int> InsertEmployeeAsync(EmployeeDto dto);
        Task<int> UpdateEmployeeAsync(EmployeeDto dto);
        Task<int> DeleteEmployeeAsync(long id);


        Task<EmployeeVehicleReportDto> GetEmployeeVehicleReportAsync(string yearMonth);
        Task<EmployeeVehicleReportDto> GetDownloadEmployeeVehicleReportAsync(IEnumerable<int> empIdString);

        Task<EmployeeReportDto> GetEmployeeReportAsync(string yearMonth);
    }
}
