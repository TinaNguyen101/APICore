using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface ICustomerRepository
    {
        IDbTransaction Transaction { get; set; }

        Task<IEnumerable<CustomerDto>> AllDDLAsync();
        Task<CustomerDto> GetDDLById(long id);
        CustomerListDto[] GetListCustomerAsync(PagingRequest<CustomerListFilterDto> paging);
        Task<CustomerListDto> GetById(long id);
        Task<int> InsertCustomerAsync(CustomerListDto dto);
        Task<int> UpdateCustomerAsync(CustomerListDto dto);
        Task<int> DeleteCustomerAsync(long id);
    }
}
