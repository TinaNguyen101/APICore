using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using HPB.API.TemplateReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_HPB.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class MonthlySalaryController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public MonthlySalaryController(IUnitOfWork UnitOfWork, ILogger<ProjectController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }

        /// <summary>
        /// check exists  Monthly Salary
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  ActionResult<Boolean> GetBonus(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var data =  _UnitOfWork.SalaryRepository.CheckExistsMonthlySalary(yearMonth);
                    return Ok(data);
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
        /// Get all MonthlySalary (paging - sort - filter)
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("All/")]
        public ActionResult GetAll([FromBody]PagingRequest<MonthlySalaryFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<MonthlySalaryListDto>();

                    var lstDto = _UnitOfWork.SalaryRepository.GetAllMonthlySalaryAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<MonthlySalaryListDto>(paging.pageSize * paging.pageNumber).Take<MonthlySalaryListDto>(paging.pageSize).ToArray();
                    pagingResponse.itemscount = recordsTotal;
                    return Ok(pagingResponse);
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
        /// Generate MonthlySalary
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(string yearMonth)
        {
            using (Logger.BeginScope("Insert MonthlySalary"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.SalaryRepository.InsertMonthlySalaryAsync(yearMonth);

                    _UnitOfWork.Commit();

                    return Ok(id);
                }
                catch (Exception ex)
                {
                    _UnitOfWork.Dispose();
                    return BadRequest(new { message = ex.Message });
                }
            }
        }



        /// <summary>
        /// Update MonthlySalary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] List<MonthlySalaryListDto> dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.SalaryRepository.UpdateMonthlySalaryAsync(dto);
                _UnitOfWork.Commit();
                return Ok(x);
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
        /// Delete  Monthly salary
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpDelete("{yearMonth}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string yearMonth)
        {
            if (string.IsNullOrEmpty(yearMonth))
                return BadRequest(new { message = "Not a valid MonthlyBonus" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.SalaryRepository.DeleteMonthlySalaryAsync(yearMonth);
                _UnitOfWork.Commit();
                return Ok(x);
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
        /// Get report  Monthly salary
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("Report/")]
        public async Task<ActionResult<MonthlySalaryReportDto>> GetMonthlySalaryReport(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.SalaryRepository.GetMonthlySalaryReportAsync(yearMonth));
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
        /// Download report  Monthly salary
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Download/")]
        public async Task<IActionResult> DownloadMonthlySalary(string yearMonth)
        {
            try
            {
                var fileName = string.Format("MonthlySalary_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", fileName);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listData = await _UnitOfWork.SalaryRepository.GetMonthlySalaryReportAsync(yearMonth);

                //save file

                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 20;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                var HtmlString = TemplateGenerator.GetMonthlySalaryHTMLString(listData, yearMonth);
                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertHtmlString(HtmlString);
                // save pdf document
                doc.Save(filePath);
                // close pdf document
                doc.Close();

                //download
                if (!System.IO.File.Exists(filePath))
                    return NotFound();
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, "application/pdf", fileName);
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


    }
}
