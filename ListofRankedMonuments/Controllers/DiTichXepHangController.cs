using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Operators;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiTichXepHangController : ControllerBase
    {
        private readonly IDiTichXepHangRepository _ditichxephangrepository;

        public DiTichXepHangController(IDiTichXepHangRepository ditichxephangreposito)
        {
            _ditichxephangrepository = ditichxephangreposito;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
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

            var result = await _ditichxephangrepository.GetAll(name, pageNumber, pageSize);
            var ditichxephangs = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (ditichxephangs.Count() == 0)
            {
                return NotFound(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = ditichxephangs

                });

            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = ditichxephangs,
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
            var ditichxephang = await _ditichxephangrepository.GetByID(id);
            if (ditichxephang == null)
            {
                return NotFound(new {Status= 0, Message = "Id not found" });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = ditichxephang });
        }

        [HttpPost("Insert")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Insert([FromBody] DiTichXepHangModel model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.TenDiTich) || model.TenDiTich.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDiTich. Must not be empty and not exceed 100 characters." });
            }

            if (model.ThuTuXepHang < 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ThuTuXepHang. Must be greater than or equal to 0 and not be empty" });
            }

            // Tạo mới đối tượng DiTichXepHang
            var newDiTich = new DiTichXepHang
            {
                TenDiTich = model.TenDiTich.Trim(),
                ThuTuXepHang = model.ThuTuXepHang
            };

            // Thêm đối tượng vào database
            await _ditichxephangrepository.Insert(newDiTich);
            return CreatedAtAction(nameof(GetByID), new { id = newDiTich.DitichXepHangID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPut("Update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update([FromBody] DiTichXepHang diTichXepHang)
        {
            var existingDiTich = await _ditichxephangrepository.GetByID(diTichXepHang.DitichXepHangID);
            if (existingDiTich == null) return NotFound(new { Status = 0, Message = "ID not found" });

            if (string.IsNullOrWhiteSpace(diTichXepHang.TenDiTich) || diTichXepHang.TenDiTich.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDiTich. Must not be empty and not exceed 100 characters." });
            }

            if (diTichXepHang.ThuTuXepHang < 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ThuTuXepHang. Must be greater than or equal to 0." });
            }

            // Cập nhật đối tượng
            existingDiTich.TenDiTich = diTichXepHang.TenDiTich.Trim();
            existingDiTich.ThuTuXepHang = diTichXepHang.ThuTuXepHang;

            await _ditichxephangrepository.Update(existingDiTich);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingDiTich = await _ditichxephangrepository.GetByID(id);
            if (existingDiTich == null) return NotFound(new { Status = 0, Message = "ID not found" });

            await _ditichxephangrepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
