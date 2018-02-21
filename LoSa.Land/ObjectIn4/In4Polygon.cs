using System;
using System.Collections.Generic;

namespace LoSa.Land.ObjectIn4
{
    interface IIn4Polygon
    {
    }

    public abstract class In4Polygon : IIn4Polygon
    {
        public List<String>  Comments { get; set; }
        public List<Point> Border { get; set; }

        public In4Polygon()
        {
            this.Comments = new List<String>();
            this.Border = new List<Point>();
        }
    }
}
    