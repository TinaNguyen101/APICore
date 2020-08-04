using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class ProjectMaintenanceController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public ProjectMaintenanceController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// Get all ProjectMaintenance by ProjectID  (paging - sort - filter)
        ///  - page: project-maintenance-info
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("all/{projectId}")]
        [ProducesResponseType(typeof(ProjectMaintenanceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetProjects(long projectId, [FromBody]PagingRequest<ProjectMaintenanceFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<ProjectMaintenanceDto>();
                    var lstDto = _UnitOfWork.ProjectMaintenanceRepository.GetProjectMaintenance(projectId, paging);
                    var recordsTotal = lstDto.Count();
                    if (paging.pageNumber != 0 && paging.pageSize != 0)
                    {//paging
                        pagingResponse.items = lstDto.Skip<ProjectMaintenanceDto>(paging.pageNumber).Take<ProjectMaintenanceDto>(paging.pageSize).ToArray();
                    }
                    else
                    {
                        var temp = lstDto.ToArray();
                        pagingResponse.items = temp;
                    }
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
        /// Get all ProjectMaintenance by ProjectID
        ///  - page: project-maintenance-info
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{projectId}")]
        [ProducesResponseType(typeof(ProjectMaintenanceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjectMaintenanceDto>>> GetProjectMaintenance(long projectId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMaintenancelist = await _UnitOfWork.ProjectMaintenanceRepository.GetProjectMaintenanceAsync(projectId);

                    return Ok(_ProjectMaintenancelist);
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
        ///  Get ProjectMaintenance by ProjectID
        ///   - page: project-maintenance-info-basic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectMaintenanceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectMaintenanceDto>> GetProjectMaintenanceById(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMaintenance = await _UnitOfWork.ProjectMaintenanceRepository.GetProjectMaintenanceByIdAsync(id);

                    return Ok(_ProjectMaintenance);
                }
            }
            catch
            {
                return BadRequest(new { message = "data not found" });
            }
            finally
            {
                _UnitOfWork.Dispose();
            }

        }

        /// <summary>
        /// Insert ProjectMaintenance
        ///  - page: project-maintenance-info-basic
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectMaintenanceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectMaintenanceDto dto)
         {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var id = await _UnitOfWork.ProjectMaintenanceRepository.InsertProjectMaintenanceAsync(dto);

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
        /// Update ProjectMaintenance
        ///   - page: project-maintenance-info-basic
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] ProjectMaintenanceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.ProjectMaintenanceRepository.UpdateProjectMaintenanceAsync(dto);
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
        /// DELETE ProjectMaintenance
        ///  - page: project-maintenance-info-basic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Project Maintenance" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                await _UnitOfWork.AttachmentFileRepository.DeleteProjectMaintenanceAttachmentFileAsync(id);
                await _UnitOfWork.MemberRepository.DeleteProjectMaintenanceMemberByProjectMaintenanceIDAsync(id);
                var x = await _UnitOfWork.ProjectMaintenanceRepository.DeleteProjectMaintenanceAsync(id);



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

    }
}
