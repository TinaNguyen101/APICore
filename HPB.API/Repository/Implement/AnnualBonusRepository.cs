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
    internal class AnnualBonusRepository : RepositoryBase, IAnnualBonusRepository
    {
        public AnnualBonusRepository()
        {

        }
        #region AnnualBonus 
        public async Task<decimal> GetBonusAsync(decimal day,int empId ,int year)
        {
            var sql1 = " SELECT  TOP 1 [Salary].Salary FROM[dbo].[Salary] WHERE [Salary].EmpId = @empId ORDER BY[Salary].ApprovalDate desc ";
            var sql2= " SELECT TOP 1 [MstWorkDayMonth].WorkDayMonth FROM [dbo].[MstWorkDayMonth] ORDER BY ApproveDate DESC ";
            var sql3 = " SELECT [AnnualRatingFactor].RatingFactorMember FROM[dbo].[AnnualRatingFactor] WHERE[AnnualRatingFactor].EmpId = @empId AND Year = @year ";
            var multi1 = await SqlMapper.QueryMultipleAsync(Connection, sql1 + ";" + sql2 + ";" + sql3, new { year = year, EmpId = empId }, transaction: Transaction); 
            var _salary = multi1.Read<decimal>().FirstOrDefault();
            var _WorkDayMonth = multi1.Read<int>().FirstOrDefault();
            var _RatingFactorMember = multi1.Read<decimal>().FirstOrDefault();

            return ((_salary / _WorkDayMonth) * _RatingFactorMember * day);

        }

        //list year + current year + 1
        public async Task<IEnumerable<int>> GetAllYearAnnualBonusAsync()
        {
            var sql = " select [year] from [dbo].[AnnualBonus] group by [year] order by year";
            IEnumerable<int> list = await SqlMapper.QueryAsync<int>(Connection, sql, transaction: Transaction);
            var currentYear = DateTime.Now.Year;
            if (!list.Any(x => x == currentYear))
            {
                list.Append(currentYear);
            }
            list.Append(currentYear + 1);

            return list;
        }
        //get all AllAnnualBonus for year and empid
        public async Task<IEnumerable<AnnualBonusListByEmpDto>> GetAllAnnualBonusAsync(int year,int empId)
        {
            IEnumerable<AnnualBonusListByEmpDto> lst;
                var sql2 = " select AnnualBonus.Id,Project.ProjectName as proName ,Project.Id as ProId "+
                            " , AnnualBonus.Bonus,AnnualBonus.Day" +
                            " ,AnnualRatingFactor.RatingFactorMember as RatingFactor,AnnualRatingFactor.Id as RatingFactorId" +
                            " , Employee.EmpName,Employee.Id as EmpId,AnnualBonus.Year" +
                        " from AnnualBonus" +
                        " LEFT JOIN  Project on Project.Id = AnnualBonus.ProjectId" +
                        " LEFT JOIN  AnnualRatingFactor on AnnualRatingFactor.Id = AnnualBonus.RatingFactorId" +
                        " LEFT JOIN  Employee on Employee.Id = AnnualBonus.EmpId" +
                        " where AnnualBonus.ProjectId is not null  and  AnnualBonus.Year = @year  and AnnualBonus.EmpId = @empId" +
                        " union" +
                        "  select AnnualBonus.Id,ProjectMaintenance.MaintenanceName as proName  " +
                         ", ProjectMaintenance.Id as ProId,AnnualBonus.Bonus,AnnualBonus.Day" +
						 ",AnnualRatingFactor.RatingFactorMember as RatingFactor, AnnualRatingFactor.Id as RatingFactorId" +
						 ",Employee.EmpName,Employee.Id as EmpId,AnnualBonus.Year" +
                        " from AnnualBonus" +
                        " LEFT JOIN  ProjectMaintenance on ProjectMaintenance.Id = AnnualBonus.ProjectMaintenanceId" +
                        " LEFT JOIN  AnnualRatingFactor on AnnualRatingFactor.Id = AnnualBonus.RatingFactorId" +
                        " LEFT JOIN  Employee on Employee.Id = AnnualBonus.EmpId" +
                        " where  AnnualBonus.ProjectMaintenanceId is not null and AnnualBonus.Year = @year   and AnnualBonus.EmpId = @empId";
                lst = await SqlMapper.QueryAsync<AnnualBonusListByEmpDto>(Connection, sql2, new { year = year, empId = empId }, transaction: Transaction);
            return lst;
        }

        

        public async Task<int> checkExistAnnualBonusAsync(int year)
        {
            var sql = " select count(*) from [dbo].[AnnualBonus]  where year = @year";
            return await SqlMapper.QuerySingleOrDefaultAsync<int>(Connection, sql, new { year = year }, transaction: Transaction);
        }
        //insert
        public async Task<int> InsertAnnualBonusAsync(AnnualBonusDto dto)
        {
           
            var sql1 = "" +
                " INSERT INTO [dbo].[AnnualBonus] " +
                   " ([Bonus] " +
                   " ,[ProjectId] " +
                   " ,[ProjectMaintenanceId] " +
                    " ,[RatingFactorId] " +
                      " ,[EmpId] " +
                        " ,[Day] " +
                   " ,[year] " +
                   " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                    " VALUES " +
                   " (@Bonus" +
                   " ,@ProjectId" +
                     " ,@ProjectMaintenanceId" +
                       " ,@RatingFactorId" +
                         " ,@EmpId" +
                          " ,@Day" +
                   " ,@year"+
                    " ,@CreatedId " +
                    " ,GETDATE() )" ;

            var result = await Connection.ExecuteAsync(sql1, new
            {
                Bonus = dto.Bonus,
                ProjectId = dto.ProjectId,
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                RatingFactorId = dto.RatingFactorId,
                EmpId = dto.EmpId,
                Day = dto.Day,
                year = dto.Year,
                CreatedId = dto.CreatedId
            }, Transaction);
            return result;
        }
        //edit
        public async Task<int> UpdateAnnualBonusAsync(AnnualBonusDto dto)
        {
                var sql1 = " UPDATE [dbo].[AnnualBonus] " +
                    "  SET Bonus = @Bonus " +
                     "  , ProjectId = @ProjectId " +
                     "  , ProjectMaintenanceId = @ProjectMaintenanceId " +
                      "  , RatingFactorId = @RatingFactorId " +
                       "  , Day = @Day " +
                         "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                    "  WHERE  EmpId = @empId" +
                    " and year =@year";

            var result = await Connection.ExecuteAsync(sql1, new
                {
                    Bonus = dto.Bonus,
                    ProjectId = dto.ProjectId,
                    ProjectMaintenanceId = dto.ProjectMaintenanceId,
                    Day = dto.Day,
                    EmpId = dto.EmpId,
                    year = dto.Year,
                UpdatedId = dto.UpdatedId
            }, Transaction);

            return result;
        }
        //delete
        public async Task<int> DeleteAnnualBonusAsync(int id)
        {
            var sql = " DELETE [dbo].[AnnualBonus] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }

        #endregion

        #region AnnualRatingFactor 
        //list year + current year + 1
        public async Task<IEnumerable<int>> GetAllYearAnnualRatingFactorAsync()
        {
            var sql = " select [year] from [dbo].[AnnualRatingFactor] group by [year] order by year";
            IEnumerable<int> list =  await SqlMapper.QueryAsync<int>(Connection, sql, transaction: Transaction);
            var currentYear = DateTime.Now.Year;
            if(!list.Any(x=>x == currentYear))
            {
                list.Append(currentYear);
            }
            list.Append(currentYear + 1);

            return list;
        }
        //list all
        public async Task<IEnumerable<AnnualRatingFactorListDto>> GetAllAnnualRatingFactorAsync(int year)
        {
            IEnumerable<AnnualRatingFactorListDto> lst ;
            var numRow = await checkExistAnnualRatingFactorAsync(year);
            if (numRow >0)
            {
                var sql1= " select Employee.Id as EmpId,Employee.EmpName" +
                    ",lastAnnualRatingFactor.RatingFactorMember as lastRatingFactorMember,lastAnnualRatingFactor.RatingFactorLeader as lastRatingFactorLeader" +
                    ", AnnualRatingFactor.RatingFactorMember, AnnualRatingFactor.RatingFactorLeader, AnnualRatingFactor.year" +
                    ", Employee.PosId" +
                        " from (select * from[dbo].[AnnualRatingFactor] where Year = @year) as [AnnualRatingFactor]" +
                        " LEFT JOIN [dbo].[Employee]  on Employee.Id = AnnualRatingFactor.EmpId" +
                        " LEFT JOIN  (select* from [dbo].[AnnualRatingFactor] where Year = ( @lastYear)) as lastAnnualRatingFactor on[AnnualRatingFactor].EmpId = lastAnnualRatingFactor.EmpId";
                lst = await SqlMapper.QueryAsync<AnnualRatingFactorListDto>(Connection, sql1, new { year = year , lastYear  = year-1 }, transaction: Transaction);
            }
            else
            {
                var sql2 = " select Employee.Id as EmpId, Employee.EmpName" +
                    ", Isnull(lastAnnualRatingFactor.RatingFactorMember,0) as lastRatingFactorMember , Isnull(lastAnnualRatingFactor.RatingFactorLeader,0) as lastRatingFactorLeader" +
                    ",  Isnull(AnnualRatingFactor.RatingFactorMember,0) as RatingFactorMember,  Isnull(AnnualRatingFactor.RatingFactorLeader,0) as RatingFactorLeader,@year" +
                         " from(select * from[dbo].[Employee] where Employee.PosId not in (1) and Employee.EmpEndDate is null and  YEAR(Employee.EmpStartDate) <= @year) as Employee" +
                         " LEFT JOIN (select* from [dbo].[AnnualRatingFactor] where Year = @year) as [AnnualRatingFactor] on Employee.Id = AnnualRatingFactor.EmpId" +
                         " LEFT JOIN (select* from [dbo].[AnnualRatingFactor] where Year = @lastYear) as lastAnnualRatingFactor  on Employee.Id = lastAnnualRatingFactor.EmpId";
                lst = await SqlMapper.QueryAsync<AnnualRatingFactorListDto>(Connection, sql2, new { year = year, lastYear = year - 1 }, transaction: Transaction);
            }
            return lst;
        }
        //check exist
        public async Task<int> checkExistAnnualRatingFactorAsync(int year)
        {
            var sql = " select count(*) from [dbo].[AnnualRatingFactor]  where year = @year";
            return  await SqlMapper.QuerySingleOrDefaultAsync<int>(Connection, sql, new { year = year }, transaction: Transaction);
        }
        //insert
        public async Task<int> InsertAnnualRatingFactorAsync(IEnumerable<AnnualRatingFactorListDto> dto)
        {
            List<AnnualRatingFactorDto> lstIns = new List<AnnualRatingFactorDto>() ;
            foreach (var item in dto)
            {
                var itemlst = new AnnualRatingFactorDto();
                itemlst.RatingFactorMember = item.RatingFactorMember;
                itemlst.RatingFactorLeader = item.RatingFactorLeader;
                itemlst.EmpId = item.EmpId;
                itemlst.year = item.year;
                lstIns.Add(itemlst);
            }
            var sql1 = "" +
                " INSERT INTO [dbo].[AnnualRatingFactor] " +
                   " ([EmpId] " +
                   " ,[RatingFactorMember] " +
                    " ,[RatingFactorLeader] " +
                   " ,[year] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                    " VALUES " +
                   " (@EmpId" +
                   " ,@RatingFactorMember" +
                    " ,@RatingFactorLeader" +
                   " ,@year" +
                     " ,@CreatedId " +
                    " ,GETDATE() )";

            var result = await Connection.ExecuteAsync(sql1, lstIns, Transaction);
            return result;
        }
        //update
        public async Task<int> UpdateAnnualRatingFactorAsync(IEnumerable<AnnualRatingFactorListDto> dto)
        {
            var result = 0;
            foreach (var item in dto)
            {
                var sql1 = " UPDATE [dbo].[AnnualRatingFactor] " +
                    "  SET RatingFactorLeader = @RatingFactorLeader " +
                     "  , RatingFactorMember = @RatingFactorMember " +
                     "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                    "  WHERE  EmpId = @empId" +
                    " and year =@year";

                result += await Connection.ExecuteAsync(sql1, new {
                    RatingFactorLeader = item.RatingFactorLeader,
                    RatingFactorMember = item.RatingFactorMember,
                    EmpId = item.EmpId,
                    year = item.year,
                    UpdatedId = item.UpdatedId
                }, Transaction);
            }
            
            return result;
        }

        #endregion

        #region Report

        //get all AllAnnualBonus for year
        public async Task<IEnumerable<AnnualBonusListDto>> GetAllAnnualBonusAsync(int year, int rateYen, int rateUSD)
        {
            IEnumerable<AnnualBonusListDto> lst;
                var sql2 = " SELECT (SELECT TOP 1[WorkDayMonth]  FROM [dbo].[MstWorkDayMonth] ORDER BY [ApproveDate] DESC) as WorkDayMonth" +
                                    ", @rateYen as rateYen , @rateUSD as rateUSD, ROW_NUMBER() OVER( ORDER BY   AnnualBonus.[EmpId] ASC) AS No " +
                                    " ,[Employee].EmpName" +
                                    " ,Max([AnnualRatingFactor].RatingFactorMember) as RatingFactorMember" +
                                    " ,(count(ProjectMember.Id) + count(ProjectMaintenanceMember.Id)) as TotalProjectMember" +
                                    " ,(sum(ProjectMember.Duration) + sum(ProjectMaintenanceMember.Duration)) as TotalDayMember" +
                                    " ,sum(AnnualBonus.Bonus) as BonusMember" +
                                    " ,Max([AnnualRatingFactor].RatingFactorLeader) as RatingFactorLeader" +
                                    " ,(count(ProjectLeader.Id) + count(ProjectMaintenanceLeader.Id)) as TotalProjectLeader" +
                                    " ,(sum(isnull(ProjectLeader.EstimateCostVND, 0)) + sum(isnull(ProjectMaintenanceLeader.EstimateCostVND, 0))) as TotalEstimateCostVNDLeader" +
                                    " ,((sum(isnull(ProjectLeader.EstimateCostVND, 0)) + sum(isnull(ProjectMaintenanceLeader.EstimateCostVND, 0))) * Max([AnnualRatingFactor].RatingFactorLeader) )as BonusLeader" +
                                    " ,(sum(AnnualBonus.Bonus) + ((sum(isnull(ProjectLeader.EstimateCostVND, 0)) + sum(isnull(ProjectMaintenanceLeader.EstimateCostVND, 0))) * Max([AnnualRatingFactor].RatingFactorLeader)))  as TotalBonus" +
                                    " FROM(SELECT * FROM[dbo].[AnnualBonus] WHERE[AnnualBonus].Year = @year) as AnnualBonus" +
                                    " LEFT JOIN[dbo].[Employee] ON[Employee].Id = [AnnualBonus].EmpId" +
                                    " LEFT JOIN(SELECT* FROM [dbo].[AnnualRatingFactor] WHERE[AnnualRatingFactor].Year =  @year) as [AnnualRatingFactor]" +
                                    "         on[AnnualRatingFactor].Id = [AnnualBonus].RatingFactorId" +
                                    " LEFT JOIN(" +
                                            " SELECT[Project].Id, [Project].ProjectName , [Task].EmpId, SUM([Task].Duration) as Duration" +
                                            " FROM[dbo].[Project]" +
                                            "         LEFT JOIN[dbo].[Task] ON[Task].ProjectId = [Project].Id" +
                                            " WHERE[Project].ProjectStatusId = 5 " +
                                            " GROUP BY[Project].Id,[Project].ProjectName ,[Task].EmpId " +
                                            " ) as ProjectMember" +
                                    " ON ProjectMember.Id = [AnnualBonus].ProjectId" +
                                    " and ProjectMember.EmpId = [AnnualBonus].EmpId" +
                                    " LEFT JOIN(" +
                                            " SELECT[ProjectMaintenance].Id, [ProjectMaintenance].MaintenanceName , [Task].EmpId, SUM([Task].Duration) as Duration" +
                                            " FROM[dbo].[Project]" +
                                            "         LEFT JOIN[dbo].[ProjectMaintenance] ON[ProjectMaintenance].ProjectId = [Project].Id" +
                                            " LEFT JOIN[dbo].[Task] ON[Task].ProjectMaintenanceId = [ProjectMaintenance].Id" +
                                            " WHERE[ProjectMaintenance].MaintenanceStatusId = 5" +
                                            " GROUP BY[ProjectMaintenance].Id,[ProjectMaintenance].MaintenanceName ,[Task].EmpId " +
                                            " ) as ProjectMaintenanceMember" +
                                    " ON ProjectMaintenanceMember.Id = [AnnualBonus].ProjectMaintenanceId" +
                                    " and ProjectMaintenanceMember.EmpId = [AnnualBonus].EmpId" +
                                    " LEFT JOIN(" +
                                            " SELECT[Project].Id, [Project].ProjectName , [Member].EmpId" +
                                            " , CASE" +
                                            " WHEN [Project].EstimateCostCurrencyId = 1 THEN (Sum([Project].EstimateCost) * @rateYen)" +
                                            " WHEN[Project].EstimateCostCurrencyId = 3 THEN(Sum([Project].EstimateCost) * @rateUSD) " +
                                            " ELSE Sum([Project].EstimateCost) END AS EstimateCostVND" +
                                            " FROM[dbo].[Project]" +
                                            "         LEFT JOIN[dbo].[Member] ON[Member].ProjectId = [Project].Id" +
                                            " WHERE[Project].ProjectStatusId = 5  and[Member].ProjectPositionId = 1" +
                                            " GROUP BY[Project].Id,[Project].ProjectName ,[Member].EmpId ,[Project].EstimateCostCurrencyId" +
                                            " ) as ProjectLeader" +
                                    " ON ProjectLeader.Id = [AnnualBonus].ProjectId" +
                                    " and ProjectLeader.EmpId = [AnnualBonus].EmpId" +
                                    " LEFT JOIN(" +
                                            " SELECT[ProjectMaintenance].Id, [ProjectMaintenance].MaintenanceName , [Member].EmpId" +
                                            " , CASE" +
                                            " WHEN [ProjectMaintenance].EstimateCostCurrencyId = 1 THEN (Sum([ProjectMaintenance].EstimateCost) * @rateYen)" +
                                            " WHEN[ProjectMaintenance].EstimateCostCurrencyId = 3 THEN(Sum([ProjectMaintenance].EstimateCost) * @rateUSD) " +
                                            " ELSE Sum([ProjectMaintenance].EstimateCost) END AS EstimateCostVND" +
                                            " FROM[dbo].[Project]" +
                                            "         LEFT JOIN[dbo].[ProjectMaintenance] ON[ProjectMaintenance].ProjectId = [Project].Id" +
                                            " LEFT JOIN[dbo].[Member] ON[Member].ProjectId = [Project].Id" +
                                            " WHERE[ProjectMaintenance].MaintenanceStatusId = 5 and[Member].ProjectPositionId = 1" +
                                            " GROUP BY[ProjectMaintenance].Id,[ProjectMaintenance].MaintenanceName ,[Member].EmpId ,[ProjectMaintenance].EstimateCostCurrencyId" +
                                            " ) as ProjectMaintenanceLeader" +
                                    " ON ProjectMaintenanceLeader.Id = [AnnualBonus].ProjectMaintenanceId" +
                                    " and ProjectMaintenanceLeader.EmpId = [AnnualBonus].EmpId" +
                                   
                                    " group by[AnnualBonus].[EmpId],[Employee].EmpName";
                lst = await SqlMapper.QueryAsync<AnnualBonusListDto>(Connection, sql2, new { year = year, rateYen= rateYen, rateUSD= rateUSD }, transaction: Transaction);

            return lst;
        }
        public async Task<IEnumerable<MemberBonusDto>> GetListMemberBonusAsync(int year)
        {
            var sql1 = " SELECT [Member].EmpId ,[Employee].EmpName " +
                        " FROM(SELECT * FROM [dbo].[Project] WHERE[Project].ProjectStatusId = 5) AS[Project] " +
                        " INNER JOIN(SELECT* FROM  [dbo].[Member] WHERE[Member].ProjectPositionId = 1) AS[Member] " +
                        " ON[Member].ProjectId = [Project].Id " +
                        " LEFT JOIN[dbo].[Employee] ON[Employee].Id = [Member].EmpId " +
                        " WHERE YEAR([Project].EndDate) = @year " +
                        " GROUP BY[Member].EmpId,[Employee].EmpName " +
                        " UNION " +
                         " SELECT [Member].EmpId ,[Employee].EmpName " +
                        " FROM(SELECT * FROM [dbo].[ProjectMaintenance] WHERE [ProjectMaintenance].MaintenanceStatusId = 5) AS [ProjectMaintenance] " +
                        " INNER JOIN ( SELECT* FROM  [dbo].[Member] WHERE [Member].ProjectPositionId = 1) AS[Member] " +
                        " ON [Member].ProjectMaintenanceId = [ProjectMaintenance].Id " +
                        " LEFT JOIN [dbo].[Employee] ON [Employee].Id = [Member].EmpId " +
                        " WHERE YEAR([ProjectMaintenance].EndDate) = @year " +
                        " GROUP BY[Member].EmpId,[Employee].EmpName ";
            return await SqlMapper.QueryAsync<MemberBonusDto>(Connection, sql1, new { year = year }, transaction: Transaction);
        }

        public async Task<AnnualBonusReportDto> GetBonusReportAsync(int year, int rateYen, int rateUSD, int empId)
        {
            var sql1 = " SELECT  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No  " +
                        " , CASE WHEN[AnnualBonus].ProjectId is null THEN[ProjectMaintenance].MaintenanceName  " +
                        "   ELSE[Project].ProjectName END as ProjectName  " +
                        " , [AnnualBonus].EmpId  " +
                        " ,[AnnualBonus].Day  " +
                        " ,[AnnualBonus].Year  " +
                        " ,[AnnualBonus].Bonus  " +
                        " FROM [dbo].[AnnualBonus]  " +
                        "         LEFT JOIN [dbo].[Employee] ON[Employee].Id = [AnnualBonus].EmpId  " +
                        " LEFT JOIN (SELECT[Project].Id, [Project].ProjectName , [Task].EmpId, SUM([Task].Duration) as Duration  " +
                        " FROM [dbo].[Project]  " +
                        "         LEFT JOIN [dbo].[Task] ON [Task].ProjectId = [Project].Id  " +
                        " WHERE [Project].ProjectStatusId = 5  " +
                        "             GROUP BY [Project].Id,[Project].ProjectName ,[Task].EmpId  " +
                        " 			) as [Project]  " +
                        "         ON [Project].Id = [AnnualBonus].ProjectId  " +
                        " LEFT JOIN (SELECT[ProjectMaintenance].Id, [ProjectMaintenance].MaintenanceName , [Task].EmpId, SUM([Task].Duration) as Duration  " +
                        " FROM [dbo].[ProjectMaintenance]  " +
                        "         LEFT JOIN [dbo].[Task] ON [Task].ProjectMaintenanceId = [ProjectMaintenance].Id  " +
                        " WHERE [ProjectMaintenance].MaintenanceStatusId = 5  " +
                        "             GROUP BY [ProjectMaintenance].Id,[ProjectMaintenance].MaintenanceName , [Task].EmpId   " +
                        " 			) as [ProjectMaintenance]  " +
                        "         ON [ProjectMaintenance].Id = [AnnualBonus].ProjectMaintenanceId " +
                        " WHERE [AnnualBonus].empId = @empId AND  [AnnualBonus].year = @year";
            var lst1 = await SqlMapper.QueryAsync<AnnualBonusMEMBERDtoDetailDto>(Connection, sql1, new { empId  = empId, year = year },  transaction: Transaction);

            var sql2 = " SELECT  CONCAT(CAST([AnnualRatingFactor].RatingFactorMember*100 AS INT),'%') AS RatingFactor  " +
                        " ,(SELECT WorkDayMonth FROM[dbo].[MstWorkDayMonth] WHERE  Year(MstWorkDayMonth.ApproveDate) = [AnnualBonus].Year) as DayWorkInMonth  " +
                        " , [AnnualBonus].EmpId  " +
                        " , [AnnualBonus].Year  " +
                        " ,[Salary].Salary  " +
                        " ,SUM([AnnualBonus].Bonus) as TotalBonus ,[Employee].EmpName " +
                        " FROM [dbo].[AnnualBonus]  " +
                        "         LEFT JOIN [dbo].[AnnualRatingFactor] ON [AnnualRatingFactor].Id = [AnnualBonus].RatingFactorId  " +
                        " LEFT JOIN [dbo].[Employee] ON[Employee].Id = [AnnualBonus].EmpId  " +
                        " LEFT JOIN [dbo].[Salary] ON [Salary].Id = [AnnualBonus].SalaryId  " +
                        " WHERE [AnnualBonus].empId = @empId  AND  [AnnualBonus].year = @year " +
                        " GROUP BY [AnnualRatingFactor].RatingFactorMember, [AnnualBonus].EmpId, [AnnualBonus].Year,[Salary].Salary ,[Employee].EmpName";
            var lst2 = await SqlMapper.QuerySingleOrDefaultAsync<AnnualBonusMEMBERDtoTotalDto>(Connection, sql2, new { empId = empId, year = year }, transaction: Transaction);

            var sql3 = " SELECT  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No  " +
                        " ,[Project].ProjectName  " +
                        " ,[Project].EstimateCost  " +
                        " ,[Project].EstimateCostCurrencyId  " +
                        " ,[Member].EmpId  " +
                        " ,@year AS YEAR  " +
                        " , CASE WHEN[Project].EstimateCostCurrencyId = 1   " +
                        "         THEN cast([AnnualRatingFactor].RatingFactorLeader* ([Project].EstimateCost* @rateYen) AS int)  " +
                        "       WHEN[Project].EstimateCostCurrencyId = 3   " +
                        "         THEN cast([AnnualRatingFactor].RatingFactorLeader* ([Project].EstimateCost* @rateUSD) AS int)  " +
                        "       ELSE cast(0.2 * ([Project].EstimateCost) AS int) END AS bonus  " +
                        " FROM(SELECT* FROM  [dbo].[Project] WHERE [Project].ProjectStatusId = 5) AS[Project]  " +
                        " INNER JOIN(SELECT* FROM  [dbo].[Member] WHERE [Member].ProjectPositionId = 1) AS[Member]  " +
                        " ON [Member].ProjectId = [Project].Id  " +
                        " LEFT JOIN[dbo].[Employee] ON [Employee].Id = [Member].EmpId  " +
                        " LEFT JOIN[dbo].[AnnualRatingFactor] ON [AnnualRatingFactor].EmpId = [Member].EmpId AND [AnnualRatingFactor].Year = @year  " +
                        " WHERE [Member].EmpId IS NOT NULL  and [Member].empId = @empId ";
            var lst3 = await SqlMapper.QueryAsync<AnnualBonusLEADERDetailDto>(Connection, sql3, new { year = year, rateYen = rateYen , rateUSD= rateUSD, empId = empId }, transaction: Transaction);

            var sql4= " SELECT @rateYen AS RateYen, @rateUSD AS RateUSD,CONCAT(CAST(AAA.RatingFactorLeader*100 AS INT),'%') AS RatingFactorLeader " +
                        " , AAA.EmpId  ,AAA.EmpName " +
                        " ,SUM(AAA.BONUS) AS TotalBonus  " +
                        " FROM  " +
                        " (SELECT ISnull( [AnnualRatingFactor].RatingFactorLeader ,0) as RatingFactorLeader , " +
                        " [Member].EmpId   ,[Employee].EmpName" +
                        " , CASE WHEN[Project].EstimateCostCurrencyId = 1 THEN cast([AnnualRatingFactor].RatingFactorLeader * ([Project].EstimateCost * @rateYen) AS int)  " +
                        "       WHEN[Project].EstimateCostCurrencyId = 3 THEN cast([AnnualRatingFactor].RatingFactorLeader * ([Project].EstimateCost * @rateUSD) AS int)  " +
                        "       ELSE cast([AnnualRatingFactor].RatingFactorLeader * ([Project].EstimateCost) AS int) END  AS BONUS  " +
                        " FROM (SELECT * FROM [dbo].[Project] WHERE [Project].ProjectStatusId = 5) AS [Project]  " +
                        " INNER JOIN (SELECT * FROM [dbo].[Member] WHERE [Member].ProjectPositionId = 1) AS [Member]  " +
                        " ON [Member].ProjectId = [Project].Id  " +
                        " LEFT JOIN [dbo].[Employee] ON [Employee].Id = [Member].EmpId  " +
                        " LEFT JOIN [dbo].[AnnualRatingFactor] ON [AnnualRatingFactor].EmpId = Member.EmpId AND [AnnualRatingFactor].Year = @year  " +
                        " ) AS AAA  " +
                        " WHERE AAA.EmpId IS NOT NULL  and AAA.empId = @empId " +
                        " GROUP BY AAA.EmpId, AAA.RatingFactorLeader,AAA.EmpName ";

            var lst4 = await SqlMapper.QuerySingleOrDefaultAsync<AnnualBonusLEADERTotalDto>(Connection, sql4, new { year = year, rateYen = rateYen, rateUSD = rateUSD, empId = empId }, transaction: Transaction);

            var result = new AnnualBonusReportDto()
            {
                AnnualBonusMEMBERDtoDetailDto = lst1,
                AnnualBonusMEMBERDtoTotalDto = lst2,
                AnnualBonusLEADERDetailDto = lst3,
                AnnualBonusLEADERTotalDto = lst4
            };
            return result;
        }
        #endregion
    }
}