// SysGroupController.cs
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
    public class SysGroupsController : ControllerBase
    {
        private readonly ISysGroupRepository _groupRepository;

        public SysGroupsController(ISysGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet("GroupsList")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetAll(string? groupName, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                groupName = groupName.Trim();
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

            var (groups, totalRecords) = await _groupRepository.GetAll(groupName, pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!groups.Any())
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "No data available",
                    Data = groups
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = groups,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindingGroup")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByID(int groupId)
        {
            var group = await _groupRepository.GetByID(groupId);
            if (group == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Id not found",
                    Data = group
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = group
            });
        }

        [CustomAuthorize(2, "ManageUsers")]
        [HttpPost("CreatingGroup")]
        public async Task<IActionResult> Create([FromBody] SysGroupInsertModel group)
        {
            if (string.IsNullOrWhiteSpace(group.GroupName) || group.GroupName.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid group name. The group name must not contain spaces and group name is required."
                });
            }

            if (group.GroupName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Group name cannot exceed 50 characters."
                });
            }

            int rowsAffected = await _groupRepository.Create(group);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while creating the group."
                });
            }

            return CreatedAtAction(nameof(GetByID), new Response
            {
                Status = 1,
                Message = "Group created successfully."
            });
        }

        [HttpPost("UpdatingGroup")]
        [CustomAuthorize(4, "ManageUsers")]
        public async Task<IActionResult> Update([FromBody] SysGroupUpdateModel group)
        {
            var existingGroup = await _groupRepository.GetByID(group.GroupID);
            if (existingGroup == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Group not found."
                });
            }

            if (string.IsNullOrWhiteSpace(group.GroupName) || group.GroupName.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid group name. The group name must not contain spaces and group name is required."
                });
            }

            if (group.GroupName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Group name cannot exceed 50 characters."
                });
            }

            int rowsAffected = await _groupRepository.Update(group);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while updating the group."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Group updated successfully."
            });
        }

        [HttpPost("DeleteGroup")]
        [CustomAuthorize(8, "ManageUsers")]
        public async Task<IActionResult> Delete(int groupId)
        {
            var existingGroup = await _groupRepository.GetByID(groupId);
            if (existingGroup == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Group not found."
                });
            }

            int rowsAffected = await _groupRepository.Delete(groupId);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while deleting the group."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Group deleted successfully."
            });
        }
    }
}