using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IMstRepository
    {
        IDbTransaction Transaction { get; set; }
        Task<dynamic> GetAllMstAsync();


        Task<dynamic> GetMstByNameAsync(string mstName);
    }
}
