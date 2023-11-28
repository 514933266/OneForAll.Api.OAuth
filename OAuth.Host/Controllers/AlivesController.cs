using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 控制器
    /// </summary>
    [Route("api/[controller]")]
    public class AlivesController : Controller
    {
        [HttpGet]
        public async Task<string> GetAsync()
        {
            return "系统运行中";
        }
    }
}
