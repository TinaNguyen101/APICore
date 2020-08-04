using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class AnnualEvaluationResult
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

        public virtual Employee Emp { get; set; }
        public virtual MstEvaluation Evaluation { get; set; }
        public virtual Employee Evaluator { get; set; }
        public virtual Evaluator EvaluatorNavigation { get; set; }
    }
}
