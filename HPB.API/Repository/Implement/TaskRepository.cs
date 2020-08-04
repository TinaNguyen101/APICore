using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HPB.API.Models;
using HPB.API.Repository.Interface;
using HPB.API.DTO;

namespace HPB.API.Repositories.Implement
{
    internal class TaskRepository : RepositoryBase, ITaskRepository
    {
        public TaskRepository()
        {

        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var sql = " SELECT Task.Id , TaskName,StartDate,EndDate, Duration,ProjectId,ProjectMaintenanceId,EmpId,EmpName " +
                ",Task.CreatedId,Task.CreatedDate,Task.UpdatedId,Task.UpdatedDate  " +
                      " FROM Task  " +
                      " LEFT JOIN  Employee on Employee.Id = Task.EmpId " +
                      " where Task.Id = @Id";
            return await SqlMapper.QueryFirstOrDefaultAsync<TaskDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }

        public async Task<DateTime> GetEndDateAsync(string startdate ,double duration,int EmpId)
        {
            double _doubleValue = 0;
            bool isInt = duration == (int)duration;
            if (!isInt)
            {
                if (duration > 1)
                {
                    
                    var temp = (int)duration;
                    _doubleValue = duration - temp;
                    // duration = temp + 1;
                    duration = temp;
                }
                else
                    duration = 1;
            }
            duration = duration - 1;
            var basicDuration = duration;
            var sql = " SELECT Holiday as Holiday1 ,Id " +
                      " FROM Holiday  ";
            var lstHoliday = await SqlMapper.QueryAsync<Holiday>(Connection, sql, transaction: Transaction);
           
            var sql1 = " SELECT DayOff as DayOff1,TimeOff " +
                     " FROM DayOff Where EmpId = @empId ";
            var lstDayOff = await SqlMapper.QueryAsync<DayOffDto>(Connection, sql1,new { empId  = EmpId } ,transaction: Transaction);

            var _tempSDate =  new DateTime(Convert.ToInt32(startdate.Substring(0, 4)), Convert.ToInt32(startdate.Substring(5, 2)), Convert.ToInt32(startdate.Substring(8, 2)));
            duration = GetDuration(basicDuration, duration, _tempSDate, _tempSDate.AddDays(duration), lstHoliday.ToList(), lstDayOff.ToList(), _doubleValue);
           
            var endDate = _tempSDate.AddDays(duration);
            return endDate;
        }
        private double GetDuration(double basicDuration, double duration, DateTime currentStartDate, DateTime currentEndDate,List<Holiday> lstCurrentHoliday, List<DayOffDto> lstDayOff, double doubleValue)
        {
            var logDuration = basicDuration;
            for (DateTime i = currentStartDate; i <= currentEndDate; i = i.AddDays(1))
            {
                var dayoff = lstDayOff.Where(x => x.DayOff1 == i).FirstOrDefault();
                if (lstDayOff.Any(x=>x.DayOff1 == i))
                {
                    var temp = (double)dayoff.TimeOff / 8;
                    bool isInt = temp == (int)temp;
                    if (doubleValue != 0)
                    {
                       
                        if (!isInt)
                        {
                            logDuration = logDuration+ doubleValue + temp;
                        }
                        else
                        {
                            logDuration = logDuration + (double)Math.Round(doubleValue, MidpointRounding.AwayFromZero) + temp;
                        }

                    }
                    else {
                        logDuration = logDuration + (double)Math.Round(temp, MidpointRounding.AwayFromZero);
                    }
                    
                }
                if (lstCurrentHoliday.Any(x => x.Holiday1 == i))
                {
                    logDuration = logDuration + 1;
                }
                else if (i.DayOfWeek == DayOfWeek.Saturday)
                {
                    logDuration = logDuration + 1;
                }
                else if (i.DayOfWeek == DayOfWeek.Sunday)
                {
                    logDuration = logDuration + 1;
                }
                if (i == currentEndDate && currentEndDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    logDuration = logDuration + 1;
                }
            }
            if(logDuration != duration)
            { 
                return GetDuration(basicDuration, logDuration, currentStartDate, currentStartDate.AddDays(logDuration), lstCurrentHoliday, lstDayOff, doubleValue);
            }
            return duration;
        }

