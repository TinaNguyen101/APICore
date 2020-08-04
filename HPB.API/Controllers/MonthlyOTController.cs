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
    public class MonthlyOTController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public MonthlyOTController(IUnitOfWork UnitOfWork, ILogger<ProjectController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }
         /// <summary>
         /// get wage Overtime
         /// </summary>
         /// <param name="empId"></param>
         /// <param name="timeOT"></param>
         /// <param name="dateOT"></param>
         /// <returns></returns>
        [HttpGet]
        [Route("{empId}/{timeOT}/{dateOT}")]
        public async Task<ActionResult<decimal>> GetWageBonusImage(int empId, decimal timeOT, DateTime dateOT)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var data = await _UnitOfWork.SalaryRepository.GetWageOvertime(empId, timeOT, dateOT);
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
        /// Get all MonthlyOT 
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("All/")]
        public async Task<ActionResult<IEnumerable<MonthlyOTDto>>> GetAll(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.SalaryRepository.GetAllMonthlyOTAsync(yearMonth);
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
        /// Get MonthlyOT by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MonthlyOTDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MonthlyOTDto>> GetMonthlyOT(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.SalaryRepository.GetMonthlyOTByIdAsync(id);
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
        /// Delete  MonthlyOT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid MonthlyOT" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var _del= await _UnitOfWork.SalaryRepository.GetMonthlyOTByIdAsync(id);
                var x= await _UnitOfWork.SalaryRepository.DeleteMonthlyOTAsync(id);
                await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(_del.EmpId.Value, _del.Otdate.Value.ToString("yyyyMM"));
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
        /// Insert MonthlyOT
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] MonthlyOTDto dto)
        {
            using (Logger.BeginScope("Insert MonthlyOT"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                    var id = await _UnitOfWork.SalaryRepository.InsertMonthlyOTAsync(dto);
                    await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(dto.EmpId.Value, dto.Otdate.Value.ToString("yyyyMM"));
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
        /// Update MonthlyOT
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] MonthlyOTDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.SalaryRepository.UpdateMonthlyOTAsync(dto);
                await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(dto.EmpId.Value, dto.Otdate.Value.ToString("yyyyMM"));
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
