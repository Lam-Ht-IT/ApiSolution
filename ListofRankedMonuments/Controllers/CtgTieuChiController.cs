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
    public class CtgTieuChiController : ControllerBase
    {
        private readonly ICtgTieuChiRepository _tieuchirepository;

        public CtgTieuChiController(ICtgTieuChiRepository tieuchirepository)
        {
            _tieuchirepository = tieuchirepository;
        }

        [CustomAuthorize(1, "ManageCriteria")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name/*, int pageNumber = 1, int pageSize = 20*/)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            //if (pageNumber <= 0)
            //{
            //    return BadRequest(new { Status = 0, Message = "Invalid page number. Page number must be greater than 0." });
            //}

            //if (pageSize <= 0 || pageSize > 50)
            //{
            //    return BadRequest(new { Status = 0, Message = "Invalid page size. Page size must be between 1 and 50." });
            //}

            var result = await _tieuchirepository.GetAll(name/*, pageNumber, pageSize*/);
            var tieuchiList = result.Item1;
            var totalRecords = result.Item2;
            //var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (tieuchiList.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data available" });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = tieuchiList,
                //PageNumber = pageNumber,
                //PageSize = pageSize,
                //TotalPages = totalPages,
                //TotalRecords = totalRecords
            });
        }

        [CustomAuthorize(1, "ManageCriteria")]
        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var tieuChi = await _tieuchirepository.GetByID(id);
            if (tieuChi == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            // Convert entity to model if needed
            return Ok(new { Status = 1, Message = "Get information successfully", Data = tieuChi });
        }

        [HttpPost("Insert")]
        [CustomAuthorize(2, "ManageCriteria")]
        public async Task<IActionResult> Insert([FromBody] CtgTieuChiModelInsert tieuchi)
        {
            if (!string.IsNullOrWhiteSpace(tieuchi.TenTieuChi))
            {
                tieuchi.TenTieuChi = tieuchi.TenTieuChi.Trim();
            }
            if (string.IsNullOrWhiteSpace(tieuchi.TenTieuChi) || tieuchi.TenTieuChi.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenTieuChi. The TenTieuChi must be required and not exceed 50 characters" });
            }

            if (string.IsNullOrWhiteSpace(tieuchi.MaTieuChi) || tieuchi.MaTieuChi.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaTieuChi. The MaTieuChi must be required and not exceed 50 characters" });
            }

            if (tieuchi.TieuChiChaID.HasValue)
            {
                var existingTieuChiCha = await _tieuchirepository.GetByID(tieuchi.TieuChiChaID.Value);
                if (existingTieuChiCha == null || tieuchi.TieuChiChaID <= 0)
                {
                    return BadRequest(new { Status = 0, Message = "TieuChiChaId not found or cannot set to 0. The TieuChiChaId must set to 'NULL' or greater than 0" });
                }
            }


            if (tieuchi.LoaiTieuChi <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid LoaiMauPhieuID. The LoaiMauPhieuID must be greater than 0" });
            }

            await _tieuchirepository.Insert(tieuchi);
            return Ok( new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPost("Update")]
        [CustomAuthorize(4, "ManageCriteria")]
        public async Task<IActionResult> Update([FromBody] CtgTieuChiModelUpdate tieuchi)
        {
            if (!string.IsNullOrWhiteSpace(tieuchi.TenTieuChi))
            {
                tieuchi.TenTieuChi = tieuchi.TenTieuChi.Trim();
            }

            if (tieuchi.TieuChiID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            if (tieuchi.TieuChiChaID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "TieuChiChaID cannot set to 0. The ChiTieuChaId must set to 'NULL' or greater than 0" });
            }

            var existingChiTieu = await _tieuchirepository.GetByID(tieuchi.TieuChiID);
            if (existingChiTieu == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            if (string.IsNullOrWhiteSpace(tieuchi.TenTieuChi) || tieuchi.TenTieuChi.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenChiTieu. The TenChiTieu must be required and not exceed 50 characters" });
            }

            if (string.IsNullOrWhiteSpace(tieuchi.MaTieuChi) || tieuchi.MaTieuChi.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid MaChiTieu. The MaChiTieu must be required and not exceed 50 characters" });
            }

            if (tieuchi.GhiChu.Length > 100)
            {
                return BadRequest(new { Status = 0, Message = "Invalid GhiChu. The GhiChu must not exceed 100 characters" });
            }

            if (tieuchi.LoaiTieuChi <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid LoaiMauPhieuID. The LoaiMauPhieuID must be greater than 0" });
            }

            await _tieuchirepository.Update(tieuchi);
            return Ok(new { Status = 1, Message = "Updated data Successfully" });
        }

        [HttpPost("Delete")]
        [CustomAuthorize(8, "ManageCriteria")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID" });
            }

            var tieuchi = await _tieuchirepository.GetByID(id);
            if (tieuchi == null)
            {
                return Ok(new { Status = 0, Message = "Not found id" });
            }

            await _tieuchirepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}

