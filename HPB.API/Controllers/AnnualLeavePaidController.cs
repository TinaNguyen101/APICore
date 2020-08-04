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
    public class AnnualLeavePaidController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public AnnualLeavePaidController(IUnitOfWork UnitOfWork, ILogger<AnnualLeavePaidController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }
        /// <summary>
        /// Get all Annual Leave Paid 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.AnnualLeavePaidRepository.AllAnnualLeavePaidAsync(year);
                    return Ok(lstDto);
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
        /// Insert Annual Leave Paid 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(int year)
        {
            using (Logger.BeginScope("Insert Annual Leave Paid "+ year))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                    //var numRow = await _UnitOfWork.AnnualLeavePaidRepository.checkYearAnnualLeavePaid(year);
                    //if(numRow>0)
                    //{
                    //    return BadRequest(new { message = "Exists Annual Leave Paid of" + year });
                    //}
                    var id = await _UnitOfWork.AnnualLeavePaidRepository.InsertAnnualLeavePaidAsync(year);
                    _UnitOfWork.Commit();

                    return Ok(id);
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
        }


        /// <summary>
        /// Download report  Annual Leave Paid 
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Download/month/")]
        public async Task<IActionResult> DownloadAnnualLeavePaidByMonth(string yearMonth)
        {
            try
            {
                var fileName = string.Format("AnnualLeavePaid_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", fileName);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listData = await _UnitOfWork.AnnualLeavePaidRepository.GetAnnualLeavePaidReportAsync(yearMonth);

                //save file

                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 5;
               converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.MarginBottom = 10;
                //converter.Options.MarginRight = 20;
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                var HtmlString = TemplateGenerator.GetAnnualLeavePaidByMonthHTMLString(listData, yearMonth);
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

        /// <summary>
        /// Download report  Annual Leave Paid 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Download/year/")]
        public async Task<IActionResult> DownloadAnnualLeavePaidByYear(int year)
        {
            try
            {
                var fileName = string.Format("AnnualLeavePaid_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", fileName);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listData = await _UnitOfWork.AnnualLeavePaidRepository.AllAnnualLeavePaidAsync(year);

                //save file

                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 5;
                converter.Options.MarginBottom = 10;
                // converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                var HtmlString = TemplateGenerator.GetAnnualLeavePaidByYearHTMLString(listData, year);
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
