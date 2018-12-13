using System;
using System.Collections.Generic;


namespace LoSa.Land.ObjectIn4
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="In4Polygon" />
    public class BL : In4Polygon
    {

        /// <summary>
        /// Gets or sets the bs.
        /// </summary>
        /// <value>
        /// The bs.
        /// </value>
        public String BS { get; set; }
        /// <summary>
        /// Gets or sets the bc.
        /// </summary>
        /// <value>
        /// The bc.
        /// </value>
        public String BC { get; set; }
        /// <summary>
        /// Gets or sets the ds.
        /// </summary>
        /// <value>
        /// The ds.
        /// </value>
        public String DS { get; set; }
        /// <summary>
        /// Gets or sets the sz.
        /// </summary>
        /// <value>
        /// The sz.
        /// </value>
        public String SZ { get; set; }
        /// <summary>
        /// Gets or sets the ab.
        /// </summary>
        /// <value>
        /// The ab.
        /// </value>
        public String AB { get; set; }
        /// <summary>
        /// Gets or sets the pb.
        /// </summary>
        /// <value>
        /// The pb.
        /// </value>
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
