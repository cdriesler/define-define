using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Define.Api
{
    public class DrawingManifest
    {
        // Drawing information
        public List<List<double>> Debug { get; set; } = new List<List<double>>();

        public DrawingManifest(InputManifest input)
        {
            // Make a drawing with the inputs
     


            //Adjacent Manifest

            //Points
            Point3d BottomLeft = new Point3d(input.Adjacent / 0.3, 0,0);
            Point3d TopLeft = new Point3d((input.Adjacent / 0.3) + 0.3, 1,0);
            Point3d BottomRight = new Point3d(input.Adjacent / 0.6, 0,0);
            Point3d TopRight = new Point3d((input.Adjacent / 0.3) - 0.3, 1,0);

            //Final Outputs Properties
            Polyline LeftEdge = null;
            Polyline RightEdge = null;

            //Edges Creation
            var leftEdgeList = new List<Point3d>() { BottomLeft, TopLeft };
            var RightEdgeList = new List<Point3d>() { BottomRight, TopRight };

            LeftEdge = new Polyline(leftEdgeList);
            RightEdge = new Polyline(RightEdgeList);

            //converting final output to svgar
            Debug.Add(RhinoPolylineToSvgar(LeftEdge));
            Debug.Add(RhinoPolylineToSvgar(RightEdge));



         


            
            
        }

        // Convert linear rhino geometry to svgar format
        private List<double> RhinoPolylineToSvgar(Polyline line)
        {
            var pts = new List<double>();

            for (int i = 0; i < line.SegmentCount; i++)
            {
                var pt = line.SegmentAt(i).ToNurbsCurve();

                var segStart = pt.PointAtStart;
                var segEnd = pt.PointAtEnd;
                var midPt = pt.PointAtNormalizedLength(0.5);

                pts.Add(segStart.X);
                pts.Add(segStart.Y);
                pts.Add(midPt.X);
                pts.Add(midPt.Y);
                pts.Add(midPt.X);
                pts.Add(midPt.Y);
                pts.Add(segEnd.X);
                pts.Add(segStart.Y);
            }

            return pts;
        }

        // Convert nonlinear rhino geometry to svgar format
        private List<double> RhinoBezierToSvgar(BezierCurve bezier)
        {
            var pts = new List<double>();

            if (bezier.ControlVertexCount == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var anchor = bezier.GetControlVertex2d(i);

                    pts.Add(anchor.X);
                    pts.Add(anchor.Y);
                }
            }

            return pts;
        }
    }
}
