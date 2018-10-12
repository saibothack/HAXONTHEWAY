using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxOnTheWay.Models
{
    public class Signature
    {
        public int iCommand { get; set; }

        public int iCalifica { get; set; }

        public String sName { get; set; }


        public String oImagen { get; set; }

        public String sDate = DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss");
    }
}
