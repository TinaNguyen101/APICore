using HPB.API.DTO;
using HPB.API.Helpers;
using HPB.API.Models;
using HPB.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_HPB.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class CustomerController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        public CustomerController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// Get All Customer for DDL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllEmployee()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.CustomerRepository.AllDDLAsync());
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
        /// Get Customer by id for group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectBasicDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _data = await _UnitOfWork.CustomerRepository.GetDDLById(id);
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
        /// Get all Customer (paging - sort - filter)
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("all")]
        public ActionResult GetCustomers([FromBody]PagingRequest<CustomerListFilterDto> paging)
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var pagingResponse = new PagingResponse<CustomerListDto>();

                    var lstDto = _UnitOfWork.CustomerRepository.GetListCustomerAsync(paging);
                    var recordsTotal = lstDto.Count();
                    //paging
                    pagingResponse.items = lstDto.Skip<CustomerListDto>(paging.pageSize * paging.pageNumber).Take<CustomerListDto>(paging.pageSize).ToArray();
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
        /// Get Customer Basic Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{id}")]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerListDto>> GetCustomerBasic(long id)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _Project = await _UnitOfWork.CustomerRepository.GetById(id);
                    return Ok(_Project);
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

        // 
        /// <summary>
        /// Insert Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CustomerListDto dto)
        {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
                try
                {
                    _UnitOfWork.Transaction = _UnitOfWork.Begin();

                    var id = await _UnitOfWork.CustomerRepository.InsertCustomerAsync(dto);

                    _UnitOfWork.Commit();

                    return Ok(id);
                }
                catch (Exception ex)
                {
                    _UnitOfWork.Dispose();
                    return BadRequest(new { message = ex.Message });
                }
        }

        // 
        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] CustomerListDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Not a valid model" + ModelState.ToString() });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();
                var x = await _UnitOfWork.CustomerRepository.UpdateCustomerAsync(dto);
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
        /// Delete Customer 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Not a valid Project" });
            try
            {
                _UnitOfWork.Transaction = _UnitOfWork.Begin();

                var x = await _UnitOfWork.CustomerRepository.DeleteCustomerAsync(id);

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
