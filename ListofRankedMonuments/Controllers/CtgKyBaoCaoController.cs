using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QUANLYVANHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtgKyBaoCaoController : ControllerBase
    {
        private readonly ICtgKyBaoCaoRepository _kyBaoCaoRepository;

        public CtgKyBaoCaoController(ICtgKyBaoCaoRepository kyBaoCaoRepository)
        {
            _kyBaoCaoRepository = kyBaoCaoRepository;
        }

        [CustomAuthorize(1, "ManageReportingPeriod")]
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

            if (pageSize <= 0 || pageSize > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid page size. Page size must be between 1 and 50." });
            }

            var result = await _kyBaoCaoRepository.GetAll(name, pageNumber, pageSize);
            var kyBaoCaoList = result.Item1;
            var totalRecords = result.Item2;
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (kyBaoCaoList.Count() == 0)
            {
                return Ok(new { Status = 0, Message = "No data available" });
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

        [CustomAuthorize(1, "ManageReportingPeriod")]
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
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            return Ok(new { Status = 1, Message = "Get information successfully", Data = kyBaoCao });
        }

        [HttpPost("Insert")]
        [CustomAuthorize(2, "ManageReportingPeriod")]
        public async Task<IActionResult> Insert([FromBody] CtgKyBaoCaoModelInsert model)
        {
            if (!string.IsNullOrWhiteSpace(model.TenKyBaoCao))
            {
                model.TenKyBaoCao = model.TenKyBaoCao.Trim();
            }
            if (string.IsNullOrWhiteSpace(model.TenKyBaoCao) || model.TenKyBaoCao.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Invalid TenKyBaoCao. The TenKyBaoCao must be required , and not exceed 50 characters" });
            }

            // Thêm đối tượng vào database

            var newKyBaoCao = new CtgKyBaoCaoModelInsert
            {
                TenKyBaoCao = model.TenKyBaoCao,
                TrangThai = model.TrangThai,
                GhiChu = model.GhiChu,
            };

            var result = await _kyBaoCaoRepository.Insert(newKyBaoCao);
            {
                return Ok(new { Status = 1, Message = "Inserted data successfully" });
            }
        }

        [HttpPost("Update")]
        [CustomAuthorize(4, "ManageReportingPeriod")]
        public async Task<IActionResult> Update(CtgKyBaoCaoModelUpdate kyBaoCao)
        {
            if (kyBaoCao.KyBaoCaoID <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var existingKyBaoCao = await _kyBaoCaoRepository.GetByID(kyBaoCao.KyBaoCaoID);
            if (existingKyBaoCao == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            if (string.IsNullOrWhiteSpace(kyBaoCao.TenKyBaoCao) || kyBaoCao.TenKyBaoCao.Length > 50)
            {
                return BadRequest(new { Status = 0, Message = "Report period name is required and not exceed 50 characters" });
            }

            await _kyBaoCaoRepository.Update(kyBaoCao);
            return Ok(new { Status = 1, Message = "Updated data successfully" });
        }

        [HttpPost("Delete")]
        [CustomAuthorize(8, "ManageReportingPeriod")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Status = 0, Message = "Invalid ID. ID must be greater than 0." });
            }

            var kyBaoCao = await _kyBaoCaoRepository.GetByID(id);
            if (kyBaoCao == null)
            {
                return Ok(new { Status = 0, Message = "ID not found" });
            }

            await _kyBaoCaoRepository.Delete(id);
            return Ok(new { Status = 1, Message = "Deleted data successfully" });
        }
    }
}
