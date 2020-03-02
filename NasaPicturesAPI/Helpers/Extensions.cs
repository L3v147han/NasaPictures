using NasaPicturesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NasaPicturesAPI
{
    public static class Extensions
    {
        public static bool TryAddDateToList(this HostedImageResponse response, string input, out DateTime date)
        {
            if (DateTime.TryParse(input, out DateTime d))
            {
                date = d;
                return true;
            }
            else
            {
                response.errors.Add(new apiError { Message = $"Unable to read date from string '{input}'" });
                date = new DateTime();
                return false;
            }
        }
    }
}
