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
    public class MonthlyBonusController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public MonthlyBonusController(IUnitOfWork UnitOfWork, ILogger<ProjectController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }

        /// <summary>
        /// get wage bonus image of photoshop
        /// </summary>
        /// <param name="imageTypeId"></param>
        /// <param name="totalImage"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{imageTypeId}/{totalImage}")]
        public async Task<ActionResult<decimal>> GetWageBonusImage(int imageTypeId, int totalImage)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var data = await _UnitOfWork.SalaryRepository.GetWageBonusImage(imageTypeId, totalImage);
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
        /// get  total image bonus 
        /// </summary>
        /// <param name="totalImageInMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{totalImageInMonth}")]
        public async Task<ActionResult<decimal>> GetTotalImageBonus(int totalImageInMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var data = await _UnitOfWork.SalaryRepository.GetTotalImageBonus(totalImageInMonth);
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
        /// Get all MonthlyBonus 
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All/")]
        public async Task<ActionResult<IEnumerable<MonthlyBonusDto>>> GetAll(string yearMonth)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.SalaryRepository.GetAllMonthlyBonusAsync(yearMonth);
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
        /// Get MonthlyBonus by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MonthlyBonusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MonthlyBonusDto>> GetMonthlyBonus(int id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.SalaryRepository.GetMonthlyBonusByIdAsync(id);
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
        /// Delete  MonthlyBonus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid MonthlyBonus" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var _del= await _UnitOfWork.SalaryRepository.GetMonthlyBonusByIdAsync(id);
                var x= await _UnitOfWork.SalaryRepository.DeleteMonthlyBonusAsync(id);
                await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(_del.EmpId.Value, _del.YearMonth.Value.ToString());
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
        /// Insert MonthlyBonus
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] MonthlyBonusDto dto)
        {
            using (Logger.BeginScope("Insert MonthlyBonus"))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                    var id = await _UnitOfWork.SalaryRepository.InsertMonthlyBonusAsync(dto);
                    await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(dto.EmpId.Value, dto.YearMonth.Value.ToString());
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
        /// Update MonthlyBonus
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] MonthlyBonusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.SalaryRepository.UpdateMonthlyBonusAsync(dto);
                await _UnitOfWork.SalaryRepository.UpdateBonusOTAsync(dto.EmpId.Value, dto.YearMonth.Value.ToString());
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
