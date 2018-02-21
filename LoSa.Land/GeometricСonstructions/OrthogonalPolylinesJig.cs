
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

using LoSa.CAD;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoSa.Utility;

namespace LoSa.Land.GeometricСonstructions
{
    public class OrthogonalPolylinesJig : AcEd.DrawJig
    {
        private AcGe.Point3dCollection allVertexes = new AcGe.Point3dCollection();
        private AcGe.Point3d lastVertex;
        private bool isMovingTowards;

        public OrthogonalPolylinesJig()
        {
        }

        public AcGe.Point3d LastVertex
        {
            get { return lastVertex; }
            set { lastVertex = value; }
        }

        public AcGe.Matrix3d UCS
        {
            get { return CurrentCAD.Editor.CurrentUserCoordinateSystem; }
        }
        protected override bool WorldDraw(AcGi.WorldDraw draw)
        {
            AcGi.WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(UCS);

                AcGe.Point3dCollection tempPts = new AcGe.Point3dCollection();
                foreach (AcGe.Point3d pt in allVertexes)
                {
                    tempPts.Add(pt);
                }
                if (lastVertex != null)
                {
                    if (tempPts.Count > 1)
                    {
                        AcDb.Line lastLine = new AcDb.Line(allVertexes[allVertexes.Count - 2], allVertexes[allVertexes.Count - 1]);
                        double offsetForward = ServiceGeodesy.GetProjectionOnLine(lastLine, lastVertex) - lastLine.Length;
                        double offsetTowards = ServiceGeodesy.GetOffsetFromLine(lastLine, lastVertex);

                        AcDb.Xline xLine = new AcDb.Xline();
                        xLine.BasePoint = allVertexes[allVertexes.Count - 1];
                        xLine.UnitDir = lastLine.Delta;

                        System.Windows.Forms.Keys mods = System.Windows.Forms.Control.ModifierKeys;
                        if ((mods & System.Windows.Forms.Keys.Control) > 0)
                        {
                            if (isMovingTowards)
                            {
                                lastVertex = xLine.GetPointAtDist(offsetForward);
                            }
                            else
                            {
                                xLine.TransformBy(AcGe.Matrix3d.Rotation(Math.PI / 2 * -1, AcGe.Vector3d.ZAxis, xLine.BasePoint));
                                lastVertex = xLine.GetPointAtParameter(offsetTowards);
                            }
                        }
                        else
                        {
                            if (Math.Abs(offsetForward) > Math.Abs(offsetTowards))
                            {
                                isMovingTowards = true;
                                lastVertex = xLine.GetPointAtDist(offsetForward);
                            }
                            else
                            {
                                isMovingTowards = false;
                                xLine.TransformBy(AcGe.Matrix3d.Rotation(Math.PI / 2 * -1, AcGe.Vector3d.ZAxis, xLine.BasePoint));
                                lastVertex = xLine.GetPointAtParameter(offsetTowards);
                            }
                        }
                    }
                    tempPts.Add(lastVertex);
                }
                if (tempPts.Count > 0)
                    geo.Polyline(tempPts, AcGe.Vector3d.ZAxis, IntPtr.Zero);

                geo.PopModelTransform();
            }

