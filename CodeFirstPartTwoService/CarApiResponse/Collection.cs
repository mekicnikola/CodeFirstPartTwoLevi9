using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPartTwoService.CarApiResponse
{
    public class Collection
    {
        public string Url { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public int Total { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
