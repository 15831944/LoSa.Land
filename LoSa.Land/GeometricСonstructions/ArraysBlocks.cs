
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoSa.CAD;

namespace LoSa.Land.GeometricСonstructions
{
    public class ArraysBlocks
    {
        public string NameBlock { get; set; }
        public double ScaleBlock { get; set; }
        public double RotationBlock { get; set; }
        public AcGe.Point3d BasePointArrays { get; set; }

        ArraysBlocks()
        {
            this.NameBlock = "";
            this.ScaleBlock = 1;
            this.RotationBlock = 0;
            this.BasePointArrays = AcGe.Point3d.Origin;
        }

        ArraysBlocks(   string nameBlock,
                        double scaleBlock,
                        double rotationBlock,
                        AcGe.Point3d basePointArrays)
        {
            this.NameBlock = nameBlock;
            this.ScaleBlock = scaleBlock;
            this.RotationBlock = rotationBlock;
            this.BasePointArrays = BasePointArrays;
        }

        public void DisplayArrays(int numberСolumns, int numberRows, double stepСolumns, double stepRows)
        {

            for (int i = 0; i < numberСolumns; i++)
            {
                for (int j = 0; j < numberRows; j++)
                {
                    AcGe.Vector3d offset = new AcGe.Vector3d(j * stepRows, i * stepСolumns, 0);
                    AcGe.Point3d insertPoint = this.BasePointArrays.Add(offset);

                    AcDb.ObjectId objId = ServiceBlockElements.InsertBlock(this.NameBlock, insertPoint, this.ScaleBlock, this.RotationBlock);
                    if (objId.IsNull) return;
                }
            }
        }

        public void DisplayAlongLine(AcGe.Point3d directionPoint, double offset, double step )
        {
            AcDb.Line line = new AcDb.Line(this.BasePointArrays, directionPoint);
            double length = offset;
            while (line.Length > length)
            {
                ServiceBlockElements.InsertBlock(this.NameBlock, line.GetPointAtDist(length), this.ScaleBlock, this.RotationBlock);
                length += step;
            }
        }

        public void DisplayAlongLine(AcGe.Point3d directionPoint, double offset, int quantity)
        {
            AcDb.Line line = new AcDb.Line(this.BasePointArrays, directionPoint);
            double length = offset;
            double step = line.Length / quantity;
            while (line.Length > length)
            {
                ServiceBlockElements.InsertBlock(this.NameBlock, line.GetPointAtDist(length), this.ScaleBlock, this.RotationBlock);
                length += step;
            }
        }

        public void DisplayAlongLine(AcGe.Point3d directionPoint, double offset, double step, int quantity)
        {
            AcDb.Ray ray = new AcDb.Ray
            {
                BasePoint = this.BasePointArrays,
                UnitDir = this.BasePointArrays.GetVectorTo(directionPoint)
            };
            double length = offset;
            for (int i = 0; i < quantity; i++ )
            {
                ServiceBlockElements.InsertBlock(this.NameBlock, ray.GetPointAtDist(length), this.ScaleBlock, this.RotationBlock);
                length += step;
            }
        }

        [AcTrx.CommandMethod("Land_IntersectionGridLines")]
        public static void DisplayIntersectionGridLines()
        {
            string strMsg = "Побудова перетину ліній координатної сітки > Масштаб креслення (1:1000 - 1.0, 1:500 - 0.5 ...) :";
            AcEd.PromptDoubleOptions pdo = 
                new AcEd.PromptDoubleOptions(strMsg)
            {
                AllowNegative = false,
                AllowZero = false,
                AllowNone = false
            };


            AcEd.PromptDoubleResult pdr = CurrentCAD.Editor.GetDouble(pdo);

            if (pdr.Status == AcEd.PromptStatus.OK)
            {
                double scaleDrawing = pdr.Value;

                AcEd.PromptPointResult ppr = CurrentCAD.Editor.GetPoint("Побудова перетину ліній координатної сітки > Вкажіть першу точку:");

                if (ppr.Status == AcEd.PromptStatus.OK)
                {
                    AcGe.Point3d basePoint = ppr.Value;

                    AcEd.PromptCornerOptions pco = new AcEd.PromptCornerOptions("Побудова перетину ліній координатної сітки > Вкажіть другу точку:", basePoint);

                    ppr = CurrentCAD.Editor.GetCorner(pco);
                    if (ppr.Status == AcEd.PromptStatus.OK)
                    {
                        AcGe.Point3d сornerPoint = ppr.Value;

                        double stepGrid = scaleDrawing * 100;

                        double newX = (double)((int)(basePoint.X / stepGrid)) * stepGrid;
                        double newY = (double)((int)(basePoint.Y / stepGrid)) * stepGrid;

                        if ((сornerPoint.X - basePoint.X) > 0) newX += stepGrid;
                        if ((сornerPoint.Y - basePoint.Y) > 0) newY += stepGrid;

                        AcGe.Point3d startPoint = new AcGe.Point3d(newX, newY, 0);

                        int numCol = (int)((сornerPoint.Y - startPoint.Y) / stepGrid);
                        int numRow = (int)((сornerPoint.X - startPoint.X) / stepGrid);

                        double stepCol = stepGrid;
                        double stepRow = stepGrid;

                        if (numCol < 0) stepCol *= -1;
                        if (numRow < 0) stepRow *= -1;

                        ArraysBlocks arrBlock = new ArraysBlocks("11", scaleDrawing, 0, startPoint);
                        arrBlock.DisplayArrays(Math.Abs(numCol) + 1, Math.Abs(numRow) + 1, stepCol, stepRow);
                    }
                }
            }
        }

        //[AcTrx.CommandMethod("Land_InsertBlocksAlongLine")]
        public static void InsertBlocksAlongLine()
        {
            AcEd.PromptPointOptions ppo = new AcEd.PromptPointOptions("Вкажіть першу точку:");
            AcEd.PromptPointResult ppr = CurrentCAD.Editor.GetPoint(ppo);
            if (ppr.Status == AcEd.PromptStatus.OK)
            {
                AcGe.Point3d startPoint = ppr.Value;
                ArraysBlocks arrBlock = new ArraysBlocks("11", 1, 0, startPoint);

                ppo.Message = "Вкажіть напрямок:";
                ppo.UseBasePoint = true;
                ppo.BasePoint = startPoint;
                ppr = CurrentCAD.Editor.GetPoint(ppo);
                AcGe.Point3d dirPoint = ppr.Value;

                arrBlock.DisplayAlongLine(dirPoint, 0, 50, 100);
            }
        }
    }   
}
