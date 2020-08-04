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
    internal class ProjectRepository : RepositoryBase, IProjectRepository
    {
        public ProjectRepository()
        {

        }
        public ProjectListDto[] AllProjectListDtoAsQuerable(PagingRequest<ProjectListFilterDto> paging)
        {
            var sql1 = " SELECT Project.Id,ProjectName,StartDate,EndDate,PaymentDate,EstimateCost " +
                       " , EstimateCostCurrencyId, MstCostCurrency.CostCurrencySymboy " +
                       " , CustId,Customer.CustName+'【'+Customer.CustShortName+'】' as CustName ,Customer.CustShortName " +
                       " , ProjectStatusId,MstProjectStatus.ProjectStatusName , MstProjectStatus.StyleCss" +
                       " FROM Project " +
                       " LEFT JOIN  Customer on Customer.Id = Project.CustId " +
                       " LEFT JOIN  MstCostCurrency on MstCostCurrency.Id = Project.EstimateCostCurrencyId " +
                       " LEFT JOIN  MstProjectStatus on MstProjectStatus.Id = Project.ProjectStatusId";
            var _ProjectDto = SqlMapper.Query<ProjectListDto>(Connection, sql1, transaction: Transaction);
            var query = _ProjectDto;

            //search condition
            var filterDto = paging.Filters;
            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.ProjectName))
                {
                    query = query.Where(emp => emp.ProjectName.Contains(filterDto.ProjectName));
                }
                if (filterDto.StartDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate >= filterDto.StartDate);
                }
                if (filterDto.EndDate.HasValue)
                {
                    query = query.Where(emp => emp.StartDate <= filterDto.EndDate);
                }
                if (!string.IsNullOrEmpty(filterDto.CustName))
                {
                    query = query.Where(emp => emp.CustName != null && emp.CustName.Contains(filterDto.CustName));
                }
                if (!string.IsNullOrEmpty(filterDto.ProjectStatusName))
                {
                    query = query.Where(emp => emp.ProjectStatusName == filterDto.ProjectStatusName);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(ProjectListDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<ProjectListDto>();
        }

        public async Task<IEnumerable<ProjectFinishListDto>> ProjectFinishAsync(int year)
        {
            var sql1 = " SELECT Id,ProjectName" +
                " FROM [dbo].[Project] WHERE YEAR(DeliveryDate) = @year ";

            return await SqlMapper.QueryAsync<ProjectFinishListDto>(Connection, sql1,new { year = year}, transaction: Transaction);
        }

        public async Task<IEnumerable<Project>> AllAsync()
        {
            return await SqlMapper.QueryAsync<Project>(Connection, "SELECT * FROM Project", transaction: Transaction);
        }

       

        public async Task<IEnumerable<ProjectListDto>> AllProjectListDtoAsync()
        {
            var sql1 = " SELECT Project.Id,ProjectName,StartDate,EndDate,PaymentDate,EstimateCost " +
                        " , EstimateCostCurrencyId, MstCostCurrency.CostCurrencySymboy " +
                        " , CustId,Customer.CustName+'【'+Customer.CustShortName+'】' as CustName ,Customer.CustShortName " +
                        " , ProjectStatusId,MstProjectStatus.ProjectStatusName , MstProjectStatus.StyleCss" +
                        " FROM Project " +
                        " LEFT JOIN  Customer on Customer.Id = Project.CustId " +
                        " LEFT JOIN  MstCostCurrency on MstCostCurrency.Id = Project.EstimateCostCurrencyId " +
                        " LEFT JOIN  MstProjectStatus on MstProjectStatus.Id = Project.ProjectStatusId " +
                        " order by Project.StartDate desc ";
            var _ProjectDto = await SqlMapper.QueryAsync<ProjectListDto>(Connection, sql1, transaction: Transaction);
            return _ProjectDto;
        }

        private async Task<Project> GetProjectByIdAsync(long id)
        {
            var sql1 = " SELECT * FROM Project where Id = @Id  ";
            var sql2 = " SELECT * FROM attachmentFile where ProjectId = @Id";
            var sql3 = " SELECT * FROM ProjectMaintenance where ProjectId = @Id";
            var sql4 = " SELECT * FROM projectMember where ProjectId = @Id";
            var sql5 = " SELECT * FROM Customer";
            var sql6 = " SELECT * FROM MstCostCurrency";
            var sql7 = " SELECT * FROM MstProjectStatus";
            var multi = await SqlMapper.QueryMultipleAsync(Connection, sql1 + ";" + sql2 + ";" + sql3 + ";" + sql4 + ";" + sql5 + ";" + sql6 + ";" + sql7, new { Id = id }, transaction: Transaction);
            var _Project = multi.Read<Project>().FirstOrDefault();
            var _attachmentFile = multi.Read<AttachmentFile>().ToList();
            var _projectMaintenance = multi.Read<ProjectMaintenance>().ToList();
            var _projectMember = multi.Read<Member>().ToList();
            var _customer = multi.Read<Customer>().ToList();
            var _MstCostCurrency = multi.Read<MstCostCurrency>().ToList();
            var _MstProjectStatus = multi.Read<MstProjectStatus>().ToList();
            _Project.AttachmentFile = _attachmentFile;
            _Project.ProjectMaintenance = _projectMaintenance;
            _Project.Member = _projectMember;
            _Project.Cust = _customer.Where(r => r.Id == _Project.CustId).FirstOrDefault();
            _Project.EstimateCostCurrency = _MstCostCurrency.Where(r => r.Id == _Project.EstimateCostCurrencyId).FirstOrDefault();
            _Project.ProjectStatus = _MstProjectStatus.Where(r => r.Id == _Project.ProjectStatusId).FirstOrDefault();
            return _Project;
        }

        public async Task<ProjectBasicDto> GetProjectBasicDtoByIdAsync(long projectId)
        {
            var sql = "SELECT Project.Id,ProjectName,ProjectDecription,StartDate,EndDate,EstimateCost,EstimateManDay,DeliveryDate,PaymentDate " +
                        " ,  Project.CreatedId,Project.CreatedDate,Project.UpdatedId,Project.UpdatedDate " +
                        " ,EstimateCostCurrencyId,MstCostCurrency.CostCurrencySymboy,MstCostCurrency.CostCurrency " +
                        " ,CustId,Customer.CustName+'【'+Customer.CustShortName+'】' as CustName,Customer.CustShortName " +
                        " ,ProjectStatusId,MstProjectStatus.ProjectStatusName , MstProjectStatus.StyleCss" +
                        " FROM Project " +
                        " LEFT JOIN  Customer on Customer.Id = Project.CustId " +
                        " LEFT JOIN  MstCostCurrency on MstCostCurrency.Id = Project.EstimateCostCurrencyId " +
                        " LEFT JOIN  MstProjectStatus on MstProjectStatus.Id = Project.ProjectStatusId" +
                        " WHERE  Project.Id = @Id ";
            return await SqlMapper.QueryFirstOrDefaultAsync<ProjectBasicDto>(Connection, sql, new { Id = projectId }, transaction: Transaction);
        }

        private ProjectBasicDto checkCurrency(ProjectBasicDto dto)
        {
            if(dto.EstimateCost == null)
            {
                dto.EstimateCostCurrencyId = null;
            }
            return dto;
        }
        public async Task<int> InsertProjectAsync(ProjectBasicDto dto)
        {
            dto = checkCurrency(dto);
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[Project] " +
                    " ([CustId] " +
                    " ,[ProjectName] " +
                    " ,[ProjectDecription] " +
                    " ,[StartDate] " +
                    " ,[EndDate] " +
                    " ,[EstimateCost] " +
                    " ,[EstimateCostCurrencyId] " +
                    " ,[EstimateManDay] " +
                    " ,[DeliveryDate] " +
                    " ,[PaymentDate] " +
                    " ,[ProjectStatusId] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@CustId " +
                    " ,@ProjectName" +
                    " ,@ProjectDecription " +
                    " ,@StartDate " +
                    " ,@EndDate " +
                    " ,@EstimateCost " +
                    " ,@EstimateCostCurrencyId " +
                    " ,@EstimateManDay " +
                    " ,@DeliveryDate " +
                    " ,@PaymentDate " +
                    " ,@ProjectStatusId " +
                    " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                CustId = dto.CustId,
                ProjectName = dto.ProjectName,
                ProjectDecription = dto.ProjectDecription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EstimateCost = dto.EstimateCost,
                EstimateCostCurrencyId = dto.EstimateCostCurrencyId,
                EstimateManDay = dto.EstimateManDay,
                DeliveryDate = dto.DeliveryDate,
                PaymentDate = dto.PaymentDate,
                ProjectStatusId = dto.ProjectStatusId,
                CreatedId = dto.CreatedId,
            }, Transaction);
            return id;
        }

        public async Task<int> UpdateProjectAsync(ProjectBasicDto dto)
        {
            dto = checkCurrency(dto);
            var sql = " UPDATE [dbo].[Project] " +
                     " SET [CustId] = @CustId " +
                     "  ,[ProjectName] = @ProjectName " +
                     "  ,[ProjectDecription] = @ProjectDecription " +
                     "  ,[StartDate] = @StartDate " +
                     "  ,[EndDate] = @EndDate " +
                     "  ,[EstimateCost] = @EstimateCost " +
                     "  ,[EstimateCostCurrencyId] = @EstimateCostCurrencyId " +
                     "  ,[EstimateManDay] = @EstimateManDay " +
                     "  ,[DeliveryDate] = @DeliveryDate " +
                     "  ,[PaymentDate] = @PaymentDate " +
                     "  ,[ProjectStatusId] = @ProjectStatusId " +
                     "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                CustId = dto.CustId,
                ProjectName = dto.ProjectName,
                ProjectDecription = dto.ProjectDecription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EstimateCost = dto.EstimateCost,
                EstimateCostCurrencyId = dto.EstimateCostCurrencyId,
                EstimateManDay = dto.EstimateManDay,
                DeliveryDate = dto.DeliveryDate,
                PaymentDate = dto.PaymentDate,
                ProjectStatusId = dto.ProjectStatusId,
                UpdatedId = dto.UpdatedId,
            }, Transaction);
            return x;
        }

        public async Task<int> HardDeleteProjectAsync(long id)
        {
            var sql = " DELETE [dbo].[Project] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id,
            }, Transaction);
            return x;
        }


        #region Project report
        public async Task<IEnumerable<ProjectReportDto>> GetProjectReportAsync(string yearMonth, string year, string status, int rateYen, int rateUSD)
        {
            var sql1 = " SELECT A.Id 'ProjectId'  " +
                               "  , Customer.CustName+'【'+Customer.CustShortName+'】' as CustName " +
                               "  , A.ProjectName " +
                               "  , A.ProjectDecription " +
                               "  , A.StartDate 'ProjectStartDate' " +
                               "  , A.EndDate 'ProjectEndDate' " +
                               "  , A.EstimateCost 'ProjectEstimateCost' " +
                               "  , A.EstimateCost1 'ProjectEstimateCostVND' " +
                               "  , MstCostCurrency.CostCurrencySymboy 'ProjectCurrencySymboy' " +
                               "  , A.DeliveryDate 'ProjectDeliveryDate' " +
                               "  , A.PaymentDate 'ProjectPaymentDate' " +
                               "  , A.ProjectStatusId 'ProjectStatusId' " +
                               "  , MstProjectStatus.ProjectStatusName 'ProjectStatus' " +
                               "  , MstProjectStatus.StyleCss 'ProjectStatusStyle' " +
                               "  , CASE " +
                               "          WHEN A.EstimateCost1 is null  THEN 0 + A.MaintenanceEstimateCost " +
                               "          WHEN A.MaintenanceEstimateCost is null  THEN A.EstimateCost1 + 0 " +
                               "          ELSE A.EstimateCost1 + A.MaintenanceEstimateCost " +
                               "    END 'ProjectTotalEstimateCost' " +
                               "   ,leader.EmpName 'ProjectLeader' " +
                               "   ,coder.EmpName 'ProjectCoder' " +
                               "   ,tester.EmpName 'ProjectTester'" +
                        " FROM(SELECT Project.Id " +
                        "         , Project.CustId " +
                        "         , Project.ProjectName " +
                        "         , Project.ProjectDecription " +
                        "         , Project.StartDate " +
                        "         , Project.EndDate " +
                        "         , Project.EstimateCost " +
                        "         , Project.EstimateCost1 " +
                        "         , Project.EstimateCostCurrencyId " +
                        "         , Project.DeliveryDate " +
                        "         , Project.PaymentDate " +
                        "         , Project.ProjectStatusId " +
                        "         , sum(ProjectMaintenance.MaintenanceEstimateCost1) as MaintenanceEstimateCost  " +
                        "         , Max(ProjectMaintenance.StartDate)  as MaintenanceStartDate " +
                        "         , Max(ProjectMaintenance.EndDate)  as MaintenanceEndDate " +
                        "         , Max(ProjectMaintenance.MaintenanceStatusId)  as MaintenanceStatusId" +
                        "      FROM  " +
                                     " ( " +
                                          "  select Project.* "+
                                          "        , CASE MstCostCurrency.CostCurrency " +
                                          "            WHEN 'yen' THEN Project.EstimateCost *  " + rateYen +
                                          "            WHEN 'usd' THEN Project.EstimateCost *  " + rateUSD +
                                          "            WHEN 'vnd' THEN Project.EstimateCost " +
                                          "            ELSE Project.EstimateCost " +
                                          "         END EstimateCost1 " +
                                          "  from Project " +
                                          "  LEFT JOIN  MstCostCurrency on Project.EstimateCostCurrencyId = MstCostCurrency.Id " +
                                    " ) as Project " + 
                            "  LEFT JOIN " +
                            " ( " +
                                  " select ProjectMaintenance.*" +
                                  " 	, CASE MstCostCurrency.CostCurrency " +
                                  "           WHEN 'yen' THEN ProjectMaintenance.EstimateCost *  " + rateYen +
                                  "           WHEN 'usd' THEN ProjectMaintenance.EstimateCost *  " + rateUSD +
                                  "           WHEN 'vnd' THEN ProjectMaintenance.EstimateCost " +
                                    "            ELSE ProjectMaintenance.EstimateCost " +
                                  "       END MaintenanceEstimateCost1 " +
                                  " from ProjectMaintenance " +
                                  " LEFT JOIN  MstCostCurrency on ProjectMaintenance.EstimateCostCurrencyId = MstCostCurrency.Id " +
                           " ) as ProjectMaintenance"+
                           "   on Project.Id = ProjectMaintenance.ProjectId " +
                       " WHERE 1=1 ";

            if (!string.IsNullOrEmpty(yearMonth))
            {
                sql1 = sql1 + "  AND (( '" + yearMonth + "'  >= FORMAT(Project.StartDate, 'yyyyMM') " +
                                 " AND '" + yearMonth + "' <= FORMAT(Project.EndDate, 'yyyyMM') ";
                if (!string.IsNullOrEmpty(status))
                {
                    sql1 = sql1 + " AND  Project.ProjectStatusId IN ( " + status + ") )";
                }
                else
                {
                    sql1 = sql1 + ")";
                }
            }
            if (!string.IsNullOrEmpty(yearMonth))
            {
                sql1 = sql1 + " OR('" + yearMonth + "' >= FORMAT(ProjectMaintenance.StartDate, 'yyyyMM') " +
                                 " AND '" + yearMonth + "' <= FORMAT(ProjectMaintenance.EndDate, 'yyyyMM')";
                if (!string.IsNullOrEmpty(status))
                {
                    sql1 = sql1 + " AND  ProjectMaintenance.MaintenanceStatusId  IN ( " + status + ") ))";
                }
                else
                {
                    sql1 = sql1 + "))";
                }
            }
            if (!string.IsNullOrEmpty(year))
            {
                sql1 = sql1 + "  AND (( '" + year + "'  >= FORMAT(Project.StartDate, 'yyyy') " +
                                 " AND '" + year + "' <= FORMAT(Project.EndDate, 'yyyy') ";
                if (!string.IsNullOrEmpty(status))
                {
                    sql1 = sql1 + " AND  Project.ProjectStatusId  IN (  " + status + " ))";
                }
                else
                {
                    sql1 = sql1 + ")";
                }
            }
            if (!string.IsNullOrEmpty(year))
            {
                sql1 = sql1 + " OR('" + year + "' >= FORMAT(ProjectMaintenance.StartDate, 'yyyy') " +
                                 " AND '" + year + "' <= FORMAT(ProjectMaintenance.EndDate, 'yyyy')";
                if (!string.IsNullOrEmpty(status))
                {
                    sql1 = sql1 + " AND  ProjectMaintenance.MaintenanceStatusId  IN (  " + status + " )))";
                }
                else
                {
                    sql1 = sql1 + "))";
                }
            }

            sql1 = sql1 + "      GROUP BY Project.Id " +
                        "         , Project.CustId " +
                        "         , Project.ProjectName " +
                        "         , Project.ProjectDecription " +
                        "         , Project.StartDate " +
                        "         , Project.EndDate " +
                        "         , Project.EstimateCost " +
                           "         , Project.EstimateCost1 " +
                        "         , Project.EstimateCostCurrencyId " +
                        "         , Project.DeliveryDate " +
                        "         , Project.PaymentDate " +
                        "         , Project.ProjectStatusId " +
                        "  ) as A " +
                        " LEFT JOIN  Customer on A.CustId = Customer.Id " +
                        " LEFT JOIN  MstCostCurrency on A.EstimateCostCurrencyId = MstCostCurrency.Id " +
                        " LEFT JOIN  MstProjectStatus on A.ProjectStatusId = MstProjectStatus.Id" +
                        " LEFT JOIN (SELECT ProjectId, ProjectPositionId, " +
                        "                     STUFF((SELECT ', ' + A.EmpName " +
                        "                             FROM(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id WHERE  ProjectPositionId = 1)  A " +
                        "                            Where A.ProjectId = B.ProjectId FOR XML PATH('') " +
                        "                          ) " +
                        "                          , 1, 1, '') As EmpName " +
                        "                  From(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id)  B " +
                        "                Group By ProjectId, ProjectPositionId " +
                        " 			) leader on A.Id = leader.ProjectId " +
                        "  LEFT JOIN (SELECT ProjectId, ProjectPositionId , " +
                        "                     STUFF((SELECT ', ' + A.EmpName " +
                        "                             FROM(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id WHERE  ProjectPositionId = 2)  A " +
                        "                         Where A.ProjectId = B.ProjectId FOR XML PATH('') " +
                        " 						 ) " +
                        " 						 ,1,1,'') As EmpName " +
                        "                  From(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id)  B " +
                        "                 Group By ProjectId, ProjectPositionId " +
                        " 			) coder on A.Id = coder.ProjectId " +
                        "  LEFT JOIN (SELECT ProjectId, ProjectPositionId , " +
                        "                     STUFF((SELECT ', ' + A.EmpName " +
                        "                             FROM(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id WHERE  ProjectPositionId = 3)  A " +
                        "                         Where A.ProjectId = B.ProjectId FOR XML PATH('') " +
                        " 						 ) " +
                        " 						 ,1,1,'') As EmpName " +
                        "                  From(select Member.*, Employee.EmpName from Member LEFT JOIN  Employee on Member.EmpId = Employee.Id)  B " +
                        "                 Group By ProjectId, ProjectPositionId " +
                        " 			) tester on A.Id = tester.ProjectId " +
                        " WHERE 1=1 " +
                        "";

            sql1 = sql1 + " GROUP BY A.Id " +
                       "                 , Customer.CustName ,Customer.CustShortName" +
                       " 				 , A.ProjectName " +
                       " 				 , A.ProjectDecription " +
                       "				 , A.StartDate " +
                       "				 , A.EndDate " +
                       "				 , A.EstimateCost " +
                        "				 , A.EstimateCost1 " +
                       "				 , MstCostCurrency.CostCurrencySymboy " +
                       "				 , A.DeliveryDate " +
                       "				 , A.PaymentDate " +
                       "				 , MstProjectStatus.ProjectStatusName " +
                       "                 , MstProjectStatus.StyleCss" +
                       "                 , A.ProjectStatusId" +
                       "				 , leader.EmpName " +
                       "				 , coder.EmpName " +
                       "				 , tester.EmpName " +
                       "				 , A.EstimateCost " +
                       "				 , A.MaintenanceEstimateCost ";


            var sql2 = " SELECT ProjectMaintenance.Id 'MaintenanceId' " +
                           " , ProjectMaintenance.ProjectId  'MaintenanceProjectId' " +
                           " , ProjectMaintenance.MaintenanceName  'MaintenanceName' " +
                           " , ProjectMaintenance.MaintenanceContent  'MaintenanceContent' " +
                           " , ProjectMaintenance.StartDate 'MaintenanceStartDate' " +
                           " , ProjectMaintenance.EndDate 'MaintenanceEndDate' " +
                           " , ProjectMaintenance.EstimateCost 'MaintenanceEstimateCost' " +
                              " 	, CASE MstCostCurrency.CostCurrency " +
                                  "           WHEN 'yen' THEN ProjectMaintenance.EstimateCost *  " + rateYen +
                                  "           WHEN 'usd' THEN ProjectMaintenance.EstimateCost *  " + rateUSD +
                                  "           WHEN 'vnd' THEN ProjectMaintenance.EstimateCost " +
                                    "            ELSE ProjectMaintenance.EstimateCost " +
                                  "       END MaintenanceEstimateCostVND " +
                           " , MstCostCurrency.CostCurrencySymboy 'MaintenanceCurrencySymboy' " +
                           " , ProjectMaintenance.DeliveryDate 'MaintenanceDeliveryDate' " +
                           " , ProjectMaintenance.PaymentDate 'MaintenancePaymentDate' " +
                           " , MstProjectStatus.ProjectStatusName 'MaintenanceStatus' " +
                           " , MstProjectStatus.StyleCss 'MaintenanceStatusStyle' " +
                       " FROM ProjectMaintenance " +
                       " LEFT JOIN  MstCostCurrency on ProjectMaintenance.EstimateCostCurrencyId = MstCostCurrency.Id " +
                       " LEFT JOIN  MstProjectStatus on ProjectMaintenance.MaintenanceStatusId = MstProjectStatus.Id" +
                       " WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(yearMonth))
            {
                sql2 = sql2 + " AND ('" + yearMonth + "' >= FORMAT(ProjectMaintenance.StartDate, 'yyyyMM') " +
                                 " AND '" + yearMonth + "' <= FORMAT(ProjectMaintenance.EndDate, 'yyyyMM'))";
            }
            if (!string.IsNullOrEmpty(year))
            {
                sql2 = sql2 + " AND ('" + year + "' >= FORMAT(ProjectMaintenance.StartDate, 'yyyy') " +
                                 " AND '" + year + "' <= FORMAT(ProjectMaintenance.EndDate, 'yyyy'))";
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql2 = sql2 + " AND ( ProjectMaintenance.MaintenanceStatusId IN ( " + status + " ) )";
            }
            var multi = await SqlMapper.QueryMultipleAsync(Connection, sql1 + sql2, transaction: Transaction);
            var _Project = multi.Read<ProjectReportDto>().ToList();
            var _ProjectMaintenance = multi.Read<ProjectMaintenanceReportDto>().ToList();
            foreach (var item in _Project)
            {
                var flagSum = false;
                item.ProjectMaintenanceReports = _ProjectMaintenance.Where(x => x.MaintenanceProjectId == item.ProjectId).OrderByDescending(x=>x.MaintenanceStartDate).ToList();
                if (!string.IsNullOrEmpty(yearMonth))
                {
                    if (item.ProjectStartDate != null)
                    {
                        if (Convert.ToInt32(item.ProjectStartDate.Value.ToString("yyyyMM")) <= Convert.ToInt32(yearMonth))
                        {
                            if (item.ProjectEndDate != null)
                            {
                                if (Convert.ToInt32(yearMonth) <= Convert.ToInt32(item.ProjectEndDate.Value.ToString("yyyyMM")))
                                {
                                    flagSum = true;
                                }
                            }
                            //flagSum = true;
                        }
                    }
                //    if (item.ProjectEndDate != null)
                //    {
                //        if (Convert.ToInt32(yearMonth) >= Convert.ToInt32(item.ProjectEndDate.Value.ToString("yyyyMM")))
                //        {
                //            flagSum = true;
                //        }
                //    }
                }
                if (!string.IsNullOrEmpty(year))
                {
                    if (item.ProjectStartDate != null)
                    {
                        if (Convert.ToInt32(item.ProjectStartDate.Value.ToString("yyyy")) <= Convert.ToInt32(year))
                        {
                            if (item.ProjectEndDate != null)
                            {
                                if (Convert.ToInt32(year) <= Convert.ToInt32(item.ProjectEndDate.Value.ToString("yyyy")))
                                {
                                    flagSum = true;
                                }
                            }
                            //flagSum = true;
                        }
                    }
                    //if (item.ProjectEndDate != null)
                    //{
                    //    if (Convert.ToInt32(year) >= Convert.ToInt32(item.ProjectEndDate.Value.ToString("yyyy")))
                    //    {
                    //        flagSum = true;
                    //    }
                    //}
                }
                if (!string.IsNullOrEmpty(status))
                {
                    if (item.ProjectStatusId == status)
                    {
                        flagSum = true;
                    }
                }
                var ProjectEstimateCost = item.ProjectEstimateCostVND == null ? 0 : item.ProjectEstimateCostVND.Value;
                var temp = _ProjectMaintenance.Where(x => x.MaintenanceProjectId == item.ProjectId).Sum(x => x.MaintenanceEstimateCostVND);
                var ProjectMaintenanceEstimateCost = temp == null ? 0 : temp;
                if (flagSum == true)
                {
                    item.ProjectTotalEstimateCost = (ProjectEstimateCost + ProjectMaintenanceEstimateCost).ToString();
                }
                else
                {
                    item.ProjectTotalEstimateCost = (ProjectMaintenanceEstimateCost).ToString();
                }
            }
            return _Project.OrderByDescending(x=> x.ProjectStartDate).ToList();
        }

        public async Task<IEnumerable<ProjectReportStatisticsDto>> GetProjectReportStatistics(string year, string yearMonthStart, string yearMonthEnd, int rateYen, int rateUSD)
        {

            var Sql = " SELECT AAA.custId ,AAA.CustName,AAA.CustStyleCss ,SUM(AAA.TotalCost) as TotalCost,sum(CountProjectMaintenance + CountProject) as TotalProject " +
                        " FROM " +
                        " ( " +
                                " SELECT Customer.Id as custId, Customer.CustName, Project.Id as projectId ,Customer.CustStyleCss  " +
                                        " , SUM(ISNULL(ProjectMaintenance.ProjectMaintenanceEstimateCost, 0)) + MAX(ISNULL(Project.ProjectEstimateCost, 0)) as TotalCost " +
                                        " , count(ProjectMaintenance.Id) as CountProjectMaintenance " +
                                        " , CASE " +
                                       " WHEN   Min(ProjectMaintenance.Id) is null THEN count(Project.Id) " +
                                       " WHEN   Min(ProjectMaintenance.Id) is not null and  Min(Project.PaymentDate) is not null THEN 1 " +
                                       " ELSE 0 END as CountProject "+
                                 " FROM Customer  " +
                                  " LEFT JOIN  " +
                                     " ( " +
                                          "  select Project.*, MstCostCurrency.CostCurrency " +
                                          "        , CASE MstCostCurrency.CostCurrency " +
                                          "            WHEN 'yen' THEN Project.EstimateCost *  " + rateYen +
                                          "            WHEN 'usd' THEN Project.EstimateCost *  " + rateUSD +
                                          "            WHEN 'vnd' THEN Project.EstimateCost " +
                                             "            ELSE Project.EstimateCost " +
                                          "         END ProjectEstimateCost " +
                                          "  from Project " +
                                          "  LEFT JOIN  MstCostCurrency on Project.EstimateCostCurrencyId = MstCostCurrency.Id " +
                                    " ) as Project on Project.CustId = Customer.Id " +
                                 " LEFT JOIN  " +
                                    " ( " +
                                          " select ProjectMaintenance.*,MstCostCurrency.CostCurrency " +
                                          " 	, CASE MstCostCurrency.CostCurrency " +
                                          "           WHEN 'yen' THEN ProjectMaintenance.EstimateCost *  " + rateYen +
                                          "           WHEN 'usd' THEN ProjectMaintenance.EstimateCost *  " + rateUSD +
                                          "           WHEN 'vnd' THEN ProjectMaintenance.EstimateCost " +
                                             "            ELSE ProjectMaintenance.EstimateCost " +
                                          "       END ProjectMaintenanceEstimateCost " +
                                          " from ProjectMaintenance " +
                                          " LEFT JOIN  MstCostCurrency on ProjectMaintenance.EstimateCostCurrencyId = MstCostCurrency.Id "+
                                          " WHERE 1 = 1 ";
                                            if (!string.IsNullOrEmpty(yearMonthStart))
                                            {
                                                Sql = Sql + " AND  '" + yearMonthStart + "'  <= FORMAT(ProjectMaintenance.PaymentDate, 'yyyyMM') ";
                                            }
                                            if (!string.IsNullOrEmpty(yearMonthEnd))
                                            {
                                                Sql = Sql + " AND  '" + yearMonthEnd + "'  >= FORMAT(ProjectMaintenance.PaymentDate, 'yyyyMM') ";
                                            }
                                            if (!string.IsNullOrEmpty(year))
                                            {
                                                Sql = Sql + " AND  '" + year + "'  = FORMAT(ProjectMaintenance.PaymentDate, 'yyyy') ";
                                            }
                                Sql +=" ) AS ProjectMaintenance ON ProjectMaintenance.ProjectId = Project.Id " +
                           " WHERE 1 = 1 ";
                            if (!string.IsNullOrEmpty(yearMonthStart))
                            {
                                if (string.IsNullOrEmpty(yearMonthEnd))
                                {
                                    Sql = Sql + " AND ( ";
                                }
                                else
                                {
                                    Sql = Sql + " AND  (( ";
                                }
                                Sql = Sql + "  '" + yearMonthStart + "'  <= FORMAT(Project.PaymentDate, 'yyyyMM') ";
                            }
                            if (!string.IsNullOrEmpty(yearMonthEnd))
                            {
                                if (string.IsNullOrEmpty(yearMonthStart))
                                {
                                    Sql = Sql + " AND ( ";
                                }
                                else
                                {
                                    Sql = Sql + " AND  ";
                                }
                                Sql = Sql + "   '" + yearMonthEnd + "'  >= FORMAT(Project.PaymentDate, 'yyyyMM') ";
                                if (!string.IsNullOrEmpty(yearMonthStart))
                                {
                                    Sql = Sql + " ) ";
                                }
                            }
                            if (!string.IsNullOrEmpty(year))
                            {
                                Sql = Sql + " AND (  '" + year + "'  = FORMAT(Project.PaymentDate, 'yyyy') ";
                            }
                            Sql = Sql + " OR  ProjectMaintenance.Id is  not null)";
                            Sql = Sql + " GROUP BY Customer.Id ,Customer.CustName,Project.Id,Customer.CustStyleCss " +
                         "  ) as AAA " +
                         "  GROUP BY AAA.custId ,AAA.CustName,AAA.CustStyleCss";

            var _report = await SqlMapper.QueryAsync<ProjectReportStatisticsDto>(Connection, Sql, transaction: Transaction);
            return _report;
        }

        public async Task<IEnumerable<ProjectReportStatisticsByCustDto>> GetProjectReportStatisticsByCust(string custId, string year, int rateYen, int rateUSD)
        {
            var sql = " WITH months(MonthNumber) AS " +
                     " (" +
                     "     SELECT 1" +
                     "     UNION ALL" +
                     "     SELECT MonthNumber + 1" +
                     "     FROM months" +
                     "     WHERE MonthNumber < 12" +
                     " )" +
                     " select CONCAT('" + year + "',Right(CONCAT('0', MonthNumber), 2)) as StatisticsMonth ,DDD.*" +
                     "   from months" +
                     "   LEFT JOIN " +
                     "  (" +
                     "      SELECT" +
                     "      CustId" +
                     "      , CustName ,CustStyleCss" +
                     "      , StartDate" +
                     "      , sum(MonthTotalCost) as MonthTotalCost" +
                     " 	,sum(MonthTotalProject) as MonthTotalProject" +
                     "   from" +
                     "       (" +
                     "       select * from" +
                     "           (SELECT Customer.Id as CustId" +
                     "           , Customer.CustName" +
                      "           , Customer.CustStyleCss" +
                     "           , FORMAT(Project.StartDate, 'yyyyMM') as StartDate" +
                     "           , sum(ISNULL(Project.EstimateCostVND, 0)) as MonthTotalCost" +
                     "           , count(Project.Id) as MonthTotalProject" +
                     "           FROM Customer" +
                                  " LEFT JOIN  " +
                                     " ( " +
                                          "  select Project.* "  +
                                          "        , CASE MstCostCurrency.CostCurrency " +
                                          "            WHEN 'yen' THEN Project.EstimateCost *  " + rateYen +
                                          "            WHEN 'usd' THEN Project.EstimateCost *  " + rateUSD +
                                          "            WHEN 'vnd' THEN Project.EstimateCost " +
                                                 "            ELSE Project.EstimateCost " +
                                          "         END EstimateCostVND " +
                                          "  from Project " +
                                          "  LEFT JOIN  MstCostCurrency on Project.EstimateCostCurrencyId = MstCostCurrency.Id " +
                                          " where Project.PaymentDate is not null " +
                            "           )  Project on Project.CustId = Customer.Id" +
                     "           group by Customer.Id, Customer.CustName, Customer.CustStyleCss, FORMAT(Project.StartDate, 'yyyyMM')" +
                     "           ) AAA" +
                     "       UNION" +
                     "       select * from" +
                    "            (SELECT Customer.Id as CustId" +
                    "            , Customer.CustName, Customer.CustStyleCss" +
                     "           , FORMAT(ProjectMaintenance.StartDate, 'yyyyMM') as StartDate" +
                     "           , sum(ISNULL(ProjectMaintenance.ProjectMaintenanceEstimateCostVND, 0)) as MonthTotalCost" +
                     "           , count(ProjectMaintenance.Id) as MonthTotalProject" +
                     "           FROM Customer" +
                 
                      " LEFT JOIN  " +
                        " ( " +
                              " select ProjectMaintenance.*, Project. CustId " +
                              " 	, CASE MstCostCurrency.CostCurrency " +
                              "           WHEN 'yen' THEN ProjectMaintenance.EstimateCost *  " + rateYen +
                              "           WHEN 'usd' THEN ProjectMaintenance.EstimateCost *  " + rateUSD +
                              "           WHEN 'vnd' THEN ProjectMaintenance.EstimateCost " +
                              "            ELSE ProjectMaintenance.EstimateCost " +
                              "       END ProjectMaintenanceEstimateCostVND " +
                       "        from Project" +
                     "          LEFT JOIN   ProjectMaintenance on ProjectMaintenance.ProjectId = Project.Id" +
                              " LEFT JOIN  MstCostCurrency on ProjectMaintenance.EstimateCostCurrencyId = MstCostCurrency.Id " +
                                "           where ProjectMaintenance.PaymentDate is not null" +
                                 "           ) ProjectMaintenance" +
                     "           on ProjectMaintenance.CustId = Customer.Id" +
                     "           group by Customer.Id, Customer.CustName, Customer.CustStyleCss, FORMAT(ProjectMaintenance.StartDate, 'yyyyMM')" +
                     "           ) BBB" +
                     "       ) as CCC" +
                      "      where CustId = " + custId + " " +
                     "       group by CustId,CustName, CustStyleCss,StartDate" +
                    " ) as DDD" +
                    " on StartDate = CONCAT('" + year + "', Left(CONCAT('0', MonthNumber), 2))" +
                    " order by StatisticsMonth,CustId";

            var _report = await SqlMapper.QueryAsync<ProjectReportStatisticsByCustDto>(Connection, sql, transaction: Transaction);
            return _report;
        }


        #endregion
    }
}