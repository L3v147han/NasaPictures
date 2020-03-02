using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NasaPicturesAPI.Models;

namespace NasaPicturesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalHostedImagesController : ControllerBase
    {
        private readonly HostImgDBContext _context;

        public ExternalHostedImagesController(HostImgDBContext context)
        {
            _context = context;
        }

        // GET: api/ExternalHostedImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExternalHostedImage>>> GetExternalHostedImages()
        {
            return await _context.ExternalHostedImages.ToListAsync();
        }

        // GET: api/ExternalHostedImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExternalHostedImage>> GetExternalHostedImage(string id)
        {
            var externalHostedImage = await _context.ExternalHostedImages.FindAsync(id);

            if (externalHostedImage == null)
            {
                return NotFound();
            }

            return externalHostedImage;
        }

        // PUT: api/ExternalHostedImages/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExternalHostedImage(string id, ExternalHostedImage externalHostedImage)
        {
            if (id != externalHostedImage.URL)
            {
                return BadRequest();
            }

            _context.Entry(externalHostedImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExternalHostedImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExternalHostedImages
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ExternalHostedImage>> PostExternalHostedImage(ExternalHostedImage externalHostedImage)
        {
            if (!ExternalHostedImageExists(externalHostedImage.URL))
                _context.ExternalHostedImages.Add(externalHostedImage);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //TODO: Add error handling
                throw;
            }

            return CreatedAtAction("GetExternalHostedImage", new { id = externalHostedImage.URL }, externalHostedImage);
        }

        // DELETE: api/ExternalHostedImages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExternalHostedImage>> DeleteExternalHostedImage(string id)
        {
            var externalHostedImage = await _context.ExternalHostedImages.FindAsync(id);
            if (externalHostedImage == null)
            {
                return NotFound();
            }

            _context.ExternalHostedImages.Remove(externalHostedImage);
            await _context.SaveChangesAsync();

            return externalHostedImage;
        }

        private bool ExternalHostedImageExists(string id)
        {
            return _context.ExternalHostedImages.Any(e => e.URL == id);
        }
    }
}
