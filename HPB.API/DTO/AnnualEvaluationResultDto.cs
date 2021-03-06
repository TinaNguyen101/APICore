﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
   
    public class AnnualEvaluationResultDto
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? EvaluationId { get; set; }
        public int? EvaluatorId { get; set; }
        public string EvaluationContent { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

}
