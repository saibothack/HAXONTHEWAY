using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxOnTheWay.Models
{
    public class Location
    {
        public int idConductor { get; set; }
        public int indConnect { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public String time { get; set; }
        public String sToken { get; set; }
    }
}
