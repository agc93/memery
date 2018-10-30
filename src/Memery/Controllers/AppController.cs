using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HeyRed.Mime;
using MediatR;
using Memery.Configuration;
using Memery.Diagnostics;
using Memery.Infrastructure;
using Memery.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Memery.Controllers
{
    public class AppController : Controller
    {

        public AppController(IMediator mediator, Services.MetadataService metadata)
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        // public IActionResult Error()
        // {
        //     ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        //     return View();
        // }

        /// <summary>
        /// Gets the details of the running instance.
        /// </summary>
        /// <returns>JSON object with details of the running Memery instance.</returns>
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            var resp = await Mediator.Send(new AppMetadataRequest());
            return Ok(resp);
        }
    }
}
