using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NasaPicturesAPI.Interfaces
{
    public interface IPhotoRetriever<T>
    {
        T GetPhotos();
    }
}
