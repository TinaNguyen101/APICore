using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IProjectMaintenanceRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<IEnumerable<ProjectFinishListDto>> ProjectFinishAsync(int year);
        ProjectMaintenanceDto[] GetProjectMaintenance(long projectId, PagingRequest<ProjectMaintenanceFilterDto> paging);
        Task<IEnumerable<ProjectMaintenanceDto>> GetProjectMaintenanceAsync(long projectId);
        Task<ProjectMaintenanceDto> GetProjectMaintenanceByIdAsync(long id);
        Task<int> InsertProjectMaintenanceAsync(ProjectMaintenanceDto dto);
        Task<int> UpdateProjectMaintenanceAsync(ProjectMaintenanceDto dto);
        Task<int> DeleteProjectMaintenanceAsync(long id);
        Task<int> DeleteProjectMaintenanceByProjectIDAsync(long projectID);

    }
}
