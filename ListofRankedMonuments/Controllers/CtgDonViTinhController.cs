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
    public class CtgDonViTinhController : ControllerBase
    {
        private readonly ICtgDonViTinhRepository _donViTinhRepository;

        public CtgDonViTinhController(ICtgDonViTinhRepository donViTinhRepository)
        {
            _donViTinhRepository = donViTinhRepository;
        }

        [CustomAuthorize(1, "ManageUnitofMeasure")]
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

            var result = await _donViTinhRepository.GetAll(name, pageNumber, pageSize);
            var donViTinhList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!donViTinhList.Any())
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = donViTinhList
                });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = donViTinhList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [CustomAuthorize(1, "ManageUnitofMeasure")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            var donViTinh = await _donViTinhRepository.GetByID(id);
            if (donViTinh == null)
            {
                return Ok(new { Status = 0, Message = "Id not found" });
            }
            return Ok(new { Status = 1, Message = "Get information successfully", Data = donViTinh });
        }

        [CustomAuthorize(2, "ManageUnitofMeasure")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CtgDonViTinhModelInsert model)
        {
            if (!string.IsNullOrWhiteSpace(model.TenDonViTinh))
            {
                model.TenDonViTinh = model.TenDonViTinh.Trim();
            }
            // Validate input data
            if (string.IsNullOrWhiteSpace(model.TenDonViTinh) || model.TenDonViTinh.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDonViTinh. Must not be empty and not exceed 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.MaDonViTinh) || model.MaDonViTinh.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaDonViTinh. Must not be empty and not exceed 50 characters." });
            }

            if (model.GhiChu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. Must not exceed 50 characters." });
            }

            // Create a new CtgDonViTinh object
            var newDonViTinh = new CtgDonViTinhModelInsert
            {
                TenDonViTinh = model.TenDonViTinh.Trim(),
                MaDonViTinh = model.MaDonViTinh.Trim(),
                GhiChu = model.GhiChu?.Trim(),
                TrangThai = model.TrangThai
            };

            // Insert the object into the database
            var result = await _donViTinhRepository.Insert(newDonViTinh);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Inserted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Insertion failed" });
        }

        [CustomAuthorize(4, "ManageUnitofMeasure")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CtgDonViTinhModelUpdate model)
        {
            var existingDonViTinh = await _donViTinhRepository.GetByID(model.DonViTinhID);
            if (existingDonViTinh == null) return Ok(new { Status = 0, Message = "ID not found" });

            if (!string.IsNullOrWhiteSpace(model.TenDonViTinh))
            {
                model.TenDonViTinh = model.TenDonViTinh.Trim();
            }


            if (string.IsNullOrWhiteSpace(model.TenDonViTinh) || model.TenDonViTinh.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenDonViTinh. Must not be empty and not exceed 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.MaDonViTinh) || model.MaDonViTinh.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaDonViTinh. Must not be empty and not exceed 50 characters." });
            }

            if (model.GhiChu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. Must not exceed 50 characters." });
            }

            var result = await _donViTinhRepository.Update(model);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Updated data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Update failed" });
        }

        [CustomAuthorize(8, "ManageUnitofMeasure")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingDonViTinh = await _donViTinhRepository.GetByID(id);
            if (existingDonViTinh == null) return Ok(new { Status = 0, Message = "ID not found" });

            var result = await _donViTinhRepository.Delete(id);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Deleted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Deletion failed" });
        }
    }
}
