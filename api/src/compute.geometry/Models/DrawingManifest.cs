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

        public DrawingManifest(InputManifest input)
        {
            // Make a drawing with the inputs
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
