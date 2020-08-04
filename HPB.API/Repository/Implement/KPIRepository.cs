using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HPB.API.Models;
using HPB.API.Repository.Interface;
using HPB.API.DTO;

namespace HPB.API.Repositories.Implement
{
    internal class KPIRepository : RepositoryBase, IKPIRepository
    {
        public KPIRepository()
        {

        }

        /// <summary>
        /// get List Evaluator
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EvaluatorListDto>> GetListEvaluatorAsync(int year)
        {
            var sql2 = "   SELECT [Evaluator].Id, [Evaluator].EmpId , [Employee].EmpName ,[Evaluator].Year" +
                         "    FROM[dbo].[Evaluator] " +
                         "    LEFT JOIN[dbo].[Employee] ON[Evaluator].EmpId = [Employee].Id " +
                        "   WHERE[Evaluator].Year  = @year";
            var lst2 = await SqlMapper.QueryAsync<EvaluatorListDto>(Connection, sql2, new { year = year }, transaction: Transaction);
            return lst2;
        }

        #region AnnualKPIResult
        public async Task<IEnumerable<MstKpidetail>> GetMstKPIDetailByKPIIDAsync(int KPIId)
        {
            var sql = " SELECT * " +
                            " FROM [dbo].[MstKPIDetail] " +
                            " WHERE [MstKPIDetail].KPIId = @KPIId ";
            return  await SqlMapper.QueryAsync<MstKpidetail>(Connection, sql, new { KPIId = KPIId }, transaction: Transaction);
            
        }

      
        public Boolean CheckExistsKPIResul(int year, int evaluatorId, int EmpId)
        {
            var sql = " SELECT  Count(*)  " +
                      " FROM " +
                      " AnnualKPIResult" +
                      " WHERE [AnnualKPIResult].year = @year" +
                        "  AND[AnnualKPIResult].EvaluatorId = @evaluatorId" +
                          "  AND [AnnualKPIResult].EmpId = @EmpId";
            var _Dto = SqlMapper.QuerySingle<int>(Connection, sql, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            if (_Dto.Equals(0))
                return false;
            return true;

        }

        /// <summary>
        /// Get list Annual KPI Result
        /// </summary>
        /// <param name="year"></param>
        /// <param name="evaluatorId"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public async Task<AnnualKPIResultListDto> GetlistAnnualKPIResultAsync(int year, int evaluatorId, int EmpId)
        {
            IEnumerable<KPIResultDto> lst2;
            if (CheckExistsKPIResul(year, evaluatorId, EmpId))
            {
                var sql2 = "   SELECT Title.KPIId,Title.KPIDetailId,Title.KPIHeading " +
                           "  ,[AnnualKPIResult].Id,[AnnualKPIResult].Year,[AnnualKPIResult].EmpId,[AnnualKPIResult].EvaluatorId " +
                           "  ,[AnnualKPIResult].Score,[AnnualKPIResult].EvaluationContent " +
                           "  FROM " +
                                   "  ( " +
                                   "  SELECT[MstKPI].Id as KPIId , 0 as KPIDetailId , 0 as KPIRate " +
                                   "  , CONVERT(nvarchar, [MstKPI].Id) +'. ' +[MstKPI].KPIHeading as KPIHeading " +
                                   "  FROM[dbo].[MstKPI] " +
                                   "          UNION " +
                                   "  SELECT[MstKPIDetail].KPIId ,[MstKPIDetail].Id as KPIDetailId ,KPIRate  " +
                                   "  ,'  ' +CONVERT(nvarchar, [MstKPIDetail].KPIId) + '.'+CONVERT(nvarchar, [MstKPIDetail].KPIDetailNo)+' '+[MstKPIDetail].KPIContent as KPIHeading " +
                                   "  FROM[dbo].[MstKPIDetail] " +
                                   "  ) as Title " +
                           "  LEFT JOIN[dbo].[AnnualKPIResult] " +
                           "          ON Title.KPIId = [AnnualKPIResult].KPIId AND Title.KPIDetailId = isnull([AnnualKPIResult].KPIDetailId,0) " +
                           "  WHERE[AnnualKPIResult].Year = @year   " +
                           "  AND[AnnualKPIResult].EvaluatorId = @evaluatorId" +
                           "  AND [AnnualKPIResult].EmpId = @EmpId";
                 lst2 = await SqlMapper.QueryAsync<KPIResultDto>(Connection, sql2, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            }
            else
            {
                var sql2 = "   SELECT Title.KPIId,Title.KPIDetailId,Title.KPIHeading " +
                           "  ,NULL as Id,@year as Year,@EmpId as EmpId,@evaluatorId as EvaluatorId " +
                           "  ,NULL as Score, '' as EvaluationContent " +
                           "  FROM " +
                                   "  ( " +
                                   "  SELECT[MstKPI].Id as KPIId , 0 as KPIDetailId , 0 as KPIRate " +
                                   "  , CONVERT(nvarchar, [MstKPI].Id) +'. ' +[MstKPI].KPIHeading as KPIHeading " +
                                   "  FROM[dbo].[MstKPI] " +
                                   "          UNION " +
                                   "  SELECT[MstKPIDetail].KPIId ,[MstKPIDetail].Id as KPIDetailId ,KPIRate  " +
                                   "  ,'  ' +CONVERT(nvarchar, [MstKPIDetail].KPIId) + '.'+CONVERT(nvarchar, [MstKPIDetail].KPIDetailNo)+' '+[MstKPIDetail].KPIContent as KPIHeading " +
                                   "  FROM[dbo].[MstKPIDetail] " +
                                   "  ) as Title ";
                lst2 = await SqlMapper.QueryAsync<KPIResultDto>(Connection, sql2, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            }

            var sql4 = "   SELECT * FROM [dbo].[MstKPIClassification] ";
            var lst4 = await SqlMapper.QueryAsync<MstKpiclassification>(Connection, sql4, transaction: Transaction);
            var _KPIClassificationResultDto = new KPIClassificationResultDto();
            _KPIClassificationResultDto.EmpId = lst2.FirstOrDefault().EmpId;
            _KPIClassificationResultDto.EvaluatorId = lst2.FirstOrDefault().EvaluatorId;
            var _TotalScore = lst2.Where(x => x.KpidetailId == 0).Sum(x => x.Score);
            var _countKPIId0 = lst2.Where(x => x.KpidetailId == 0).Count();
            var temp = (_TotalScore / _countKPIId0) > 100 ? 100 : (_TotalScore / _countKPIId0);
            _KPIClassificationResultDto.Classification = lst4.Where(x => x.StartScoreRange <= temp && x.EndScoreRange > temp).FirstOrDefault().Classification;
            var result = new AnnualKPIResultListDto()
            {
                KPIResultDto = lst2,
                KPIClassificationResultDto = _KPIClassificationResultDto
            };
            return result;
    }

        public async Task<int> InsertAnnualKPIResultAsync(IList<AnnualKPIResultDto> Lstdto)
        {
            var sql2 = "   SELECT [MstKPI].Id as KPIId , 0 as KPIDetailId " +
                             "  FROM[dbo].[MstKPI] " +
                             "  UNION " +
                             "  SELECT[MstKPIDetail].KPIId ,[MstKPIDetail].Id as KPIDetailId " +
                            " FROM[dbo].[MstKPIDetail]";

           var lst2 = await SqlMapper.QueryAsync<KPITitleDto>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.Kpiid == item.Kpiid && x.KpidetailId == item.KpidetailId).FirstOrDefault();
                var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AnnualKPIResult] " +
                    " ([EmpId] " +
                    " ,[Year] " +
                    " ,[KPIId] " +
                    " ,[KPIDetailId]" +
                    " ,[EvaluatorId]" +
                    " ,[Score] " +
                      " ,[EvaluationContent] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@EmpId" +
                    " ,@Year" +
                    " ,@KPIId" +
                    " ,@KPIDetailId" +
                    " ,@EvaluatorId" +
                    " ,@Score" +
                    " ,@EvaluationContent" +
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

                result = await Connection.ExecuteAsync(sql, new
                {
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    KPIId = dto.Kpiid,
                    KPIDetailId = dto.KpidetailId,
                    EvaluatorId = dto.EvaluatorId,
                    Score = dto.Score,
                    EvaluationContent = dto.EvaluationContent,
                    CreatedId = dto.CreatedId
                }, Transaction);
            }
            
            return result;
        }

