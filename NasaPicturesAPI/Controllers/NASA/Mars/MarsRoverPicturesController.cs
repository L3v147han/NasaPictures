using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NasaPicturesAPI.Interfaces;
using NasaPicturesAPI.Models;
using NasaPicturesAPI.NASA.Mars;

namespace NasaPicturesAPI.Controllers
{
    //TODO: Add option to select rover
    //TODO: add chache or save image locations for dates we have already looked for 
    [Route("api/Pictures/Rover")]
    [ApiController]
    public class MarsRoverPicturesController : Controller
    {
        private IPhotoRetriever<ValueTask<HostedImageResponse>> _r;
        [HttpGet]
        public async Task<ActionResult<HostedImageResponse>> GetExternalHostedImage([FromQuery]string date)
        {
            if (date != null)
                _r = new RoverPhotoFinder(date);
            else
                _r = new RoverPhotoFinder();
            return await _r.GetPhotos();
        }
    }
}