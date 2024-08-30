using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUsersController : ControllerBase
    {
        private readonly ISysUserRepository _userRepository;
        private readonly IUserService _userService;

        public SysUsersController(ISysUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [CustomAuthorize(1, "ManageUsers")]
        [HttpGet("UsersList")]
        public async Task<IActionResult> GetAll(string? userName, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                userName = userName.Trim();
            }

            if (pageNumber <= 0)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page number. Page number must be greater than 0."
                });
            }

            if (pageSize <= 0 || pageSize > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page size. Page size must be between 1 and 50."
                });
            }

            var (users, totalRecords) = await _userRepository.GetAll(userName, pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!users.Any())
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "No data available",
                    Data = users
                });
            }

            return Ok(new Response
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
        
        [CustomAuthorize(1, "ManageUsers")]
        [HttpGet("FindingUser")]
        public async Task<IActionResult> GetByID(int userId)
        {
            var user = await _userRepository.GetByID(userId);
            if (user == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Id not found",
                    Data = user
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = user
            });
        }
        [CustomAuthorize(2, "ManageUsers")]
        [HttpPost("CreatingUser")]
        public async Task<IActionResult> Create([FromBody] SysUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                user.UserName = user.UserName.Trim();
            }

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid username. The username username is required."
                });
            }

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid password. The password must not contain spaces and password is required."
                });
            }

            if (user.UserName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Username cannot exceed 20 characters."
                });
            }

            if (user.Password.Length > 100)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Password cannot exceed 64 characters."
                });
            }   
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Email is required."
                });
            }

            if (user.Note.Length > 100)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Note cannot exceed 100 characters."
                });
            }

            int rowsAffected = await _userRepository.Create(user);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while creating the user."
                });
            }

            return CreatedAtAction(nameof(GetByID), new { userId = user.UserID }, new Response
            {
                Status = 1,
                Message = "User created successfully."
            });
        }
        [CustomAuthorize(4, "ManageUsers")]
        [HttpPut("UpdatingUser")]
        public async Task<IActionResult> Update([FromBody] SysUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                user.UserName = user.UserName.Trim();
            }
            var existingUser = await _userRepository.GetByID(user.UserID);
            if (existingUser == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "User not found."
                });
            }

            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid username. The username must not contain spaces and username is required."
                });
            }

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid password. The password must not contain spaces and password is required."
                });
            }

            if (user.UserName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Username cannot exceed 20 characters."
                });
            }

            if (user.Password.Length > 100)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Password cannot exceed 64 characters."
                });
            }

            // Validation for other fields
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Email is required."
                });
            }

            if (user.Note.Length > 100)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Note cannot exceed 100 characters."
                });
            }

            int rowsAffected = await _userRepository.Update(user);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while updating the user."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "User updated successfully."
            });
        }

        [CustomAuthorize(8, "ManageUsers")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete(int userId)
        {
            var existingUser = await _userRepository.GetByID(userId);
            if (existingUser == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "User not found."
                });
            }

            int rowsAffected = await _userRepository.Delete(userId);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while deleting the user."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "User deleted successfully."
            });
        }

            
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Kiểm tra tính hợp lệ của model
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new Response { Status = 0, Message = "Username and password are required to log in." });
            }

            // Xác thực người dùng và tạo token
            var (isValid, token, message) = await _userService.AuthenticateUser(model.UserName, model.Password);

            if (!isValid)
            {
                return Unauthorized(new Response { Status = 0, Message = message });
            }

            // Trả về token nếu xác thực thành công
            return Ok(new Response
            {
                Status = 1,
                Message = message,
                Data = new { Token = token }
            });
        }
    }
}
