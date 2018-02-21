using System;
using System.Collections.Generic;


namespace LoSa.Land.ObjectIn4
{
    public class BL: In4Polygon
    {
        public String BS { get; set; }
        public String BC { get; set; }
        public String DS { get; set; }
        public String SZ { get; set; }
        public String AB { get; set; }
        public String PB { get; set; }

        public List<SR> SR { get; set; }
        public BR BR { get; set; }
        public List<NB> NB { get; set; }

        public BL() : base() 
        {
            this.SR = new List<SR>();
            this.BR = null;
            this.NB = new List<NB>();
        }
    }
}
