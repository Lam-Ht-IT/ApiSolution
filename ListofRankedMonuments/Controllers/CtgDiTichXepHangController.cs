using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtgDiTichXepHangController : ControllerBase
    {
        private readonly ICtgDiTichXepHangRepository _ditichxephangrepository;

        public CtgDiTichXepHangController(ICtgDiTichXepHangRepository ditichxephangrepository)
        {
            _ditichxephangrepository = ditichxephangrepository;
        }

        [CustomAuthorize(1, "ManageMonumentRanking")]
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
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Invalid page number. Page number must be greater than 0."
                });
            }

            if (pageSize <= 0 || pageSize > 50)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Invalid page size. Page size must be between 1 and 50."
                });
            }

            var result = await _ditichxephangrepository.GetAll(name, pageNumber, pageSize);
            var ditichxephangs = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!ditichxephangs.Any())
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = ditichxephangs
                });
            }

            return Ok(new Response
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

        [CustomAuthorize(1, "ManageMonumentRanking")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            var ditichxephang = await _ditichxephangrepository.GetByID(id);
            if (ditichxephang == null)
            {
                return Ok(new { Status = 0, Message = "Id not found", Data = ditichxephang });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = ditichxephang });
        }

        [CustomAuthorize(2, "ManageMonumentRanking")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CtgDiTichXepHangModelInsert model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.TenDiTich) || model.TenDiTich.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDiTich. Must not be empty and not exceed 50 characters." });
            }

            int ghiChuAsInt;
            if (!int.TryParse(model.GhiChu, out ghiChuAsInt) || ghiChuAsInt < 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. Must be a valid integer greater than or equal to 0." });
            }

            // Tạo mới đối tượng DiTichXepHang
            var newDiTich = new CtgDiTichXepHangModelInsert
            {
                TenDiTich = model.TenDiTich.Trim(),
                GhiChu = ghiChuAsInt.ToString()
            };

            // Thêm đối tượng vào database
            var result = await _ditichxephangrepository.Insert(newDiTich);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Inserted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Insertion failed" });
        }

        [CustomAuthorize(4, "ManageMonumentRanking")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CtgDiTichXepHangModelUpdate diTichXepHang)
        {
            var existingDiTich = await _ditichxephangrepository.GetByID(diTichXepHang.DiTichXepHangID);
            if (existingDiTich == null) return Ok(new { Status = 0, Message = "ID not found" });
            if (!string.IsNullOrWhiteSpace(diTichXepHang.TenDiTich))
            {
                diTichXepHang.TenDiTich = diTichXepHang.TenDiTich.Trim();
            }

            if (string.IsNullOrWhiteSpace(diTichXepHang.TenDiTich) || diTichXepHang.TenDiTich.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDiTich. Must not be empty and not exceed 50 characters." });
            }

            int ghiChuAsInt;
            if (!int.TryParse(diTichXepHang.GhiChu, out ghiChuAsInt) || ghiChuAsInt < 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. Must be a valid integer greater than or equal to 0." });
            }

            var result = await _ditichxephangrepository.Update(diTichXepHang);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Updated data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Update failed" });
        }

        [CustomAuthorize(8, "ManageMonumentRanking")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingDiTich = await _ditichxephangrepository.GetByID(id);
            if (existingDiTich == null) return Ok(new { Status = 0, Message = "ID not found" });

            var result = await _ditichxephangrepository.Delete(id);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Deleted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Deletion failed" });
        }
    }
}
