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
    internal class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository()
        {

        }

        public EmployeeListDto[] AllAsQuerable(PagingRequest<EmployeeListFilterDto> paging)
        {
            var filterDto = paging.Filters;
            var sql = " SELECT  [Employee].Id ,EmpName,EmpBirthday,EmpIdentityNo,EmpPassportNo," +
                               "EmpAddress,EmpMobile1,EmpEmail1,EmpStartDate" +
                               ",EmpGender,EmpStatusId " +
                               ",Employee.CreatedDate,Employee.UpdatedDate,Employee.DeptId,MstDepartment.DepartmentName" +
                               ",[MstEmplomentStatus].EmpStatus,PosId,[MstPosition].PositionName" +
                       " FROM  [Employee] " +
                       " LEFT JOIN   [MstEmplomentStatus] on [Employee].EmpStatusId = [MstEmplomentStatus].Id" +
                       " LEFT JOIN   [MstPosition] on [Employee].PosId = [MstPosition].Id" +
                       " LEFT JOIN   [MstDepartment] on [Employee].DeptId = [MstDepartment].Id" +
                       " WHERE IsDelete not in (1)" +
                       " ORDER BY  [Employee].PosId DESC";
            var _dto = SqlMapper.Query<EmployeeListDto>(Connection, sql,  transaction: Transaction);
            var query = _dto;

            //search condition

            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.EmpName))
                {
                    query = query.Where(emp => emp.EmpName.Contains(filterDto.EmpName));
                }
                if (filterDto.EmpStartDate.HasValue)
                {
                    query = query.Where(emp => emp.EmpStartDate >= filterDto.EmpStartDate);
                }
                if (filterDto.EmpEndDate.HasValue)
                {
                    query = query.Where(emp => emp.EmpStartDate <= filterDto.EmpEndDate);
                }
                if (!string.IsNullOrEmpty(filterDto.EmpAddress))
                {
                    query = query.Where(emp => emp.EmpAddress.Contains(filterDto.EmpAddress));
                }
                if (filterDto.EmpGender.HasValue)
                {
                    query = query.Where(emp => emp.EmpGender == filterDto.EmpGender);
                }
                if (filterDto.EmpStatusId.HasValue)
                {
                    query = query.Where(emp => emp.EmpStatusId == filterDto.EmpStatusId);
                }
                if (filterDto.DeptId.HasValue)
                {
                    query = query.Where(emp => emp.DeptId == filterDto.DeptId);
                }

            }

            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(EmployeeListDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<EmployeeListDto>();
        }

        public async Task<IEnumerable<ApprovedDto>> GetApprovedAsync()
        {
            var sql = " SELECT [MstPosition].PositionName,[Employee].EmpName,[Employee].Id,[Employee].PosId " +
                        " FROM[dbo].[Employee] " +
                        " LEFT JOIN[dbo].[MstPosition] ON[Employee].PosId = [MstPosition].Id " +
                        " WHERE[Employee].PosId <> 4  " +
                        " ORDER BY[Employee].PosId,[Employee].EmpName";
            return await SqlMapper.QueryAsync<ApprovedDto>(Connection, sql, transaction: Transaction);
        }

        public async Task<IEnumerable<EmployeeMemberDto>> GetByDeptAsync(int deptId)
        {
            var sql = " SELECT  [Employee].Id ,EmpName,EmpMobile1,EmpMobile2,EmpEmail1,EmpEmail2,EmpImage,EmpGender,EmpStatusId " +
                                ",[MstEmplomentStatus].EmpStatus,PosId,[MstPosition].PositionName" +
                      "  FROM  [Employee] " +
                      "  LEFT JOIN   [MstEmplomentStatus] on [Employee].EmpStatusId = [MstEmplomentStatus].Id" +
                      "  LEFT JOIN   [MstPosition] on [Employee].PosId = [MstPosition].Id" +
                      "  WHERE  [MstEmplomentStatus].Id <> 5  and DeptId = @DeptId" +
                      "  ORDER BY  [Employee].PosId DESC ";
            return await SqlMapper.QueryAsync<EmployeeMemberDto>(Connection, sql, new { DeptId = deptId }, transaction: Transaction);
        }

        public async Task<EmployeeDto> GetById(long id,string pathFolderImage)
        {
            var sql = " SELECT  [Employee].Id ,PosId,[MstPosition].PositionName" +
                            ",EmpName,EmpBirthday,EmpIdentityNo,EmpIdentityDate,EmpIdentityPlace,EmpPassportNo,EmpPassportDate,EmpPassportExpiryDate" +
                            ",EmpAddress,EmpAddressBirth,EmpMobile1,EmpMobile2,EmpEmail1,EmpEmail2,'" + pathFolderImage + "'+EmpImage as EmpImage" +
                            ",EmpStartDate,EmpEndDate,EmpGender,EmpComment,CreatedId,CreatedDate,UpdatedId,UpdatedDate" +
                            ",EmpStatusId,[MstEmplomentStatus].EmpStatus" +
                            ",DeptId,[MstDepartment].DepartmentName" +
                            ",LicensePlate,VehicleTypeId,[MstVehicleType].VehicleType,VehicleComment,RegularDate" +
                      "  FROM  [Employee] " +
                      "  LEFT JOIN   [MstEmplomentStatus] on [Employee].EmpStatusId = [MstEmplomentStatus].Id" +
                      "  LEFT JOIN   [MstPosition] on [Employee].PosId = [MstPosition].Id" +
                      "  LEFT JOIN   [MstDepartment] on [Employee].DeptId = [MstDepartment].Id" +
                      "  LEFT JOIN   [MstVehicleType] on [Employee].VehicleTypeId = [MstVehicleType].Id" +
                      "  WHERE   [Employee].Id = @id";
            return await SqlMapper.QueryFirstOrDefaultAsync<EmployeeDto>(Connection, sql, new { id = id }, transaction: Transaction);
        }

        public async Task<int> InsertEmployeeAsync(EmployeeDto dto)
        {
            var sql = " DECLARE @ID int;" +
                      " INSERT INTO [dbo].[Employee]" +
                        " ([PosId]" +
                        " ,[EmpName]" +
                        " ,[EmpBirthday]" +
                        " ,[EmpIdentityNo]" +
                        " ,[EmpIdentityDate]" +
                        " ,[EmpIdentityPlace]" +
                        " ,[EmpPassportNo]" +
                        " ,[EmpPassportDate]" +
                        " ,[EmpPassportExpiryDate]" +
                        " ,[EmpAddress]" +
                        " ,[EmpAddressBirth]" +
                        " ,[EmpMobile1]" +
                        " ,[EmpMobile2]" +
                        " ,[EmpEmail1]" +
                        " ,[EmpEmail2]" +
                        " ,[EmpImage]" +
                        " ,[EmpStartDate]" +
                        " ,[EmpEndDate]" +
                        " ,[EmpGender]" +
                        " ,[EmpComment]" +
                        " ,[EmpStatusId]" +
                        " ,[CreatedId]" +
                        " ,[CreatedDate]" +
                       "  ,[DeptId]" +
                       "  ,[LicensePlate]" +
                        " ,[VehicleTypeId]" +
                         " ,[RegularDate]" +
                         " ,[IsDelete]" +
                        " ,[VehicleComment])" +
                 " VALUES" +
                     "    (@PosId" +
                     "   ,@EmpName" +
                     "   ,@EmpBirthday" +
                     "   ,@EmpIdentityNo" +
                     "   ,@EmpIdentityDate" +
                      "   ,@EmpIdentityPlace" +
                     "   ,@EmpPassportNo" +
                     "   ,@EmpPassportDate" +
                     "   ,@EmpPassportExpiryDate" +
                     "   ,@EmpAddress" +
                     "   ,@EmpAddressBirth" +
                     "   ,@EmpMobile1" +
                     "   ,@EmpMobile2" +
                     "   ,@EmpEmail1" +
                     "   ,@EmpEmail2" +
                     "   ,@EmpImage" +
                     "   ,@EmpStartDate" +
                     "   ,@EmpEndDate" +
                     "   ,@EmpGender" +
                     "   ,@EmpComment" +
                     "   ,@EmpStatusId" +
                     "   ,@CreatedId" +
                     "   ,GETDATE() " +
                     "   ,@DeptId" +
                     "   ,@LicensePlate" +
                     "   ,@VehicleTypeId" +
                     "   ,@RegularDate " +
                     "    , 0" +
                     "   ,@VehicleComment)" +
                  " SET @ID = SCOPE_IDENTITY(); " +
                  "  SELECT  @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                PosId = dto.PosId,
                EmpName = dto.EmpName,
                EmpBirthday = dto.EmpBirthday,
                EmpIdentityNo = dto.EmpIdentityNo,
                EmpIdentityDate = dto.EmpIdentityDate,
                EmpIdentityPlace = dto.EmpIdentityPlace,
                EmpPassportNo = dto.EmpPassportNo,
                EmpPassportDate = dto.EmpPassportDate,
                EmpPassportExpiryDate = dto.EmpPassportExpiryDate,
                EmpAddress = dto.EmpAddress,
                EmpAddressBirth = dto.EmpAddressBirth,
                EmpMobile1 = dto.EmpMobile1,
                EmpMobile2 = dto.EmpMobile2,
                EmpEmail1 = dto.EmpEmail1,
                EmpEmail2 = dto.EmpEmail2,
                EmpImage = dto.EmpImage,
                EmpStartDate = dto.EmpStartDate,
                EmpEndDate = dto.EmpEndDate,
                EmpGender = dto.EmpGender,
                EmpComment = dto.EmpComment,
                EmpStatusId = dto.EmpStatusId,
                CreatedId = dto.CreatedId,
                DeptId = dto.DeptId,
                LicensePlate = dto.LicensePlate,
                VehicleTypeId = dto.VehicleTypeId,
                RegularDate = dto.RegularDate,
                VehicleComment = dto.VehicleComment,
            }, Transaction);
            return id;
        }
        public async Task<int> UpdateEmployeeAsync(EmployeeDto dto)
        {
            var sql = " UPDATE [dbo].[Employee] " +
                        " SET[PosId] = @PosId"+
                          " ,[EmpName] = @EmpName" +
                          " ,[EmpBirthday] = @EmpBirthday" +
                          " ,[EmpIdentityNo] = @EmpIdentityNo" +
                          " ,[EmpIdentityDate] = @EmpIdentityDate" +
                          " ,[EmpIdentityPlace] = @EmpIdentityPlace" +
                          " ,[EmpPassportNo] = @EmpPassportNo" +
                          " ,[EmpPassportDate] = @EmpPassportDate" +
                          " ,[EmpPassportExpiryDate] = @EmpPassportExpiryDate" +
                          " ,[EmpAddress] = @EmpAddress" +
                          " ,[EmpAddressBirth] = @EmpAddressBirth" +
                          " ,[EmpMobile1] = @EmpMobile1" +
                          " ,[EmpMobile2] = @EmpMobile2" +
                          " ,[EmpEmail1] = @EmpEmail1" +
                          " ,[EmpEmail2] = @EmpEmail2" +
                          " ,[EmpImage] = @EmpImage" +
                          " ,[EmpStartDate] = @EmpStartDate" +
                          " ,[EmpEndDate] = @EmpEndDate" +
                          " ,[EmpGender] = @EmpGender" +
                          " ,[EmpComment] = @EmpComment" +
                          " ,[EmpStatusId] = @EmpStatusId" +
                          " ,[UpdatedId] = @UpdatedId" +
                          " ,[UpdatedDate] =GetDate()" +
                          " ,[DeptId] = @DeptId" +
                          " ,[LicensePlate] = @LicensePlate" +
                          " ,[VehicleTypeId] = @VehicleTypeId" +
                          " ,[VehicleComment] = @VehicleComment" +
                          " ,[RegularDate] = @RegularDate " +
                          "  WHERE   Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = dto.Id,
                PosId = dto.PosId,
                EmpName = dto.EmpName,
                EmpBirthday = dto.EmpBirthday,
                EmpIdentityNo = dto.EmpIdentityNo,
                EmpIdentityDate = dto.EmpIdentityDate,
                EmpIdentityPlace = dto.EmpIdentityPlace,
                EmpPassportNo = dto.EmpPassportNo,
                EmpPassportDate = dto.EmpPassportDate,
                EmpPassportExpiryDate = dto.EmpPassportExpiryDate,
                EmpAddress = dto.EmpAddress,
                EmpAddressBirth = dto.EmpAddressBirth,
                EmpMobile1 = dto.EmpMobile1,
                EmpMobile2 = dto.EmpMobile2,
                EmpEmail1 = dto.EmpEmail1,
                EmpEmail2 = dto.EmpEmail2,
                EmpImage = dto.EmpImage,
                EmpStartDate = dto.EmpStartDate,
                EmpEndDate = dto.EmpEndDate,
                EmpGender = dto.EmpGender,
                EmpComment = dto.EmpComment,
                EmpStatusId = dto.EmpStatusId,
                UpdatedId = dto.UpdatedId,
                DeptId = dto.DeptId,
                LicensePlate = dto.LicensePlate,
                VehicleTypeId = dto.VehicleTypeId,
                VehicleComment = dto.VehicleComment,
                RegularDate =dto.RegularDate,
            }, Transaction);
            return x;
        }

        public async Task<int> DeleteEmployeeAsync(long id)
        {
            var sql = " UPDATE [dbo].[Employee] " +
                        " SET[IsDelete] = @IsDelete" +
                          "  WHERE   Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id,
                IsDelete = 1,
               
            }, Transaction);
            return x;
        }
        #region report 
        public async Task<EmployeeVehicleReportDto> GetEmployeeVehicleReportAsync(string yearMonth)
        {
            var sql1 = "  SELECT  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No,[EmpName],[LicensePlate] "+
                             " , CASE WHEN  VehicleTypeId = 1  THEN  '1' ELSE  '' END as VehicleType1 " +
                             " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 1) as ParkingFee1 " +
                             " , CASE WHEN  VehicleTypeId = 2  THEN  '1' ELSE '' END as VehicleType2 " +
                             " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 2) as ParkingFee2 " +
                             " ,[VehicleComment],Id " +
                        " FROM  [dbo].[Employee]  " +
                        " WHERE [PosId] not IN (1) " +
                        " AND SUBSTRING(convert(varchar, [EmpStartDate], 112), 1, 6) <  @yearMonth " +
                        " AND (SUBSTRING(convert(varchar, [EmpEndDate], 112), 1, 6) >=  @yearMonth OR [EmpEndDate] is null)";

            var lst1  = await SqlMapper.QueryAsync<EmployeeVehicleDetailReportDto>(Connection, sql1, new { yearMonth = yearMonth }, transaction: Transaction);
            

            var sql2 = "  SELECT  sum(AAA.VehicleType1) as  CountVehicleType1  " +
                             " ,(AAA.ParkingFee1 * sum(AAA.VehicleType1)) as TotalParkingFee1 " +
                               " ,AAA.ParkingFee1 " +
                             " ,sum(AAA.VehicleType2) as CountVehicleType2 " +
                             " , (AAA.ParkingFee2 * sum(AAA.VehicleType2)) as TotalParkingFee2 " +
                             " , AAA.ParkingFee2 " +
                             " ,(AAA.ParkingFee1 * sum(AAA.VehicleType1)) + (AAA.ParkingFee2 * sum(AAA.VehicleType2)) as totalFee " +
                        "  FROM  ( " +
                            "  SELECT  " +
                                "  CASE WHEN  VehicleTypeId = 1  THEN  1 ELSE 0 END as VehicleType1 " +
                                " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 1) as ParkingFee1 " +
                                " , CASE WHEN  VehicleTypeId = 2  THEN  1 ELSE 0  END as VehicleType2 " +
                                " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 2) as ParkingFee2 " +
                            "  FROM  [dbo].[Employee]  WHERE [PosId] not IN (1) " +
                            " AND SUBSTRING(convert(varchar, [EmpStartDate], 112), 1, 6) <  @yearMonth " +
                            " AND (SUBSTRING(convert(varchar, [EmpEndDate], 112), 1, 6) >=  @yearMonth OR [EmpEndDate] is null) " +
                        " ) as AAA " +
                        " group by AAA.ParkingFee1,AAA.ParkingFee2";
            var lst2 = await SqlMapper.QuerySingleOrDefaultAsync<EmployeeVehicleTotalReportDto>(Connection, sql2, new { yearMonth = yearMonth }, transaction: Transaction);
            if (lst2 == null)
            {
               
                var sql = "SELECT  *  FROM  MstVehicleType";
                var _lstmst = await SqlMapper.QueryAsync<MstVehicleType>(Connection, sql, transaction: Transaction);
                lst2 = new EmployeeVehicleTotalReportDto();
                lst2.ParkingFee1 = _lstmst.Where(x => x.Id == 1).FirstOrDefault().ParkingFee.Value;
                lst2.ParkingFee2 = _lstmst.Where(x => x.Id == 2).FirstOrDefault().ParkingFee.Value;
            }
            var result = new EmployeeVehicleReportDto()
            {
                VehicleDetail = lst1,
                VehicleTotal = lst2
            };
            return result;
        }

        public async Task<EmployeeVehicleReportDto> GetDownloadEmployeeVehicleReportAsync(IEnumerable<int> empIdString)
        {
            var inconditions = empIdString.Distinct().ToArray();
            var sql1 = "  SELECT  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No,[EmpName],[LicensePlate] " +
                             " , CASE WHEN  VehicleTypeId = 1  THEN  '1' ELSE '' END as VehicleType1 " +
                             " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 1) as ParkingFee1 " +
                             " , CASE WHEN  VehicleTypeId = 2  THEN  '1' ELSE '' END as VehicleType2 " +
                             " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 2) as ParkingFee2 " +
                             " ,[VehicleComment] , Id" +
                     "   FROM  [dbo].[Employee]   WHERE  [Id] IN @ids  ";

            var lst1 = await SqlMapper.QueryAsync<EmployeeVehicleDetailReportDto>(Connection, sql1, new { ids = inconditions }, transaction: Transaction);

            var sql2 = "  SELECT  sum(AAA.VehicleType1) as  CountVehicleType1  " +
                             " ,(AAA.ParkingFee1 * sum(AAA.VehicleType1)) as TotalParkingFee1 " +
                             " ,sum(AAA.VehicleType2) as CountVehicleType2 " +
                             " , (AAA.ParkingFee2 * sum(AAA.VehicleType2)) as TotalParkingFee2 " +
                             " ,(AAA.ParkingFee1 * sum(AAA.VehicleType1)) + (AAA.ParkingFee2 * sum(AAA.VehicleType2)) as totalFee " +
                         "  FROM  ( " +
                             "  SELECT  " +
                                 "  CASE WHEN  VehicleTypeId = 1  THEN  1 ELSE 0 END as VehicleType1 " +
                                 " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 1) as ParkingFee1 " +
                                 " , CASE WHEN  VehicleTypeId = 2  THEN  1 ELSE 0  END as VehicleType2 " +
                                 " ,( SELECT  ParkingFee  FROM  MstVehicleType  WHERE  Id = 2) as ParkingFee2 " +
                             "  FROM  [dbo].[Employee]   WHERE  [Id] IN @ids  " +
                             " ) as AAA " +
                         " group by AAA.ParkingFee1,AAA.ParkingFee2";
            var lst2 = await SqlMapper.QuerySingleOrDefaultAsync<EmployeeVehicleTotalReportDto>(Connection, sql2,
            new { ids = inconditions }, transaction: Transaction);
            var result = new EmployeeVehicleReportDto()
            {
                VehicleDetail = lst1,
                VehicleTotal = lst2
            };
            return result;
        }


        public async Task<EmployeeReportDto> GetEmployeeReportAsync(string yearMonth)
        {
            var sql1 = "  SELECT  Id, ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No " +
                             "  ,[EmpName],[EmpBirthday],[EmpIdentityNo],[EmpIdentityDate],[EmpIdentityPlace] " +
                             "  ,[EmpPassportNo],[EmpPassportDate],[EmpPassportExpiryDate] " +
                             "  ,[EmpAddressBirth],[EmpAddress],[EmpMobile1],[EmpEmail1],[EmpEmail2],[EmpStartDate],[EmpComment] " +
                        "  FROM[dbo].[Employee] " +
                        "  WHERE" +
                        "  ( SUBSTRING(convert(varchar, [RegularDate], 112), 1, 6) <= @yearMonth " +
                        "  AND (SUBSTRING(convert(varchar, [EmpEndDate], 112), 1, 6) > @yearMonth OR EmpEndDate is null))";

            var lst1 = await SqlMapper.QueryAsync<EmployeeRegularReportDto>(Connection, sql1, new { yearMonth = yearMonth }, transaction: Transaction);

            var sql2 = "  SELECT Id,  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No " +
                            "  ,[EmpName],[EmpBirthday],[EmpIdentityNo],[EmpIdentityDate],[EmpIdentityPlace] " +
                            "  ,[EmpAddressBirth],[EmpAddress],[EmpMobile1],[EmpEmail1],[EmpEmail2],[EmpStartDate],[EmpComment] " +
                        "  FROM[dbo].[Employee] " +
                        "  WHERE" +
                        "  ( SUBSTRING(convert(varchar, [EmpStartDate], 112), 1, 6) <=  @yearMonth " +
                        "     AND  (SUBSTRING(convert(varchar, [RegularDate], 112), 1, 6) > @yearMonth  OR RegularDate is null)) ";

            var lst2 = await SqlMapper.QueryAsync<EmployeeProbationReportDto>(Connection, sql2, new { yearMonth = yearMonth }, transaction: Transaction);

            var sql3 = "  SELECT Id,  ROW_NUMBER() OVER( ORDER BY  [Employee].[Id] ASC) AS No " +
                            "  ,[EmpName],[EmpBirthday],[EmpIdentityNo],[EmpIdentityDate],[EmpIdentityPlace] " +
                            "  ,[EmpAddressBirth],[EmpAddress],[EmpMobile1],[EmpEmail1],[EmpEmail2] " +
                            "  ,[EmpStartDate],[EmpEndDate],[EmpComment] " +
                        "  FROM[dbo].[Employee] " +
                        "  WHERE SUBSTRING(convert(varchar, [EmpEndDate], 112), 1, 6) =  @yearMonth  ";

            var lst3 = await SqlMapper.QueryAsync<EmployeeLeaveReportDto>(Connection, sql3, new { yearMonth = yearMonth }, transaction: Transaction);

            

            var result = new EmployeeReportDto()
            {
                EmployeeRegular = lst1,
                EmployeeProbation = lst2,
                EmployeeLeave = lst3
            };
            return result;
        }
        #endregion

    }
}