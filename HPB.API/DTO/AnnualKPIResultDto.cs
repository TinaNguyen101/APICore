using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    
    public class AnnualKPIResultDto
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
    }

    public class KPITitleDto
    {
     
      
        public int? Kpiid { get; set; }
        public int? KpidetailId { get; set; }
      
    }

}
