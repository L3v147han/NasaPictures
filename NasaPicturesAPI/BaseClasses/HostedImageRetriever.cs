using Microsoft.EntityFrameworkCore;
using NasaPicturesAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NasaPicturesAPI.Models;
using System.Net.Http;

namespace NasaPicturesAPI.BaseClasses
{ 
    public abstract class HostedImageRetriever<T> : IPhotoRetriever<T>
    {
        public abstract T GetPhotos();
    }
}