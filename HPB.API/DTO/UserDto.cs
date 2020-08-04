using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class UserDto
    {

        public string Id { get; set; }
        public string Password { get; set; }

        public int? EmpId { get; set; }

        public string EmpName { get; set; }

        public string Token { get; set; }
    }
}
