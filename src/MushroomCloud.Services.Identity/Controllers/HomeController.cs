using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MushroomCloud.Services.Identity.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
