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
    public class Test : NancyModule
    {
        public Test(Nancy.Routing.IRouteCacheProvider routeCacheProvider)
        {
            Get["/test"] = _ => Something(Context);
        }

        static Response Something(NancyContext Context)
        {
            var fakeCurve = new LineCurve(Point3d.Origin, new Point3d(0, 1, 0));

            var receivedCurve = JsonConvert.DeserializeObject<Curve>(Context.Request.Body.AsString());

            var offsetCurve = fakeCurve.Offset(Plane.WorldXY, 5, 0.1, CurveOffsetCornerStyle.None)[0];

            return JsonConvert.SerializeObject(offsetCurve);
        }

    }
}
