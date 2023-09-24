using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace course_work_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        [HttpGet]
        public int lol()
        {
            return 1;
        }
    }
}
