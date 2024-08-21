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

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name, int pageNumber = 1, int pageSize = 20)
        {
            // Xử lý và kiểm tra dữ liệu đầu vào
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            // Xác thực số trang và kích thước trang
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

            // Gọi repository để lấy dữ liệu
            var result = await _tieuchirepository.GetAll(name, pageNumber, pageSize);
            var tieuchiList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (tieuchiList.Count() == 0)
            {
                return NotFound(new
                {
                    Status = 0,
                    Message = "No data available",
                    Data = tieuchiList

                });

            }


            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = tieuchiList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("FindByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID" });
            }

            var tieuchi = await _tieuchirepository.GetByID(id);
            if (tieuchi == null)
            {
                return NotFound(new { Status = 0, Message = "Id not found" });
            }

            // Convert entity to model if needed
            var tieuchiModel = new CtgTieuChiModel
            {
                TieuChiID = tieuchi.TieuChiID,
                MaTieuChi = tieuchi.MaTieuChi,
                TenTieuChi = tieuchi.TenTieuChi,
                TieuChiChaID = tieuchi.TieuChiChaID,
                GhiChu = tieuchi.GhiChu,
                KieuDuLieuCot = tieuchi.KieuDuLieuCot,
                TrangThai = tieuchi.TrangThai,
                LoaiTieuChi = tieuchi.LoaiTieuChi,
                Children = tieuchi.Children
            };

            return Ok(new { Status = 1, Message = "Get information successfully", Data = tieuchiModel });
        }
        [HttpPost("Insert")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Insert([FromBody] CtgTieuChiModel tieuchiModel)
        {
            if (tieuchiModel == null)
            {
                return BadRequest(new { Status = 0, Message = "Invalid data" });
            }

            if (string.IsNullOrEmpty(tieuchiModel.TenTieuChi))
            {
                return BadRequest(new { Status = 0, Message = "Name cannot be null or empty" });
            }

            var tieuchi = new CtgTieuChi
            {
                MaTieuChi = tieuchiModel.MaTieuChi,
                TenTieuChi = tieuchiModel.TenTieuChi,
                TieuChiChaID = tieuchiModel.TieuChiChaID,
                GhiChu = tieuchiModel.GhiChu,
                KieuDuLieuCot = tieuchiModel.KieuDuLieuCot,
                TrangThai = tieuchiModel.TrangThai,
                LoaiTieuChi = tieuchiModel.LoaiTieuChi
            };

            await _tieuchirepository.Insert(tieuchi);
            return CreatedAtAction(nameof(GetByID), new { id = tieuchi.TieuChiID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPut("Update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update([FromBody] CtgTieuChiModel tieuchiModel)
        {
            if (tieuchiModel == null || tieuchiModel.TieuChiID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid data or ID" });
            }

            var existingTieuChi = await _tieuchirepository.GetByID(tieuchiModel.TieuChiID);
            if (existingTieuChi == null)
            {
                return NotFound(new { Status = 0, Message = "Not Found ID" });
            }

            var tieuchi = new CtgTieuChi
            {
                TieuChiID = tieuchiModel.TieuChiID,
                MaTieuChi = tieuchiModel.MaTieuChi,
                TenTieuChi = tieuchiModel.TenTieuChi,
                TieuChiChaID = tieuchiModel.TieuChiChaID,
                GhiChu = tieuchiModel.GhiChu,
                KieuDuLieuCot = tieuchiModel.KieuDuLieuCot,
                TrangThai = tieuchiModel.TrangThai,
                LoaiTieuChi = tieuchiModel.LoaiTieuChi
            };

            await _tieuchirepository.Update(tieuchi);
            return Ok(new { Status = 1, Message = "Updated data Successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID" });
            }

            var tieuchi = await _tieuchirepository.GetByID(id);
            if (tieuchi == null)
            {
                return NotFound(new { Status = 0, Message = "Not found id" });
            }

            await _tieuchirepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}

