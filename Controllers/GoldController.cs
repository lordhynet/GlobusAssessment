
using GlobusAssessment.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GLobusAssessment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldController : ControllerBase
    {
        private readonly IGoldService _goldService;

        public GoldController(IGoldService goldService)
        {
            _goldService = goldService;
        }

        /// <summary>
        /// This endpoint returns gold price live
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var response = await _goldService.GetAllGoldAsync();

            if (response?.Result.Count < 1) return NotFound();

            return Ok(response);
        }
    }
}
