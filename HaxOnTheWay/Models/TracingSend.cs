using System;

namespace HaxOnTheWay.Models
{
    public class TracingSend
    {
        public int iCommand { get; set; }

        public int iAction { get; set; }

        public String sNombre { get; set; }

        public String sNotas { get; set; }

        public String oImagen { get; set; }

        public String sDate = DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss");

    }
}
