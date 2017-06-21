using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDYW.Services
{
    public class WikiRequest
    {

        [JsonProperty(PropertyName = "action")]
        public string Action { get { return "query"; } }



        [JsonProperty(PropertyName = "prop", NullValueHandling = NullValueHandling.Ignore)]
        public string QueryProperties {
            get
            {
                List<string> props = new List<string>();
                if (GetThumbnail) props.Add("pageimages");
                if (GetDescription) props.Add("pageterms");
                if (GetRevision) props.Add("revisions");
                if (props.Count > 0)
                {
                    return string.Join("|", props);
                }
                else
                {
                    return null;
                }
            }
        } //= "pageimages|pageterms|revisions";

        [JsonIgnore]
        public bool GetThumbnail { get; set; } = true;

        [JsonIgnore]
        public bool GetDescription { get; set; } = true;

        [JsonIgnore]
        public bool GetRevision { get; set; } = true;

        [JsonProperty(PropertyName = "format")]
        public string Format { get { return "json"; } } 



        [JsonProperty(PropertyName ="pilimit")]
        public int PageImagesLimit { get; set; } = 5;
        public bool ShouldSerializePageImagesLimit()
        {
            return QueryProperties.Contains("pageimages");
        }
        [JsonProperty(PropertyName ="piprop")]
        public string PageImagesProperty { get; set; } = "thumbnail";
        public bool ShouldSerializePageImagesProperty()
        {
            return QueryProperties.Contains("pageimages");
        }

        
        public string wbptterms { get; set; } = "description";



        [JsonProperty(PropertyName = "generator", NullValueHandling = NullValueHandling.Ignore)]
        public string Generator { get; set; } = "search";

     

        [JsonProperty(PropertyName ="gsrlimit")]
        public int SearchLimit { get; set; } = 5;
        public bool ShouldSerializeSearchLimit()
        {
            return !string.IsNullOrEmpty(Generator);
        }

        [JsonProperty(PropertyName = "pageids")]
        public int PageId
        {
            get
            {
                return _pageid;
            }
            set
            {
                _pageid = value;
                if (_pageid > 0)
                {
                    Generator = null;
                }
            }
        }// = 984052;
        private int _pageid { get; set; }
        public bool ShouldSerializePageId()
        {
            return Generator ==null;
        }

        [JsonProperty(PropertyName = "gsrsearch")]
        public string SearchString
        {
            get
            {
                string s = "hastemplate:" + _template + " " + _terms;
                return s.Trim();
            }
            set
            {
                _terms = value;
                Generator = "search";
            }
        } 
        public bool ShouldSerializeSearchString()
        {
            return !string.IsNullOrEmpty(Generator);
        }

        [JsonIgnore]
        public bool SearchLiving
        {
            get
            {
                return _template == "Birth_date_and_age";
            }
            set
            {
                if (value == true)
                {
                    _template = "Birth_date_and_age";
                }
                else
                {
                    _template = "Birth_date";
                }
            }
        }
        private string _terms { get; set; }
        private string _template { get; set; } = "Birth_date_and_age";




        [JsonProperty(PropertyName = "rvprop")]
        public string RevisionProperty{get; set;} = "content";
        public bool ShouldSerializeRevisionProperty()
        {
            return QueryProperties.Contains("revisions");
        }

        [JsonProperty(PropertyName ="rvsection")]
        public int RevisionSection{get; set;}=0;
        public bool ShouldSerializeRevisionSection()
        {
            return QueryProperties.Contains("revisions");
        }


}
}
