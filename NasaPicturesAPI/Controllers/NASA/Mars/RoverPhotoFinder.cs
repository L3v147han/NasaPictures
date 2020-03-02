using NasaPicturesAPI.BaseClasses;
using NasaPicturesAPI.Interfaces;
using NasaPicturesAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NasaPicturesAPI.NASA.Mars
{
    public class RoverPhotoFinder : HostedImageRetriever<ValueTask<HostedImageResponse>>, IPhotoRetriever<ValueTask<HostedImageResponse>>
    {
        HostedImageResponse apiResponse = new HostedImageResponse();

        private List<DateTime> _DatesOfPhotosToGet = new List<DateTime>();
        private readonly List<string> _rovers = new List<string>() { "Curiosity", "Opportunity", "Spirit" };
        public RoverPhotoFinder()
        {
            // if no input was given then we can use the default dates from the test file.
            var t = AppDomain.CurrentDomain.BaseDirectory + @"Controllers\NASA\Mars\dates.txt";
            using (TextReader tr = new StreamReader(t))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    if (apiResponse.TryAddDateToList(line, out DateTime d))
                        _DatesOfPhotosToGet.Add(d);
                }
            }
        }
        public RoverPhotoFinder(string date)
        {
            if (apiResponse.TryAddDateToList(date, out DateTime d))
                _DatesOfPhotosToGet.Add(d);
        }
        public RoverPhotoFinder(string start, string end)
        {
            //foreach (var date in dates)
            //{
            //    if (apiResponse.TryAddDateToList(date, out DateTime d))
            //        _DatesOfPhotosToGet.Add(d);
            //}
        }
        public async override ValueTask<HostedImageResponse> GetPhotos()
        {
            var pictures = new List<ExternalHostedImage>() { };
            //TODO: change this to a HttpClient Factory
            string baseURL = "https://api.nasa.gov/mars-photos/api/v1/rovers/";
            HttpClient client = new HttpClient();
            foreach (var rover in _rovers)
            {
                foreach (var date in _DatesOfPhotosToGet)
                {
                    //if (pictures.Any()) continue;// let's only make one call at a time for now so that we don't run out of calls
                    string imgUrl = baseURL + $"{rover.ToLower()}/photos?earth_date={date.ToString("yyyy-MM-dd")}&api_key=9C8SeEo60CQxn0xmdA0Bv2F76DpAct110OYWCVYb";
                    HttpResponseMessage response = await client.GetAsync(imgUrl);

                    IEnumerable<string> values;
                    string totalLimit;
                    string callsLeft;
                    if (response.Headers.TryGetValues("X-RateLimit-Limit", out values))
                        totalLimit = values.First();
                    if (response.Headers.TryGetValues("X-RateLimit-Remaining", out values))
                        callsLeft = values.First();


                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    MarsRoverPhotosResponse parsedResponse = JsonConvert.DeserializeObject<MarsRoverPhotosResponse>(responseBody);
                    foreach (var photo in parsedResponse.photos)
                    {
                        pictures.Add(new ExternalHostedImage() { URL = photo.img_src, Description = $"Picture taken by the {photo.camera.full_name} on Rover:{rover} on {photo.earth_date}" });
                    }
                }
            }
            apiResponse.hostedImages = pictures;
            if (!apiResponse.hostedImages.Any())
            {
                apiResponse.hostedImages = null;
                if (!apiResponse.errors.Any())
                    apiResponse.Message = "Unable to Find Pictures for the dates requested";
                return apiResponse;
            }
            if (!apiResponse.errors.Any())
                apiResponse.errors = null; // so we don't send the extra characters if we don't need to
            return apiResponse;
        }
    }

    public class MarsRoverPhotosResponse
    {
        public Photo[] photos { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public int sol { get; set; }
        public Camera camera { get; set; }
        public string img_src { get; set; }
        public string earth_date { get; set; }
        public Rover rover { get; set; }
    }

    public class Camera
    {
        public int id { get; set; }
        public string name { get; set; }
        public int rover_id { get; set; }
        public string full_name { get; set; }
    }

    public class Rover
    {
        public int id { get; set; }
        public string name { get; set; }
        public string landing_date { get; set; }
        public string launch_date { get; set; }
        public string status { get; set; }
        public int max_sol { get; set; }
        public string max_date { get; set; }
        public int total_photos { get; set; }
        public Camera1[] cameras { get; set; }
    }

    public class Camera1
    {
        public string name { get; set; }
        public string full_name { get; set; }
    }
}
