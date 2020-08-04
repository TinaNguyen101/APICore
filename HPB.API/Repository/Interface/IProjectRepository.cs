using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IProjectRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<IEnumerable<ProjectFinishListDto>> ProjectFinishAsync(int year);
        ProjectListDto[] AllProjectListDtoAsQuerable(PagingRequest<ProjectListFilterDto> paging);
        Task<IEnumerable<Project>> AllAsync();
        Task<IEnumerable<ProjectListDto>> AllProjectListDtoAsync();
        Task<ProjectBasicDto> GetProjectBasicDtoByIdAsync(long projectId);
        Task<int> InsertProjectAsync(ProjectBasicDto projectBasicDto);
        Task<int> UpdateProjectAsync(ProjectBasicDto projectBasicDto);
        Task<int> HardDeleteProjectAsync(long id);


        Task<IEnumerable<ProjectReportDto>> GetProjectReportAsync(string yearMonth, string year, string status, int rateYen, int rateUSD);
        Task<IEnumerable<ProjectReportStatisticsDto>> GetProjectReportStatistics(string year, string yearMonthStart, string yearMonthEnd, int rateYen, int rateUSD);
        Task<IEnumerable<ProjectReportStatisticsByCustDto>> GetProjectReportStatisticsByCust(string custId, string year, int rateYen, int rateUSD);

    }
}
