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
    internal class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        public CustomerRepository()
        {

        }

        public async Task<IEnumerable<CustomerDto>> AllDDLAsync()
        {
            var sql = "SELECT Id,CustName+'【'+CustShortName+'】' as CustName " +
                " FROM [Customer] " +
                " where [CustIsDelete] = 0 " +
                " order by [CreatedDate] desc";
            return await SqlMapper.QueryAsync<CustomerDto>(Connection, sql, transaction: Transaction);
        }

        public async Task<CustomerDto> GetDDLById(long id)
        {
            var sql = "SELECT Id,CustName+'【'+CustShortName+'】' as CustName  " +
              " FROM [Customer] " +
              " where [CustIsDelete] = 0  and Id = @id " +
              " order by [CreatedDate] desc";
            return await SqlMapper.QueryFirstOrDefaultAsync<CustomerDto>(Connection, sql, new { id = id }, transaction: Transaction);
        }


        public CustomerListDto[] GetListCustomerAsync(PagingRequest<CustomerListFilterDto> paging)
        {
            var sql1 = "SELECT * " +
              " FROM [Customer] " +
              " where [CustIsDelete] = 0 " +
              " order by [CreatedDate] desc";
            var _ProjectDto = SqlMapper.Query<CustomerListDto>(Connection, sql1, transaction: Transaction);
            var query = _ProjectDto;

            //search condition
            var filterDto = paging.Filters;
            if (filterDto != null)
            {
                if (!string.IsNullOrEmpty(filterDto.CustName))
                {
                    query = query.Where(emp => emp.CustName.Contains(filterDto.CustName) || emp.CustShortName.Contains(filterDto.CustName) || emp.CustEngName.Contains(filterDto.CustName));
                }
                if (!string.IsNullOrEmpty(filterDto.CustContactName))
                {
                    query = query.Where(emp => emp.CustContactName.Contains(filterDto.CustContactName));
                }
            }
            //sort
            if (!string.IsNullOrEmpty(paging.sortColumn))
            {
                var param = paging.sortColumn;
                var propertyInfo = typeof(CustomerListDto).GetProperty(param);
                query = paging.sortDir == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return query.ToArray<CustomerListDto>();
        }
        public async Task<CustomerListDto> GetById(long id)
        {
            var sql = "SELECT *e  " +
              " FROM [Customer] " +
              " where [CustIsDelete] = 0  and Id = @id " +
              " order by [CreatedDate] desc";
            return await SqlMapper.QueryFirstOrDefaultAsync<CustomerListDto>(Connection, sql, new { id = id }, transaction: Transaction);
        }

        public async Task<int> InsertCustomerAsync(CustomerListDto dto)
        {
            var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[Customer] " +
                    " ([CustName] " +
                    " ,[CustShortName] " +
                    " ,[CustEngName] " +
                    " ,[CustContactName] " +
                    " ,[CustContactEmail] " +
                    " ,[CustContactPhone] " +
                    " ,[CustContactFax] " +
                    " ,[CustContactSkype] " +
                    " ,[CustAddress] " +
                    " ,[CustWebsite] " +
                    " ,[CustPostCode] " +
                    " ,[CustComment] " +
                    " ,[CustIsDelete] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate] " +
                    " ,[CustStyleCss]) " +
                     " VALUES " +
                    " (@CustName " +
                    " ,@CustShortName" +
                    " ,@CustEngName " +
                    " ,@CustContactName " +
                    " ,@CustContactEmail " +
                    " ,@CustContactPhone " +
                    " ,@CustContactFax " +
                    " ,@CustContactSkype " +
                    " ,@CustAddress " +
                    " ,@CustWebsite " +
                    " ,@CustPostCode " +
                    " , @CustComment " +
                    " ,@CustIsDelete " +
                    " ,@CreatedId " +
                    " ,GETDATE()" +
                    " ,@CustStyleCss)" +
              " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

            var id = await Connection.QuerySingleAsync<int>(sql, new
            {
                CustName = dto.CustName,
                CustShortName = dto.CustShortName,
                CustEngName = dto.CustEngName,
                CustContactName = dto.CustContactName,
                CustContactEmail = dto.CustContactEmail,
                CustContactPhone = dto.CustContactPhone,
                CustContactFax = dto.CustContactFax,
                CustContactSkype = dto.CustContactSkype,
                CustAddress = dto.CustAddress,
                CustWebsite = dto.CustWebsite,
                CustPostCode = dto.CustPostCode,
                CustComment = dto.CustComment,
                CustIsDelete = 0,
                CreatedId = dto.CreatedId,
                CustStyleCss = dto.CustStyleCss,
            }, Transaction);
            return id;
        }

        public async Task<int> UpdateCustomerAsync(CustomerListDto dto)
        {
            var sql = " UPDATE [dbo].[Project] " +
                     " SET [CustName] = @CustName " +
                     "  ,[CustShortName] = @CustShortName " +
                     "  ,[CustEngName] = @CustEngName " +
                     "  ,[CustContactName] = @CustContactName " +
                     "  ,[CustContactEmail] = @CustContactEmail " +
                     "  ,[CustContactPhone] = @CustContactPhone " +
                     "  ,[CustContactFax] = @CustContactFax " +
                     "  ,[CustContactSkype] = @CustContactSkype " +
                     "  ,[CustAddress] = @CustAddress " +
                     "  ,[CustWebsite] = @CustWebsite " +
                     "  ,[CustPostCode] = @CustPostCode " +
                     "  ,[CustComment] = @CustComment " +
                      "  ,[CustStyleCss] = @CustStyleCss " +
                     "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     " WHERE  Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                CustName = dto.CustName,
                CustShortName = dto.CustShortName,
                CustEngName = dto.CustEngName,
                CustContactName = dto.CustContactName,
                CustContactEmail = dto.CustContactEmail,
                CustContactPhone = dto.CustContactPhone,
                CustContactFax = dto.CustContactFax,
                CustContactSkype = dto.CustContactSkype,
                CustAddress = dto.CustAddress,
                CustWebsite = dto.CustWebsite,
                CustPostCode = dto.CustPostCode,
                CustComment = dto.CustComment,
                CustStyleCss = dto.CustStyleCss,
                UpdatedId = dto.UpdatedId,
            }, Transaction);
            return x;
        }
        public async Task<int> DeleteCustomerAsync(long id)
        {
            var sql = " UPDATE [dbo].[Customer] " +
                        " SET[CustIsDelete] = @CustIsDelete" +
                          "  WHERE   Id = @Id";

            var x = await Connection.ExecuteAsync(sql, new
            {
                Id = id,
                CustIsDelete = 1,

            }, Transaction);
            return x;
        }
    }
}