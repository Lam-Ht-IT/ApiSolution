using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using QUANLYVANHOA.Repositories;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUsersController : ControllerBase
    {
        private readonly ISysUserRepository _userRepository;
        private readonly ISysUserInGroupRepository _userInGroupRepository;
        private readonly IUserService _userService;

        public SysUsersController(ISysUserRepository userRepository, ISysUserInGroupRepository userInGroupRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _userInGroupRepository = userInGroupRepository;
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
        public async Task<IActionResult> Create([FromBody] SysUserInsertModel user)
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

            var existingUser = await _userRepository.GetByUserName(user.UserName);
            if (existingUser != null)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Username already exists. Please choose a different username."
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

            return Ok(new Response
            {
                Status = 1,
                Message = "User created successfully."
            });
        }
        [CustomAuthorize(4, "ManageUsers")]
        [HttpPost("UpdatingUser")]
        public async Task<IActionResult> Update([FromBody] SysUserUpdateModel user)
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
                    Message = "An error occurred while creating the user."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "User updated successfully."
            });
        }


        [HttpPost("UpdateRefreshToken")]
        public async Task<IActionResult> UpdateRefreshToken([FromBody] SysUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userRepository.UpdateRefreshToken(model);

                if (result > 0)
                {
                    return Ok(new { message = "Refresh token updated successfully." });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to update refresh token." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the refresh token.", error = ex.Message });
            }
        }

        [CustomAuthorize(8, "ManageUsers")]
        [HttpPost("DeleteUser")]
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
            var (isValid, token, refreshToken, message) = await _userService.AuthenticateUser(model.UserName, model.Password);

            if (!isValid)
            {
                return Unauthorized(new Response { Status = 0, Message = message });
            }

            // Trả về token nếu xác thực thành công
            return Ok(new
            {
                Status = 1,
                Message = message,
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Validate the incoming model
            if (model == null || string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                return BadRequest(new Response { Status = 0, Message = "Username, password, and confirm password are required." });
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(new Response { Status = 0, Message = "Password and confirm password do not match." });
            }

            if (model.Password.Contains(" ") || model.Password.Length > 100)
            {
                return BadRequest(new Response { Status = 0, Message = "Invalid password. Password must not contain spaces and must be less than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new Response { Status = 0, Message = "Email is required." });
            }

            // Validate email format
            if (!IsValidEmail(model.Email))
            {
                return BadRequest(new Response { Status = 0, Message = "Invalid email format." });
            }

            // Check if the username already exists
            var existingUser = await _userRepository.GetByUserName(model.UserName);
            if (existingUser != null)
            {
                return BadRequest(new Response { Status = 0, Message = "Username already exists. Please choose a different username." });
            }

            // Call stored procedure to register the user
            int rowsAffected = await _userRepository.Register(model);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response { Status = 0, Message = "An error occurred while registering the user." });
            }

            // Retrieve the newly created user by username
            var newUser = await _userRepository.GetByUserName(model.UserName);
            if (newUser == null)
            {
                return StatusCode(500, new Response { Status = 0, Message = "User was registered, but unable to retrieve new user details." });
            }

            // Add the new user to the default group (Group ID: 2)
            var userInGroupModel = new SysUserInGroupCreateModel
            {
                UserID = newUser.UserID,  // Use the ID of the newly created user
                GroupID = 2               // Default group ID
            };

            int groupRowsAffected = await _userInGroupRepository.Create(userInGroupModel);
            if (groupRowsAffected == 0)
            {
                return StatusCode(500, new Response { Status = 0, Message = "User registered but could not be added to the default group." });
            }

            // Success response
            return Ok(new Response { Status = 1, Message = "User registered and added to default group successfully." });
        }

        // Helper method to validate email format using regex
        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var (newToken, newRefreshToken) = await _userService.RefreshToken(model.RefreshToken);

            if (newToken == null)
            {
                return Unauthorized(new Response { Status = 0, Message = "Invalid or expired refresh token." });
            }

            return Ok(new ResponseRefreshToken
            {
                Status = 1,
                Data = newToken,
                Message = "Success!",
                RefreshToken = newRefreshToken
            });
        }
    }
}
