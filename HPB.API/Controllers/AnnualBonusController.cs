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
using System.IO.Compression;
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
    public class AnnualBonusController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;

        public AnnualBonusController(IUnitOfWork UnitOfWork, ILogger<AnnualLeavePaidController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }

        /// <summary>
        /// Get Bonus
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Bonus/")]
        public async Task<ActionResult<decimal>> GetBonus(decimal day, int empId, int year)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var data = await _UnitOfWork.AnnualBonusRepository.GetBonusAsync(day, empId, year);
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
        /// Get all year Annual Rating Factor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetAllYearAnnualBonus()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var lstYear = await _UnitOfWork.AnnualBonusRepository.GetAllYearAnnualBonusAsync();
                    return Ok(lstYear);
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
        /// Get all Annual Bonus by EmpId
        /// </summary>
        /// <param name="year"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All/Employee/")]
        public async Task<ActionResult<IEnumerable<AnnualBonusDto>>> GetAll(int year,int empId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.AnnualBonusRepository.GetAllAnnualBonusAsync(year, empId);
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
        /// Delete  Annual Bonus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Annual Bonus" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x = await _UnitOfWork.AnnualBonusRepository.DeleteAnnualBonusAsync(id);

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
        /// Insert  Annual Bonus
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AnnualBonusDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AnnualBonusDto dto)
        {
            using (Logger.BeginScope("Insert Annual Bonus"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.AnnualBonusRepository.InsertAnnualBonusAsync(dto);

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
        /// Update Annual Bonus
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] AnnualBonusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.AnnualBonusRepository.UpdateAnnualBonusAsync(dto);
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
        /// Report all Annual Bonus for year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/")]
        public async Task<ActionResult<IEnumerable<AnnualBonusListDto>>> GetAllAnnualBonusReport(int year, int rateYen, int rateUSD)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.AnnualBonusRepository.GetAllAnnualBonusAsync(year, rateYen, rateUSD);
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
        /// Download report Annual Bonus
        /// </summary>
        /// <param name="year"></param>
        /// <param name="rateYen"></param>
        /// <param name="rateUSD"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Download/")]
        public async Task<IActionResult> DownloadFileAnnualBonus(int year, int rateYen=255, int rateUSD=23000)
        {
            try
            {
                var folderName = string.Format("AnnualBonus{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", folderName);
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listEmp = await _UnitOfWork.AnnualBonusRepository.GetListMemberBonusAsync(year);
                //save file
                if(!listEmp.Any())
                {
                    return BadRequest(new { message = "Not data"});
                }
                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 20;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
               
                foreach (var item in listEmp)
                {
                    var listData = await _UnitOfWork.AnnualBonusRepository.GetBonusReportAsync(year, rateYen, rateUSD, item.EmpId.Value);
                    var HtmlString = "";
                    if (listData.AnnualBonusLEADERDetailDto.Any())
                    {
                        HtmlString += TemplateGenerator.GetAnnualBonusForLeaderHTMLString(listData, year);
                    }
                    if (listData.AnnualBonusMEMBERDtoDetailDto.Any())
                    {
                        HtmlString += TemplateGenerator.GetAnnualBonusForMemberHTMLString(listData, year);
                    }
                    if (!string.IsNullOrEmpty(HtmlString))
                    {
                        PdfDocument doc = converter.ConvertHtmlString(HtmlString);
                        var fileName = string.Format("Bonus{0}_{1}.pdf", year.ToString(), item.EmpName.Replace(" ","_"));
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
                string zipFileName = string.Format("AnnualBonus{0}_{1:yyyyMMddHHmmss}.zip",year, DateTime.Now);
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


    }
}
