using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;

namespace ImageDowloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TextFile1.txt"))
            //{
            //    string json = r.ReadToEnd();
            //    var asdf = JsonConvert.DeserializeObject<Images>(json);

            //    foreach (var item in asdf.hostedImages)
            //    {
            //        SaveImage(item.url);
            //    }
            //}

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44315/api/Pictures/Rover");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var imageLocations = JsonConvert.DeserializeObject<Images>(responseBody);
            foreach (var item in imageLocations.hostedImages)
            {
                SaveImage(item.url);
            }
        }
        public static Image DownloadImageFromUrl(string imageUrl)
        {
            Image image = null;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                WebResponse webResponse = webRequest.GetResponse();

                Stream stream = webResponse.GetResponseStream();

                image = Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        public static void SaveImage(string url)
        {
            Image image = DownloadImageFromUrl(url);

            string filename = Path.GetFileName(new Uri(url).LocalPath);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\DownLoads\\test";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Path.Combine(path, filename);
            image.Save(fileName);
        }
    }
}

