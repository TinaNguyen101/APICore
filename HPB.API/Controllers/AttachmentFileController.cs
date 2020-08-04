using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_HPB.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
   //[Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class AttachmentFileController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public AttachmentFileController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
            //TEST TSK1
        }

        

        [HttpGet]
        [Route("Download/")]
        public IActionResult DownloadFile([FromQuery] string fileName, [FromQuery]string flag, [FromQuery]string id)
        {
            var folderName = Path.Combine("Resources", "Upload");
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathToSave = Path.Combine(currentDirectory, (flag == "0" ? "Project_" + id : "ProjectMaintenance_" + id));
            var filePath = Path.Combine(pathToSave, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), fileName);

            ////Response.ContentType= GetContentType(filePath);
            ////Response.Headers.Add("Content-Disposition", "attachment; filename="+ fileName);
            ////return new FileContentResult(System.IO.File.ReadAllBytes(filePath), GetContentType(filePath));
            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentDisposition =
            //  new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            //  {
            //      FileName = fileName
            //  };
            //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(GetContentType(filePath));
            //return result;


        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        /// <summary>
        /// Upload AttachmentFile
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="flag">0 : Project, 1 : ProjectMaintenance </param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Upload/")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload([FromForm]IFormFile file1, [FromHeader]string flag, [FromHeader]string id)
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Resources", "Upload");
                var RootpathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!System.IO.Directory.Exists(RootpathToSave))
                {
                    System.IO.Directory.CreateDirectory(RootpathToSave);
                }
                var pathToSave = Path.Combine(RootpathToSave, (flag == "0" ? "Project_" + id : "ProjectMaintenance_" + id));
                if (!System.IO.Directory.Exists(pathToSave))
                {
                    System.IO.Directory.CreateDirectory(pathToSave);
                }


                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }
                

                foreach (var file in files)
                {
                    var filePath = Path.Combine(pathToSave, file.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        return BadRequest(new { message = "File exists" });
                    }
                    var fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }
        }

        /// <summary>
        /// Get AttachmentFile By Id
        ///  -  page: project-info-attachfile, project-maintenance-info-attachfile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectAttachmentFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectAttachmentFileDto>> GetProjectAttachmentFileById(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectAttachmentFile = await _UnitOfWork.AttachmentFileRepository.GetAttachmentFileByIdAsync(id);

                    return Ok(_ProjectAttachmentFile);
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
        /// Delete AttachmentFile by Id
        ///  -  page: project-info-attachfile, project-maintenance-info-attachfile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag">0 : Project, 1 : ProjectMaintenance </param>
        /// <param name="idPr"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id, [FromHeader]string flag, [FromHeader]string idPr)
        {
            if (id <= 0)
                return BadRequest(new { message = LoggingEvents.GetItemNotFound });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var itemDel = await _UnitOfWork.AttachmentFileRepository.GetAttachmentFileByIdAsync(id);
                //await _UnitOfWork.ProjectMemberRepository.DeleteProjectMemberByProjectIDAsync(id);
                var x = await _UnitOfWork.AttachmentFileRepository.DeleteAttachmentFileAsync(id);
                //remove file in server
                var folderName = Path.Combine("Resources", "Upload");
                var RootpathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToSave = Path.Combine(RootpathToSave, (flag == "0" ? "Project_" + idPr : "ProjectMaintenance_" + idPr));
                var filePath = Path.Combine(pathToSave, itemDel.AttachmentFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

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
        /// Get all AttachmentFile of Project
        ///  -  page: project-info-attachfile
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Project/{projectId}")]
        [ProducesResponseType(typeof(ProjectAttachmentFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjectAttachmentFileDto>>> GetProjectAttachmentFile(long projectId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectAttachmentFileList = await _UnitOfWork.AttachmentFileRepository.GetProjectAttachmentFileAsync(projectId);

                    return Ok(_ProjectAttachmentFileList);
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
        /// Insert AttachmentFile of Project
        /// -   page: project-info-attachfile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Project/")]
        [ProducesResponseType(typeof(ProjectAttachmentFileDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectAttachmentFileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var duplicate = await _UnitOfWork.AttachmentFileRepository.GetProjectAttachmentFileNameAsync(dto);
                if (duplicate.Any())
                {
                    return BadRequest(new { message = "Duplicated file" });
                }
                var id = await _UnitOfWork.AttachmentFileRepository.InsertProjectAttachmentFileAsync(dto);

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





        /// <summary>
        /// Get all AttachmentFile of Project Maintenance
        /// - page: project-maintenance-info-attachfile
        /// </summary>
        /// <param name="projectMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectMaintenance/{ProjectMaintenanceId}")]
        [ProducesResponseType(typeof(ProjectMaintenanceAttachmentFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjectMaintenanceAttachmentFileDto>>> GetProjectMaintenanceAttachmentFile(long projectMaintenanceId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _list = await _UnitOfWork.AttachmentFileRepository.GetProjectMaintenanceAttachmentFileAsync(projectMaintenanceId);

                    return Ok(_list);
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
        /// Insert AttachmentFile of ProjectMaintenance
        ///  - page: project-maintenance-info-attachfile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProjectMaintenance/")]
        [ProducesResponseType(typeof(ProjectAttachmentFileDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectMaintenanceAttachmentFileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var duplicate = await _UnitOfWork.AttachmentFileRepository.GetProjectMaintenanceAttachmentFileNameAsync(dto);
                if (duplicate.Any())
                {
                    return BadRequest(new { message = "Duplicated file" });
                }
                var id = await _UnitOfWork.AttachmentFileRepository.InsertProjectMaintenanceAttachmentFileAsync(dto);

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


        /// <summary>
        /// Get all AttachmentFile of Employee
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Employee/{EmpId}")]
        [ProducesResponseType(typeof(EmployeeAttachmentFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeAttachmentFileDto>>> GetEmployeeAttachmentFile(long EmpId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _list = await _UnitOfWork.AttachmentFileRepository.GetEmployeeAttachmentFileAsync(EmpId);

                    return Ok(_list);
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
        /// Insert AttachmentFile of Employee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Employee/")]
        [ProducesResponseType(typeof(EmployeeAttachmentFileDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] EmployeeAttachmentFileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var duplicate = await _UnitOfWork.AttachmentFileRepository.GetEmployeeAttachmentFileNameAsync(dto);
                if (duplicate.Any())
                {
                    return BadRequest(new { message = "Duplicated file" });
                }
                var id = await _UnitOfWork.AttachmentFileRepository.InsertEmployeeAttachmentFileAsync(dto);

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
}
