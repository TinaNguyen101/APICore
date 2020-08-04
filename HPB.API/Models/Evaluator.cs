using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class Evaluator
    {
        public Evaluator()
        {
            AnnualEvaluationResult = new HashSet<AnnualEvaluationResult>();
            AnnualKpiresult = new HashSet<AnnualKpiresult>();
        }

        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual ICollection<AnnualEvaluationResult> AnnualEvaluationResult { get; set; }
        public virtual ICollection<AnnualKpiresult> AnnualKpiresult { get; set; }
    }
}
