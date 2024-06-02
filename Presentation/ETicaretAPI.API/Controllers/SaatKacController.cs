using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaatKacController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var serverTime = DateTime.Now;
            var utcTime = DateTime.UtcNow;

            return Ok(new { ServerTime = serverTime, UtcTime = utcTime });
        }
    }
}
