using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpChiTietMauPhieuController : ControllerBase
    {
        private readonly IRpChiTietMauPhieu _chiTietMauPhieuRepository;

        public RpChiTietMauPhieuController(IRpChiTietMauPhieu chiTietMauPhieuRepository)
        {
            _chiTietMauPhieuRepository = chiTietMauPhieuRepository;
        }

        [CustomAuthorize(1, "ManageReportForm")]
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

            var result = await _chiTietMauPhieuRepository.GetAll(name);
            var chiTietMauPhieuList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!chiTietMauPhieuList.Any())
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = chiTietMauPhieuList
                });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = chiTietMauPhieuList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [CustomAuthorize(1, "ManageReportForm")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var chiTietMauPhieu = await _chiTietMauPhieuRepository.GetByID(id);
            if (chiTietMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = chiTietMauPhieu });
        }

        [CustomAuthorize(2, "ManageReportForm")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] RpChiTietMauPhieu model)
        {
            // Trim and validate strings
            model.NoiDung = model.NoiDung?.Trim();
            model.TieuChiIDs = model.TieuChiIDs?.Trim();
            model.GhiChu = model.GhiChu?.Trim();

            // Initialize a list to hold validation errors
            var validationErrors = new List<string>();

            // Validate required fields and their constraints
            if (model.MauPhieuId <= 0)
            {
                validationErrors.Add("MauPhieuId must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(model.NoiDung))
            {
                validationErrors.Add("NoiDung must not be empty.");
            }
            else if (model.NoiDung.Length > 500)
            {
                validationErrors.Add("NoiDung must not exceed 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.TieuChiIDs))
            {
                validationErrors.Add("TieuChiIDs must not be empty.");
            }
            else if (model.TieuChiIDs.Length > 200)
            {
                validationErrors.Add("TieuChiIDs must not exceed 200 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.GhiChu))
            {
                model.GhiChu = string.Empty; // Set GhiChu to empty string if null
            }
            else if (model.GhiChu.Length > 500)
            {
                validationErrors.Add("GhiChu must not exceed 500 characters.");
            }

            // Validate numeric fields
            if (model.ChitieuIDs <= 0)
            {
                validationErrors.Add("ChitieuIDs must be greater than 0.");
            }

            if (model.GopCot < 0)
            {
                validationErrors.Add("GopCot cannot be negative.");
            }

            if (model.GoptuCot < 0)
            {
                validationErrors.Add("GoptuCot cannot be negative.");
            }

            if (model.GopDenCot < 0)
            {
                validationErrors.Add("GopDenCot cannot be negative.");
            }

            if (model.SoCotGop < 0)
            {
                validationErrors.Add("SoCotGop cannot be negative.");
            }

            // Return validation errors if any
            if (validationErrors.Any())
            {
                return BadRequest(new { Status = 0, Message = "Validation failed", Errors = validationErrors });
            }

            var result = await _chiTietMauPhieuRepository.Insert(model);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Inserted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Insertion failed" });
        }

        [CustomAuthorize(4, "ManageReportForm")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] RpChiTietMauPhieu model)
        {
            if (model.ChiTietMauPhieuId <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ChiTietMauPhieuId. Must be greater than 0." });
            }

            var existingChiTietMauPhieu = await _chiTietMauPhieuRepository.GetByID(model.ChiTietMauPhieuId);
            if (existingChiTietMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "ChiTietMauPhieuId not found" });
            }

            // Trim and validate strings
            model.NoiDung = model.NoiDung?.Trim();
            model.TieuChiIDs = model.TieuChiIDs?.Trim();
            model.GhiChu = model.GhiChu?.Trim();

            // Initialize a list to hold validation errors
            var validationErrors = new List<string>();

            // Validate required fields and their constraints
            if (model.MauPhieuId <= 0)
            {
                validationErrors.Add("MauPhieuId must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(model.NoiDung))
            {
                validationErrors.Add("NoiDung must not be empty.");
            }
            else if (model.NoiDung.Length > 500)
            {
                validationErrors.Add("NoiDung must not exceed 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.TieuChiIDs))
            {
                validationErrors.Add("TieuChiIDs must not be empty.");
            }
            else if (model.TieuChiIDs.Length > 200)
            {
                validationErrors.Add("TieuChiIDs must not exceed 200 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.GhiChu))
            {
                model.GhiChu = string.Empty; // Set GhiChu to empty string if null
            }
            else if (model.GhiChu.Length > 500)
            {
                validationErrors.Add("GhiChu must not exceed 500 characters.");
            }

            // Validate numeric fields
            if (model.ChitieuIDs <= 0)
            {
                validationErrors.Add("ChitieuIDs must be greater than 0.");
            }

            if (model.GopCot < 0)
            {
                validationErrors.Add("GopCot cannot be negative.");
            }

            if (model.GoptuCot < 0)
            {
                validationErrors.Add("GoptuCot cannot be negative.");
            }

            if (model.GopDenCot < 0)
            {
                validationErrors.Add("GopDenCot cannot be negative.");
            }

            if (model.SoCotGop < 0)
            {
                validationErrors.Add("SoCotGop cannot be negative.");
            }

            // Return validation errors if any
            if (validationErrors.Any())
            {
                return BadRequest(new { Status = 0, Message = "Validation failed", Errors = validationErrors });
            }

            var result = await _chiTietMauPhieuRepository.Update(model);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Updated data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Update failed" });
        }

        [CustomAuthorize(8, "ManageReportForm")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var existingChiTietMauPhieu = await _chiTietMauPhieuRepository.GetByID(id);
            if (existingChiTietMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            var result = await _chiTietMauPhieuRepository.Delete(id);
            if (result > 0)
            {
                return Ok(new { Status = 1, Message = "Deleted data successfully" });
            }
            return StatusCode(500, new { Status = 0, Message = "Deletion failed" });
        }
    }
}
