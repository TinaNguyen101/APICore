using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public int? EmpId { get; set; }
    }
}
