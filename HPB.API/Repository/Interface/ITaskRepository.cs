using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface ITaskRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<int> DeleteTaskAsync(int id);
        Task<int> UpdateTaskAsync(TaskDto dto);
        ProjectTaskDto[] AllProjectTaskDtoAsQuerable(PagingRequest<ProjectTaskListFilterDto> paging);
        Task<int> InsertProjectTaskAsync(ProjectTaskDto dto);
        ProjectMaintenanceTaskDto[] AllProjectMaintenanceTaskDtoAsQuerable(PagingRequest<ProjectMaintenanceTaskListFilterDto> paging);
        Task<int> InsertProjectMaintenanceTaskAsync(ProjectMaintenanceTaskDto dto);

        Task<DateTime> GetEndDateAsync(string startdate, double duration, int EmpId);
    }
}
