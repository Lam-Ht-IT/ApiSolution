using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtgLoaiMauPhieuController : ControllerBase
    {
        private readonly ICtgLoaiMauPhieuRepository _loaiMauPhieuRepository;

        public CtgLoaiMauPhieuController(ICtgLoaiMauPhieuRepository loaiMauPhieuRepository)
        {
            _loaiMauPhieuRepository = loaiMauPhieuRepository;
        }

        [CustomAuthorize(1, "ManageFormType")]
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

            var result = await _loaiMauPhieuRepository.GetAll(name, pageNumber, pageSize);
            var loaiMauPhieuList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!loaiMauPhieuList.Any())
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = loaiMauPhieuList
                });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = loaiMauPhieuList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindByID")]
        [CustomAuthorize(1, "ManageFormType")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }
            var loaiMauPhieu = await _loaiMauPhieuRepository.GetByID(id);
            if (loaiMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = loaiMauPhieu });
        }

        [HttpPost("Insert")]
        [CustomAuthorize(2, "ManageFormType")]
        public async Task<IActionResult> Insert([FromBody] CtgLoaiMauPhieuModelInsert model)
        {
            if (!string.IsNullOrWhiteSpace(model.TenLoaiMauPhieu))
            {
                model.TenLoaiMauPhieu = model.TenLoaiMauPhieu.Trim();
            }
            // Validate input data
            if (string.IsNullOrWhiteSpace(model.TenLoaiMauPhieu) || model.TenLoaiMauPhieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenLoaiMauPhieu. Must not be empty and not exceed 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.MaLoaiMauPhieu) || model.MaLoaiMauPhieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaLoaiMauPhieu. Must not be empty and not exceed 50 characters." });
            }

            int ghiChuAsInt;
            if (!int.TryParse(model.GhiChu, out ghiChuAsInt) || ghiChuAsInt < 1)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. Must be a valid integer greater than or equal to 0." });
            }


            // Create a new CtgLoaiMauPhieu object
            var newLoaiMauPhieu = new CtgLoaiMauPhieuModelInsert
            {
                TenLoaiMauPhieu = model.TenLoaiMauPhieu.Trim(),
                MaLoaiMauPhieu = model.MaLoaiMauPhieu.Trim(),
                GhiChu = ghiChuAsInt.ToString()
            };

            // Insert the object into the database
            var result = await _loaiMauPhieuRepository.Insert(newLoaiMauPhieu);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Inserted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Insertion failed" });
        }

        [HttpPost("Update")]
        [CustomAuthorize(4, "ManageFormType")]
        public async Task<IActionResult> Update([FromBody] CtgLoaiMauPhieuModelUpdate model)
        {
            if (model.LoaiMauPhieuID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var existingLoaiMauPhieu = await _loaiMauPhieuRepository.GetByID(model.LoaiMauPhieuID);
            if (existingLoaiMauPhieu == null) return Ok(new { Status = 0, Message = "ID not found" });

            if (!string.IsNullOrWhiteSpace(model.TenLoaiMauPhieu)) 
            {
                model.TenLoaiMauPhieu = model.TenLoaiMauPhieu.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.TenLoaiMauPhieu) || model.TenLoaiMauPhieu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenLoaiMauPhieu. Must not be empty and not exceed 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.MaLoaiMauPhieu) || model.MaLoaiMauPhieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaLoaiMauPhieu. Must not be empty and not exceed 50 characters." });
            }

            var result = await _loaiMauPhieuRepository.Update(model);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Updated data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Update failed" });
        }

        [HttpPost("Delete")]
        [CustomAuthorize(8, "ManageFormType")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingLoaiMauPhieu = await _loaiMauPhieuRepository.GetByID(id);
            if (existingLoaiMauPhieu == null) return Ok(new { Status = 0, Message = "ID not found" });

            var result = await _loaiMauPhieuRepository.Delete(id);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Deleted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Deletion failed" });
        }
    }
}
