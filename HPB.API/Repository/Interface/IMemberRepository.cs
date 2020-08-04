using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IMemberRepository
    {
        IDbTransaction Transaction { get; set; }

        Task<int> DeleteMemberAsync(int id);
        Task<int> UpdateProjectMemberAsync(ProjectMemberDto dto);
        Task<int> UpdateProjectMaintenanceMemberAsync(ProjectMaintenanceMemberDto dto);
        Task<IEnumerable<ProjectMemberDto>> GetProjectMemberAsync(int projectId,string pathFolderImage);
        Task<int> CheckprojectExitMemberPosition(long projectId, long empId, long positionId);
        Task<ProjectMemberDto> GetProjectMemberByIdAsync(int id);
        Task<int> InsertMultiProjectMemberAsync(List<ProjectMemberInsertDto> dtolst);
        Task<int> InsertProjectMemberAsync(ProjectMemberDto dto);
        Task<int> DeleteProjectMemberByProjectIDAsync(int projectID);
        Task<IEnumerable<ProjectMaintenanceMemberDto>> GetProjectMaintenanceMemberAsync(int projectId,string pathFolderImage);
        Task<ProjectMaintenanceMemberDto> GetProjectMaintenanceMemberByIdAsync(int id);
        Task<int> CheckprojectMaintenanceExitMemberPosition(long projectMaintenanceId, long empId, long positionId);
        Task<int> InsertMultiProjectMaintenanceMemberAsync(List<ProjectMaintenanceMemberInsertDto> dtolst);
        Task<int> InsertProjectMaintenanceMemberAsync(ProjectMaintenanceMemberDto dto);
        Task<int> DeleteProjectMaintenanceMemberByProjectMaintenanceIDAsync(int projectMaintenanceID);
    }
}
