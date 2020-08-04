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
    [ApiController]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;
        private readonly AppSettings _appSettings;
        public UserController(IUnitOfWork UnitOfWork, IOptions<AppSettings> appSettings)
        {
            _UnitOfWork = UnitOfWork;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllEmployee()
        {
            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    return Ok(await _UnitOfWork.UserRepository.AllAsync());
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
        /// Check login
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        // GET: api/CheckLogin
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {

            try
            {
                using (_UnitOfWork.Transaction = _UnitOfWork.Begin())
                {
                    var _userDto = _UnitOfWork.UserRepository.Authenticate(userDto.Id, userDto.Password);

                    if (_userDto == null)
                        return BadRequest(new { message = "Username or password is incorrect" });

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, _userDto.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    _userDto.Token = tokenString;
                    return Ok(_userDto);
                }
            }
            catch (ArgumentException ex)
            {
                _UnitOfWork.Dispose();
                return BadRequest(new { message = ex.Message });
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