        private int GetDuration(double duration, DateTime currentStartDate, DateTime dateTime, List<DateTime> lstCurrentHoliday)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            var sql = " DELETE [dbo].[Task] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }
        public async Task<int> UpdateTaskAsync(TaskDto dto)
        {
            var sql = " UPDATE [dbo].[Task] " +
                     "  SET [TaskName] = @TaskName " +
                     "     ,[StartDate] = @StartDate " +
                     "     ,[EndDate] = @EndDate " +
                      "     ,[EmpId] = @EmpId " +
                     "     ,[Duration] = @Duration " +
                        "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  Task.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                TaskName = dto.TaskName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Duration = dto.Duration,
                UpdatedId = dto.UpdatedId,
                EmpId = dto.EmpId
            }, Transaction);
            return x;
        }


        #region project task

        public ProjectTaskDto[] AllProjectTaskDtoAsQuerable(PagingRequest<ProjectTaskListFilterDto> paging)
        {
            var filterDto = paging.Filters;
            var sql = " SELECT Task.Id , TaskName,Task.StartDate,Task.EndDate, Duration,ProjectId,ProjectName,EmpId,EmpName" +
                ",Task.CreatedId,Task.CreatedDate,Task.UpdatedId,Task.UpdatedDate  " +
                      " FROM Task " +
                      " LEFT JOIN  Employee on Employee.Id = Task.EmpId " +
                      " LEFT JOIN  Project on Project.Id = Task.ProjectId " +
                      " where ProjectId = @ProjectId" ;
            var _dto = SqlMapper.Query<ProjectTaskDto>(Connection, sql,new { ProjectId = filterDto.ProjectId }, transaction: Transaction);
            var query = _dto;

            //search condition
            
            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.TaskName))
                {
                    query = query.Where(emp => emp.TaskName.Contains(filterDto.TaskName));
                }
                if (filterDto.StartDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate >= filterDto.StartDate);
                }
                if (filterDto.EndDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate <= filterDto.EndDate);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(ProjectTaskDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<ProjectTaskDto>();
        }

        public async Task<int> InsertProjectTaskAsync(ProjectTaskDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[Task] " +
                    " ([TaskName] " +
                    " ,[StartDate] " +
                    " ,[EndDate]" +
                    " ,[Duration]" +
                    " ,[ProjectId] " +
                    " ,[EmpId] " +
                      " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@TaskName" +
                    " ,@StartDate" +
                    " ,@EndDate" +
                    " ,@Duration" +
                    " ,@ProjectId " +
                    " ,@EmpId" +
                       " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                TaskName = dto.TaskName,
                ProjectId = dto.ProjectId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Duration = dto.Duration,
                EmpId = dto.EmpId,
                CreatedId=dto.CreatedId
            }, Transaction);
            return id;
        }



        #endregion

        #region project Maintenance task

        public ProjectMaintenanceTaskDto[] AllProjectMaintenanceTaskDtoAsQuerable(PagingRequest<ProjectMaintenanceTaskListFilterDto> paging)
        {
            var filterDto = paging.Filters;
            var sql = " SELECT Task.Id , TaskName,Task.StartDate,Task.EndDate, Duration,ProjectMaintenanceId as ProjectId,ProjectMaintenance.MaintenanceName as ProjectName,Task.EmpId,EmpName  " +
                 ",Task.CreatedId,Task.CreatedDate,Task.UpdatedId,Task.UpdatedDate  " +
                      " FROM Task " +
                      " LEFT JOIN  Employee on Employee.Id = Task.EmpId " +
                      " LEFT JOIN  ProjectMaintenance on ProjectMaintenance.Id = Task.ProjectMaintenanceId " +
                      " where ProjectMaintenanceId = @ProjectMaintenanceId";
            var _dto = SqlMapper.Query<ProjectMaintenanceTaskDto>(Connection, sql, new { ProjectMaintenanceId = filterDto.ProjectMaintenanceId }, transaction: Transaction);
            var query = _dto;

            //search condition

            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.TaskName))
                {
                    query = query.Where(emp => emp.TaskName.Contains(filterDto.TaskName));
                }
                if (filterDto.StartDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate >= filterDto.StartDate);
                }
                if (filterDto.EndDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate <= filterDto.EndDate);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(ProjectMaintenanceTaskDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<ProjectMaintenanceTaskDto>();
        }
     
        public async Task<int> InsertProjectMaintenanceTaskAsync(ProjectMaintenanceTaskDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[Task] " +
                    " ([TaskName] " +
                    " ,[StartDate] " +
                    " ,[EndDate]" +
                    " ,[Duration]" +
                    " ,[ProjectMaintenanceId] " +
                    " ,[EmpId] " +
                       " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@TaskName" +
                    " ,@StartDate" +
                    " ,@EndDate" +
                    " ,@Duration" +
                    " ,@ProjectMaintenanceId " +
                    " ,@EmpId" +
                         " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                TaskName = dto.TaskName,
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Duration = dto.Duration,
                EmpId = dto.EmpId,
                CreatedId=dto.CreatedId
            }, Transaction);
            return id;
        }

       




        #endregion



    }
}