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
    public class ProjectMemberController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public ProjectMemberController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;

        }


        /// <summary>
        /// Get all  Member of Project 
        ///  - page: project-info-member
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{projectId}")]
        [ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetProjectMember(int projectId)
        {
            string pathFolderImage = Request.Scheme.ToString() + "://" + this.Request.Host.ToString() + "/Resources/Members/";

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMemberList = await _UnitOfWork.MemberRepository.GetProjectMemberAsync(projectId, pathFolderImage);

                    return Ok(_ProjectMemberList);
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
        /// Get  Member of Project by Id
        ///  - page: project-info-member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectMemberDto>> GetProjectMemberById(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _ProjectMember = await _UnitOfWork.MemberRepository.GetProjectMemberByIdAsync(id);

                    return Ok(_ProjectMember);
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
        /// Insert multi Member of Project 
        /// - page: project-info-member
        /// </summary>
        /// <param name="dtolst"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list/")]
        [ProducesResponseType(typeof(Member), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] List<ProjectMemberInsertDto> dtolst)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            if (dtolst == null)
                return BadRequest(new { message = "data not found" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var countR = 0;
                var lstInsrt = new List<ProjectMemberInsertDto>();
                foreach (var item in dtolst)
                {
                    countR = await _UnitOfWork.MemberRepository.CheckprojectExitMemberPosition(item.ProjectId, item.EmpId, item.ProjectPositionId);
                    if (countR == 0)
                        lstInsrt.Add(item);
                }

                var id = await _UnitOfWork.MemberRepository.InsertMultiProjectMemberAsync(lstInsrt);

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
        ///// Insert Member of Project 
        /////  - page: project-info-member
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Post([FromBody] ProjectMemberDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
        //    try
        //    {
        //        _UnitOfWork.Transaction = _UnitOfWork.Begin();

        //        var id = await _UnitOfWork.MemberRepository.InsertProjectMemberAsync(dto);

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
        ///// Update Member of Project 
        /////  - page: project-info-member
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPut()]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Put([FromBody]  ProjectMemberDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
        //    try
        //    {
        //        _UnitOfWork.Transaction = _UnitOfWork.Begin();
        //        var x = await _UnitOfWork.MemberRepository.UpdateProjectMemberAsync(dto);
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
        /// Delete  Member of Project 
        ///  - page: project-info-member
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
