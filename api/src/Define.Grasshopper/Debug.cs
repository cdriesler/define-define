using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Define.Grasshopper
{
    public class SideEdgesDistance : GH_Component
    {
        public SideEdgesDistance() : base("Distance", "EDist", "Calculates the distance between the outer edges.", "Curve", "Util")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)

        // inputs 
        {
            pManager.AddCurveParameter("Left Edge", "L", "Left Curve Test", GH_ParamAccess.item);
            pManager.AddCurveParameter("Right Edge", "R", "Right Curve Test", GH_ParamAccess.item);
            pManager.AddCurveParameter("canvas boundary", "B", "Canvas Boundary", GH_ParamAccess.item);

        }

        //Outputs
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Distance Between Edges", "D", "Center To Center Measure", GH_ParamAccess.item);
            pManager.AddCurveParameter("Canvas", "B", "New Canvas", GH_ParamAccess.item);
        }


        //inputs reference
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Curve LeftEdge = null;
            DA.GetData(0, ref LeftEdge);

            Curve RightEdge = null;
            DA.GetData(1, ref RightEdge);

            Curve canvasBoundary = null;
            DA.GetData(2, ref canvasBoundary);





            //Logics
        
            double Distance = 0;

            double LeftLength = LeftEdge.GetLength();
            double RightLength = RightEdge.GetLength();

            Point3d LeftMidPoint = LeftEdge.PointAtLength(LeftLength / 2);
            Point3d RightMidPoint = RightEdge.PointAtLength(RightLength / 2);

            Line DistanceProjection = new Line(LeftMidPoint, RightMidPoint);

            Distance = DistanceProjection.ToNurbsCurve().GetLength();



            // Canvas Boundary Test
            var area = AreaMassProperties.Compute(canvasBoundary).Area;

            if (area != 864)
            {
                int width = 36;
                int height = 24;
               
                canvasBoundary = new Rectangle3d(Plane.WorldXY, width, height).ToNurbsCurve();
            }

            else if( area == 864)
            {
                Point3d CanvasCenter = canvasBoundary.GetBoundingBox(true).Center;
                var Vector = new Vector3d(CanvasCenter.X, CanvasCenter.Y, CanvasCenter.Z) * (-1);
                canvasBoundary.Transform(Transform.Translation(Vector));       
            }

            // Assign Output
            DA.SetData(0, Distance);
            DA.SetData(1, canvasBoundary);
            
        }

        protected override System.Drawing.Bitmap Icon
        {
            //get { return Properties.Resources.icon; }
            get { return null; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("f05e9e11-1660-4342-9c16-31344fdca591"); }
        }
    }
}
