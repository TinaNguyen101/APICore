using HPB.API.DTO;
using HPB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Repository.Interface
{
    public interface IKPIRepository
    {
        IDbTransaction Transaction { get; set; }

               
        Task<IEnumerable<EvaluatorListDto>> GetListEvaluatorAsync(int year);


        Task<IEnumerable<MstKpidetail>> GetMstKPIDetailByKPIIDAsync(int KPIId);
        Boolean CheckExistsKPIResul(int year, int evaluatorId, int EmpId);
        Task<AnnualKPIResultListDto> GetlistAnnualKPIResultAsync(int year, int evaluatorId, int EmpId);
        Task<int> InsertAnnualKPIResultAsync(IList<AnnualKPIResultDto> Lstdto);
        Task<int> UpdateAnnualKPIResultAsync(IList<AnnualKPIResultDto> Lstdto);


        Boolean CheckExistsEvaluationResult(int year, int evaluatorId, int EmpId);
        Task<IEnumerable<AnnualEvaluationResultListDto>> GetlistAnnualEvaluationResultAsync(int year, int evaluatorId, int EmpId);
        Task<int> InsertAnnualEvaluationResultAsync(IList<AnnualEvaluationResultDto> Lstdto);

        Task<int> UpdateAnnualEvaluationResultAsync(IList<AnnualEvaluationResultDto> Lstdto);

        Boolean CheckExistsAnnualReview(int year, int EmpId);
        Task<IEnumerable<AnnualReviewListDto>> GetlistAnnualReviewAsync(int year, int EmpId);
        Task<int> InsertAnnualReviewAsync(IList<AnnualReviewDto> Lstdto);
        Task<int> UpdateAnnualReviewAsync(IList<AnnualReviewDto> Lstdto);



        Task<IEnumerable<KPIEmployeeReportDto>> GetKPIEmployeeAsync(int year);

        Task<KPIReportDto> GetKPIReportAsync(int year, int empId);
    }
}
