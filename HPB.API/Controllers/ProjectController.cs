using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class ProjectController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public ProjectController(IUnitOfWork UnitOfWork, ILogger<ProjectController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
            _Logger.LogCritical("Project Controller");
        }

        /// <summary>
        /// get project finish of year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="flagProject"> 0 : project , 1:ProjectMaintanent</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Finish/{year}/{flagProject}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProjectFinishListDto>>> GetProjectFinishAsync(int year, int flagProject)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    IEnumerable<ProjectFinishListDto> _data = new List<ProjectFinishListDto>(); 
                    switch (flagProject)
                    {
                        case 0:
                            _data = await _UnitOfWork.ProjectRepository.ProjectFinishAsync(year);
                            break;
                        case 1:
                            _data = await _UnitOfWork.ProjectMaintenanceRepository.ProjectFinishAsync(year);
                            break;
                    }
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
        ///check exist Member  of Project 
        ///  - page: project-info-member 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="empId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkExist/{projectId}/{empId}/{positionId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult> CheckProjectMember(int projectId, int empId, int positionId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    int countR = await _UnitOfWork.MemberRepository.CheckprojectExitMemberPosition(projectId, empId, positionId);
                    if (countR >= 1)
                        return Ok(false);
                    return Ok(true);
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

        // 
        /// <summary>
        /// Get all Project (paging - sort - filter)
        ///  - page: project-list
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("all")]
        public ActionResult GetProjects([FromBody]PagingRequest<ProjectListFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<ProjectListDto>();

                    var lstDto = _UnitOfWork.ProjectRepository.AllProjectListDtoAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<ProjectListDto>(paging.pageSize * paging.pageNumber).Take<ProjectListDto>(paging.pageSize).ToArray();
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

        // 
        /// <summary>
        /// Get all Project
        ///  - page: project-list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectListDto>>> GetAllProject()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectLst = await _UnitOfWork.ProjectRepository.AllProjectListDtoAsync();
                    return Ok(_ProjectLst);
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

        // GET: api/Project/5

        /// <summary>
        /// Get Project Basic Info
        /// - page: project-info-basic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectBasicDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectBasicDto>> GetProjectBasic(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _Project = await _UnitOfWork.ProjectRepository.GetProjectBasicDtoByIdAsync(id);
                    return Ok(_Project);
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

        // 
        /// <summary>
        /// Insert Project
        /// - page: project-info-basic
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectBasicDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectBasicDto dto)
        {
            using (Logger.BeginScope("Insert Project"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.ProjectRepository.InsertProjectAsync(dto);

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

        // 
        /// <summary>
        /// Update Project
        /// - page:  project-info-basic
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] ProjectBasicDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.ProjectRepository.UpdateProjectAsync(dto);
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

        // 
        /// <summary>
        /// Delete Project 
        ///  - page:   project-info-basic
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

                await _UnitOfWork.AttachmentFileRepository.DeleteProjectAttachmentFileAsync(id);
                await _UnitOfWork.MemberRepository.DeleteProjectMemberByProjectIDAsync(id);
                var ProjectMaintenanceDelLst = _UnitOfWork.ProjectMaintenanceRepository.GetProjectMaintenanceAsync(id).Result.ToList();
                foreach (var item in ProjectMaintenanceDelLst)
                {
                    await _UnitOfWork.AttachmentFileRepository.DeleteProjectMaintenanceAttachmentFileAsync(item.Id);
                    await _UnitOfWork.MemberRepository.DeleteProjectMaintenanceMemberByProjectMaintenanceIDAsync(item.Id);
                }
                await _UnitOfWork.ProjectMaintenanceRepository.DeleteProjectMaintenanceByProjectIDAsync(id);
                var x = await _UnitOfWork.ProjectRepository.HardDeleteProjectAsync(id);

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
