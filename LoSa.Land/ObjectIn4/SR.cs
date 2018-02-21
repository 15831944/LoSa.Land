using System;
using System.Collections.Generic;

namespace LoSa.Land.ObjectIn4
{
    public class SR : In4Polygon
    {
        public String SC { get; set; }
        public String AT { get; set; }
        public String NM { get; set; }
        public String AD { get; set; }
        public String TX { get; set; }
        public String ZS { get; set; }
        public String PF { get; set; }
        public String CH { get; set; }
        public String KU { get; set; }
        public String KF { get; set; }

        public String AU { get; set; }
        public String SU { get; set; }
        public String PP { get; set; }

        public String PV { get; set; }
        public String CV { get; set; }
        public String CZ { get; set; }
        public String PZ { get; set; }
        public String TD { get; set; }
        public String GA { get; set; }
        public String FL { get; set; }
        public String KZ { get; set; }

        public String RNM { get; set; }
        public String RAU { get; set; }
        public String RKU { get; set; }
        public String RPV { get; set; }
        public String RKZ { get; set; }
        public String RPF { get; set; }

        public List<CL> CL { get; set; }
        public List<OB> OB { get; set; }
        public List<NB> NB { get; set; }

        public String AS { get; set; }
        public String PS { get; set; }

        public SR() : base() 
        {
            this.CL = new List<CL>();
            this.OB = new List<OB>();
            this.NB = new List<NB>();
        }
    }
}
