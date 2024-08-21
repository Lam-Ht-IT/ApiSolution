using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Repositories;
using System.Text.RegularExpressions;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserService _userService;

        public UsersController(IUserRepository userRepository, IUserService userService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _roleRepository = roleRepository;
        }

        [HttpGet("UsersList")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAll (string? userName, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                userName = userName.Trim();
            }
            // Validate pageNumber and pageSize
            if (pageNumber <= 0)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page number. Page number must be greater than 0."
                });
            }

            if (pageSize <= 0)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page size. Page size must be between 1 and 50."
                });
            }


            var result = await _userRepository.GetAll(userName, pageNumber, pageSize);
            var users = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (users.Count() == 0)
            {
                return NotFound(new 
                {
                    Status = 0,
                    Message = "No data available",
                    Data = users

                });

            }
            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });

        }

        [HttpGet("FindingUser")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetByID(int userId)
        {
            var user = await _userRepository.GetByID(userId);
            if (user == null)
            {
                return NotFound(new { Status = 0, Message = "Id not found" , Data = user});
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = user });
        }

        [HttpPost("Registrasion")]
        public async Task<IActionResult> Insert([FromBody] RegisterModel model)
        {

            // Validate username: no spaces and only unaccented characters
            if (string.IsNullOrWhiteSpace(model.UserName) || model.UserName.Contains(" "))
            {
                return BadRequest(new  { Status = 0, Message = "Invalid username. The username must not contain spaces and username required " });
            }

            // Validate password: no spaces and only unaccented characters
            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Contains(" "))
            {
                return BadRequest(new  { Status = 0, Message = "Invalid password. The password must not contain spaces and password required" });
            }
            if (model.UserName.Length > 20)
            {
                return BadRequest(new  { Status = 0, Message = "Username can not exceed 20 characters" });
            }

            if (model.Password.Length > 64)
            {
                return BadRequest(new  { Status = 0, Message = "Password can not exceed 64 characters" });
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(new  { Status = 0, Message = "Passwords do not match." });
            }


            // Create new User object
            var newUser = new User
            {
                UserName = model.UserName,
                Password = model.Password,
                RoleID = 3 // Default to RoleID 3 for regular users
            };

            // Insert new user into repository
            await _userRepository.Insert(newUser);

            // Check if passwords match
            

            return CreatedAtAction(nameof(GetByID), new { userId = newUser.UserID }, new { Status = 1, Message = "Inserted data successfully" });
        }



        [HttpPut("UpdatingUser")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(User user)
        {

            var existingUser = await _userRepository.GetByID(user.UserID);
            if (existingUser == null) return NotFound(new { Status = 0, Message = "Not Found ID" });

            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Contains(" "))
            {
                return BadRequest(new { Status = 0, Message = "Invalid username. The username must not contain spaces and username required " });
            }

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Contains(" "))
            {
                return BadRequest(new { Status = 0, Message = "Invalid password. The password must not contain spaces and password required" });
            }

            if (user.UserName.Length > 20)
            {
                return BadRequest(new { Status = 0, Message = "Username can not exceed 20 characters" });
            }

            if (user.Password.Length > 64)
            {
                return BadRequest(new { Status = 0, Message = "Password can not exceed 64 characters" });
            }

            // Validate roleID
            var x = await _roleRepository.GetByID(user.RoleID);
            if (x == null)
            {
                return BadRequest(new Response { Status = 0, Message = "Invalid role ID. Role ID does not exist." });
            }

            await _userRepository.Update(user);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }


        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int userId)
        {
            var user = await _userRepository.GetByID(userId);
            if (user == null)
            {
                return NotFound(new { Status = 0, Message = "Not found id" });
            }

            await _userRepository.Delete(userId);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new Response { Status = 0, Message = "Username and password are required to log in." });
            }

            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || model.UserName.Contains(" ") || model.Password.Contains(" "))
            {
                return BadRequest(new Response { Status = 0, Message = "Invalid username or password." });
            }

            var (isValid, token, message) = await _userService.AuthenticateUser(model.UserName, model.Password);

            if (!isValid)
            {
                return Unauthorized(new Response { Status = 0, Message = message });

            }

            return Ok(new Response
            {
                Status = 1,
                Token = token,
                Message = message
            });
        }
    }
}
