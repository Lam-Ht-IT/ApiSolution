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
    public class RpMauPhieuController : ControllerBase
    {
        private readonly IRpMauPhieuRepository _mauPhieuRepository;

        public RpMauPhieuController(IRpMauPhieuRepository mauPhieuRepository)
        {
            _mauPhieuRepository = mauPhieuRepository;
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

            try
            {
                var (mauPhieuList, totalRecords) = await _mauPhieuRepository.GetAllMauPhieu(name, pageNumber, pageSize);
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                if (!mauPhieuList.Any())
                {
                    return Ok(new Response
                    {
                        Status = 0,
                        Message = "No data available",
                        Data = mauPhieuList
                    });
                }

                return Ok(new Response
                {
                    Status = 1,
                    Message = "Get information successfully",
                    Data = mauPhieuList,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalRecords = totalRecords
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "An error occurred while fetching data.", Error = ex.Message });
            }
        }

        [CustomAuthorize(1, "ManageReportForm")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            try
            {
                var mauPhieu = await _mauPhieuRepository.GetMauPhieuByID(id);
                if (mauPhieu == null)
                {
                    return Ok(new { Status = 0, Message = "ID not found" });
                }

                return Ok(new { Status = 1, Message = "Get information successfully", Data = mauPhieu });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "An error occurred while fetching data.", Error = ex.Message });
            }
        }

        [CustomAuthorize(2, "ManageReportForm")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] RpMauPhieuInsertModel model)
        {
            try
            {
                var existingMauPhieu = await _mauPhieuRepository.GetAllMauPhieu(model.TenMauPhieu, 1, 20);
                if (existingMauPhieu.Item1.Any())
                {
                    return Ok(new { Status = 0, Message = "TenMauPhieu already exists" });
                }

                if (!string.IsNullOrWhiteSpace(model.TenMauPhieu))
                {
                    model.TenMauPhieu = model.TenMauPhieu.Trim();
                }

                // Validate input data
                if (string.IsNullOrWhiteSpace(model.TenMauPhieu) || model.TenMauPhieu.Length > 50)
                {
                    return BadRequest(new { Status = 0, Message = "Invalid TenMauPhieu. Must not be empty and not exceed 50 characters." });
                }

                if (string.IsNullOrWhiteSpace(model.MaMauPhieu) || model.MaMauPhieu.Length > 50)
                {
                    return BadRequest(new { Status = 0, Message = "Invalid MaMauPhieu. Must not be empty and not exceed 50 characters." });
                }

                // Allow ChiTietMauPhieus to be null
                if (model.ChiTietMauPhieus == null)
                {
                    model.ChiTietMauPhieus = new List<RpChiTietMauPhieuInsertModel>();
                }

                var result = await _mauPhieuRepository.CreateMauPhieu(model);
                if (result > 0)
                {
                    return Ok(new { Status = 1, Message = "Inserted data successfully" });
                }
                return StatusCode(500, new { Status = 0, Message = "Insertion failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "An error occurred while inserting data.", Error = ex.Message });
            }
        }

        [CustomAuthorize(4, "ManageReportForm")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] RpMauPhieuUpdateModel model)
        {
            try
            {
                var existingTenMauPhieu = await _mauPhieuRepository.GetAllMauPhieu(model.TenMauPhieu, 1, 20);
                if (existingTenMauPhieu.Item1.Any())
                {
                    return Ok(new { Status = 0, Message = "TenMauPhieu already exists" });
                }

                if (model.MauPhieuID <= 0)
                {
                    return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
                }

                var existingMauPhieu = await _mauPhieuRepository.GetMauPhieuByID(model.MauPhieuID);
                if (existingMauPhieu == null) return Ok(new { Status = 0, Message = "ID not found" });

                if (!string.IsNullOrWhiteSpace(model.TenMauPhieu))
                {
                    model.TenMauPhieu = model.TenMauPhieu.Trim();
                }

                if (string.IsNullOrWhiteSpace(model.TenMauPhieu) || model.TenMauPhieu.Length > 100)
                {
                    return BadRequest(new { Status = 0, Message = "Invalid TenMauPhieu. Must not be empty and not exceed 100 characters." });
                }

                if (string.IsNullOrWhiteSpace(model.MaMauPhieu) || model.MaMauPhieu.Length > 50)
                {
                    return BadRequest(new { Status = 0, Message = "Invalid MaMauPhieu. Must not be empty and not exceed 50 characters." });
                }

                var result = await _mauPhieuRepository.UpdateMauPhieu(model);
                if (result > 0)
                {
                    return Ok(new { Status = 1, Message = "Updated data successfully" });
                }
                return StatusCode(500, new { Status = 0, Message = "Update failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "An error occurred while updating data.", Error = ex.Message });
            }
        }

        [CustomAuthorize(8, "ManageReportForm")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingMauPhieu = await _mauPhieuRepository.GetMauPhieuByID(id);
                if (existingMauPhieu == null) return Ok(new { Status = 0, Message = "ID not found" });

                var result = await _mauPhieuRepository.DeleteMauPhieu(id);
                if (result > 0)
                {
                    return Ok(new { Status = 1, Message = "Deleted data successfully" });
                }
                return StatusCode(500, new { Status = 0, Message = "Deletion failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "An error occurred while deleting data.", Error = ex.Message });
            }
        }
    }
}