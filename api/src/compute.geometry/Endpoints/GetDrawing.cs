using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace Define.Api
{
    public class GetDrawing : NancyModule
    {
        public GetDrawing()
        {
            Post["/d"] = _ => OnGetDrawing(Context);
        }

        public Response OnGetDrawing(NancyContext ctx)
        {
            try
            {
                var input = JsonConvert.DeserializeObject<InputManifest>(ctx.Request.Body.AsString());

                var drawing = new DrawingManifest(input);

                var result = (Response)JsonConvert.SerializeObject(drawing);
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
    }
}
