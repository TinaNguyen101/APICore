using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class MstEvaluation
    {
        public MstEvaluation()
        {
            AnnualEvaluationResult = new HashSet<AnnualEvaluationResult>();
        }

        public int Id { get; set; }
        public string EvaluationHeading { get; set; }

        public virtual ICollection<AnnualEvaluationResult> AnnualEvaluationResult { get; set; }
    }
}
