using Microsoft.AspNetCore.Mvc;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("error")]
        [HttpGet]
        public IActionResult Error(int code)
        {
            if(code == 404) 
            {
                return StatusCode(404);
            }            
            else if(code == 403)
            {
                return StatusCode(403);
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
