using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtgChiTieuController : ControllerBase
    {
        private readonly ICtgChiTieuRepository _chiTieuRepository;

        public CtgChiTieuController(ICtgChiTieuRepository chiTieuRepository)
        {
            _chiTieuRepository = chiTieuRepository;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name, int pageNumber = 1, int pageSize = 20)
        {
            var result = await _chiTieuRepository.GetAll(name, pageNumber, pageSize);
            var chiTieuList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = chiTieuList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            var chiTieu = await _chiTieuRepository.GetByID(id);
            if (chiTieu == null)
            {
                return NotFound(new { Status = 0, Message = "Id not found" });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = chiTieu });
        }

        [HttpPost("Insert")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Insert(CtgChiTieu chiTieu)
        {
            await _chiTieuRepository.Insert(chiTieu);
            return CreatedAtAction(nameof(GetByID), new { id = chiTieu.ChiTieuID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPut("Update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(CtgChiTieu chiTieu)
        {
            var existingChiTieu = await _chiTieuRepository.GetByID(chiTieu.ChiTieuID);
            if (existingChiTieu == null)
                return NotFound(new { Status = 0, Message = "Not Found ID" });

            await _chiTieuRepository.Update(chiTieu);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var chiTieu = await _chiTieuRepository.GetByID(id);
            if (chiTieu == null)
            {
                return NotFound(new { Status = 0, Message = "Id not found" });
            }

            await _chiTieuRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
