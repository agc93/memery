using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Memery.Diagnostics;
using Memery.Infrastructure;
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

        /// <summary>
        /// Returns the meme with the requested name or ID
        /// </summary>
        /// <param name="id">The name or ID of the meme.</param>
        /// <returns>The image file.</returns>
        /// <response code="200">The file is successfully returned (with a correct Content-Type).</response>
        /// <response code="404">The image was not found in the index.</response>
        [HttpGet("/{id}")]
        [AngularConstraint]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Returns the index of available images.
        /// </summary>
        /// <returns>The complete index of images.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, ImageResponse>), 200)]
        public async Task<IActionResult> GetImages()
        {
            var req = new IndexRequest();
            var res = await Mediator.Send(req);
            return Ok(res.ToDictionary(k => k.Key, v => v.Value as ImageResponse));
        }

        /// <summary>
        /// Uploads an image (using its file name).
        /// </summary>
        /// <remarks>Provide only one of the URL or file. URL takes precedence.</remarks>
        /// <param name="file">The image file to upload.</param>
        /// <param name="url">The URL of the image to add.</param>
        /// <returns>Details of the indexed image.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ImageResponse), 200)]
        public async Task<IActionResult> AddImage([FromForm] IFormFile file, [FromQuery] string url)
        {
            var req = string.IsNullOrWhiteSpace(url)
                ? new ImageUploadRequest(file)
                : new ImageUploadRequest(new Uri(url));
            var resp = await Mediator.Send(req);
            return Ok(resp as ImageResponse);
        }

        /// <summary>
        /// Uploads an image (with a specified name).
        /// </summary>
        /// <remarks>Provide only one of the URL or file. URL takes precedence.</remarks>
        /// <param name="name">The name to use for the image.</param>
        /// <param name="file">The image file to upload.</param>
        /// <param name="url">The URL of the image to add.</param>
        /// <returns>Details of the indexed image.</returns>
        /// <response code="200">Returns the details of the image as they appear in the index.</response>
        [HttpPut("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ImageResponse), 200)]
        public async Task<IActionResult> AddNamedImage(string name, [FromForm] IFormFile file, [FromQuery] string url)
        {
            var req = string.IsNullOrWhiteSpace(url)
                ? new ImageUploadRequest(file, name)
                : new ImageUploadRequest(new Uri(url), name);
            var resp = await Mediator.Send(req);
            return Ok(resp as ImageResponse);
        }

        /// <summary>
        /// Removes an image from the index.
        /// </summary>
        /// <remarks>Note that, by default, this will also delete the image from the local file store. Be careful!</remarks>
        /// <param name="id">The ID of the image to delete. Names may not be used here.</param>
        /// <response code="204">The image has been deleted.</response>
        /// <response code="410">The image was not present. It was probably already deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(410)]
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
