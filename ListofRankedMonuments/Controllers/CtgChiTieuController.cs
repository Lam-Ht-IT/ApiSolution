using QUANLYVANHOA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Controllers
{

    [Route("api/[controller]")]
    [CustomAuthorize(1, "ManageTarget")]
    [ApiController]
    public class CtgChiTieuController : ControllerBase
    {
        private readonly ICtgChiTieuRepository _chiTieuRepository;
        private readonly ICtgLoaiMauPhieuRepository _loaiMauPhieuRepository;

        public CtgChiTieuController(ICtgChiTieuRepository chiTieuRepository, ICtgLoaiMauPhieuRepository loaiMauPhieuRepository)
        {
            _chiTieuRepository = chiTieuRepository;
            _loaiMauPhieuRepository = loaiMauPhieuRepository;
        }

        [CustomAuthorize(1, "ManageTarget")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name/*, int pageNumber = 1, int pageSize = 100*/)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            //if (pageNumber <= 0)
            //{
            //    return BadRequest(new { Status = 0, Message = "Invalid page number. Page number must be greater than 0." });
            //}

            //if (pageSize <= 0 || pageSize > 100)
            //{
            //    return BadRequest(new { Status = 0, Message = "Invalid page size. Page size must be between 1 and 100." });
            //}

            var result = await _chiTieuRepository.GetAll(name/*, pageNumber, pageSize*/);
            var chiTieuList = result.Item1;
            //var totalRecords = result.Item2;
            //var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (chiTieuList.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data available" });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = chiTieuList,
                //PageNumber = pageNumber,
                //PageSize = pageSize,
                //TotalPages = totalPages,
                //TotalRecords = totalRecords
            });
        }

        [CustomAuthorize(1, "ManageTarget")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var chiTieu = await _chiTieuRepository.GetByID(id);
            if (chiTieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = chiTieu });
        }

        [HttpGet("GetByLoaiMauPhieuID")]
        [CustomAuthorize(2, "ManageTarget")]
        public async Task<IActionResult> GetByLoaiMauPhieuID(int loaiMauPhieuID)
        {
            if (loaiMauPhieuID <= 0)
            {
                return BadRequest("Invalid LoaiMauPhieuID.");
            }

            try
            {
                var chiTieuHierarchy = await _chiTieuRepository.GetByLoaiMauPhieuID(loaiMauPhieuID);

                if (chiTieuHierarchy == null)
                {
                    return NotFound($"No ChiTieu found for LoaiMauPhieuID: {loaiMauPhieuID}");
                }

                return Ok(chiTieuHierarchy);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [CustomAuthorize(2, "ManageTarget")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CtgChiTieuInsertModel chiTieu)
        {
            if (!string.IsNullOrWhiteSpace(chiTieu.TenChiTieu))
            {
                chiTieu.TenChiTieu = chiTieu.TenChiTieu.Trim();
            }
            if (string.IsNullOrWhiteSpace(chiTieu.TenChiTieu) || chiTieu.TenChiTieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenChiTieu. The TenChiTieu must be required and not exceed 50 characters" });
            }

            if (string.IsNullOrWhiteSpace(chiTieu.MaChiTieu) || chiTieu.MaChiTieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaChiTieu. The MaChiTieu must be required and not exceed 50 characters" });
            }

            if (chiTieu.ChiTieuChaID.HasValue)
            {
                var existingChiTieuCha = await _chiTieuRepository.GetByID(chiTieu.ChiTieuChaID.Value);
                if (existingChiTieuCha == null || chiTieu.ChiTieuChaID <= 0)
                {
                    return BadRequest(new { Status = 0, Message = "ChiTieuChaId not found or cannot set to 0. The ChiTieuChaId must set to 'NULL' or greater than 0" });
                }
            }

            if (chiTieu.GhiChu.Length > 300)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. The GhiChu must not exceed 300 characters" });
            }

            var existingLoaiMauPhieu = await _loaiMauPhieuRepository.GetByID(chiTieu.LoaiMauPhieuID);
            if (existingLoaiMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "LoaiMauPhieu does not exist" });
            }
            if (chiTieu.LoaiMauPhieuID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid LoaiMauPhieuID. It must be greater than 0." });
            }
   
            await _chiTieuRepository.Insert(chiTieu);
            return Ok(new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPost("InsertChildren")]
        [CustomAuthorize(2, "ManageTarget")]
        public async Task<IActionResult> InsertChiTieuCon([FromBody] CtgChiTieuInsertChidrenModel chiTieuModelInsertChidren)
        {
            if (!string.IsNullOrWhiteSpace(chiTieuModelInsertChidren.TenChiTieu))
            {
                chiTieuModelInsertChidren.TenChiTieu = chiTieuModelInsertChidren.TenChiTieu.Trim();
            }
            if (string.IsNullOrWhiteSpace(chiTieuModelInsertChidren.TenChiTieu) || chiTieuModelInsertChidren.TenChiTieu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenChiTieu. The TenChiTieu must be required and not exceed 50 characters" });
            }

            if (string.IsNullOrWhiteSpace(chiTieuModelInsertChidren.MaChiTieu) || chiTieuModelInsertChidren.MaChiTieu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaChiTieu. The MaChiTieu must be required and not exceed 50 characters" });
            }

            if (chiTieuModelInsertChidren.ChiTieuChaID.HasValue)
            {
                var existingChiTieuCha = await _chiTieuRepository.GetByID(chiTieuModelInsertChidren.ChiTieuChaID.Value);
                if (existingChiTieuCha == null || chiTieuModelInsertChidren.ChiTieuChaID <= 0)
                {
                    return BadRequest(new { Status = 0, Message = "ChiTieuChaId not found or cannot set to 0. The ChiTieuChaId must set to 'NULL' or greater than 0" });
                }
            }

            if (chiTieuModelInsertChidren.GhiChu.Length > 300)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. The GhiChu must not exceed 300 characters" });
            }
            try
            {
                await _chiTieuRepository.InsertChildren(chiTieuModelInsertChidren);
                return Ok(new { Status = 1, Message = "Inserted successfully." });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [CustomAuthorize(4, "ManageTarget")]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(CtgChiTieuUpdateModel chiTieu)
        {
            if (!string.IsNullOrWhiteSpace(chiTieu.TenChiTieu))
            {
                chiTieu.TenChiTieu = chiTieu.TenChiTieu.Trim();
            }

            if (chiTieu.ChiTieuID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            if (chiTieu.ChiTieuChaID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "ChiTieuChaID cannot set to 0. The ChiTieuChaId must set to 'NULL' or greater than 0" });
            }

            var existingChiTieu = await _chiTieuRepository.GetByID(chiTieu.ChiTieuID);
            if (existingChiTieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            if (string.IsNullOrWhiteSpace(chiTieu.TenChiTieu) || chiTieu.TenChiTieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenChiTieu. The TenChiTieu must be required and not exceed 50 characters" });
            }

            if (string.IsNullOrWhiteSpace(chiTieu.MaChiTieu) || chiTieu.MaChiTieu.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaChiTieu. The MaChiTieu must be required and not exceed 50 characters" });
            }

            if (chiTieu.GhiChu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. The GhiChu must not exceed 100 characters" });
            }

            var existingLoaiMauPhieu = await _loaiMauPhieuRepository.GetByID(chiTieu.LoaiMauPhieuID);
            if (existingLoaiMauPhieu == null)
            {
                return Ok(new { Status = 0, Message = "LoaiMauPhieu does not exist" });
            }
            if (chiTieu.LoaiMauPhieuID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid LoaiMauPhieuID. It must be greater than 0." });
            }


            await _chiTieuRepository.Update(chiTieu);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [CustomAuthorize(8, "ManageTarget")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var chiTieu = await _chiTieuRepository.GetByID(id);
            if (chiTieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _chiTieuRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
