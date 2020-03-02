using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NasaPicturesAPI.Models
{
    public class ApiResponse
    {
        public List<apiError> errors { get; set; }
        public string Message { get; set; }
    }

    public class apiError
    {
        //TODO:add severity 
        public string Message { get; set; }
    }
}
