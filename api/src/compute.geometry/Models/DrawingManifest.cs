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
            // Parse adjacent

            // Generate anchor points from input.Adjacent
            var position = 0.4 * input.Adjacent;
            var BottomLeft = new Point3d(0.5 - position, 0, 0);
            var TopLeft = new Point3d(0.5 - position, 1, 0);
            var BottomRight = new Point3d(0.5 + position, 0,0);
            var TopRight = new Point3d(0.5 + position, 1, 0);

            // Generate interior points from input.Openings
            var pointCount = Convert.ToInt32(Math.Round(input.Openings * 9));
            double step = 1.0 / pointCount;
            var leftPoints = new List<Point3d>();
            var rightPoints = new List<Point3d>();

            var r = new Random();

            for (var i = 1; i < pointCount; i++)
            {
                var y = i * step;
                var x = r.NextDouble() * (input.Openings * 0.25);

                leftPoints.Add(new Point3d((0.5 - position - x), y, 0));
                rightPoints.Add(new Point3d((0.5 + position + x), y, 0));
            }

            var leftEdgePoints = new List<Point3d>() { BottomLeft };
            leftEdgePoints.AddRange(leftPoints);
            leftEdgePoints.Add(TopLeft);

            var rightEdgePoints = new List<Point3d>() { BottomRight };
            rightEdgePoints.AddRange(rightPoints);
            rightEdgePoints.Add(TopRight);

            //Generate polylines
            var leftEdge = new Polyline(leftEdgePoints);
            var rightEdge = new Polyline(rightEdgePoints);

            //converting final output to svgar
            Debug.Add(RhinoPolylineToSvgar(leftEdge));
            Debug.Add(RhinoPolylineToSvgar(rightEdge));



         


            
            
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
