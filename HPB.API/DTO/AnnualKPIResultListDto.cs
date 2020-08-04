using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class AnnualKPIResultListDto
    {
        public IEnumerable<KPIResultDto> KPIResultDto { get; set; }
        public KPIClassificationResultDto KPIClassificationResultDto { get; set; }
    }
    public class KPIResultDto
    {
        public int? Kpiid { get; set; }
        public int? KpidetailId { get; set; }
        public string KPIHeading { get; set; }
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? EvaluatorId { get; set; }
        public double? Score { get; set; }
        public string EvaluationContent { get; set; }
    }

    public class KPIClassificationResultDto
    {
        public int? EmpId { get; set; }
        public int? EvaluatorId { get; set; }
        public string Classification { get; set; }
    }
}