            return true;
        }

        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            AcEd.JigPromptPointOptions jppo = new AcEd.JigPromptPointOptions( "\nВершина ('Enter' закінчити):");
            jppo.UseBasePoint = false;
            jppo.UserInputControls = AcEd.UserInputControls.NullResponseAccepted |
                                        AcEd.UserInputControls.Accept3dCoordinates |
                                        AcEd.UserInputControls.GovernedByUCSDetect |
                                        AcEd.UserInputControls.GovernedByOrthoMode |
                                        AcEd.UserInputControls.AcceptMouseUpAsPoint;
            AcEd.PromptPointResult prResult1 = prompts.AcquirePoint(jppo);
            if (prResult1.Status == AcEd.PromptStatus.Cancel || prResult1.Status == AcEd.PromptStatus.Error)
            {
                return AcEd.SamplerStatus.Cancel;
            }
            AcGe.Point3d tmpPt = prResult1.Value.TransformBy(UCS.Inverse());
            lastVertex = tmpPt;
            return AcEd.SamplerStatus.OK;
        }

        public static OrthogonalPolylinesJig jigger;
        [AcTrx.CommandMethod("Land_OrthogonalPolylines")]
        public static void BuildingOrthogonalPolylines()
        {
            try
            {
                AcDb.Database db = CurrentCAD.Database;
                jigger = new OrthogonalPolylinesJig();
                AcEd.PromptResult jigRes;
                do
                {
                    jigRes = CurrentCAD.Editor.Drag(jigger);
                    if (jigRes.Status == AcEd.PromptStatus.OK)
                        jigger.allVertexes.Add(jigger.lastVertex);
                } while (jigRes.Status == AcEd.PromptStatus.OK);

                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);

                    Teigha.DatabaseServices.Polyline ent = new Teigha.DatabaseServices.Polyline();
                    ent.SetDatabaseDefaults();
                    for (int i = 0; i < jigger.allVertexes.Count; i++)
                    {
                        AcGe.Point3d pt3d = jigger.allVertexes[i];
                        AcGe.Point2d pt2d = new AcGe.Point2d(pt3d.X, pt3d.Y);
                        ent.AddVertexAt(i, pt2d, 0, db.Plinewid, db.Plinewid);
                    }
                    ent.TransformBy(jigger.UCS);
                    btr.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);

                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {
                CurrentCAD.Editor.WriteMessage(ex.ToString());
            }
        }
    }
}
/*
namespace AcadNetAddinWizard_Namespace
{
    public class DrawJigger6 : DrawJig
    {
        #region Fields

        private Point3dCollection mAllVertexes = new Point3dCollection();
        private Point3d mLastVertex;

        #endregion

        #region Constructors

        public DrawJigger6()
        {
        }

        ~DrawJigger6()
        {
        }

        #endregion

        #region Properties

        public Point3d LastVertex
        {
            get { return mLastVertex; }
            set { mLastVertex = value; }
        }

        private Editor Editor
        {
            get
            {
                return ServiceBricsCAD.Editor;
            }
        }

        public Matrix3d UCS
        {
            get
            {
                return ServiceBricsCAD.Editor.CurrentUserCoordinateSystem;
            }
        }

        #endregion

        #region Overrides

        protected override bool WorldDraw(WorldDraw draw)
        {
            WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(UCS);

                Point3dCollection tempPts = new Point3dCollection();
                foreach (Point3d pt in mAllVertexes)
                {
                    tempPts.Add(pt);
                }
                if (mLastVertex != null)
                {
                    if (tempPts.Count > 1)
                    {
                        Line lastLine = new Line(mAllVertexes[mAllVertexes.Count - 2], mAllVertexes[mAllVertexes.Count - 1]);
                        double offsetForward = ServiceGeodesy.GetProjectionOnLine(lastLine, mLastVertex) - lastLine.Length;
                        double offsetTowards = ServiceGeodesy.GetOffsetFromLine(lastLine, mLastVertex);

                        Xline xLine = new Xline();
                        xLine.BasePoint = mAllVertexes[mAllVertexes.Count - 1];
                        xLine.UnitDir = lastLine.Delta;

                        if (Math.Abs(offsetForward) > Math.Abs(offsetTowards))
                        {
                            mLastVertex = xLine.GetPointAtDist(offsetForward);
                        }
                        else
                        {
                            mLastVertex = xLine.GetPointAtParameter(offsetTowards);
                        }
                    }

                    tempPts.Add(mLastVertex);
                }

                if (tempPts.Count > 0)
                {
                    geo.Polyline(tempPts, Vector3d.ZAxis, IntPtr.Zero);
                }
                geo.PopModelTransform();
            }

            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions prOptions1 = new JigPromptPointOptions("\nVertex(Enter to finish)");
            prOptions1.UseBasePoint = false;
            prOptions1.UserInputControls =
                UserInputControls.NullResponseAccepted | UserInputControls.Accept3dCoordinates |
                UserInputControls.GovernedByUCSDetect | UserInputControls.GovernedByOrthoMode |
                UserInputControls.AcceptMouseUpAsPoint;

            PromptPointResult prResult1 = prompts.AcquirePoint(prOptions1);
            if (prResult1.Status == PromptStatus.Cancel || prResult1.Status == PromptStatus.Error)
                return SamplerStatus.Cancel;

            Point3d tempPt = prResult1.Value.TransformBy(UCS.Inverse());
            mLastVertex = tempPt;

            return SamplerStatus.OK;
        }

        #endregion

        #region Commands

        public static DrawJigger6 jigger;
        [CommandMethod("TestDrawJigger6")]
        public static void TestDrawJigger6_Method()
        {
            try
            {
                Database db = HostApplicationServices.WorkingDatabase;
                jigger = new DrawJigger6();
                PromptResult jigRes;
                do
                {
                    jigRes = ServiceBricsCAD.Editor.Drag(jigger);
                    if (jigRes.Status == PromptStatus.OK)
                        jigger.mAllVertexes.Add(jigger.mLastVertex);
                } while (jigRes.Status == PromptStatus.OK);

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    Teigha.DatabaseServices.Polyline ent = new Teigha.DatabaseServices.Polyline();
                    ent.SetDatabaseDefaults();
                    for (int i = 0; i < jigger.mAllVertexes.Count; i++)
                    {
                        Point3d pt3d = jigger.mAllVertexes[i];
                        Point2d pt2d = new Point2d(pt3d.X, pt3d.Y);
                        ent.AddVertexAt(i, pt2d, 0, db.Plinewid, db.Plinewid);
                    }
                    ent.TransformBy(jigger.UCS);
                    btr.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);

                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {
                ServiceBricsCAD.Editor.WriteMessage(ex.ToString());
            }
        }

        #endregion

    }
}
*/