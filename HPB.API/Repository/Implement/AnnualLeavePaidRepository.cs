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
    internal class AnnualLeavePaidRepository : RepositoryBase, IAnnualLeavePaidRepository
    {
        public AnnualLeavePaidRepository()
        {

        }
        //check exit AnnualLeavePaid by year
        public async Task<int> checkYearAnnualLeavePaid(int year)
        {
            var sql0 = "select count(*) from AnnualLeavePaid where year =@year";
            return await SqlMapper.QuerySingleOrDefaultAsync<int>(Connection, sql0,new { year = year}, transaction: Transaction);
        }
        //list AnnualLeavePaid & DayOff
        public async Task<IEnumerable<AnnualLeavePaidListDto>> AllAnnualLeavePaidAsync(int year)
        {
            var sql = "select ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No  , Employee.Id as EmpId,Employee.EmpName,AnnualLeavePaid.year,AnnualLeavePaid.DayRemainLastYear, AnnualLeavePaid.DayCurrentYear " +
                   ", ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float))/ 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 1  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Jan' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 2  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Feb' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 3  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Mar' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 4  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Apr' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 5  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'May' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 6  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Jun' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 7  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Jul' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 8  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Aug' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 9  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Sep' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 10  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Oct' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 11  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Nov' " +
                   ",ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year and MONTH([DayOff].DayOff) = 12  and [DayOff].EmpId =Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0)as 'Dec' " +
                   ", CASE WHEN  EmpStatusId = 1 and (DATEDIFF(day,EmpStartDate, GETDATE())/365) < 1 "+
                     " THEN DATEDIFF(MONTH, EmpStartDate, GETDATE())-ISNULL(DayOff.DayOff, 0)" +
                     " ELSE((AnnualLeavePaid.DayRemainLastYear + AnnualLeavePaid.DayCurrentYear) - ISNULL(DayOff.DayOff, 0))" +
                     " END as 'TotalDay' , Employee.EmpStartDate,Employee.EmpEndDate " +
                   "from " +
                     "(select* from Employee where Employee.PosId not in (1,2) " +
                       " and YEAR([EmpStartDate]) <=  @year " +
                        " AND (YEAR([EmpEndDate]) >=  @year OR [EmpEndDate] is null)" +
                     "  ) as Employee " +
                   "LEFT JOIN  " +
                  "(select * from[AnnualLeavePaid] where year = @year) as AnnualLeavePaid " +
                   "on Employee.Id = AnnualLeavePaid.EmpId " +
                   "LEFT JOIN  " +
                   "(SELECT YEAR([DayOff].DayOff) As[YEAR],SUM(CAST(DayOff.TimeOff as float))/ 8 As DayOff, EmpId FROM DayOff where YEAR([DayOff].DayOff) = @year GROUP BY YEAR([DayOff].DayOff),EmpId) as DayOff " +
                   "on Employee.Id = DayOff.EmpId " +
                 "order by Employee.PosId,Employee.DeptId ";
            return await SqlMapper.QueryAsync<AnnualLeavePaidListDto>(Connection, sql,new { year = year }, transaction: Transaction);
        }

        //create AnnualLeavePaid for  year
        public async Task<int> InsertAnnualLeavePaidAsync(int year)
        {
            var sql = "select Employee.Id as EmpId , CurrentDay.DayCurrentYear  " +
                        ",CASE WHEN lastDay.DayRemainLastYear > 12 THEN  12  WHEN lastDay.DayRemainLastYear is null THEN  0  ELSE lastDay.DayRemainLastYear END AS DayRemainLastYear " +
                        ",@year as year " +
                        "from " +
                        "(select* from Employee where Employee.PosId not in (1,2) " +
                               " and YEAR([EmpStartDate]) <=  @year " +
                                " AND (YEAR([EmpEndDate]) >=  @year OR [EmpEndDate] is null)" +
                             "  ) as Employee " +
                        "LEFT JOIN  " +
                        "(" +
                        " (select 12 + (DATEDIFF(year, EmpStartDate, GETDATE())/5)  as DayCurrentYear, Id from Employee where EmpStatusId = 1 and (DATEDIFF(year, EmpStartDate, GETDATE())) >= 5) " +
                        " union" +
                        "(select '12' as DayCurrentYear, Id from Employee where EmpStatusId = 1 and (DATEDIFF(year, EmpStartDate, GETDATE())) >= 1 and (DATEDIFF(year, EmpStartDate, GETDATE())) < 5 )" +
                        "union " +
                        "(select '0' as DayCurrentYear, Id from Employee where EmpStatusId = 1 and (DATEDIFF(year, EmpStartDate, GETDATE())) < 1 )  " +
                        "union " +
                        "(select '0' as DayCurrentYear, Id from Employee where EmpStatusId<> 1 ) " +
                        ") CurrentDay on CurrentDay.Id = Employee.Id " +
                        "LEFT JOIN  " +
                        " (select AnnualLeavePaid.EmpId, ((AnnualLeavePaid.DayRemainLastYear+AnnualLeavePaid.DayCurrentYear)-ISNULL(DayOff.DayOff,0)) as DayRemainLastYear " +
                         "  from " +
                         "  (select* from [AnnualLeavePaid] where year = @lastyear) as AnnualLeavePaid " +
                         " LEFT JOIN  " +
                        " (SELECT YEAR([DayOff].DayOff) As[YEAR],ISNULL(SUM([DayOff].TimeOff)/8,0) As DayOff, EmpId FROM DayOff where YEAR([DayOff].DayOff) =  @lastyear GROUP BY YEAR([DayOff].DayOff),EmpId) as DayOff " +
                        "on AnnualLeavePaid.EmpId = DayOff.EmpId " +
                        "  )lastDay on lastDay.EmpId = Employee.Id";

                  var lst =  await SqlMapper.QueryAsync<AnnualLeavePaidDto>(Connection, sql, new { year = year, lastyear  = year-1}, transaction: Transaction);
            var result = 0;
            foreach (var item in lst)
            {
                var sql2 = "select * from[AnnualLeavePaid] where year = @year And EmpId =@empId ";
                var lst2 = await SqlMapper.QueryAsync<AnnualLeavePaid>(Connection, sql2, new { year = item.year, empId = item.EmpId}, transaction: Transaction);
                if (!lst2.Any())
                {
                    var sql1 = "" +
                   " INSERT INTO [dbo].[AnnualLeavePaid] " +
                      " ([EmpId] " +
                      " ,[DayCurrentYear] " +
                      " ,[DayRemainLastYear] " +
                      " ,[year] " +
                        " ,[CreatedId] " +
                       " ,[CreatedDate]) " +
                       " VALUES " +
                      " (@EmpId" +
                      " ,@DayCurrentYear" +
                      " ,@DayRemainLastYear" +
                      " ,@year" +
               " ,@CreatedId " +
                    " ,GETDATE() )";
                    result += await Connection.ExecuteAsync(sql1, item, Transaction);
                }
            }
            return result;
        }
        public async Task<int> GetMaxYear(int year, int empId)
        {
            var sql0 = "select Max([AnnualLeavePaid].year) from AnnualLeavePaid";
            return  await SqlMapper.QuerySingleOrDefaultAsync<int>(Connection, sql0, transaction: Transaction);
        }

        public async Task<int> UpdateDayRemainAsync(int year,int empId,int maxYear)
        {
            var x = 0;
            for (int i = year; i < maxYear; i++)
            {
                var sql = "select  ((AnnualLeavePaid.DayRemainLastYear+AnnualLeavePaid.DayCurrentYear)-ISNULL(DayOff.DayOff,0)) as DayRemainLastYear  " +
                        "from  " +
                        "(  " +
                        "(select * from[AnnualLeavePaid] where year = @year) as AnnualLeavePaid  " +
                        "LEFT JOIN   " +
                        "(SELECT YEAR([DayOff].DayOff) As[YEAR],ISNULL(SUM([DayOff].TimeOff) / 8,0) As DayOff, EmpId FROM DayOff where YEAR([DayOff].DayOff) = @year GROUP BY YEAR([DayOff].DayOff),EmpId) as DayOff  " +
                        " on AnnualLeavePaid.EmpId = DayOff.EmpId  " +
                        ")   " +
                        "where AnnualLeavePaid.EmpId = @empId";

                var DayRemainLastYear = await SqlMapper.QuerySingleOrDefaultAsync<AnnualLeavePaidDto>(Connection, sql, new { year = i, empId = empId }, transaction: Transaction);

                var sql1 = " UPDATE [dbo].[AnnualLeavePaid] " +
                    "  SET DayRemainLastYear = @DayRemainLastYear " +
                     "  ,[UpdatedDate] = GetDate() " +
                    "  WHERE  AnnualLeavePaid.EmpId = @empId" +
                    " and year =@year";

                x += await Connection.ExecuteAsync(sql1, new
                {
                    DayRemainLastYear = DayRemainLastYear.DayRemainLastYear,
                    empId = empId,
                    year = i+1,
                }, Transaction);
            }
            return x;
        }


        #region DayOff
        //list all not paging
        public async Task<IEnumerable<DayOffDto>> GetAllDayOffAsync(string yearMonth)
        {
            var sql = " SELECT  DayOff.Id,DayOff.DayOff,Reason,TimeOff,ApprovedBy,ApprovedDate,Approved.EmpName as ApprovedName,EmpId,Employee.EmpName " +
                      "   from [DayOff]" +
                      "  LEFT JOIN  Employee as Approved on Approved.Id = [DayOff].ApprovedBy" +
                      "  LEFT JOIN  Employee on Employee.Id = [DayOff].EmpId" +
                      " where SUBSTRING(convert(varchar, DayOff.[DayOff], 112), 1, 6) = @yearMonth" +
                      "  order by DayOff.[DayOff] desc ";
            return await SqlMapper.QueryAsync<DayOffDto>(Connection, sql, new { yearMonth = yearMonth }, transaction: Transaction);
        }
        //byID
        public async Task<DayOffDto> GetDayOffByIdAsync(int id)
        {
            var sql = " SELECT   DayOff.Id,DayOff.DayOff,Reason,TimeOff,ApprovedBy,ApprovedDate,Approved.EmpName as ApprovedName,EmpId,Employee.EmpName" +
                        "   from [DayOff]" +
                      "  LEFT JOIN  Employee as Approved on Approved.Id = [DayOff].ApprovedBy" +
                      "  LEFT JOIN  Employee on Employee.Id = [DayOff].EmpId" +
                      " where DayOff.[Id] = @Id";
            return await SqlMapper.QuerySingleOrDefaultAsync<DayOffDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
        //insert
        public async Task<int> InsertDayOffAsync(DayOffDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[DayOff] " +
                    " ([DayOff] " +
                    " ,[Reason] " +
                    " ,[TimeOff] " +
                    " ,[ApprovedBy] " +
                    " ,[ApprovedDate] " +
                    " ,[EmpId] " +
                     " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@DayOff" +
                    " ,@Reason" +
                    " ,@TimeOff" +
                    " ,@ApprovedBy" +
                    " ,@ApprovedDate" +
                    " ,@EmpId" +
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var result = await Connection.ExecuteAsync(sql, new
            {
                DayOff = dto.DayOff1,
                Reason = dto.Reason,
                TimeOff = dto.TimeOff,
                ApprovedDate = dto.ApprovedDate,
                ApprovedBy = dto.ApprovedBy,
                EmpId = dto.EmpId,
                CreatedId = dto.CreatedId
            }, Transaction);
            return result;
        }
        //update
        public async Task<int> UpdateDayOffAsync(DayOffDto dto)
        {
            var sql = " UPDATE [dbo].[DayOff] " +
                     "  SET DayOff.[DayOff] = @DayOff " +
                     "     ,[Reason] = @Reason " +
                     "     ,[TimeOff] = @TimeOff " +
                     "     ,[ApprovedBy] = @ApprovedBy " +
                      "     ,[ApprovedDate] = @ApprovedDate " +
                       "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  DayOff.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                DayOff = dto.DayOff1,
                Reason = dto.Reason,
                TimeOff = dto.TimeOff,
                ApprovedBy = dto.ApprovedBy,
                ApprovedDate = dto.ApprovedDate,
                UpdatedId =dto.UpdatedId
            }, Transaction);
            return x;
        }
        //delete
        public async Task<int> DeleteDayOffAsync(int id)
        {
            var sql = " DELETE [dbo].[DayOff] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }
        #endregion

        #region Report 
        public async Task<AnnualLeavePaidReportDto> GetAnnualLeavePaidReportAsync(string yearMonth)
        {
            var sql1 = " DECLARE @startDate DATETIME= @month + '/' + '01/' +  @year " +
                        " ; WITH Calender AS " +
                        "  ( " +
                        "      SELECT @startDate AS CalanderDate " +
                        "      UNION ALL " +
                        "      SELECT CalanderDate + 1 FROM Calender " +
                        "      WHERE CalanderDate + 1 <= DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, @startDate) + 1, 0)) " +
                        "  )" +
                        " SELECT CONVERT(VARCHAR(10),CalanderDate,25) as DayOff,CAST(DayOff.TimeOff as float) / 8 as countDayOff, DayOff.EmpId " +
                        " , Holiday.Holiday"  +
                        " FROM Calender " +
                        " LEFT JOIN DayOff ON DayOff = CONVERT(VARCHAR(10), CalanderDate, 25) " +
                        "  LEFT JOIN Holiday ON Holiday  = CalanderDate " +
                        " OPTION(MAXRECURSION 0) ";

            var lst1 = await SqlMapper.QueryAsync<AnnualLeavePaidDayOffDto>(Connection, sql1, new { month = yearMonth.Substring(4,2), year = yearMonth.Substring(0, 4) }, transaction: Transaction);
            var sql2 = " SELECT ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No  ,[Employee].EmpName ,[AnnualLeavePaid].EmpId,[Employee].EmpStartDate,[Employee].EmpEndDate " +
                        " , [AnnualLeavePaid].DayRemainLastYear + [AnnualLeavePaid].DayCurrentYear as DayRemainLast " +
                        " ,ISNULL((SELECT SUM(CAST(DayOff.TimeOff as float)) / 8  FROM DayOff where YEAR([DayOff].DayOff) = @year  and Month([DayOff].DayOff) = @month   and [DayOff].EmpId = Employee.Id  GROUP BY YEAR([DayOff].DayOff),MONTH([DayOff].DayOff),EmpId),0) as countDayOff " +
                        " ,CASE WHEN  EmpStatusId = 1 and(DATEDIFF(day, EmpStartDate, GETDATE()) / 365) < 1 " +
                        "                    THEN DATEDIFF(MONTH, EmpStartDate, GETDATE())-ISNULL(DayOff.DayOff, 0) " +
                        "                       ELSE((AnnualLeavePaid.DayRemainLastYear + AnnualLeavePaid.DayCurrentYear) - ISNULL(DayOff.DayOff, 0)) " +
                        "                       END as TotalDay " +
                        "FROM " +
                        "(select* from Employee where Employee.PosId not in (1,2) " +
                               " and (SUBSTRING(convert(varchar, EmpStartDate, 112), 1, 6) <= @yearMonth) " +
                                " AND (SUBSTRING(convert(varchar, EmpEndDate, 112), 1, 6) >=  @yearMonth  OR [EmpEndDate] is null)" +
                             "  ) as Employee " +
                        " LEFT JOIN [dbo].[AnnualLeavePaid] " +
                        " ON [Employee].Id = [AnnualLeavePaid].EmpId " +
                        " LEFT JOIN(SELECT YEAR([DayOff].DayOff) As[YEAR],SUM(CAST (DayOff.TimeOff as float))/ 8 As DayOff, EmpId FROM DayOff where YEAR([DayOff].DayOff) = @year and Month([DayOff].DayOff) = @month GROUP BY YEAR([DayOff].DayOff),EmpId) as DayOff " +
                        " on Employee.Id = DayOff.EmpId" +
                        " WHERE [AnnualLeavePaid].year = @year";

            var lst2 = await SqlMapper.QueryAsync<AnnualLeavePaidResultDto>(Connection, sql2, new {  year = yearMonth.Substring(0, 4), month = yearMonth.Substring(4, 2),yearMonth = yearMonth }, transaction: Transaction);
            var sql3 = " DECLARE @startDate1 DATETIME= @month + '/' + '01/' +  @year " +
                      " ; WITH Calender AS " +
                      "  ( " +
                      "      SELECT @startDate1 AS CalanderDate " +
                      "      UNION ALL " +
                      "      SELECT CalanderDate + 1 FROM Calender " +
                      "      WHERE CalanderDate + 1 <= DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, @startDate1) + 1, 0)) " +
                      "  )" +
                      " SELECT CONVERT(VARCHAR(10),CalanderDate,25) as DayOff, SUM(CAST (DayOff.TimeOff as float)/8)  as TotalDayOff " +
                      " FROM Calender " +
                      " LEFT JOIN DayOff ON DayOff = CONVERT(VARCHAR(10), CalanderDate, 25) " +
                      " GROUP BY CalanderDate " +
                      " OPTION(MAXRECURSION 0) ";

            var lst3 = await SqlMapper.QueryAsync<AnnualLeavePaidTotalResultDto>(Connection, sql3, new { month = yearMonth.Substring(4, 2), year = yearMonth.Substring(0, 4) }, transaction: Transaction);
            var result = new AnnualLeavePaidReportDto()
            {
                AnnualLeavePaidDayOffDto = lst1,
                AnnualLeavePaidResultDto = lst2,
                AnnualLeavePaidTotalResultDto = lst3
            };
            return result;
        }

       
        #endregion

    }
}