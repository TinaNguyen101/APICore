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
    public class ProjectMaintenanceMemberController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public ProjectMaintenanceMemberController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        ///check exist Member  of Project Maintenance
        ///  - page: project-maintenance-info-member 
        /// </summary>
        /// <param name="projectMaintenanceId"></param>
        /// <param name="empId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("checkExist/{projectMaintenanceId}/{empId}/{positionId}")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //public async Task<ActionResult> CheckProjectMaintenanceMember(int projectMaintenanceId, int empId, int positionId)
        //{
        //    using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
        //    {
        //        try
        //        {
        //            int countR = await _UnitOfWork.MemberRepository.CheckprojectMaintenanceExitMemberPosition(projectMaintenanceId, empId, positionId);
        //            if(countR >= 1)
        //                return Ok(false);
        //            return Ok(true);
        //        }
        //        catch (Exception ex)
        //        {
        //            _UnitOfWork.Dispose();
        //            return BadRequest(new { message = ex.Message });
        //        }
        //        finally
        //        {
        //            _UnitOfWork.Dispose();
        //        }
        //    }
        //}

        /// <summary>
        /// Get all  Member of Project Maintenance
        ///  - page: project-maintenance-info-member
        /// </summary>
        /// <param name="projectMaintenanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{projectMaintenanceId}")]
        [ProducesResponseType(typeof(ProjectMaintenanceMemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjectMaintenanceMemberDto>>> GetProjectMaintenanceMember(int projectMaintenanceId)
        {
            string pathFolderImage = Request.Scheme.ToString() + "://" + this.Request.Host.ToString() + "/Resources/Members/";

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMaintenanceMemberList = await _UnitOfWork.MemberRepository.GetProjectMaintenanceMemberAsync(projectMaintenanceId, pathFolderImage);

                    return Ok(_ProjectMaintenanceMemberList);
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
        /// Get Member of Project Maintenance by Id
        ///  - page: project-maintenance-info-member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectMaintenanceMemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectMaintenanceMemberDto>> GetProjectMaintenanceMemberById(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMaintenanceMember = await _UnitOfWork.MemberRepository.GetProjectMaintenanceMemberByIdAsync(id);

                    return Ok(_ProjectMaintenanceMember);
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
        /// Insert multi Member of Project Maintenance
        ///  - page: project-maintenance-info-member
        /// </summary>
        /// <param name="dtolst"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list/")]
        [ProducesResponseType(typeof(Member), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] List<ProjectMaintenanceMemberInsertDto> dtolst)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            if (dtolst == null)
                return BadRequest(new { message = "data not found" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var countR = 0;
                var lstInsrt = new List<ProjectMaintenanceMemberInsertDto>();
                foreach (var item in dtolst)
                {
                    countR = await _UnitOfWork.MemberRepository.CheckprojectMaintenanceExitMemberPosition(item.ProjectMaintenanceId, item.EmpId, item.ProjectPositionId);
                    if (countR == 0)
                        lstInsrt.Add(item);
                }

                var id = await _UnitOfWork.MemberRepository.InsertMultiProjectMaintenanceMemberAsync(lstInsrt);

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

        ///// <summary>
        ///// Insert  Member of Project Maintenance
        /////  - page: project-maintenance-info-member
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ProducesResponseType(typeof(ProjectMaintenanceMemberDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Post([FromBody] ProjectMaintenanceMemberDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
        //    try
        //    {
        //        _UnitOfWork.Transaction = _UnitOfWork.Begin();

        //        var id = await _UnitOfWork.MemberRepository.InsertProjectMaintenanceMemberAsync(dto);

        //        _UnitOfWork.Commit();

        //        return Ok(id);
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

        ///// <summary>
        ///// update  Member of Project Maintenance
        /////  - page: project-maintenance-info-member
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPut()]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Put([FromBody]  ProjectMaintenanceMemberDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
        //    try
        //    {
        //        _UnitOfWork.Transaction = _UnitOfWork.Begin();
        //        var x = await _UnitOfWork.MemberRepository.UpdateProjectMaintenanceMemberAsync(dto);
        //        _UnitOfWork.Commit();
        //        return Ok(x);
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
        /// delete  Member of Project Maintenance
        ///  - page: project-maintenance-info-member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Project Member" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x = await _UnitOfWork.MemberRepository.DeleteMemberAsync(id);



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
