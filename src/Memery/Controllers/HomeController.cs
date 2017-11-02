using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HeyRed.Mime;
using MediatR;
using Memery.Diagnostics;
using Memery.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Memery.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Meme(string id)
        {
            try
            {
                var req = new ImageRequest(id, true);
                var resp = await Mediator.Send(req);
                return new FileStreamResult(System.IO.File.OpenRead(resp.FileLocation.FullName), resp.ContentType);
            }
            catch (ImageNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
