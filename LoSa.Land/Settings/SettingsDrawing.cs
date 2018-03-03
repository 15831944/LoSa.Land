
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LoSa.Land.Tables
{
    public class SettingsDrawing :Settings.ISettings
    {
        public SettingsDrawingScale Scale { get; set; }
        public SettingsDrawingPlan Plan { get; set; }
        public SettingsDrawingTables Tables { get; set; }

        public SettingsDrawing()
        { 
        
        }

        public static SettingsDrawing Default
        {
            get
            {
                SettingsDrawing setsDwg = new SettingsDrawing();
                setsDwg.Scale = SettingsDrawingScale.Default;
                setsDwg.Plan = SettingsDrawingPlan.Default;
                setsDwg.Tables = SettingsDrawingTables.Default;
                return setsDwg;
            }
        }
    }

    public partial class SettingsDrawingScale : Settings.ISettings
    {
        [XmlAttributeAttribute()]
        public double Value { get; set; }

        public static SettingsDrawingScale Default
        {
            get
            {
                SettingsDrawingScale setsDefault = new SettingsDrawingScale();
                setsDefault.Value = 1.0;
                return setsDefault;
            }
        }
    }

    public class SettingsDrawingPlan : Settings.ISettings
    {
        public SettingsDrawingBlock Point { get; set; }
        public SettingsDrawingText NumberPoint { get; set; }
        public SettingsDrawingText LengthLine { get; set; }
        public SettingsDrawingBlock Neighbors { get; set; }
        public SettingsDrawingText NeighborsValue { get; set; }
        public SettingsDrawingFillPolygon FillParcel { get; set; }
        public SettingsDrawingFillPolygon FillLand { get; set; }
        public SettingsDrawingFillPolygon FillLimiting { get; set; }

        public static SettingsDrawingPlan Default
        {
            get
            {
                SettingsDrawingPlan setsPlan = new SettingsDrawingPlan();
                setsPlan.Point = SettingsDrawingBlock.Default;
                setsPlan.NumberPoint = SettingsDrawingText.Default;
                setsPlan.LengthLine = SettingsDrawingText.Default;
                setsPlan.Neighbors = SettingsDrawingBlock.Default;
                setsPlan.NeighborsValue = SettingsDrawingText.Default;
                setsPlan.FillParcel = SettingsDrawingFillPolygon.Default;
                setsPlan.FillLand = SettingsDrawingFillPolygon.Default;
                setsPlan.FillLimiting = SettingsDrawingFillPolygon.Default;
                return setsPlan;
            }
        }
    }

    public class SettingsDrawingTables : Settings.ISettings
    {
        public SettingsDrawingBlock Parcel { get; set; }
        public SettingsDrawingBlock Land { get; set; }
        public SettingsDrawingBlock Limiting { get; set; }

        public static SettingsDrawingTables Default
        {
            get
            {
                SettingsDrawingTables setsTables = new SettingsDrawingTables();
                setsTables.Parcel = SettingsDrawingBlock.Default;
                setsTables.Land = SettingsDrawingBlock.Default;
                setsTables.Limiting = SettingsDrawingBlock.Default;
                return setsTables;
            }
        }
    }

    public class SettingsDrawingBlock : Settings.ISettings
    {
        [XmlAttributeAttribute()]
        public string Layer { get; set; }

        [XmlAttributeAttribute()]
        public string NameBlock { get; set; }

        public static SettingsDrawingBlock Default
        {
            get
            {
                SettingsDrawingBlock sets = new SettingsDrawingBlock();
                sets.Layer = "0";
                sets.NameBlock = "nameBlock";
                return sets;
            }
        }
    }

    public class SettingsDrawingText : Settings.ISettings
    {
        [XmlAttributeAttribute()]
        public string TextStyleName { get; set; }

        [XmlAttributeAttribute()]
        public double TextHeight { get; set; }

        [XmlAttributeAttribute()]
        public string Layer { get; set; }

        [XmlAttributeAttribute()]
        public int ColorIndex { get; set; }

        public static SettingsDrawingText Default
        {
            get
            {
                SettingsDrawingText sets = new SettingsDrawingText();
                sets.TextStyleName = "Standart";
                sets.TextHeight = 1.8;
                sets.Layer = "0";
                sets.ColorIndex = 0;
                return sets;
            }
        }
    }

    public class SettingsDrawingFillPolygon : Settings.ISettings
    {
        [XmlAttributeAttribute()]
        public string Layer { get; set; }

        public static SettingsDrawingFillPolygon Default
        {
            get
            {
                SettingsDrawingFillPolygon sets = new SettingsDrawingFillPolygon();
                sets.Layer = "0";
                return sets;
            }
        }
    }
}
