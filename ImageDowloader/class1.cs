using System;
using System.Collections.Generic;
using System.Text;

namespace ImageDowloader
{
        public class Images 
        {
            public Hostedimage[] hostedImages { get; set; }
        }

        public class Hostedimage
        {
            public string url { get; set; }
            public string description { get; set; }
        }
   
}
