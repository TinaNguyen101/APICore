using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
   
    public class AnnualEvaluationResultListDto
    {
        public int? EmpId { get; set; }
        public int? Year { get; set; }

        public int? EvaluatorId { get; set; }

        public int? EvaluationId { get; set; }

        public string EvaluationHeading { get; set; }
        public string EvaluationContent { get; set; }

    }

}
