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
    internal class ProjectMaintenanceRepository : RepositoryBase, IProjectMaintenanceRepository
    {
        public ProjectMaintenanceRepository()
        {

        }
        public async Task<IEnumerable<ProjectFinishListDto>> ProjectFinishAsync(int year)
        {
            var sql1 = " SELECT Id,MaintenanceName as ProjectName" +
                " FROM [dbo].[Maintenance] WHERE YEAR(DeliveryDate) = @year ";

            return await SqlMapper.QueryAsync<ProjectFinishListDto>(Connection, sql1, new { year = year }, transaction: Transaction);
        }
        public  ProjectMaintenanceDto[] GetProjectMaintenance(long projectId, PagingRequest<ProjectMaintenanceFilterDto> paging)
        {
            var sql = " SELECT  ProjectMaintenance.Id , ProjectId , StartDate , EndDate ,MaintenanceName, MaintenanceContent,EstimateCost,EstimateCostCurrencyId,EstimateManDay,DeliveryDate,PaymentDate,MaintenanceStatusId" +
                    " , CreatedId , CreatedDate  , UpdatedId , UpdatedDate  " +
                    ", MstProjectStatus.ProjectStatusName, MstProjectStatus.StyleCss, MstCostCurrency.CostCurrency, MstCostCurrency.CostCurrencySymboy " +
                      " FROM ProjectMaintenance " +
                      " LEFT JOIN  MstProjectStatus on  MstProjectStatus.Id = ProjectMaintenance.MaintenanceStatusId " +
                      " LEFT JOIN  MstCostCurrency on  MstCostCurrency.Id = ProjectMaintenance.EstimateCostCurrencyId " +
                      " where ProjectId = @ProjectId" ;
            var _ProjectDto =  SqlMapper.Query<ProjectMaintenanceDto>(Connection, sql, new { ProjectId = projectId }, transaction: Transaction);
            var query = _ProjectDto;

            //search condition
            var filterDto = paging.Filters;
            if (filterDto != null)
            {
                if (filterDto.StartDate.HasValue)
                {
                    query = query.AsQueryable().Where(emp => emp.StartDate >= filterDto.StartDate);
                }
                if (filterDto.EndDate.HasValue)
                {
                    query = query.AsQueryable().Where(emp => emp.StartDate <= filterDto.EndDate);
                }
                if (!string.IsNullOrEmpty(filterDto.MaintenanceName))
                {
                    query = query.AsQueryable().Where(emp => emp.MaintenanceName.Contains(filterDto.MaintenanceName));
                }
                if (!string.IsNullOrEmpty(filterDto.ProjectStatusName))
                {
                    query = query.AsQueryable().Where(emp => emp.ProjectStatusName == filterDto.ProjectStatusName);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(ProjectMaintenanceDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<ProjectMaintenanceDto>();
        }


        public async Task<IEnumerable<ProjectMaintenanceDto>> GetProjectMaintenanceAsync(long projectId)
        {
            var sql = " SELECT  ProjectMaintenance.Id , ProjectId , StartDate , EndDate ,MaintenanceName, MaintenanceContent,EstimateCost,EstimateCostCurrencyId,EstimateManDay,DeliveryDate,PaymentDate,MaintenanceStatusId" +
                    " , CreatedId , CreatedDate  , UpdatedId , UpdatedDate  " +
                    ", MstProjectStatus.ProjectStatusName, MstProjectStatus.StyleCss, MstCostCurrency.CostCurrency, MstCostCurrency.CostCurrencySymboy " +
                      " FROM ProjectMaintenance " +
                      " LEFT JOIN  MstProjectStatus on  MstProjectStatus.Id = ProjectMaintenance.MaintenanceStatusId " +
                      " LEFT JOIN  MstCostCurrency on  MstCostCurrency.Id = ProjectMaintenance.EstimateCostCurrencyId " +
                      " where ProjectId = @ProjectId" +
                      " order by ProjectMaintenance.StartDate desc";
            return await SqlMapper.QueryAsync<ProjectMaintenanceDto>(Connection, sql, new { ProjectId = projectId }, transaction: Transaction);
        }

        public async Task<ProjectMaintenanceDto> GetProjectMaintenanceByIdAsync(long id)
        {
            var sql = " SELECT  ProjectMaintenance.Id , ProjectId , StartDate , EndDate , MaintenanceName,MaintenanceContent,EstimateCost,EstimateCostCurrencyId,EstimateManDay,DeliveryDate,PaymentDate,MaintenanceStatusId" +
                    " , CreatedId , CreatedDate  , UpdatedId , UpdatedDate  " +
                    ", MstProjectStatus.ProjectStatusName, MstProjectStatus.StyleCss, MstCostCurrency.CostCurrency, MstCostCurrency.CostCurrencySymboy " +
                      " FROM ProjectMaintenance " +
                      " LEFT JOIN  MstProjectStatus on  MstProjectStatus.Id = ProjectMaintenance.MaintenanceStatusId " +
                      " LEFT JOIN  MstCostCurrency on  MstCostCurrency.Id = ProjectMaintenance.EstimateCostCurrencyId " +
                      " where ProjectMaintenance.Id = @Id";
            return await SqlMapper.QueryFirstAsync<ProjectMaintenanceDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
        private ProjectMaintenanceDto checkCurrency(ProjectMaintenanceDto dto)
        {
            if (dto.EstimateCost == null)
            {
                dto.EstimateCostCurrencyId = null;
            }
            return dto;
        }
        public async Task<int> InsertProjectMaintenanceAsync(ProjectMaintenanceDto dto)
        {
            dto = checkCurrency(dto);
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[ProjectMaintenance] " +
                    " ([ProjectId] " +
                    " ,[StartDate] " +
                    " ,[EndDate] " +
                    " ,[MaintenanceName] " +
                    " ,[MaintenanceContent] " +
                     " ,[EstimateCost] " +
                      " ,[EstimateCostCurrencyId] " +
                       " ,[EstimateManDay] " +
                        " ,[DeliveryDate] " +
                         " ,[PaymentDate] " +
                          " ,[MaintenanceStatusId] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@ProjectId " +
                    " ,@StartDate" +
                    " ,@EndDate " +
                    " ,@MaintenanceName " +
                     " ,@MaintenanceContent " +
                    " ,@EstimateCost " +
                      " ,@EstimateCostCurrencyId " +
                       " ,@EstimateManDay " +
                        " ,@DeliveryDate " +
                         " ,@PaymentDate " +
                          " ,@MaintenanceStatusId " +
                    " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                ProjectId = dto.ProjectId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EstimateCost = dto.EstimateCost,
                EstimateCostCurrencyId = dto.EstimateCostCurrencyId,
                EstimateManDay = dto.EstimateManDay,
                DeliveryDate = dto.DeliveryDate,
                PaymentDate = dto.PaymentDate,
                MaintenanceStatusId = dto.MaintenanceStatusId,
                MaintenanceName = dto.MaintenanceName,
                MaintenanceContent = dto.MaintenanceContent,
                CreatedId = dto.CreatedId,
            }, Transaction);
            return id;
        }

        public async Task<int> UpdateProjectMaintenanceAsync(ProjectMaintenanceDto dto)
        {
            dto = checkCurrency(dto);
            var sql = " UPDATE [dbo].[ProjectMaintenance] " +
                     " SET [ProjectId] = @ProjectId " +
                     "  ,[StartDate] = @StartDate " +
                     "  ,[EndDate] = @EndDate " +
                      "  ,[MaintenanceName] = @MaintenanceName " +
                     "  ,[MaintenanceContent] = @MaintenanceContent " +
                     "  ,[EstimateCost] = @EstimateCost " +
                     "  ,[EstimateCostCurrencyId] = @EstimateCostCurrencyId " +
                     "  ,[EstimateManDay] = @EstimateManDay " +
                     "  ,[DeliveryDate] = @DeliveryDate " +
                     "  ,[PaymentDate] = @PaymentDate " +
                     "  ,[MaintenanceStatusId] = @MaintenanceStatusId " +
                     "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                ProjectId = dto.ProjectId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EstimateCost = dto.EstimateCost,
                EstimateCostCurrencyId = dto.EstimateCostCurrencyId,
                EstimateManDay = dto.EstimateManDay,
                DeliveryDate = dto.DeliveryDate,
                PaymentDate = dto.PaymentDate,
                MaintenanceStatusId = dto.MaintenanceStatusId,
                MaintenanceName = dto.MaintenanceName,
                MaintenanceContent = dto.MaintenanceContent,
                UpdatedId = dto.UpdatedId,
            }, Transaction);
            return x;
        }

        public async Task<int> DeleteProjectMaintenanceAsync(long id)
        {
            var sql = " DELETE [dbo].[ProjectMaintenance] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id,
            }, Transaction);
            return x;
        }

        public async Task<int> DeleteProjectMaintenanceByProjectIDAsync(long projectID)
        {
            var sql = " DELETE [dbo].[ProjectMaintenance] " +
                     " WHERE  ProjectID = @ProjectID";

            var x = await Connection.ExecuteAsync(sql, new
            {
                ProjectID = projectID,
            }, Transaction);
            return x;
        }
    }
}