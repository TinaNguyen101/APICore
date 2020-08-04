using System;
using System.Collections.Generic;

namespace HPB.API.DTO
{
    public partial class ApprovedDto
    {
        public int Id { get; set; }
        public int? PosId { get; set; }
        public string PositionName { get; set; }
        public string EmpName { get; set; }
       
    }
}
