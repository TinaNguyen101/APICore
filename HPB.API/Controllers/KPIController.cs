using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using HPB.API.TemplateReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_HPB.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class KPIController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        private readonly AppSettings _appSettings;
        public KPIController(IUnitOfWork UnitOfWork, IOptions<AppSettings> appSettings)
        {
            _UnitOfWork = UnitOfWork;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get List Evaluator
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Evaluator/{year}")]
        public async Task<ActionResult<IEnumerable<EvaluatorListDto>>> GetListEvaluatorAsync(int year)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.KPIRepository.GetListEvaluatorAsync(year);
                    return Ok(_data);
                }
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        #region AnnualKPIResult

        /// <summary>
        /// Get list Annual KPI Result
        /// </summary>
        /// <param name="year"></param>
        /// <param name="evaluatorId"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AnnualKPIResult/")]
        public async Task<ActionResult<AnnualKPIResultListDto>> GetlistAnnualKPIResultAsync(int year, int evaluatorId, int EmpId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.KPIRepository.GetlistAnnualKPIResultAsync(year, evaluatorId, EmpId);
                    return Ok(_data);
                }
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Get MstKPIDetail By KPIID
        /// </summary>
        /// <param name="KPIId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AnnualKPIResult/KPIDetail/{KPIId}")]
        public async Task<ActionResult<IEnumerable<MstKpidetail>>> GetMstKPIDetailByKPIIDAsync(int KPIId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.KPIRepository.GetMstKPIDetailByKPIIDAsync(KPIId);
                    return Ok(_data);
                }
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Insert - Update  Annual KPI Result
        /// </summary>
        /// <param name="lstdto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AnnualKPIResult/")]
        [ProducesResponseType(typeof(AnnualKPIResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] IList<AnnualKPIResultDto> lstdto)
        {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var _tempItem = lstdto.FirstOrDefault();
                var result = 0;
                var isUpd =  _UnitOfWork.KPIRepository.CheckExistsKPIResul(_tempItem.Year.Value, _tempItem.EvaluatorId.Value, _tempItem.EmpId.Value);
                if (isUpd)
                {
                    result = await _UnitOfWork.KPIRepository.UpdateAnnualKPIResultAsync(lstdto);
                }
                else
                {
                    result = await _UnitOfWork.KPIRepository.InsertAnnualKPIResultAsync(lstdto);
                }
                    

                    _UnitOfWork.Commit();

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _UnitOfWork.Dispose();
                    return BadRequest(new { message = ex.Message });
                }
        }

        #endregion

        #region AnnualEvaluationResult
        /// <summary>
        /// Get list Annual Evaluation Result
        /// </summary>
        /// <param name="year"></param>
        /// <param name="evaluatorId"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AnnualEvaluationResult/")]
        public async Task<ActionResult<AnnualEvaluationResultListDto>> GetlistAnnualEvaluationResultAsync(int year, int evaluatorId, int EmpId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.KPIRepository.GetlistAnnualEvaluationResultAsync(year, evaluatorId, EmpId);
                    return Ok(_data);
                }
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Insert - Update  Annual Evaluation Result
        /// </summary>
        /// <param name="lstdto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AnnualEvaluationResult/")]
        [ProducesResponseType(typeof(AnnualEvaluationResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] IList<AnnualEvaluationResultDto> lstdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var _tempItem = lstdto.FirstOrDefault();
                var result = 0;
                var isUpd = _UnitOfWork.KPIRepository.CheckExistsEvaluationResult(_tempItem.Year.Value, _tempItem.EvaluatorId.Value, _tempItem.EmpId.Value);
                if (isUpd)
                {
                    result = await _UnitOfWork.KPIRepository.UpdateAnnualEvaluationResultAsync(lstdto);
                }
                else
                {
                    result = await _UnitOfWork.KPIRepository.InsertAnnualEvaluationResultAsync(lstdto);
                }


                _UnitOfWork.Commit();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region AnnualReview
        /// <summary>
        /// Get list Annual Review
        /// </summary>
        /// <param name="year"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AnnualReview/")]
        public async Task<ActionResult<AnnualReviewListDto>> GetlistAnnualReviewAsync(int year,  int EmpId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.KPIRepository.GetlistAnnualReviewAsync(year,  EmpId);
                    return Ok(_data);
                }
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Insert - Update  Annual Review
        /// </summary>
        /// <param name="lstdto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AnnualReview/")]
        [ProducesResponseType(typeof(AnnualReviewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] IList<AnnualReviewDto> lstdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var _tempItem = lstdto.FirstOrDefault();
                var result = 0;
                var isUpd = _UnitOfWork.KPIRepository.CheckExistsAnnualReview(_tempItem.Year.Value, _tempItem.EmpId.Value);
                if (isUpd)
                {
                    result = await _UnitOfWork.KPIRepository.UpdateAnnualReviewAsync(lstdto);
                }
                else
                {
                    result = await _UnitOfWork.KPIRepository.InsertAnnualReviewAsync(lstdto);
                }
                _UnitOfWork.Commit();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Report
        /// <summary>
        /// Download report KPI
        /// </summary>
        /// <param name="year"></param>
        /// <param name="rateYen"></param>
        /// <param name="rateUSD"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Download/")]
        public async Task<IActionResult> DownloadFileKPI(int year)
        {
            try
            {
                var folderName = string.Format("KPI{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", folderName);
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listEmp = await _UnitOfWork.KPIRepository.GetKPIEmployeeAsync(year);
                //save file
                if (!listEmp.Any())
                {
                    return BadRequest(new { message = "Not data" });
                }
                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                foreach (var item in listEmp)
                {
                    var listData = await _UnitOfWork.KPIRepository.GetKPIReportAsync(year, item.EmpId.Value);
                    var HtmlString = "";
                    HtmlString += TemplateGenerator.GetKPIHTMLString(listData, year, item.EmpName, item.DepartmentName);
                    if (!string.IsNullOrEmpty(HtmlString))
                    {
                        PdfDocument doc = converter.ConvertHtmlString(HtmlString);
                        var fileName = string.Format("KPI{0}_{1}.pdf", year.ToString(), item.EmpName.Replace(" ", "_"));
                        var filePath = Path.Combine(folderPath, fileName);
                        doc.Save(filePath);
                        // close pdf document
                        doc.Close();
                    }
                }



                // download the constructed zip
                //get a list of files
                string[] filesToZip = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                //final archive name (I use date / time)
                string zipFileName = string.Format("KPI{0}_{1:yyyyMMddHHmmss}.zip", year, DateTime.Now);
                var zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", zipFileName);
                using (MemoryStream zipMS = new MemoryStream())
                {
                    using (ZipArchive zipArchive = new ZipArchive(zipMS, ZipArchiveMode.Create, true))
                    {
                        //loop through files to add
                        foreach (string fileToZip in filesToZip)
                        {
                            //exclude some files? -I don't want to ZIP other .zips in the folder.
                            if (new FileInfo(fileToZip).Extension == ".zip") continue;
                            //exclude some file names maybe?
                            if (fileToZip.Contains("node_modules")) continue;
                            //read the file bytes
                            byte[] fileToZipBytes = System.IO.File.ReadAllBytes(fileToZip);
                            //create the entry - this is the zipped filename
                            //change slashes - now it's VALID
                            ZipArchiveEntry zipFileEntry = zipArchive.CreateEntry(fileToZip.Replace(folderPath, "").Replace('\\', '/'));
                            //add the file contents
                            using (Stream zipEntryStream = zipFileEntry.Open())
                            using (BinaryWriter zipFileBinary = new BinaryWriter(zipEntryStream))
                            {
                                zipFileBinary.Write(fileToZipBytes);
                            }
                        }
                    }
                    using (FileStream finalZipFileStream = new FileStream(zipFilePath, FileMode.Create))
                    {
                        zipMS.Seek(0, SeekOrigin.Begin);
                        zipMS.CopyTo(finalZipFileStream);
                    }
                }
                if (System.IO.Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch (IOException)
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Directory.Delete(folderPath, true);
                    }
                }

                const string contentType = "application/zip";
                HttpContext.Response.ContentType = contentType;
                var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
                {
                    FileDownloadName = zipFileName
                };

                return result;
                //return File(zipFilePath, "application/zip");
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        #endregion



    }
}
