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
    public class AnnualRatingFactorController : ControllerBase
    {
        private readonly ILogger Logger;
        private IUnitOfWork _UnitOfWork;
        public AnnualRatingFactorController(IUnitOfWork UnitOfWork, ILogger<AnnualRatingFactorController> _Logger)
        {
            _UnitOfWork = UnitOfWork;
            Logger = _Logger;
        }
        /// <summary>
        /// Get all Annual Rating Factor
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("All/")]
        public async Task<ActionResult<IEnumerable<AnnualRatingFactorDto>>> GetAllAnnualRatingFactor(int year)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {

                    var lstDto = await _UnitOfWork.AnnualBonusRepository.GetAllAnnualRatingFactorAsync(year);
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
        /// Get all year Annual Rating Factor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetAllYearAnnualRatingFactor()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var lstYear = await _UnitOfWork.AnnualBonusRepository.GetAllYearAnnualRatingFactorAsync();
                    return Ok(lstYear);
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
        /// Insert Update Annual Leave Paid 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] IEnumerable<AnnualRatingFactorListDto> dto)
        {
            using (Logger.BeginScope("Insert Annual Rating Factor "))
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    var id = 0;
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();
                    var numRow = await _UnitOfWork.AnnualBonusRepository.checkExistAnnualRatingFactorAsync(dto.FirstOrDefault().year);
                    if (numRow > 0)
                    {
                         id = await _UnitOfWork.AnnualBonusRepository.UpdateAnnualRatingFactorAsync(dto);
                    }
                    else
                    {
                         id = await _UnitOfWork.AnnualBonusRepository.InsertAnnualRatingFactorAsync(dto);
                    }
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
}
