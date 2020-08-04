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
    public class SalaryController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public SalaryController(IUnitOfWork UnitOfWork, ILogger<SalaryController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }

        /// <summary>
        /// Get all Salary (paging - sort - filter)
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("All/")]
        public ActionResult GetAll([FromBody]PagingRequest<SalaryFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<SalaryListDto>();

                    var lstDto = _UnitOfWork.SalaryRepository.GetAllSalaryAsQuerable(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<SalaryListDto>(paging.pageSize * paging.pageNumber).Take<SalaryListDto>(paging.pageSize).ToArray();
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
        /// Get Salary by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SalaryListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SalaryListDto>> GetSalary(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.SalaryRepository.GetSalaryByIdAsync(id);
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
        /// Delete  Salary
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Salary" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x= await _UnitOfWork.SalaryRepository.DeleteSalaryAsync(id);

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
        /// Insert Salary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] SalaryListDto dto)
        {
            using (Logger.BeginScope("Insert Salary"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.SalaryRepository.InsertSalaryAsync(dto);

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
        /// Update Salary
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] SalaryListDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.SalaryRepository.UpdateSalaryAsync(dto);
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
