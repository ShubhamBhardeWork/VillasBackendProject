    using Microsoft.AspNetCore.Mvc;

    namespace Villas.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class VillasController : ControllerBase
        {
            private static readonly List<string> _villas = new List<string>
            {
                "Royal Sunshine Villa",
                "Blue Ridge Villa",
                "Next To Way Villa"
            };

            [HttpGet]
            public IActionResult GetVillas()
            {
                return Ok(_villas);
            }
        }
    }
