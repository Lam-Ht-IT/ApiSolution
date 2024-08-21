using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KyBaoCaoController : ControllerBase
    {
        private readonly IKyBaoCaoRepository _kyBaoCaoRepository;

        public KyBaoCaoController(IKyBaoCaoRepository kyBaoCaoRepository)
        {
            _kyBaoCaoRepository = kyBaoCaoRepository;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(string? name, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            if (pageNumber <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid page number. Page number must be greater than 0." });
            }

            if (pageSize <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid page size. Page size must be between 1 and 50." });
            }

            var result = await _kyBaoCaoRepository.GetAll(name, pageNumber, pageSize);
            var kyBaoCaoList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (kyBaoCaoList.Count() == 0)
            {
                return NotFound(new { Status = 0, Message = "No data available" });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Get information successfully",
                Data = kyBaoCaoList,
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
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var kyBaoCao = await _kyBaoCaoRepository.GetByID(id);
            if (kyBaoCao == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = kyBaoCao });
        }

        [HttpPost("Insert")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Insert([FromBody] KyBaoCaoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TenKyBaoCao) || model.TenKyBaoCao.Length > 255)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenKyBaoCao. The TenKyBaoCao must be required , and not exceed 255 characters" });
            }
            
            var newKyBaoCao = new KyBaoCao
            {
                TenKyBaoCao = model.TenKyBaoCao,
                TrangThai = model.TrangThai,
                GhiChu = model.GhiChu,
                LoaiKyBaoCao = model.LoaiKyBaoCao
            };

            await _kyBaoCaoRepository.Insert(newKyBaoCao);
            return CreatedAtAction(nameof(GetByID), new { id = newKyBaoCao.KyBaoCaoID }, new { Status = 1, Message = "Inserted data successfully" });
        }

        [HttpPut("Update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(KyBaoCao kyBaoCao)
        {
            if (kyBaoCao.KyBaoCaoID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var existingKyBaoCao = await _kyBaoCaoRepository.GetByID(kyBaoCao.KyBaoCaoID);
            if (existingKyBaoCao == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }

            if (string.IsNullOrWhiteSpace(kyBaoCao.TenKyBaoCao) || kyBaoCao.TenKyBaoCao.Length > 255)
            {
                return BadRequest(new { Status = 0, Message = "Report period name is required and not exceed 255 characters" });
            }

            await _kyBaoCaoRepository.Update(kyBaoCao);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var kyBaoCao = await _kyBaoCaoRepository.GetByID(id);
            if (kyBaoCao == null)
            {
                return NotFound(new { Status = 0, Message = "ID not found" });
            }

            await _kyBaoCaoRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
