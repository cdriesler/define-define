using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;
using Rhino.Geometry;

namespace Define.Api
{
    public class PathData
    {
        public List<List<double>> Paths { get; set; }
    }

    public class GetInput : NancyModule
    {
        public static Dictionary<string, Func<List<Polyline>, double>> Translations { get; private set; } = new Dictionary<string, Func<List<Polyline>, double>>()
        {
            { "tutorial", OnTutorial },
            { "adjacent", OnAdjacent },
            { "openings", OnOpenings },
            { "disjoint", OnDisjoint },
            { "largethreshold", OnThreshold },
            { "porosity", OnPorosity },
            { "parallel", OnParallel }
        };

        public GetInput()
        {
            Post["/in/{name}"] = param => OnMeasureInput(Context, param.name);
        }

        public static Response OnMeasureInput(NancyContext ctx, string inputName)
        {
            try
            {
                var p = JsonConvert.DeserializeObject<PathData>(ctx.Request.Body.AsString());
                var lines = SvgarToPolyline(p.Paths);

                Console.WriteLine(lines.Count);

                if (lines.Count <= 0)
                {
                    var res = new Response();
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = "Could not convert input lines to Rhino geometry.";
                    return res;
                }

                Translations.TryGetValue(inputName, out var translator);

                if (translator == null)
                {
                    var res = new Response();
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = "No routine written for given input.";
                    return res;
                }

                var measurement = -1.0;

                try
                {
                    measurement = translator(lines);
                }
                catch (Exception e)
                {
                    // Do nothing
                    Console.WriteLine(e.Message);
                }
                
                if (measurement < 0)
                {
                    measurement = 0;
                }

                if (measurement > 1)
                {
                    measurement = 1;
                }

                var result = (Response)JsonConvert.SerializeObject(measurement);
                result.StatusCode = HttpStatusCode.OK;
                return result;
            }
            catch (Exception e)
            {
                var res = new Response();
                res.StatusCode = HttpStatusCode.BadRequest;
                res.ReasonPhrase = e.Message;
                return res;
            }
        }

        public static List<Polyline> SvgarToPolyline(List<List<double>> coordinates)
        {
            var lines = new List<Polyline>();

            coordinates.ForEach(x =>
            {
                var linePoints = new List<Point3d>();

                if (x.Count % 8 == 0)
                {
                    for (int i = 0; i < x.Count; i += 8)
                    {
                        linePoints.Add(new Point3d(x[i], x[i + 1], 0));
                    }

                    linePoints.Add(new Point3d(x[x.Count - 2], x[x.Count - 1], 0));
                }

                if (linePoints.Count >= 2)
                {
                    lines.Add(new Polyline(linePoints));
                }
            });

            return lines;
        }

        // Translator methods
        public static double OnTutorial(List<Polyline> crvs)
        {
            var r = new Random();
            return r.NextDouble();
        }

        public static double OnAdjacent(List<Polyline> crvs)
        {
            var max = Math.Sqrt(2);

            if (crvs.Count < 2)
            {
                var r = new Random();
                return Math.Round(r.NextDouble());
            }

            var distanceA = crvs[0].ToNurbsCurve().PointAtStart.DistanceTo(crvs[1].ToNurbsCurve().PointAtStart) / max;
            var distanceB = crvs[0].ToNurbsCurve().PointAtEnd.DistanceTo(crvs[1].ToNurbsCurve().PointAtEnd) / max;

            return (distanceA + distanceB) / 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crvs"></param>
        /// <returns></returns>
        public static double OnOpenings(List<Polyline> crvs)
        {
            if (crvs.Count <= 0)
            {
                return -1;
            }
            else if (crvs.Count == 1)
            {
                var c = crvs[0].ToNurbsCurve();

                var mid = c.PointAtNormalizedLength(0.5);

                var start = c.PointAtStart - mid;
                var end = c.PointAtEnd - mid;

                var angle = Vector3d.VectorAngle(new Vector3d(start), new Vector3d(end), Plane.WorldXY);
                var deg = (angle * (180 / Math.PI)) % 90;

                return deg / 90;
            }
            else if (crvs.Count >= 2)
            {
                var cA = crvs[0].ToNurbsCurve();
                var cB = crvs[crvs.Count - 1].ToNurbsCurve();

                var cApts = new List<Point3d>() { cA.PointAtStart, cA.PointAtEnd }.OrderBy(x => x.Y);
                var cBpts = new List<Point3d>() { cB.PointAtStart, cB.PointAtEnd }.OrderBy(x => x.Y);

                var angle = Vector3d.VectorAngle(new Vector3d(cApts.Last() - cApts.First()), new Vector3d(cBpts.Last() - cBpts.First()));
                var deg = (angle * (180 / Math.PI)) % 90;

                return deg / 90;
            }

            return -1;
        }

        public static double OnDisjoint(List<Polyline> crvs)
        {
            var angles = new List<double>();

            crvs.ForEach(x =>
            {
                var c = x.ToNurbsCurve();
                var angle = Vector3d.VectorAngle(Vector3d.XAxis, new Vector3d(c.PointAtEnd - c.PointAtStart));
                angles.Add(Math.Abs((angle * (180 / Math.PI))) % 90);
            });

            return angles.Average() / 90;
        }

        public static double OnThreshold(List<Polyline> crvs)
        {
            if (crvs.Count == 0)
            {
                return 0.1;
            }
            else
            {
                var sizes = new List<double>();

                crvs.ForEach(x =>
                {
                    sizes.Add(x.ToNurbsCurve().GetLength());
                });

                return sizes.Average() / 3;
            }
        }

        public static double OnPorosity(List<Polyline> crvs)
        {
            if (crvs.Count < 2)
            {
                return -1;
            }
            else
            {
                var areas = new List<double>();

                crvs.ForEach(x =>
                {
                    areas.Add(x.ToNurbsCurve().GetBoundingBox(false).Area);
                });

                areas.Sort();

                return areas.First() / areas.Last();
            }
        }

        public static double OnParallel(List<Polyline> crvs)
        {
            if (crvs.Count < 2)
            {
                return 1;
            }
            else 
            {
                var angles = new List<double>();

                crvs.ForEach(x =>
                {
                    var c = x.ToNurbsCurve();
                    angles.Add(Vector3d.VectorAngle(Vector3d.XAxis, new Vector3d(c.PointAtEnd - c.PointAtStart)));
                });

                angles.Sort();

                return angles.First() / angles.Last();
            }
        }

    }
}
