using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysFunctionInGroupController : ControllerBase
    {
        private readonly ISysFunctionInGroupRepository _sysFunctionInGroupRepository;

        public SysFunctionInGroupController(ISysFunctionInGroupRepository sysFunctionInGroupRepository)
        {
            _sysFunctionInGroupRepository = sysFunctionInGroupRepository;
        }

        [CustomAuthorize(1, "ManageUsers")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var functionInGroups = await _sysFunctionInGroupRepository.GetAll();
            if (!functionInGroups.Any())
            {
                return Ok(new { Status = 0, Message = "No data available" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = functionInGroups });
        }

        [CustomAuthorize(1, "ManageUsers")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            var functionInGroup = await _sysFunctionInGroupRepository.GetByID(id);
            if (functionInGroup == null)
            {
                return Ok(new { Status = 0, Message = "Id not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = functionInGroup });
        }

        [CustomAuthorize(1, "ManageUsers")]
        [HttpGet("FindByGroupID")]
        public async Task<IActionResult> GetByGroupID(int groupId)
        {
            var functionInGroups = await _sysFunctionInGroupRepository.GetByGroupID(groupId);
            if (!functionInGroups.Any())
            {
                return Ok(new { Status = 0, Message = "No data found for this group" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = functionInGroups });
        }

        [HttpGet("FindByFunctionID")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByFunctionID(int functionId)
        {
            var functionInGroups = await _sysFunctionInGroupRepository.GetByFunctionID(functionId);
            if (!functionInGroups.Any())
            {
                return Ok(new { Status = 0, Message = "No data found for this function" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = functionInGroups });
        }

        [HttpPost("Create")]
        [CustomAuthorize(2, "ManageUsers")]
        public async Task<IActionResult> Create([FromBody] SysFunctionInGroupInsertModel model)
        {
            if (model.GroupID <= 0 || model.FunctionID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid data. GroupID and FunctionID must be greater than 0." });
            }

            var newFunctionInGroupID = await _sysFunctionInGroupRepository.Create(model);
            return CreatedAtAction(nameof(GetByID), new { id = newFunctionInGroupID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPost("Update")]
        [CustomAuthorize(4, "ManageUsers")]
        public async Task<IActionResult> Update([FromBody] SysFunctionInGroupUpdateModel model)
        {
            var existingFunctionInGroup = await _sysFunctionInGroupRepository.GetByID(model.FunctionInGroupID);
            if (existingFunctionInGroup == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _sysFunctionInGroupRepository.Update(model);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpPost("Delete")]
        [CustomAuthorize(8, "ManageUsers")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingFunctionInGroup = await _sysFunctionInGroupRepository.GetByID(id);
            if (existingFunctionInGroup == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _sysFunctionInGroupRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
