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

                var measurement = translator(lines);

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
    }
}
