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

        /// <summary>
        /// Given an input manifest of named values between zero and one, generate a drawing.
        /// </summary>
        /// <param name="input"></param>
        public DrawingManifest(InputManifest input)
        {
            //Parse adjacent

            //Points
            Point3d BottomLeft = new Point3d(0.5 - (0.4 * input.Adjacent), 0, 0);
            Point3d TopLeft = new Point3d(0.5 - (0.4 * input.Adjacent), 1, 0);
            Point3d BottomRight = new Point3d(0.5 + (0.4 * input.Adjacent), 0,0);
            Point3d TopRight = new Point3d(0.5 + (0.4 * input.Adjacent), 1, 0);

            //Point collections
            var leftEdgeList = new List<Point3d>() { BottomLeft, TopLeft };
            var RightEdgeList = new List<Point3d>() { BottomRight, TopRight };

            //Generate polylines
            var LeftEdge = new Polyline(leftEdgeList);
            var RightEdge = new Polyline(RightEdgeList);

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
                var seg = line.SegmentAt(i);

                var segStart = seg.From;
                var segEnd = seg.To;
                var midPt = (segStart + segEnd) / 2;

                pts.Add(segStart.X);
                pts.Add(segStart.Y);
                pts.Add(midPt.X);
                pts.Add(midPt.Y);
                pts.Add(midPt.X);
                pts.Add(midPt.Y);
                pts.Add(segEnd.X);
                pts.Add(segEnd.Y);
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
