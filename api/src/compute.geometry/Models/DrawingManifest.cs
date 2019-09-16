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
            var pts = Convert.ToInt32(Math.Round(input.Openings * 9));
            var pointCount = pts > 3 ? pts : 3;
            double step = 1.0 / pointCount;
            var leftPoints = new List<Point3d>();
            var rightPoints = new List<Point3d>();

            var r = new Random(Convert.ToInt32(input.Tutorial * 100));

            var rotateL = Transform.Rotation((input.Openings * 30) * (Math.PI / 180), Vector3d.ZAxis, new Point3d(input.Adjacent, input.Adjacent, 0));
            var rotateR = Transform.Rotation((input.Openings * -30) * (Math.PI / 180), Vector3d.ZAxis, new Point3d(1 - input.Adjacent , 1 - input.Adjacent, 0));

            for (var i = 1; i < pointCount; i++)
            {
                var y = i * step;
                var x = r.NextDouble() * (input.Openings * 0.25);

                var ptLeft = new Point3d((0.5 - position - x), y, 0);
                var ptRight = new Point3d((0.5 + position + x), y, 0);

                ptLeft.Transform(rotateL);
                ptRight.Transform(rotateR);

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
            var lex = new List<Line>();
            var rex = new List<Line>();

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

                lex.AddRange(new List<Line>() { cLexA, cLexB });
                rex.AddRange(new List<Line>() { cRexA, cRexB });
            }

            var movedLex = new List<Line>();
            var movedRex = new List<Line>();

            lex.ForEach(x =>
            {
                var tx = Transform.Translation(new Vector3d(x.To - x.From) * 0.15);

                x.Transform(tx);

                movedLex.Add(x);
            });

            rex.ForEach(x =>
            {
                var tx = Transform.Translation(new Vector3d(x.To - x.From) * 0.15);

                x.Transform(tx);

                movedRex.Add(x);
            });

            for (var i = 0; i < movedLex.Count; i++)
            {
                Debug.Add(RhinoPolylineToSvgar(new Polyline(new List<Point3d>() { movedLex[i].From, movedLex[i].To })));
                Debug.Add(RhinoPolylineToSvgar(new Polyline(new List<Point3d>() { movedRex[i].From, movedRex[i].To })));
            }

            for (var i = 0; i < movedLex.Count; i+=2)
            {
                var crvA = movedLex[i];
                var targetA = crvA.To.X > crvA.From.X ? new Point3d(rightEdge.SegmentAt(i / 2).To.X, crvA.To.Y, 0) : new Point3d(0, crvA.To.Y, 0);

                var crvB = movedLex[i + 1];
                var targetB = crvB.To.X > crvB.From.X ? new Point3d(rightEdge.SegmentAt(i / 2).From.X, crvB.To.Y, 0) : new Point3d(0, crvB.To.Y, 0);

                var crvC = movedRex[i];
                var targetC = crvC.To.X < crvC.From.X ? new Point3d(leftEdge.SegmentAt(i / 2).To.X, crvC.To.Y, 0) : new Point3d(1, crvC.To.Y, 0);

                var crvD = movedRex[i + 1];
                var targetD = crvD.To.X < crvD.From.X ? new Point3d(leftEdge.SegmentAt(i / 2).From.X, crvD.To.Y, 0) : new Point3d(1, crvD.To.Y, 0);

                var ext = new List<Line>()
                {
                    new Line(crvA.To, targetA),
                    new Line(crvB.To, targetB),
                    new Line(crvC.To, targetC),
                    new Line(crvD.To, targetD)
                };

                var allExtensions = new List<Polyline>();

                ext.ForEach(x =>
                {
                    var scale = Transform.Scale(x.From, input.Disjoint);
                    if (x.To.X > 0 && x.To.X < 1) x.Transform(scale);
                    allExtensions.Add(new Polyline(new List<Point3d>() { x.From, x.To }));
                });

                allExtensions.ForEach(x =>
                {
                    Debug.Add(RhinoPolylineToSvgar(x));
                });
            }

            var largeLines = new List<Polyline>();

            for (var i = 0; i < leftEdge.SegmentCount; i++)
            {
                var lc = leftEdge.SegmentAt(i);
                var rc = rightEdge.SegmentAt(i);

                var threshold = input.Adjacent / pointCount;

                if (lc.Length > threshold)
                {
                    var largeC = new Line(lc.From, lc.To);
                    largeC.ExtendThroughBox(new BoundingBox(new Point3d(0, 0, 0), new Point3d(1, 1, 1)));
                    var largeCL = new Line(largeC.From, largeC.To);
                    var largeCR = new Line(largeC.From, largeC.To);

                    var moveR = Transform.Translation(new Vector3d(0.04, 0, 0));
                    var moveL = Transform.Translation(new Vector3d(-0.04, 0, 0));

                    largeCL.Transform(moveL);
                    largeCR.Transform(moveR);

                    largeLines.Add(new Polyline(new List<Point3d>() { largeCL.From, largeCL.To }));
                    largeLines.Add(new Polyline(new List<Point3d>() { largeC.From, largeC.To }));
                    largeLines.Add(new Polyline(new List<Point3d>() { largeCR.From, largeCR.To }));
                }

                if (rc.Length > threshold)
                {
                    var largeC = new Line(rc.From, rc.To);
                    largeC.ExtendThroughBox(new BoundingBox(new Point3d(0, 0, 0), new Point3d(1, 1, 1)));
                    var largeCL = new Line(largeC.From, largeC.To);
                    var largeCR = new Line(largeC.From, largeC.To);

                    var moveR = Transform.Translation(new Vector3d(0.04, 0, 0));
                    var moveL = Transform.Translation(new Vector3d(-0.04, 0, 0));

                    largeCL.Transform(moveL);
                    largeCR.Transform(moveR);

                    largeLines.Add(new Polyline(new List<Point3d>() { largeCL.From, largeCL.To }));
                    largeLines.Add(new Polyline(new List<Point3d>() { largeC.From, largeC.To }));
                    largeLines.Add(new Polyline(new List<Point3d>() { largeCR.From, largeCR.To }));
                }
            }

            largeLines.ForEach(x =>
            {
                Debug.Add(RhinoPolylineToSvgar(x));
            });

            var proportion = leftEdge.ToNurbsCurve().GetLength() / rightEdge.ToNurbsCurve().GetLength();
            var sL = 0.25 * input.Porosity * proportion;
            var sR = 0.25 * input.Porosity * (1 / proportion);

            var iL = new Interval(-sL / 2, sL / 2);
            var iR = new Interval(-sR / 2, sR / 2);

            var anchorL = new Plane(new Point3d(0.2 * (1 - input.Adjacent), 0.3 * (1 - input.Adjacent), 0), Vector3d.ZAxis);
            var rectL = new Rectangle3d(anchorL, iL, iL);
            var rotL = Transform.Rotation((90 * input.Openings) * (Math.PI / 180), anchorL.Origin);
            var stretchL = Transform.Scale(anchorL, 1, 1 + (input.Porosity * proportion), 1);
            rectL.Transform(rotL);
            rectL.Transform(stretchL);

            var anchorR = new Plane(new Point3d(1 - (0.2 * (1 - input.Adjacent)), 1 - (0.3 * (1 - input.Adjacent)), 0), Vector3d.ZAxis);
            var rectR = new Rectangle3d(anchorR, iR, iR);
            var rotR = Transform.Rotation((-90 * input.Openings) * (Math.PI / 180), anchorR.Origin);
            var stretchR = Transform.Scale(anchorR, 1, 1 + (input.Porosity * (1 / proportion)), 1);
            rectR.Transform(rotR);
            rectR.Transform(stretchR);

            Debug.Add(RhinoPolylineToSvgar(rectL.ToPolyline()));
            Debug.Add(RhinoPolylineToSvgar(rectR.ToPolyline()));
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
