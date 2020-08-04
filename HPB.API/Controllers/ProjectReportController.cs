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
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class ProjectReportController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public ProjectReportController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// Get Report Project by (yearMonth / year / statusId)
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="year"></param>
        /// <param name="statusId"></param>
        /// <param name="rateYen"></param>
        /// <param name="rateUSD"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Project/Report/")]
        public async Task<ActionResult<IEnumerable<ProjectReportDto>>> GetProjectReport([FromQuery]string yearMonth = "", [FromQuery]string year = "", [FromQuery]string statusId = "", [FromQuery]int rateYen = 1, [FromQuery] int rateUSD = 1)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.ProjectRepository.GetProjectReportAsync(yearMonth, year, statusId, rateYen, rateUSD));
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
        /// Get Report Cost Statistics of Project by (year / yearMonthStart / yearMonthEnd)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="yearMonthStart"></param>
        /// <param name="yearMonthEnd"></param>
        /// <param name="rateYen"></param>
        /// <param name="rateUSD"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Project/ReportStatistics/")]
        public async Task<ActionResult<IEnumerable<ProjectReportStatisticsDto>>> GetProjectReportCostStatistics([FromQuery]string year = "", [FromQuery]string yearMonthStart = "", [FromQuery]string yearMonthEnd = "", [FromQuery]int rateYen = 1, [FromQuery] int rateUSD = 1)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.ProjectRepository.GetProjectReportStatistics(year, yearMonthStart, yearMonthEnd, rateYen, rateUSD));
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
        /// Get Report Statistics of customer by (custId and year )
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="year"></param>
        /// <param name="rateYen"></param>
        /// <param name="rateUSD"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Project/ReportStatisticsByCust/{custId}/{year}")]
        public async Task<ActionResult<IEnumerable<ProjectReportStatisticsByCustDto>>> GetProjectReportStatisticsByCust(string custId, string year, [FromQuery]int rateYen = 1, [FromQuery] int rateUSD = 1)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.ProjectRepository.GetProjectReportStatisticsByCust(custId, year, rateYen, rateUSD));
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
    }
}
