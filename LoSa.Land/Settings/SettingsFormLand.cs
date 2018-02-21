using System.Xml.Serialization;

namespace LoSa.Land.Tables
{
    public class SettingsFormLand
    {
        public int IndexTabControl { get; set; }

        private double scaleDrawing = 1;

        public double ScaleDrawing 
        {
            get 
            {
                //if (scaleDrawing < 1) scaleDrawing = 1.0;
                return scaleDrawing; 
            }
            set 
            {
                //if (value < 1) value = 1.0;
                scaleDrawing = value; 
            } 
        }

        public bool DisplayPointNumbers { get; set; }
        public bool AutomaticDisplayPointNumbers { get; set; }

        public bool DisplayLengthLine { get; set; }
        public bool AutomaticDisplayLengthLine { get; set; }

        public bool DisplayFillParcel { get; set; }

        public bool DisplayFillLand { get; set; }

        public bool DisplayFillLimiting { get; set; }

        public bool DisplayFillNeighbors { get; set; }
        public bool DisplayFillNeighborsStateAct { get; set; }

        public bool DisplayArea { get; set; }
        public bool UnitArea { get; set; }

        public bool DisplayPerimeter { get; set; }

        public SettingsFormLand()
        {
            this.IndexTabControl = 0;
        }

        public static SettingsFormLand Default
        {
            get
            {
                SettingsFormLand setsDefault = new SettingsFormLand();
                setsDefault.ScaleDrawing = 1.0;
                return setsDefault;
            }
        }
    }
}
