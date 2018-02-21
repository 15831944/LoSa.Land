using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoSa.Land.ObjectGeo
{
    public class BasePoint
    {
        public static BasePoint ZERO = new BasePoint("",0,0,0,"");

        public String Name { get; set; }

        public double N { get; set; }

        public double E { get; set; }

        public double H { get; set; }

        public String Code { get; set; }

        public BasePoint(String name, double n, double e, double h, String code) 
        {
            this.Name = name;
            this.N = n;
            this.E = e;
            this.H = h;
            this.Code = code;
        }
    }

}
