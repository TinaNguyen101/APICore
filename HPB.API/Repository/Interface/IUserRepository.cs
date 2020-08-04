using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IUserRepository
    {
        IDbTransaction Transaction { get; set; }

        Task<IEnumerable<User>> AllAsync();
        Task<User> GetById(string id);
        UserDto Authenticate(string userId, string pass);
    }
}
