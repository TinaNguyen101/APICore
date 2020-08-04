using System;
using System.Collections.Generic;

namespace HPB.API.Models
{
    public partial class AnnualKpiresult
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? Kpiid { get; set; }
        public int? KpidetailId { get; set; }
        public int? EvaluatorId { get; set; }
        public double? Score { get; set; }
        public string EvaluationContent { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual Employee Evaluator { get; set; }
        public virtual Evaluator EvaluatorNavigation { get; set; }
        public virtual MstKpi Kpi { get; set; }
        public virtual MstKpidetail Kpidetail { get; set; }
    }
}
