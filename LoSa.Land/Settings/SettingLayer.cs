using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#if BCAD 
using AcRx = Bricscad.Runtime;
using AcTrx = Teigha.Runtime;
using AcAp = Bricscad.ApplicationServices;
using AcDb = Teigha.DatabaseServices;
using AcGe = Teigha.Geometry;
using AcEd = Bricscad.EditorInput;
using AcGi = Teigha.GraphicsInterface;
using AcClr = Teigha.Colors;
using AcWnd = Bricscad.Windows;
using AcApp = Bricscad.ApplicationServices.Application;
using AcPApp = BricscadApp;
#endif

#if NCAD
using AcRx = HostMgd.Runtime; 
using AcTrx = Teigha.Runtime;
using AcAp = HostMgd.ApplicationServices; 
using AcDb = Teigha.DatabaseServices; 
using AcGe = Teigha.Geometry; 
using AcEd = HostMgd.EditorInput; 
using AcGi = Teigha.GraphicsInterface; 
using AcClr = Teigha.Colors; 
using AcWnd = HostMgd.Windows; 
using AcApp = HostMgd.ApplicationServices.Application; 
using AcPApp = HostMgd;
using AcPDb = Teigha;
#endif

#if ACAD
using AcRx = Autodesk.AutoCAD.Runtime; 
using AcTrx = Autodesk.AutoCAD.Runtime; 
using AcAp = Autodesk.AutoCAD.ApplicationServices; 
using AcDb = Autodesk.AutoCAD.DatabaseServices; 
using AcGe = Autodesk.AutoCAD.Geometry; 
using AcEd = Autodesk.AutoCAD.EditorInput; 
using AcGi = Autodesk.AutoCAD.GraphicsInterface; 
using AcClr = Autodesk.AutoCAD.Colors; 
using AcWnd = Autodesk.AutoCAD.Windows; 
using AcApp = Autodesk.AutoCAD.ApplicationServices.Application; 
using AcPApp = Autodesk.AutoCAD.Interop; 
#endif

namespace LoSa.Land.Settings
{
    /// <summary>
    /// Setting Layer
    /// </summary>
    /// <seealso cref="LoSa.Land.Settings.ISettings" />
    public class SettingLayer : ISettings
    {
        public SettingLayer()
        {

        }

        [XmlAttributeAttribute()]
        public string Name { get; set; }

        [XmlAttributeAttribute()]
        public string Description { get; set; }

        [XmlAttributeAttribute()]
        public bool IsFrozen { get; set; }

        [XmlAttributeAttribute()]
        public bool IsLocked { get; set; }

        [XmlAttributeAttribute()]
        public bool IsOff { get; set; }

        [XmlAttributeAttribute()]
        public bool IsPlottable { get; set; }

        [XmlAttributeAttribute()]
        public AcDb.LineWeight LineWeight { get; set; }

        [XmlAttributeAttribute()]
        public string LineTypeName { get; set; }

        [XmlAttributeAttribute()]
        public AcClr.Color Color { get; set; }

        public static SettingLayer DEFAULT
        {
            get
            {
                SettingLayer settingLayerDefault = new SettingLayer();
                settingLayerDefault.Name = "0";
                settingLayerDefault.Description = "";
                settingLayerDefault.IsFrozen = false;
                settingLayerDefault.IsLocked = false;
                settingLayerDefault.IsOff = false;
                settingLayerDefault.IsPlottable = true;
                settingLayerDefault.LineWeight = AcDb.LineWeight.LineWeight000;
                settingLayerDefault.LineTypeName = "";
                settingLayerDefault.Color = AcClr.Color.FromColor(System.Drawing.Color.DarkMagenta);

                return settingLayerDefault;
            }
        }
    }
}