        public async Task<int> UpdateAnnualKPIResultAsync(IList<AnnualKPIResultDto> Lstdto)
        {
            var sql2 = "   SELECT [MstKPI].Id as KPIId , 0 as KPIDetailId " +
                             "  FROM[dbo].[MstKPI] " +
                             "  UNION " +
                             "  SELECT[MstKPIDetail].KPIId ,[MstKPIDetail].Id as KPIDetailId " +
                            " FROM[dbo].[MstKPIDetail]";

            var lst2 = await SqlMapper.QueryAsync<KPITitleDto>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.Kpiid == item.Kpiid && x.KpidetailId == item.KpidetailId).FirstOrDefault();
                var sql = " UPDATE [dbo].[AnnualKPIResult] " +
                     "  SET [EmpId] = @EmpId " +
                     "     ,[Year] = @Year " +
                      "     ,[KPIId] = @KPIId " +
                       "     ,[KPIDetailId] = @KPIDetailId " +
                      "     ,[EvaluatorId] = @EvaluatorId " +
                     "     ,[Score] = @Score " +
                       "     ,[EvaluationContent] = @EvaluationContent " +
                        "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  AnnualKPIResult.Id = @Id";

                result += await Connection.ExecuteAsync(sql, new
                {
                    Id = dto.Id,
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    Kpiid = dto.Kpiid,
                    KpidetailId = dto.KpidetailId,
                    EvaluatorId = dto.EvaluatorId,
                    Score = dto.Score,
                    EvaluationContent = dto.EvaluationContent,
                    UpdatedId = dto.UpdatedId
                }, Transaction);
            }
            return result;
        }
        #endregion

        #region AnnualEvaluationResult
        public Boolean CheckExistsEvaluationResult(int year, int evaluatorId, int EmpId)
        {
            var sql = " SELECT  Count(*)  " +
                      " FROM " +
                      " AnnualEvaluationResult" +
                      " WHERE [AnnualEvaluationResult].year = @year" +
                        "  AND[AnnualEvaluationResult].EvaluatorId = @evaluatorId" +
                          "  AND [AnnualEvaluationResult].EmpId = @EmpId";
            var _Dto = SqlMapper.QuerySingle<int>(Connection, sql, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            if (_Dto.Equals(0))
                return false;
            return true;

        }

      
        public async Task<IEnumerable<AnnualEvaluationResultListDto>> GetlistAnnualEvaluationResultAsync(int year, int evaluatorId, int EmpId)
        {
            IEnumerable<AnnualEvaluationResultListDto> lst2;
            if (CheckExistsKPIResul(year, evaluatorId, EmpId))
            {
                var sql2 = "   SELECT  [AnnualEvaluationResult].EmpId  ,[AnnualEvaluationResult].Year ,[AnnualEvaluationResult].EvaluatorId" +
                           "  ,[MstEvaluation].Id as EvaluationId, " +
                           "  ,[MstEvaluation].EvaluationHeading,[AnnualEvaluationResult].EvaluationContent " +
                           "  FROM  [dbo].[MstEvaluation] " +
                           "  LEFT JOIN[dbo].[AnnualEvaluationResult] " +
                           "          ON MstEvaluation.Id = [AnnualKPIResult].EvaluationId " +
                           "  WHERE[AnnualEvaluationResult].Year = @year   " +
                           "  AND[AnnualEvaluationResult].EvaluatorId = @evaluatorId" +
                           "  AND [AnnualEvaluationResult].EmpId = @EmpId";
                lst2 = await SqlMapper.QueryAsync<AnnualEvaluationResultListDto>(Connection, sql2, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            }
            else
            {
                var sql2 = "   SELECT @EmpId as EmpId,@year as Year,@evaluatorId as EvaluatorId   " +
                           "  ,[MstEvaluation].Id as EvaluationId " +
                           "  ,[MstEvaluation].EvaluationHeading,'' as EvaluationContent " +
                           "  FROM [dbo].[MstEvaluation] " ;
                lst2 = await SqlMapper.QueryAsync<AnnualEvaluationResultListDto>(Connection, sql2, new { year = year, evaluatorId = evaluatorId, EmpId = EmpId }, transaction: Transaction);
            }
            return lst2;
        }

        public async Task<int> InsertAnnualEvaluationResultAsync(IList<AnnualEvaluationResultDto> Lstdto)
        {
            var sql2 = "   SELECT * " +
                            " FROM[dbo].[MstEvaluation]";

            var lst2 = await SqlMapper.QueryAsync<MstEvaluation>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.EvaluationId == item.Id).FirstOrDefault();
                var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AnnualEvaluationResult] " +
                    " ([EmpId] " +
                    " ,[Year] " +
                    " ,[EvaluationId] " +
                    " ,[EvaluatorId]" +
                      " ,[EvaluationContent] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@EmpId" +
                    " ,@Year" +
                    " ,@EvaluationId" +
                    " ,@EvaluatorId" +
                    " ,@EvaluationContent" +
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

                result = await Connection.ExecuteAsync(sql, new
                {
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    EvaluationId = dto.EvaluationId,
                    EvaluatorId = dto.EvaluatorId,
                    EvaluationContent = dto.EvaluationContent,
                    CreatedId = dto.CreatedId
                }, Transaction);
            }

            return result;
        }

        public async Task<int> UpdateAnnualEvaluationResultAsync(IList<AnnualEvaluationResultDto> Lstdto)
        {
            var sql2 = "   SELECT * " +
                            " FROM[dbo].[MstEvaluation]";

            var lst2 = await SqlMapper.QueryAsync<MstEvaluation>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.EvaluationId == item.Id).FirstOrDefault();
                var sql = " UPDATE [dbo].[AnnualEvaluationResult] " +
                     "  SET [EmpId] = @EmpId " +
                     "     ,[Year] = @Year " +
                      "     ,[EvaluationId] = @EvaluationId " +
                      "     ,[EvaluatorId] = @EvaluatorId " +
                       "     ,[EvaluationContent] = @EvaluationContent " +
                        "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  AnnualEvaluationResult.Id = @Id";

                result += await Connection.ExecuteAsync(sql, new
                {
                    Id = dto.Id,
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    EvaluationId = dto.EvaluationId,
                    EvaluatorId = dto.EvaluatorId,
                    EvaluationContent = dto.EvaluationContent,
                    UpdatedId = dto.UpdatedId
                }, Transaction);
            }
            return result;
        }
        #endregion

        #region AnnualReview
        public Boolean CheckExistsAnnualReview(int year,  int EmpId)
        {
            var sql = " SELECT  Count(*)  " +
                      " FROM " +
                      " AnnualReview" +
                      " WHERE [AnnualReview].year = @year" +
                          "  AND [AnnualReview].EmpId = @EmpId";
            var _Dto = SqlMapper.QuerySingle<int>(Connection, sql, new { year = year,  EmpId = EmpId }, transaction: Transaction);
            if (_Dto.Equals(0))
                return false;
            return true;

        }


        public async Task<IEnumerable<AnnualReviewListDto>> GetlistAnnualReviewAsync(int year, int EmpId)
        {
            IEnumerable<AnnualReviewListDto> lst2;
            if (CheckExistsAnnualReview(year, EmpId))
            {
                var sql2 = "   SELECT  [AnnualReview].EmpId  ,[AnnualReview].Year " +
                           "  ,[MstReview].Id as ReviewId, " +
                           "  ,[MstEvaluation].ReviewHeading,[AnnualReview].ReviewContent " +
                           "  FROM  [dbo].[MstReview] " +
                           "  LEFT JOIN[dbo].[AnnualReview]  ON MstReview.Id = [AnnualReview].ReviewId " +
                           "  WHERE[AnnualReview].Year = @year   " +
                           "  AND [AnnualReview].EmpId = @EmpId";
                lst2 = await SqlMapper.QueryAsync<AnnualReviewListDto>(Connection, sql2, new { year = year, EmpId = EmpId }, transaction: Transaction);
            }
            else
            {
                var sql2 = "   SELECT @EmpId as EmpId,@year as Year  " +
                           "  ,[MstReview].Id as ReviewId " +
                           "  ,[MstReview].ReviewHeading,'' as ReviewContent " +
                           "  FROM [dbo].[MstReview] ";
                lst2 = await SqlMapper.QueryAsync<AnnualReviewListDto>(Connection, sql2, new { year = year, EmpId = EmpId }, transaction: Transaction);
            }
            return lst2;
        }

        public async Task<int> InsertAnnualReviewAsync(IList<AnnualReviewDto> Lstdto)
        {
            var sql2 = "   SELECT * " +
                            " FROM[dbo].[MstReview]";

            var lst2 = await SqlMapper.QueryAsync<MstReview>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.ReviewId == item.Id).FirstOrDefault();
                var sql = " DECLARE @ID int;" +
                 " INSERT INTO [dbo].[AnnualReview] " +
                    " ([EmpId] " +
                    " ,[Year] " +
                    " ,[ReviewId] " +
                      " ,[ReviewContent] " +
                    " ,[CreatedId] " +
                    " ,[CreatedDate]) " +
                     " VALUES " +
                    " (@EmpId" +
                    " ,@Year" +
                    " ,@ReviewId" +
                    " ,@ReviewContent" +
                      " ,@CreatedId " +
                    " ,GETDATE() )" +
                      " SET @ID = SCOPE_IDENTITY(); " +
              " SELECT @ID";

                result = await Connection.ExecuteAsync(sql, new
                {
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    ReviewId = dto.ReviewId,
                    ReviewContent = dto.ReviewContent,
                    CreatedId = dto.CreatedId
                }, Transaction);
            }

            return result;
        }

        public async Task<int> UpdateAnnualReviewAsync(IList<AnnualReviewDto> Lstdto)
        {
            var sql2 = "   SELECT * " +
                            " FROM[dbo].[MstReview]";

            var lst2 = await SqlMapper.QueryAsync<MstEvaluation>(Connection, sql2, transaction: Transaction);
            var result = 0;
            foreach (var item in lst2)
            {
                var dto = Lstdto.Where(x => x.ReviewId == item.Id).FirstOrDefault();
                var sql = " UPDATE [dbo].[AnnualReview] " +
                     "  SET [EmpId] = @EmpId " +
                     "     ,[Year] = @Year " +
                      "     ,[ReviewId] = @ReviewId " +
                       "     ,[ReviewContent] = @ReviewContent " +
                        "  ,[UpdatedId] = @UpdatedId" +
                     "  ,[UpdatedDate] = GetDate() " +
                     "  WHERE  AnnualReview.Id = @Id";

                result += await Connection.ExecuteAsync(sql, new
                {
                    Id = dto.Id,
                    EmpId = dto.EmpId,
                    Year = dto.Year,
                    ReviewId = dto.ReviewId,
                    ReviewContent = dto.ReviewContent,
                    UpdatedId = dto.UpdatedId
                }, Transaction);
            }
            return result;
        }
        #endregion


        #region Report 

        /// <summary>
        /// Get list employee for view KPI
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<IEnumerable<KPIEmployeeReportDto>> GetKPIEmployeeAsync(int year)
        {
            var sql2 = "   SELECT Distinct [AnnualKPIResult].EmpId , [Employee].EmpName ,[MstDepartment].DepartmentName" +
                         "    FROM[dbo].[AnnualKPIResult] " +
                         "    LEFT JOIN[dbo].[Employee] ON[AnnualKPIResult].EmpId = [Employee].Id " +
                         "  LEFT JOIN [dbo].[MstDepartment] on [MstDepartment].Id = [Employee].DeptId" +
                        "   WHERE[AnnualKPIResult].Year  = @year";
            var lst2 = await SqlMapper.QueryAsync<KPIEmployeeReportDto>(Connection, sql2, new { year = year }, transaction: Transaction);

           
            return lst2;
        }
        public async Task<KPIReportDto> GetKPIReportAsync(int year,int empId)
        {
            var sql1 = " SELECT distinct Evaluator.Id as EvaluationId " +
                         " ,[Employee].EmpName as EvaluationName " +
                         " ,[MstPosition].PositionName " +
                         " FROM Evaluator " +
                         " LEFT JOIN[dbo].[Employee] " +
                         "         ON Evaluator.EmpId = [Employee].Id " +
                         " LEFT JOIN [dbo].[MstPosition] ON [Employee].PosId = [MstPosition].Id" +
                         " WHERE Evaluator.Year = @year ";

             var sql2 = "   SELECT [MstKPI].Id as KPIId , 0 as KPIDetailId , 0 as KPIRate " +
                              " , CONVERT(nvarchar, [MstKPI].Id) +'. ' +[MstKPI].KPIHeading as KPIHeading " +
                             "  FROM[dbo].[MstKPI] " +
                             "  UNION " +
                             "  SELECT[MstKPIDetail].KPIId ,[MstKPIDetail].Id as KPIDetailId ,KPIRate " +
                             "  ,'  ' +CONVERT(nvarchar, [MstKPIDetail].KPIId) + '.'+CONVERT(nvarchar, [MstKPIDetail].KPIDetailNo)+' '+[MstKPIDetail].KPIContent as KPIHeading " +
                            " FROM[dbo].[MstKPIDetail]";

            var sql3 = "  SELECT [AnnualKPIResult].KPIId " +
                         "  ,isnull([AnnualKPIResult].KPIDetailId ,0) as KPIDetailId " +
                         "  ,[AnnualKPIResult].Score " +
                         "  ,[AnnualKPIResult].EvaluatorId " +
                         "  ,[AnnualKPIResult].EvaluationContent " +
                         "  ,[AnnualKPIResult].EmpId " +
                         "  FROM[dbo].[AnnualKPIResult]" +
                         " WHERE EmpId = @EmpId";

            var sql4 = "   SELECT * FROM [dbo].[MstKPIClassification] ";
            var sql5 = "   SELECT * FROM [dbo].[MstEvaluation] ";

            var sql6 = " SELECT Id ,EvaluationId,EvaluationContent,EvaluatorId,Year,EmpId " +
                         " FROM[dbo].[AnnualEvaluationResult]" +
                         " WHERE Year = @year AND EmpId = @EmpId";

            var sql7 = "   SELECT * FROM [dbo].[MstReview] ";

            var sql8 = " SELECT ReviewId ,ReviewContent,EmpId,Year " +
                        " FROM[dbo].[AnnualReview]" +
                        " WHERE Year = @year AND EmpId = @EmpId";

            var multi1 = await SqlMapper.QueryMultipleAsync(Connection, sql1 + ";" + sql2+";"+ sql3 + ";" + sql4 + ";" + sql5 + ";" + sql6 + ";" + sql7 + ";" + sql8, new {year = year ,EmpId = empId }, transaction: Transaction);;
            var _lstEvaluationBy = multi1.Read<KPIEvaluationReportDto>().ToList();
            var _lstTitleKPI = multi1.Read<KPITitleReportDto>().ToList();
            var _lstKPIResult = multi1.Read<KPIResultReportDto>().ToList();
            var _lstclassification = multi1.Read<KPIClassificationReportDto>().ToList();
            var _lstMstEvaluation = multi1.Read<MstEvaluation>().ToList();
            var _lstEvaluation = multi1.Read<EvaluationResultReportDto>().ToList();
            var _lstMstReview = multi1.Read<MstReview>().ToList();
            var _lstReview = multi1.Read<ReviewForEmployeeReportDto>().ToList();


            var _listKPIResultForEmployeeReportDto = new List<KPIResultForEmployeeReportDto>();
            var _listKPIClassificationForEmployeeReportDto = new List<KPIClassificationForEmployeeReportDto>();
            foreach (var item in _lstTitleKPI)
            {
                var _KPIResult = _lstKPIResult.Where(x => x.Kpiid == item.Kpiid && x.KPIDetailId == item.KPIDetailId).ToList();
                for (int i = 0; i < _lstEvaluationBy.Count; i++)
                {
                    var _item = new KPIResultForEmployeeReportDto();
                    var item1 = _KPIResult.Where(x => _lstEvaluationBy[i].EvaluationId == x.EvaluatorId).SingleOrDefault();
                    if (item1 != null)
                    {
                        _item.Kpiid = item.Kpiid;
                        _item.KPIDetailId = item.KPIDetailId;
                        _item.Kpiheading = item.Kpiheading;
                        _item.Score = item1.Score;
                        _item.KPIRate = item.KPIRate;
                        _item.EvaluationContent = item1.EvaluationContent;
                        _item.EmpId = item1.EmpId;
                        _item.EvaluatorId = item1.EvaluatorId;
                    }
                    else
                    {
                        _item.Kpiid = item.Kpiid;
                        _item.KPIDetailId = item.KPIDetailId;
                        _item.Kpiheading = item.Kpiheading;
                        _item.Score = 0;
                        _item.KPIRate = item.KPIRate;
                        _item.EvaluationContent = "";
                        _item.EmpId =empId;
                        _item.EvaluatorId = _lstEvaluationBy[i].EvaluationId;
                    }
                    _listKPIResultForEmployeeReportDto.Add(_item);
                }
            }
            foreach (var item1 in _listKPIResultForEmployeeReportDto)
            {
                if(item1.KPIDetailId == 0)
                {
                    var _countKpiTitle = _lstTitleKPI.Count(x => x.Kpiid == item1.Kpiid) - 1;
                    if (_countKpiTitle > 0)
                    {
                        item1.Score = _listKPIResultForEmployeeReportDto.Where(x => x.Kpiid == item1.Kpiid && x.EvaluatorId == item1.EvaluatorId).Sum(x => (double)((x.Score * x.KPIRate) / _countKpiTitle));
                    }
                    item1.Score = item1.Score == double.NaN ? 0 : Math.Round(item1.Score.Value);
                }
            }
            for (int j = 0; j < _lstEvaluationBy.Count(); j++)
            {
                var item = new KPIClassificationForEmployeeReportDto();
                var _TotalScore = _listKPIResultForEmployeeReportDto.Where(x => x.KPIDetailId == 0 && x.EvaluatorId == _lstEvaluationBy[j].EvaluationId).Sum(x => x.Score);
                var _countKPIId0 = _listKPIResultForEmployeeReportDto.Where(x => x.KPIDetailId == 0 && x.EvaluatorId == _lstEvaluationBy[j].EvaluationId).Count();
                if(_countKPIId0 != 0)
                {
                    var temp = (_TotalScore / _countKPIId0) > 100 ? 100 : (_TotalScore / _countKPIId0);
                    var _vartemp = _lstclassification.Where(x => x.StartScoreRange <= temp && x.EndScoreRange > temp).FirstOrDefault();
                    item.Classification = _vartemp!=null? _vartemp.Classification: "J";
                }
                else
                {
                    item.Classification = "";
                }
                item.EmpId = empId;
                item.EvaluationBy = _lstEvaluationBy[j].EvaluationId;
                _listKPIClassificationForEmployeeReportDto.Add(item);
            }

            var _listEvaluationResultForEmployeeReportDto = new List<EvaluationResultForEmployeeReportDto>();
            for (int k = 0; k < _lstEvaluationBy.Count(); k++)
            {
                
                foreach (var item in _lstMstEvaluation)
                {
                    var _item = new EvaluationResultForEmployeeReportDto();
                    var _data = _lstEvaluation.Where(x => x.EvaluationId == item.Id && x.EvaluatorId == _lstEvaluationBy[k].EvaluationId).FirstOrDefault();
                    if (_data == null)
                    {
                        _item.EvaluationId = item.Id;
                        _item.EvaluatorId = _lstEvaluationBy[k].EvaluationId;
                        _item.EmpId = empId;
                        _item.EvaluationContent = "";
                        _item.EvaluationHeading = item.EvaluationHeading;
                    }
                    else
                    {
                        _item.EvaluationId = item.Id;
                        _item.EvaluatorId = _data.EvaluatorId;
                        _item.EmpId = _data.EmpId;
                        _item.EvaluationContent = _data.EvaluationContent;
                        _item.EvaluationHeading = item.EvaluationHeading;
                    }
                    _listEvaluationResultForEmployeeReportDto.Add(_item);
                }

            }

            var _listReviewForEmployeeReportDto = new List<ReviewForEmployeeReportDto>();
                
                foreach (var item in _lstMstReview)
                {
                    var _item = new ReviewForEmployeeReportDto();
                    var _data = _lstReview.Where(x => x.ReviewId == item.Id ).FirstOrDefault();
                    if (_data == null)
                    {
                        _item.ReviewHeading = item.ReviewHeading;
                        _item.ReviewId = item.Id;
                        _item.EmpId = empId;
                        _item.ReviewContent = "";
                    }
                    else
                    {
                    _item.ReviewHeading = item.ReviewHeading;
                    _item.ReviewId = item.Id;
                    _item.EmpId = empId;
                    _item.ReviewContent = _data.ReviewContent;
                    }
                    _listReviewForEmployeeReportDto.Add(_item);
                }

            var result = new KPIReportDto()
            {
                KPIEvaluationReportDto = _lstEvaluationBy,
                KPIResultForEmployeeReportDto = _listKPIResultForEmployeeReportDto,
                KPIClassificationForEmployeeReportDto  = _listKPIClassificationForEmployeeReportDto,
                EvaluationResultForEmployeeReportDto=   _listEvaluationResultForEmployeeReportDto,
               ReviewForEmployeeReportDto = _listReviewForEmployeeReportDto
            };
            return result;
        }


        #endregion


    }
}