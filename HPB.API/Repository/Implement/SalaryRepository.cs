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
    internal class SalaryRepository : RepositoryBase, ISalaryRepository
    {
        public SalaryRepository()
        {

        }

#region Salary
        public SalaryListDto[] GetAllSalaryAsQuerable(PagingRequest<SalaryFilterDto> paging)
        {
            var sql = " SELECT  Salary.Id,Salary,ApprovalDate,EmpId,EmpName " +
                     ", CASE (select count(*) from MonthlySalary where MonthlySalary.SalaryId = Salary.Id) WHEN 0 THEN 1 ELSE 0 END AS isEdit "+
                     " FROM " +
                     " ( SELECT  Salary.Id,Salary,ApprovalDate,EmpId, " +
                     " row_number() over(partition by EmpId order by ApprovalDate desc) as rn" +
                     " FROM Salary  )as Salary" +
                     " LEFT JOIN  Employee on Employee.Id = Salary.EmpId " +
                     " where rn = 1 " +
                     " order by Employee.PosId,Employee.DeptId ";
            var _Dto = SqlMapper.Query<SalaryListDto>(Connection, sql,transaction: Transaction);
            var query = _Dto;

            //search condition
            var filterDto = paging.Filters;
            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.EmpName))
                {
                    query = query.Where(emp => emp.EmpName.Contains(filterDto.EmpName));
                }
                if (filterDto.ApprovalDateStart.HasValue)
                {
                    query = query.Where(emp => emp.ApprovalDate >= filterDto.ApprovalDateStart);
                }
                if (filterDto.ApprovalDateEnd.HasValue)
                {
                    query = query.Where(emp => emp.ApprovalDate <= filterDto.ApprovalDateEnd);
                }
                if (filterDto.SalaryFrom.HasValue)
                {
                    query = query.Where(emp => emp.Salary >= filterDto.SalaryFrom);
                }
                if (filterDto.SalaryTo.HasValue)
                {
                    query = query.Where(emp => emp.Salary <= filterDto.SalaryTo);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(SalaryListDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<SalaryListDto>();
        }

        public async Task<SalaryListDto> GetSalaryByIdAsync(long id)
        {
            var sql = " SELECT  Salary.Id,Salary,ApprovalDate,EmpId,EmpName  " +
                      " FROM Salary  " +
                       " LEFT JOIN  Employee on Employee.Id = Salary.EmpId " +
                      " where Salary.Id = @Id";
            return await SqlMapper.QueryFirstOrDefaultAsync<SalaryListDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }

        public async Task<int> DeleteSalaryAsync(int id)
        {
            var sql = " DELETE [dbo].[Salary] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }

        public async Task<int> UpdateSalaryAsync(SalaryListDto dto)
        {
            var sql = " UPDATE [dbo].[Salary] " +
                     "  SET [Salary] = @Salary " +
                     "     ,[ApprovalDate] = @ApprovalDate " +
                     "     ,[EmpId] = @EmpId " +
                       "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  Salary.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                Salary = dto.Salary,
                ApprovalDate = dto.ApprovalDate,
                EmpId = dto.EmpId,
                UpdatedId = dto.UpdatedId
            }, Transaction);
            return x;
        }

        public async Task<int> InsertSalaryAsync(SalaryListDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[Salary] " +
                    " ([Salary] " +
                    " ,[ApprovalDate] " +
                    " ,[EmpId] " +
                     " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@Salary" +
                    " ,@ApprovalDate" +
                    " ,@EmpId" +
                     " ,@CreatedId " +
                    " ,GETDATE() )" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                Salary = dto.Salary,
                ApprovalDate = dto.ApprovalDate,
                EmpId = dto.EmpId,
                CreatedId = dto.CreatedId
            }, Transaction);
            return id;
        }

        #endregion

        #region MonthlySalary
       


        //check exists  Monthly Salary
        public Boolean CheckExistsMonthlySalary(string yearMonth)
        {
            var sql = " SELECT  Count(*)  " +
                      " FROM " +
                      " MonthlySalary" +
                      " WHERE YearMonth = @YearMonth" ;
            var _Dto = SqlMapper.QuerySingle<int>(Connection, sql,new { YearMonth  = yearMonth }, transaction: Transaction);
            if (_Dto.Equals(0))
                return false;
            return true;
            
        }

        //all for month
        public MonthlySalaryListDto[] GetAllMonthlySalaryAsQuerable(PagingRequest<MonthlySalaryFilterDto> paging)
        {
            var sql = " SELECT  MonthlySalary.Id,Salary.Salary,BonusOT,Allowance,Deduction,YearMonth" +
                      " ,MonthlySalary.EmpId,EmpName  " +
                      " FROM " +
                      " MonthlySalary" +
                      " LEFT JOIN  Employee on Employee.Id = MonthlySalary.EmpId " +
                      " LEFT JOIN  Salary on Salary.Id = MonthlySalary.SalaryId " +
                      " order by Employee.PosId,Employee.DeptId ";
            var _Dto = SqlMapper.Query<MonthlySalaryListDto>(Connection, sql, transaction: Transaction);
            var query = _Dto;

            //search condition
            var filterDto = paging.Filters;
            if (filterDto != null)
            {
                query = query.Where(emp => emp.YearMonth == filterDto.yearMonth);

                if (!string.IsNullOrEmpty(filterDto.EmpName))
                {
                    query = query.Where(emp => emp.EmpName.Contains(filterDto.EmpName));
                }
                if (filterDto.SalaryFrom.HasValue)
                {
                    query = query.Where(emp => emp.Salary >= filterDto.SalaryFrom);
                }
                if (filterDto.SalaryTo.HasValue)
                {
                    query = query.Where(emp => emp.Salary <= filterDto.SalaryTo);
                }
                
            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(MonthlySalaryListDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<MonthlySalaryListDto>();
        }

        //ceate
        public async Task<int> InsertMonthlySalaryAsync(string YearMonth)
        {
            var select = "select Salary.Id as SalaryId,null as BonusOT,null as Allowance,null as Deduction, @YearMonth as YearMonth,Employee.Id as EmpId" +
                " from(select * from Employee where Employee.EmpEndDate is  null ) as Employee" +
                " LEFT JOIN   (select * from Salary where Salary.ApprovalDate = (select Max(ApprovalDate) from Salary as s where s.EmpId = Salary.EmpId )) as Salary" +
                " on Employee.Id = Salary.EmpId";
            var _list = await SqlMapper.QueryAsync<MonthlySalaryDto>(Connection, select,new { YearMonth = YearMonth }, transaction: Transaction);
            var result = 0;
            foreach (var item in _list)
            {
                var sql = "" +
                " INSERT INTO [dbo].[MonthlySalary] " +
                   " ([SalaryId] " +
                   " ,[BonusOT] " +
                   " ,[Allowance] " +
                   " ,[Deduction] " +
                   " ,[YearMonth] " +
                   " ,[EmpId] " +
                     " ,[CreatedId] " +
                   " ,[CreatedDate]) " +
                    " VALUES " +
                   " (@SalaryId" +
                   " ,@BonusOT" +
                   " ,@Allowance" +
                   " ,@Deduction" +
                   " ,@YearMonth" +
                   " ,@EmpId" +
                   " ,@CreatedId " +
                   " ,GETDATE() )";
                 result += await Connection.ExecuteAsync(sql, item, Transaction);
            }
           
            return result;
        }

        //delete
        public async Task<int> DeleteMonthlySalaryAsync(string YearMonth)
        {
            var sql = " DELETE [dbo].[MonthlySalary] " +
                     " WHERE  YearMonth = @YearMonth";

            var x = await Connection.ExecuteAsync(sql, new
            {
                YearMonth = YearMonth
            }, Transaction);
            return x;
        }

        //update 
        public async Task<int> UpdateMonthlySalaryAsync(List<MonthlySalaryListDto> lst)
        {
            var x = 0;
            foreach (var item in lst)
            {
                var sql = " UPDATE [dbo].[MonthlySalary] " +
                    "  SET [SalaryId] = @SalaryId " +
                    "     ,[BonusOT] = @BonusOT " +
                     "     ,[Allowance] = @Allowance " +
                      "     ,[Deduction] = @Deduction " +
                        "     ,[YearMonth] = @YearMonth " +
                    "     ,[EmpId] = @EmpId " +
                     "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                    "  WHERE  Id = @Id";

                x += await Connection.ExecuteAsync(sql, new
                {
                    Id = item.Id,
                    SalaryId = item.SalaryId,
                    BonusOT = item.BonusOt,
                    Allowance = item.Allowance,
                    Deduction = item.Deduction,
                    YearMonth = item.YearMonth,
                    EmpId = item.EmpId,
                    UpdatedId=item.UpdatedId
                }, Transaction);
            }
            return x;
        }

        public async Task<int> UpdateBonusOTAsync(int empId,string yearMonth)
        {
            var _bonus = "SELECT sum([Wage]) from [MonthlyBonus] where EmpId =@empId and YearMonth = @yearMonth";
            var bonus = await SqlMapper.QuerySingleAsync(Connection,_bonus, new
            {
                empId = empId,
                yearMonth = yearMonth
            }, Transaction);
            var _ot = " select sum([Wage]) from [MonthlyOT] where [EmpId] =@empId and  SUBSTRING(convert(varchar, [OTDate], 112), 1, 6) =  @yearMonth";
            var ot = await SqlMapper.QuerySingleAsync(Connection, _ot, new
            {
                empId = empId,
                yearMonth = yearMonth
            }, Transaction);

            var sql = " UPDATE [dbo].[MonthlySalary] " +
                    "  SET [BonusOT] = @BonusOT " +
                     "  ,[UpdatedDate] = GetDate() " +
                    "  WHERE   EmpId =@empId and YearMonth = @yearMonth";

            var  x = await Connection.ExecuteAsync(sql, new
                {
                     empId = empId,
                     yearMonth = yearMonth,
                    BonusOT = bonus + ot,
            }, Transaction);
            return x;
        }

        #endregion

        #region MonthlyBonus  bonus image of photoshop

        // get wage bonus image of photoshop
        public async Task<decimal> GetWageBonusImage(int imageTypeId,int totalImage )
        {
            var sql1 = " SELECT TOP 1 UnitPrice FROM [dbo].[MstImageUnitPrice] WHERE [MstImageUnitPrice].Id = @imageTypeId ORDER BY ApproveDate DESC  ";
            var _UnitPrice =  await SqlMapper.QueryFirstAsync<decimal>(Connection, sql1, new { imageTypeId = imageTypeId }, transaction: Transaction);
            var _wage = _UnitPrice * totalImage;
            return _wage;
        }

        //get  total image bonus 
        public async Task<int> GetTotalImageBonus( int totalImageInMonth)
        {
            var sql1 = " SELECT TOP 1 TotalImageBasic FROM [dbo].[MstImageBasicMonth]  ORDER BY ApproveDate DESC   ";
            var _TotalImageBasic = await SqlMapper.QueryFirstAsync<int>(Connection, sql1, transaction: Transaction);
            return totalImageInMonth - _TotalImageBasic;
        }

        //list all not paging
        public async Task<IEnumerable<MonthlyBonusDto>> GetAllMonthlyBonusAsync(string yearMonth)
        {
            var sql = " SELECT  MonthlyBonus.Id , [Wage], [ImageUnitPriceId] ,[EmpId] ,[TotalImageBonus],[YearMonth],[Comment]" +
                       "   ,MstImageUnitPrice.ImageType,MstImageUnitPrice.UnitPrice" +
                        "   from [MonthlyBonus]" +
                        "  LEFT JOIN  MstImageUnitPrice on MstImageUnitPrice.Id = [MonthlyBonus].ImageUnitPriceId" +
                      " where MonthlyBonus.[YearMonth] = @yearMonth" +
                      "  order by Wage ";
            return await SqlMapper.QueryAsync<MonthlyBonusDto>(Connection, sql, new { yearMonth = yearMonth }, transaction: Transaction);
        }
        //byID
        public async Task<MonthlyBonusDto> GetMonthlyBonusByIdAsync(int id)
        {
            var sql = " SELECT  MonthlyBonus.Id , [Wage], [ImageUnitPriceId] ,[EmpId] ,[TotalImageBonus],[YearMonth],[Comment]" +
                       "   ,MstImageUnitPrice.ImageType,MstImageUnitPrice.UnitPrice" +
                        "   from [MonthlyBonus]" +
                        "  LEFT JOIN  MstImageUnitPrice on MstImageUnitPrice.Id = [MonthlyBonus].ImageUnitPriceId" +
                      " where MonthlyBonus.[Id] = @Id";
            return await SqlMapper.QuerySingleOrDefaultAsync<MonthlyBonusDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
        //insert
        public async Task<int> InsertMonthlyBonusAsync(MonthlyBonusDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[MonthlyBonus] " +
                    " ([Wage] " +
                    " ,[ImageUnitPriceId] " +
                    " ,[TotalImageBonus] " +
                    " ,[YearMonth] " +
                    " ,[Comment] " +
                    " ,[EmpId] " +
                     " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@Wage" +
                    " ,@ImageUnitPriceId" +
                    " ,@TotalImageBonus" +
                    " ,@YearMonth" +
                    " ,@Comment" +
                    " ,@EmpId"+
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var result = await Connection.ExecuteAsync(sql,new {
                Wage = dto.Wage,
                ImageUnitPriceId = dto.ImageUnitPriceId,
                TotalImageBonus = dto.TotalImageBonus,
                YearMonth = dto.YearMonth,
                EmpId = dto.EmpId,
                CreatedId =dto.CreatedId
            }, Transaction);
            return result;
        }
        //update
        public async Task<int> UpdateMonthlyBonusAsync(MonthlyBonusDto dto)
        {
            var sql = " UPDATE [dbo].[MonthlyBonus] " +
                     "  SET [Wage] = @Wage " +
                     "     ,[ImageUnitPriceId] = @ImageUnitPriceId " +
                     "     ,[TotalImageBonus] = @TotalImageBonus " +
                     "     ,[YearMonth] = @YearMonth " +
                     "     ,[Comment] = @Comment " +
                      "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  MonthlyBonus.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                Wage = dto.Wage,
                ImageUnitPriceId = dto.ImageUnitPriceId,
                TotalImageBonus = dto.TotalImageBonus,
                Comment = dto.Comment,
                YearMonth = dto.YearMonth,
                UpdatedId = dto.UpdatedId
            }, Transaction);
            return x;
        }
        //delete
        public async Task<int> DeleteMonthlyBonusAsync(int id)
        {
            var sql = " DELETE [dbo].[MonthlyBonus] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }
        #endregion

        #region MonthlyOT  OT of IT
        // get wage Overtime
        public async Task<decimal> GetWageOvertime(int empId, decimal timeOT,DateTime dateOT)
        {
            var sql1 = " SELECT TOP 1 [MstWorkDayMonth].WorkDayMonth FROM [dbo].[MstWorkDayMonth] ORDER BY ApproveDate DESC ";
            var sql2 = " SELECT TOP 1 [Salary].Salary FROM[dbo].[Salary] WHERE EmpId = @EmpId  ORDER BY[Salary].ApprovalDate DESC  ";
            var sql3 = " SELECT * FROM[dbo].[Holiday]   ";
            var sql4 = " SELECT a.* " +
                        " FROM MstOvertimeRate a " +
                        " INNER JOIN( " +
                        "     SELECT RateOTType, MAX(ApproveDate) ApproveDate " +
                        "     FROM MstOvertimeRate " +
                        "     GROUP BY RateOTType " +
                        " ) b ON a.RateOTType = b.RateOTType AND a.ApproveDate = b.ApproveDate  ";
            var _multi = await SqlMapper.QueryMultipleAsync(Connection, sql1+";"+ sql2+";" + sql3 + ";" + sql4, new { EmpId = empId }, transaction: Transaction);
            var _WorkDayMonth = _multi.ReadFirstOrDefault();
            var _Salary = _multi.ReadFirstOrDefault();
            var _lstHoliday = _multi.Read<Holiday>().ToList();
            var _lstMstOvertimeRate = _multi.Read<MstOvertimeRate>().ToList();
            var _wage = ((_Salary / _WorkDayMonth) / 8) * timeOT;
            //holiday
            if (_lstHoliday.Any(x => x.Holiday1 == dateOT))
            {
                _wage = _wage * _lstMstOvertimeRate.Where(x => x.RateOttype == 3).FirstOrDefault().RateOt;
            }
            //sunday - saturday
            else if (dateOT.DayOfWeek == DayOfWeek.Saturday || dateOT.DayOfWeek == DayOfWeek.Sunday)
            {
                _wage = _wage * _lstMstOvertimeRate.Where(x => x.RateOttype == 2).FirstOrDefault().RateOt;
            }
            else
            {
                _wage = _wage * _lstMstOvertimeRate.Where(x => x.RateOttype == 1).FirstOrDefault().RateOt;
            }
            return _wage;
        }

        //list all not paging
        public async Task<IEnumerable<MonthlyOTListDto>> GetAllMonthlyOTAsync(string yearMonth)
        {
            var sql = " select MonthlyOT.*,Employee.EmpName from (" +
                "  SELECT [MonthlyOT].[Id] ,[MonthlyOT].[ProjectId] as ProID ,[Project].ProjectName as ProName ,[OTDate]" +
                              "  ,[StartTime],[EndTime],[Wage],[EmpId]" +
                                "  FROM[HPB].[dbo].[MonthlyOT]" +
                          "  LEFT JOIN [dbo].[Project] on[MonthlyOT].ProjectId = [Project].Id" +
                        "  LEFT JOIN [dbo].[ProjectMaintenance] on[MonthlyOT].ProjectMaintenanceId = [ProjectMaintenance].Id" +
                        "  where[ProjectMaintenanceId] is null" +
                          "  union" +
                          "  SELECT[MonthlyOT].[Id] ,[ProjectMaintenanceId] as ProID ,[ProjectMaintenance].MaintenanceName  as ProName ,[OTDate]" +
                             "   ,[StartTime],[EndTime] ,[Wage],[EmpId]" +
                             "     FROM[HPB].[dbo].[MonthlyOT]" +
                             "     LEFT JOIN [dbo].[Project] on[MonthlyOT].ProjectId = [Project].Id" +
                        "  LEFT JOIN [dbo].[ProjectMaintenance] on[MonthlyOT].ProjectMaintenanceId = [ProjectMaintenance].Id" +
                        "  where[ProjectMaintenanceId] is not null" +
                        " ) as  MonthlyOT " +
                       " LEFT JOIN  Employee on Employee.Id = MonthlyOT.EmpId " +
                      " where SUBSTRING(convert(varchar, MonthlyOT.[OTDate], 112), 1, 6) = @yearMonth" +
                      "  order by MonthlyOT.OTDate  desc";
            return await SqlMapper.QueryAsync<MonthlyOTListDto>(Connection, sql, new { yearMonth = yearMonth }, transaction: Transaction);
        }
        //byID
        public async Task<MonthlyOTDto> GetMonthlyOTByIdAsync(int id)
        {
            var sql = " SELECT  MonthlyBonus.*" +
                       "   ,[ProjectMaintenance].MaintenanceName,[Project].ProjectName" +
                        "   from [MonthlyOT]" +
                          "  LEFT JOIN [dbo].[Project] on[MonthlyOT].ProjectId = [Project].Id" +
                         "  LEFT JOIN [dbo].[ProjectMaintenance] on[MonthlyOT].ProjectMaintenanceId = [ProjectMaintenance].Id" +
                      " where MonthlyOT.[Id] = @Id";
            return await SqlMapper.QuerySingleOrDefaultAsync<MonthlyOTDto>(Connection, sql, new { Id = id }, transaction: Transaction);
        }
        //insert
        public async Task<int> InsertMonthlyOTAsync(MonthlyOTDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[MonthlyOT] " +
                    " ([ProjectId] " +
                    " ,[ProjectMaintenanceId] " +
                    " ,[OTDate] " +
                    " ,[StartTime] " +
                    " ,[EndTime]" +
                    " ,[Wage]" +
                    " ,[EmpId] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@ProjectId" +
                    " ,@ProjectMaintenanceId" +
                    " ,@Otdate" +
                    " ,@StartTime" +
                    " ,@EndTime" +
                    " ,@Wage" +
                    " ,@EmpId " +
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var result = await Connection.ExecuteAsync(sql, new
            {
                Wage = dto.Wage,
                ProjectId = dto.ProjectId,
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Otdate = dto.Otdate,
                EmpId = dto.EmpId,
                CreatedId = dto.CreatedId
            }, Transaction);
            return result;
        }
        //update
        public async Task<int> UpdateMonthlyOTAsync(MonthlyOTDto dto)
        {
            var sql = " UPDATE [dbo].[MonthlyOT] " +
                     "  SET [Wage] = @Wage " +
                     "     ,[ProjectId] = @ProjectId " +
                      "     ,[ProjectMaintenanceId] = @ProjectMaintenanceId " +
                       "     ,[Otdate] = @Otdate " +
                      "     ,[StartTime] = @StartTime " +
                     "     ,[EndTime] = @EndTime " +
                        "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  MonthlyOT.Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                Wage = dto.Wage,
                ProjectId = dto.ProjectId,
                ProjectMaintenanceId = dto.ProjectMaintenanceId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Otdate = dto.Otdate,
                UpdatedId = dto.UpdatedId
            }, Transaction);
            return x;
        }
        //delete
        public async Task<int> DeleteMonthlyOTAsync(int id)
        {
            var sql = " DELETE [dbo].[MonthlyOT] " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id
            }, Transaction);
            return x;
        }

        #endregion

        #region Report 
        public async Task<MonthlySalaryReportDto> GetMonthlySalaryReportAsync(string yearMonth)
        {
            var sql1 = " SELECT ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No  ,  DayOff.TotalDay  as TotalDayOff, [Employee].EmpName,[MstPosition].PositionName "+
                        ",[Salary].Salary,[MonthlySalary].BonusOT,[MonthlySalary].Allowance,[MonthlySalary].Deduction " +
                        ", ( isnull([Salary].Salary,0) + isnull([MonthlySalary].BonusOT,0) +isnull([MonthlySalary].Allowance,0)-isnull([MonthlySalary].Deduction,0)) as ActualSalary " +
                        ",Comment = stuff((select  ', '+ Comment from MonthlyBonus where EmpId = [MonthlySalary].EmpId and YearMonth = @yearMonth for xml path('')),1,1,'') " +
                        "FROM[dbo].[MonthlySalary] " +
                        "        LEFT JOIN[dbo].[Salary] ON[MonthlySalary].[SalaryId] = [Salary].[Id] " +
                        "        LEFT JOIN[dbo].[Employee] ON[MonthlySalary].EmpId = [Employee].[Id] " +
                        "        LEFT JOIN[dbo].[MstPosition] ON[MstPosition].Id = [Employee].PosId " +
                        "LEFT JOIN( " +
                        "SELECT[AnnualLeavePaid].EmpId, " +
                                "CASE WHEN  EmpStatusId = 1 and (DATEDIFF(day, EmpStartDate, GETDATE())/365) < 1  THEN DATEDIFF(MONTH, EmpStartDate, GETDATE())-ISNULL(DayOff.DayOff, 0) " +
                        "           ELSE((AnnualLeavePaid.DayRemainLastYear + AnnualLeavePaid.DayCurrentYear) - ISNULL(DayOff.DayOff, 0)) " +
                        "      END as 'TotalDay'  " +
                        "  FROM[HPB].[dbo].[AnnualLeavePaid] " +
                        "        LEFT JOIN[dbo].[Employee] on[AnnualLeavePaid].EmpId = [Employee].Id " +
                        "LEFT JOIN(SELECT YEAR([DayOff].DayOff) As[YEAR],SUM([DayOff].TimeOff)/ 8 As DayOff, EmpId FROM DayOff " +

                        "                   WHERE YEAR([DayOff].DayOff) = @year GROUP BY YEAR([DayOff].DayOff),EmpId) as DayOff " +
                        "                   on Employee.Id = DayOff.EmpId ) as DayOff on DayOff.EmpId = [MonthlySalary].EmpId " +
                        "WHERE[MonthlySalary].YearMonth = @yearMonth ";

            var lst1 = await SqlMapper.QueryAsync<MonthlySalaryDetailReportDto>(Connection, sql1, new { yearMonth = yearMonth ,year = yearMonth.Substring(0,4) }, transaction: Transaction);
            var sql2 = " SELECT SUM([Salary].Salary) as TotalSalary , SUM([MonthlySalary].BonusOT) as TotalBonusOt " +
                            " ,SUM([MonthlySalary].Allowance) as TotalAllowance ,SUM([MonthlySalary].Deduction) as TotalDeduction" +
                            ",  SUM(( isnull([Salary].Salary,0) + isnull([MonthlySalary].BonusOT,0) +isnull([MonthlySalary].Allowance,0)-isnull([MonthlySalary].Deduction,0))) as TotalActualSalary" +
                        " FROM[dbo].[MonthlySalary] " +
                        " LEFT JOIN[dbo].[Salary] ON[MonthlySalary].[SalaryId] = [Salary].[Id] " +
                        "         WHERE[MonthlySalary].YearMonth =  @yearMonth " +
                        " GROUP BY[MonthlySalary].YearMonth";

            var lst2 = await SqlMapper.QuerySingleOrDefaultAsync<MonthlySalaryTotalReportDto>(Connection, sql2, new { yearMonth = yearMonth, year = yearMonth.Substring(0, 4) }, transaction: Transaction);

            var result = new MonthlySalaryReportDto()
            {
                detail = lst1,
                total = lst2
            };
            return result;
        }
        #endregion
    }


}