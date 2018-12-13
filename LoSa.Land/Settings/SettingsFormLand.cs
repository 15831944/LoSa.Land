using System.Xml.Serialization;

namespace LoSa.Land.Tables
{
    public class SettingsFormLand : Settings.ISettings
    {
        public double ScaleDrawing { get; set; }

        public int IndexTabControl { get; set; }

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
            this.ScaleDrawing = 1;
        }

        /*
        public static SettingsFormLand Default
        {
            get
            {
                SettingsFormLand defaultSettingsFormLand = new SettingsFormLand();
                return defaultSettingsFormLand;
            }
        }
        */
    }
}
