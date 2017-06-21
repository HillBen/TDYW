using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW
{
    public class ServerOptions
    {
        public string WikiApiBaseUrl { get; set; } = "https://en.wikipedia.org/w/api.php";
        public int HrsBetweenCheckUps { get; set; } = 2;
        public int HrsUntilCold { get; set; } = 8;
    }
}
