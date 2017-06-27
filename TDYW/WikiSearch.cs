using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW
{
    public class WikiSearch
    {
        private string _action { get; set; }
        private string _generator { get; set; }
        private string _gsrlimit { get; set; }
        private string _prop { get; set; }
        private string _exlimit { get; set; }
        private string _explaintext { get; set; }
        private string _exintro { get; set; }


        public WikiSearch(int pageId)
        {
            //https://en.wikipedia.org/w/api.php?action=query&generator=search&gsrlimit=10&prop=pageimages|extracts&exlimit=max&explaintext&exintro&pilimit=10&pithumbsize=100&gsrsearch=hastemplate:Birth_date_and_age+albert&format=json
        }

        //public WikiSearch(string searchTerm)
        //{
        //    //zzz.@continue = "";
        //}
        //private Continue zzz { get; set; }
    }

    //public class Continue
    //{
    //    public string continue{get; set;}
    //}

}
