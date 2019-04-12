using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MushroomCloud.Services.SignalR.Hubs;

namespace MushroomCloud.Services.SignalR.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IHubContext<MushroomCloudHub> _hubContext;
        public HomeController(IHubContext<MushroomCloudHub> hubContext)
        {
            _hubContext = hubContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            await _hubContext.Clients.All.SendAsync("Notify", $"SignalR is loaded on: {DateTime.Now}");
            return View();
        }
    }
}
