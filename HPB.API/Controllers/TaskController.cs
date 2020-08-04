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
    public class TaskController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public TaskController(IUnitOfWork UnitOfWork, ILogger<TaskController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
            _Logger.LogCritical("Task Controller");
        }

        /// <summary>
        /// Get end date of Task by startdate and duration
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EndDate/")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DateTime>> GetEndDate(string startdate, double duration,int empId)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.TaskRepository.GetEndDateAsync(startdate, duration, empId);
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
        /// Get all Task of Project(paging - sort - filter)
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Project/")]
        public ActionResult GetAllProjectTask([FromBody]PagingRequest<ProjectTaskListFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<ProjectTaskDto>();

                    var lstDto = _UnitOfWork.TaskRepository.AllProjectTaskDtoAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<ProjectTaskDto>(paging.pageSize * paging.pageNumber).Take<ProjectTaskDto>(paging.pageSize).ToArray();
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
        /// Get all Task of ProjectMaintenance(paging - sort - filter)
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProjectMaintenance/")]
        public ActionResult GetAllProjectMaintenanceTask([FromBody]PagingRequest<ProjectMaintenanceTaskListFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<ProjectMaintenanceTaskDto>();

                    var lstDto = _UnitOfWork.TaskRepository.AllProjectMaintenanceTaskDtoAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<ProjectMaintenanceTaskDto>(paging.pageSize * paging.pageNumber).Take<ProjectMaintenanceTaskDto>(paging.pageSize).ToArray();
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
        /// Get Task by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.TaskRepository.GetTaskByIdAsync(id);
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
        /// Delete  Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Task" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x= await _UnitOfWork.TaskRepository.DeleteTaskAsync(id);

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
        /// Insert Project Task
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add/Project/")]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectTaskDto dto)
        {
            using (Logger.BeginScope("Insert Project Task"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.TaskRepository.InsertProjectTaskAsync(dto);

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
        /// Insert Project Maintenance Task
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add/ProjectMaintenance/")]
        [ProducesResponseType(typeof(ProjectMaintenanceTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectMaintenanceTaskDto dto)
        {
            using (Logger.BeginScope("Insert Project Maintenance Task"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.TaskRepository.InsertProjectMaintenanceTaskAsync(dto);

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
        /// Update Task
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] TaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.TaskRepository.UpdateTaskAsync(dto);
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
