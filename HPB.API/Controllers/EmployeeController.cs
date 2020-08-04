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
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class EmployeeController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public EmployeeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// Get All Employee (paging - sort - filter)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("All/")]
        public ActionResult GetAllEmployee([FromBody]PagingRequest<EmployeeListFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<EmployeeListDto>();
                    
                    var lstDto = _UnitOfWork.EmployeeRepository.AllAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<EmployeeListDto>(paging.pageSize * paging.pageNumber).Take<EmployeeListDto>(paging.pageSize).ToArray();
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
        /// Get All Employee by department
        ///  - Page : project-info-member
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("Department/{deptId}")]
        public async Task<ActionResult<IEnumerable<EmployeeMemberDto>>> GetEmployeeByDept(int deptId)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.EmployeeRepository.GetByDeptAsync(deptId));
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
        /// Get Approved 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route("Approved/")]
        [ProducesResponseType(typeof(ApprovedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApprovedDto>> GetApproved()
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.EmployeeRepository.GetApprovedAsync();
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
        /// Get Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    string pathFolderImage = Request.Scheme.ToString() + "://" + this.Request.Host.ToString() + "/Resources/Members/";
                    var _data = await _UnitOfWork.EmployeeRepository.GetById(id, pathFolderImage);
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
        /// Insert Employee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] EmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var id = await _UnitOfWork.EmployeeRepository.InsertEmployeeAsync(dto);

                _UnitOfWork.Commit();

                return Ok(id);
            }
            catch (Exception ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
            }
        }

        // 
        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] EmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.EmployeeRepository.UpdateEmployeeAsync(dto);
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
        /// Delete Employee 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Project" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x = await _UnitOfWork.EmployeeRepository.DeleteEmployeeAsync(id);

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

        #region report 
        /// <summary>
        /// Get report Employee Vehicle
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("Report/EmployeeVehicle/")]
        public async Task<ActionResult<IEnumerable<EmployeeMemberDto>>> GetEmployeeVehicleReport(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.EmployeeRepository.GetEmployeeVehicleReportAsync(yearMonth));
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
        /// Download report Employee Vehicle
        /// </summary>
        /// <param name="lstEmpId"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/EmployeeVehicle/Download/")]
        public async Task<IActionResult> DownloadFileEmployeeVehicle([FromHeader] IEnumerable<int> lstEmpId, string yearMonth)
        {
            try
            {
                var fileName = string.Format("EmployeeVehicleReport_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", fileName);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listData = await _UnitOfWork.EmployeeRepository.GetDownloadEmployeeVehicleReportAsync(lstEmpId);

                //save file

                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                var HtmlString = TemplateGenerator.GetEmployeeVehicleHTMLString(listData, yearMonth);
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

        ///// <summary>
        ///// Get report Employee 
        ///// </summary>
        ///// <param name="yearMonth"></param>
        ///// <returns></returns>
        //[HttpGet()]
        //[Route("Report/Employee/")]
        //public async Task<ActionResult<IEnumerable<EmployeeMemberDto>>> GetEmployeeReport(string yearMonth)
        //{
        //    try
        //    {
        //        using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
        //        {
        //            return Ok(await _UnitOfWork.EmployeeRepository.GetEmployeeReportAsync(yearMonth));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _UnitOfWork.Dispose();
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    finally
        //    {
        //        _UnitOfWork.Dispose();
        //    }
        //}

        /// <summary>
        /// Download report Employee 
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Report/Employee/Download/")]
        public async Task<IActionResult> DownloadFileEmployee(string yearMonth)
        {
            try
            {
                var folderName = string.Format("Employee{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Report", folderName);
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                //get data
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var listData = await _UnitOfWork.EmployeeRepository.GetEmployeeReportAsync(yearMonth);

                //save file

                // instantiate a html to pdf converter object
                var converter = new HtmlToPdf();
                // specify the number of seconds the conversion is delayed
                converter.Options.MinPageLoadTime = 2;
                // set the page timeout (in seconds)
                converter.Options.MaxPageLoadTime = 30;
                converter.Options.MarginTop = 20;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

                var HtmlString = TemplateGenerator.GetEmployeeRegularHTMLString(listData, yearMonth);
                PdfDocument doc = converter.ConvertHtmlString(HtmlString);
                var fileNameRegular = string.Format("EmployeeRegularReport.pdf");
                var filePathRegular = Path.Combine(folderPath, fileNameRegular);
                doc.Save(filePathRegular);

                HtmlString = TemplateGenerator.GetEmployeeProbationHTMLString(listData, yearMonth);
                doc = converter.ConvertHtmlString(HtmlString);
                var fileNameProbation = string.Format("EmployeeProbationReport.pdf");
                var filePathProbation = Path.Combine(folderPath, fileNameProbation);
                doc.Save(filePathProbation);

                HtmlString = TemplateGenerator.GetEmployeeLeaveHTMLString(listData, yearMonth);
                doc = converter.ConvertHtmlString(HtmlString);
                var fileNameLeave = string.Format("EmployeeLeaveReport.pdf");
                var filePathLeave = Path.Combine(folderPath, fileNameLeave);
                doc.Save(filePathLeave);
                // close pdf document
                doc.Close();

                // download the constructed zip
                //get a list of files
                string[] filesToZip = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                //final archive name (I use date / time)
                string zipFileName = string.Format("Employee{0:yyyyMMddHHmmss}.zip", DateTime.Now);
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
