using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Services
{
    public class WikiResponse
    {
        public string batchcomplete { get; set; }
        public Query query { get; set; }
        public Limits limits { get; set; }

        [JsonProperty(PropertyName = "continue")]
        public Continue @continue { get; set; }
    }

    public class Query
    {
        public Dictionary<string, pageval> pages { get; set; } = new Dictionary<string, pageval>();
    }

    public class pageval
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public int index { get; set; }
        public Thumbnail thumbnail { get; set; }
        public string pageimage { get; set; }
        public string extract { get; set; }
        public Terms terms { get; set; }
    }




    public class Limits
    {
        public int extracts { get; set; }
    }



    public class Continue
    {
        public int gsroffset { get; set; }

        [JsonProperty(PropertyName ="continue")]
        public string _continue { get; set; }
    }

    public class Thumbnail
    {
        public string source { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Terms
    {
        public List<string> description { get; set; } = new List<string>();
    }
}

