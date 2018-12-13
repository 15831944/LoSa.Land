using System;


namespace LoSa.Land.ObjectIn4
{
    /// <summary>
    /// In4 CL
    /// </summary>
    /// <seealso cref="LoSa.Land.ObjectIn4.In4Polygon" />
    public class CL : In4Polygon
    {
        public String CI { get; set; }
        public String CN { get; set; }
        public String CC { get; set; }
        public String CZR { get; set; }
        public String CZG { get; set; }
        public String CPR { get; set; }
        public String CPD { get; set; }
        public String AL { get; set; }
        public String PL { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CL"/> class.
        /// </summary>
        public CL() : base()
        {
        }

    }
}
