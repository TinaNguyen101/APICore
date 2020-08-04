using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IAttachmentFileRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<IEnumerable<ProjectAttachmentFileDto>> GetProjectAttachmentFileNameAsync(ProjectAttachmentFileDto dto);
        Task<IEnumerable<ProjectMaintenanceAttachmentFileDto>> GetProjectMaintenanceAttachmentFileNameAsync(ProjectMaintenanceAttachmentFileDto dto);
        Task<ProjectAttachmentFileDto> GetAttachmentFileByIdAsync(long id);
        Task<int> DeleteAttachmentFileAsync(int id);
        Task<IEnumerable<ProjectAttachmentFileDto>> GetProjectAttachmentFileAsync(long projectId);
        Task<int> InsertProjectAttachmentFileAsync(ProjectAttachmentFileDto dto);
        Task<int> DeleteProjectAttachmentFileAsync(long projectId);
        Task<IEnumerable<ProjectMaintenanceAttachmentFileDto>> GetProjectMaintenanceAttachmentFileAsync(long ProjectMaintenanceId);
        Task<int> InsertProjectMaintenanceAttachmentFileAsync(ProjectMaintenanceAttachmentFileDto dto);
        Task<int> DeleteProjectMaintenanceAttachmentFileAsync(long projectMaintenanceId);
        Task<IEnumerable<EmployeeAttachmentFileDto>> GetEmployeeAttachmentFileAsync(long EmpId);
        Task<int> InsertEmployeeAttachmentFileAsync(EmployeeAttachmentFileDto dto);
        Task<int> DeleteEmployeeAttachmentFileAsync(long empId);
        Task<IEnumerable<EmployeeAttachmentFileDto>> GetEmployeeAttachmentFileNameAsync(EmployeeAttachmentFileDto dto);



    }
}
