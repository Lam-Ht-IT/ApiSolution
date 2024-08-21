using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            if (pageNumber <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid page number. Page number must be greater than 0." });
            }

            if (pageSize <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid page size. Page size must be greater than 0." });
            }

            var result = await _roleRepository.GetAll(name, pageNumber, pageSize);
            var roles = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (roles.Count() == 0)
            {
                return NotFound(new { Status = 0, Message = "No data available", Data = roles });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = roles,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindByID")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetByID(int id)
        {
            var role = await _roleRepository.GetByID(id);
            if (role == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = role });
        }

        [HttpPost("Insert")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Insert([FromBody] RoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleName) || model.RoleName.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid RoleName. Must not be empty and not exceed 100 characters." });
            }

            var newRole = new Role
            {
                RoleName = model.RoleName.Trim()
            };

            await _roleRepository.Insert(newRole);
            return CreatedAtAction(nameof(GetByID), new { id = newRole.RoleID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPut("Update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update([FromBody] Role role)
        {
            var existingRole = await _roleRepository.GetByID(role.RoleID);
            if (existingRole == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }

            if (string.IsNullOrWhiteSpace(role.RoleName) || role.RoleName.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid RoleName. Must not be empty and not exceed 100 characters." });
            }

            existingRole.RoleName = role.RoleName.Trim();

            await _roleRepository.Update(existingRole);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingRole = await _roleRepository.GetByID(id);
            if (existingRole == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }

            await _roleRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
