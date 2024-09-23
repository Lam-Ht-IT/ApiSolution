// SysFunctionController.cs
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
    public class SysFunctionsController : ControllerBase
    {
        private readonly ISysFunctionRepository _functionRepository;

        public SysFunctionsController(ISysFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }

        [HttpGet("FunctionsList")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetAll(string? functionName, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(functionName))
            {
                functionName = functionName.Trim();
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

            var (functions, totalRecords) = await _functionRepository.GetAll(functionName, pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!functions.Any())
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "No data available",
                    Data = functions
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = functions,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindingFunction")]
        [CustomAuthorize(1, "ManageUsers")]
        public async Task<IActionResult> GetByID(int functionId)
        {
            var function = await _functionRepository.GetByID(functionId);
            if (function == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Id not found",
                    Data = function
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = function
            });
        }

        [CustomAuthorize(2, "ManageUsers")]
        [HttpPost("CreatingFunction")]
        public async Task<IActionResult> Create([FromBody] SysFunctionInsertModel function)
        {
            if (string.IsNullOrWhiteSpace(function.FunctionName) || function.FunctionName.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid function name. The function name must not contain spaces and function name is required."
                });
            }

            if (function.FunctionName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Function name cannot exceed 50 characters."
                });
            }

            int rowsAffected = await _functionRepository.Create(function);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while creating the function."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Function created successfully."
            });
        }

        [HttpPost("UpdatingFunction")]
        [CustomAuthorize(4, "ManageUsers")]
        public async Task<IActionResult> Update([FromBody] SysFunctionUpdateModel function)
        {
            var existingFunction = await _functionRepository.GetByID(function.FunctionID);
            if (existingFunction == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Function not found."
                });
            }

            if (string.IsNullOrWhiteSpace(function.FunctionName) || function.FunctionName.Contains(" "))
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid function name. The function name must not contain spaces and function name is required."
                });
            }

            if (function.FunctionName.Length > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Function name cannot exceed 50 characters."
                });
            }

            int rowsAffected = await _functionRepository.Update(function);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while updating the function."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Function updated successfully."
            });
        }

        [HttpPost("DeleteFunction")]
        [CustomAuthorize(8, "ManageUsers")]
        public async Task<IActionResult> Delete(int functionId)
        {
            var existingFunction = await _functionRepository.GetByID(functionId);
            if (existingFunction == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Function not found."
                });
            }

            int rowsAffected = await _functionRepository.Delete(functionId);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while deleting the function."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Function deleted successfully."
            });
        }
    }
}
