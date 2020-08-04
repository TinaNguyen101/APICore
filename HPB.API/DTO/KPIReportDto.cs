using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class KPIReportDto
    {
        public IEnumerable<KPIEvaluationReportDto> KPIEvaluationReportDto { get; set; }
        public IEnumerable<KPITitleReportDto> KPITitleReportDto { get; set; }
        public IEnumerable<KPIResultReportDto> KPIResultReportDto { get; set; }
        public IEnumerable<KPIResultForEmployeeReportDto> KPIResultForEmployeeReportDto { get; set; }
        public IEnumerable<KPIClassificationReportDto> KPIClassificationReportDto { get; set; }
        public IEnumerable<KPIClassificationForEmployeeReportDto> KPIClassificationForEmployeeReportDto { get; set; }

        public IEnumerable<EvaluationResultForEmployeeReportDto> EvaluationResultForEmployeeReportDto { get; set; }
        public IEnumerable<ReviewForEmployeeReportDto> ReviewForEmployeeReportDto { get; set; }


    }
    public class KPIEvaluationReportDto
    {
        public int EvaluationId { get; set; }
        public string EvaluationName { get; set; }
        public string PositionName { get; set; }

    }

    public class KPITitleReportDto
    {
        public int Kpiid { get; set; }
        public int KPIDetailId { get; set; }
        public int KPIRate { get; set; }
        public string Kpiheading { get; set; }

    }
    public class KPIResultReportDto
    {
        public int Kpiid { get; set; }
        public int KPIDetailId { get; set; }
        public double? Score { get; set; }
        public string EvaluationContent { get; set; }
        public int? EvaluatorId { get; set; }
        public int? EmpId { get; set; }
       

    }

    public class KPIResultForEmployeeReportDto
    {
        public int Kpiid { get; set; }
        public int KPIDetailId { get; set; }
        public string Kpiheading { get; set; }
        public double? Score { get; set; }
        public int KPIRate { get; set; }
        public string EvaluationContent { get; set; }
        public int? EvaluatorId { get; set; }
        public int? EmpId { get; set; }


    }
    
    public class KPIClassificationReportDto
    {
        public string Classification { get; set; }
        public int? StartScoreRange { get; set; }
        public int? EndScoreRange { get; set; }
    }

    public class KPIClassificationForEmployeeReportDto
    {
        public string Classification { get; set; }
        public int? EvaluationBy { get; set; }
        public int? EmpId { get; set; }

    }
    public class EvaluationResultReportDto
    {
        public int Id { get; set; }
    public int? EvaluationId { get; set; }
    public string EvaluationContent { get; set; }
    public int? EvaluatorId { get; set; }
    public int? Year { get; set; }
    public int? EmpId { get; set; }
    }
    public class EvaluationResultForEmployeeReportDto
    {
        public int? EvaluationId { get; set; }
        public string EvaluationContent { get; set; }
        public int? EvaluatorId { get; set; }
        public int? EmpId { get; set; }
        public string EvaluationHeading { get; set; }

    }
    public class ReviewForEmployeeReportDto
    {
        public string ReviewHeading { get; set; }
        public int? ReviewId { get; set; }
        public string ReviewContent { get; set; }
        public int? EmpId { get; set; }

    }

}
