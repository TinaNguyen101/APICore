using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class EvaluatorListDto
    {

        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public int? Year { get; set; }
    }
}
