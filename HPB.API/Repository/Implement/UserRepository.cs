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
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository()
        {

        }

        public async Task<IEnumerable<User>> AllAsync()
        {
            return await SqlMapper.QueryAsync<User>(Connection, "SELECT * FROM [User]", transaction: Transaction);
        }

        public async Task<User> GetById(string id)
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<User>(Connection, "SELECT * FROM  [User] where Id = @id", new { id = id }, transaction: Transaction);
        }

        public  UserDto Authenticate(string userId, string pass)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pass))
                return null;
            var sql = "SELECT [dbo].[User].[Id],[Password], [EmpId], [EmpName] " +
                    " FROM [dbo].[User] " +
                    " LEFT JOIN  Employee on[dbo].[User].EmpId = Employee.Id " +
                    " where [dbo].[User].[Id]=@userId  ";
            var user =  SqlMapper.QueryFirstOrDefault<UserDto>(Connection, sql, new { userId = userId }, transaction: Transaction);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPassword(pass, user.Password))
                return null;

            // authentication successful
            return user;
        }


        private static bool VerifyPassword(string password,string dbPass)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.");
            if(password!= dbPass) throw new ArgumentException("Invalid password.");
            return true;
        }


    }
}