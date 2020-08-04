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
    public class DayOffController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public DayOffController(IUnitOfWork UnitOfWork, ILogger<DayOffController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }

        /// <summary>
        /// Get all Day Off
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DayOffDto>>> GetAll(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.AnnualLeavePaidRepository.GetAllDayOffAsync(yearMonth);
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
        /// Get  Day Off by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DayOffDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DayOffDto>> GetDayOff(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.AnnualLeavePaidRepository.GetDayOffByIdAsync(id);
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
        /// Insert  Day Off
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DayOffDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] DayOffDto dto)
        {
            using (Logger.BeginScope("Insert Day Off "))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                    var maxYear = await _UnitOfWork.AnnualLeavePaidRepository.GetMaxYear(dto.DayOff1.Value.Year, dto.EmpId.Value);
                    var id = await _UnitOfWork.AnnualLeavePaidRepository.InsertDayOffAsync(dto);
                    if (maxYear > dto.DayOff1.Value.Year)
                    {
                        await _UnitOfWork.AnnualLeavePaidRepository.UpdateDayRemainAsync(dto.DayOff1.Value.Year, dto.EmpId.Value, maxYear);
                    }
                    
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
        /// Delete  Day Off
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid  Day Off" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var del = await _UnitOfWork.AnnualLeavePaidRepository.GetDayOffByIdAsync(id);
                var maxYear = await _UnitOfWork.AnnualLeavePaidRepository.GetMaxYear(del.DayOff1.Value.Year, del.EmpId.Value);
                var x = await _UnitOfWork.AnnualLeavePaidRepository.DeleteDayOffAsync(id);
                if (maxYear > del.DayOff1.Value.Year)
                {
                    await _UnitOfWork.AnnualLeavePaidRepository.UpdateDayRemainAsync(del.DayOff1.Value.Year, del.EmpId.Value, maxYear);
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
        /// Update  Day Off
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] DayOffDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var maxYear = await _UnitOfWork.AnnualLeavePaidRepository.GetMaxYear(dto.DayOff1.Value.Year, dto.EmpId.Value);
                var x = await _UnitOfWork.AnnualLeavePaidRepository.UpdateDayOffAsync(dto);
                if (maxYear > dto.DayOff1.Value.Year)
                {
                    await _UnitOfWork.AnnualLeavePaidRepository.UpdateDayRemainAsync(dto.DayOff1.Value.Year, dto.EmpId.Value, maxYear);
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


    }
}
