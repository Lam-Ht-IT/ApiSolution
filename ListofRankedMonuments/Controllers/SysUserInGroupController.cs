using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserInGroupController : ControllerBase
    {
        private readonly ISysUserInGroupRepository _sysUserInGroupRepository;

        public SysUserInGroupController(ISysUserInGroupRepository sysUserInGroupRepository)
        {
            _sysUserInGroupRepository = sysUserInGroupRepository;
        }

        [HttpGet("List")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetAll()
        {
            var userInGroups = await _sysUserInGroupRepository.GetAll();
            if (userInGroups.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data available" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = userInGroups });
        }

        [HttpGet("FindByID")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByID(int id)
        {
            var userInGroup = await _sysUserInGroupRepository.GetByID(id);
            if (userInGroup == null)
            {
                return Ok(new { Status = 0, Message = "Id not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = userInGroup });
        }

        [HttpGet("FindByGroupID")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByGroupID(int groupId)
        {
            var userInGroups = await _sysUserInGroupRepository.GetByGroupID(groupId);
            if (userInGroups.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data found for this group" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = userInGroups });
        }

        [HttpGet("FindByUserID")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByUserID(int userId)
        {
            var userInGroups = await _sysUserInGroupRepository.GetByUserID(userId);
            if (userInGroups.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data found for this user" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = userInGroups });
        }

        [HttpPost("Create")]
        [CustomAuthorize(2, "ManageUsers")]
        public async Task<IActionResult> Create([FromBody] SysUserInGroupCreateModel model)
        {
            var existingUserInGroup = await _sysUserInGroupRepository.GetByID(model.UserID);


            if (model.UserID <= 0 || model.GroupID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid data. UserID and GroupID must be greater than 0." });
            }

            var newUserInGroupID = await _sysUserInGroupRepository.Create(model);
            return CreatedAtAction(nameof(GetByID), new { id = newUserInGroupID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPost("Update")]
        [CustomAuthorize(4, "ManageUsers")]
        public async Task<IActionResult> Update([FromBody] SysUserInGroupUpdateModel model)
        {
            var existingUserInGroup = await _sysUserInGroupRepository.GetByID(model.UserInGroupID);
            if (existingUserInGroup == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _sysUserInGroupRepository.Update(model);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpPost("Delete")]
        [CustomAuthorize(8, "ManageUsers")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUserInGroup = await _sysUserInGroupRepository.GetByID(id);
            if (existingUserInGroup == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _sysUserInGroupRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
