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

            var r = new Random(Convert.ToInt32(input.Tutorial * 100));

            var rotate = Transform.Rotation((input.Openings * 30) * (Math.PI / 180), Vector3d.ZAxis, new Point3d(input.Adjacent, input.Adjacent, 0));

            for (var i = 1; i < pointCount; i++)
            {
                var y = i * step;
                var x = r.NextDouble() * (input.Openings * 0.25);

                var ptLeft = new Point3d((0.5 - position - x), y, 0);
                var ptRight = new Point3d((0.5 + position + x), y, 0);

                ptLeft.Transform(rotate);
                ptRight.Transform(rotate);

                leftPoints.Add(ptLeft);
                rightPoints.Add(ptRight);
            }

            var leftEdgePoints = new List<Point3d>() { BottomLeft };
            leftEdgePoints.AddRange(leftPoints);
            leftEdgePoints.Add(TopLeft);

            var rightEdgePoints = new List<Point3d>() { BottomRight };
            rightEdgePoints.AddRange(rightPoints);
            rightEdgePoints.Add(TopRight);

            // Generate polylines
            var leftEdge = new Polyline(leftEdgePoints);
            var rightEdge = new Polyline(rightEdgePoints);

            // Push lines to output
            Debug.Add(RhinoPolylineToSvgar(leftEdge));
            Debug.Add(RhinoPolylineToSvgar(rightEdge));

            // Extend line segments
            var extensions = new List<Line>();
            for (var i = 0; i < leftEdge.SegmentCount; i++)
            {
                var l = input.Disjoint * 0.35;
                var cL = leftEdge.SegmentAt(i);
                var cR = rightEdge.SegmentAt(i);

                var cLextended = new Line(cL.From, cL.To);
                cLextended.Extend(l, l);
                var cRextended = new Line(cR.From, cR.To);
                cRextended.Extend(l, l);

                var cLexA = new Line(cL.From, cLextended.From);
                var cLexB = new Line(cL.To, cLextended.To);
                var cRexA = new Line(cR.From, cRextended.From);
                var cRexB = new Line(cR.To, cRextended.To);

                extensions.AddRange(new List<Line>() { cLexA, cLexB, cRexA, cRexB });
            }

            var moved = new List<Line>();

            extensions.ForEach(x =>
            {
                var tx = Transform.Translation(new Vector3d(x.To - x.From) * 0.15);

                x.Transform(tx);

                moved.Add(x);
            });

            moved.ForEach(x =>
            {
                Debug.Add(RhinoPolylineToSvgar(new Polyline(new List<Point3d>() { ( x.From + x.To ) / 2, x.To })));
                Debug.Add(RhinoPolylineToSvgar(new Polyline(new List<Point3d>() { (x.From + x.To) / 2, x.From })));
            });



         


            
            
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
