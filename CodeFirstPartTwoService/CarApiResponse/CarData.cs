using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPartTwoService.CarApiResponse
{
    public class CarData
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        public string Name { get; set; }
        public Make Make { get; set; }
    }
}
