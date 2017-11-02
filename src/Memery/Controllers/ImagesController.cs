using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Memery.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memery.Controllers
{
    [Route("images")]
    public class ImagesController : Controller
    {
        public ImagesController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var req = new IndexRequest();
            var res = await Mediator.Send(req);
            return Ok(res.ToDictionary(k => k.Key, v => v.Value as ImageResponse));
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromForm] IFormFile file, [FromQuery] string url)
        {
            var req = string.IsNullOrWhiteSpace(url)
                ? new ImageUploadRequest(file)
                : new ImageUploadRequest(new Uri(url));
            var resp = await Mediator.Send(req);
            return Ok(resp as ImageResponse);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> AddNamedImage(string name, [FromForm] IFormFile file, [FromQuery] string url)
        {
            var req = string.IsNullOrWhiteSpace(url)
                ? new ImageUploadRequest(file, name)
                : new ImageUploadRequest(new Uri(url), name);
            var resp = await Mediator.Send(req);
            return Ok(resp as ImageResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveImage(string id)
        {
            try
            {

                var iReq = new ImageRequest(id);
                var iResp = await Mediator.Send(iReq);
                var req = new ImageDeletionRequest(iResp);
                await Mediator.Send(req);
                return NoContent();
            }
            catch (Diagnostics.ImageNotFoundException ex)
            {
                return StatusCode(410, ex.Message);
            }
        }
    }
}
