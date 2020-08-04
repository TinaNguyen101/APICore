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
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class MstController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public MstController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// Get All Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public async Task<dynamic> GetAllMst()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return await _UnitOfWork.MstRepository.GetAllMstAsync();
                }
            }
            catch(Exception ex)
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
        /// Get Master by Name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{mstName}")]
        public async Task<dynamic> GetMstByName(string mstName)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return await _UnitOfWork.MstRepository.GetMstByNameAsync(mstName);
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
