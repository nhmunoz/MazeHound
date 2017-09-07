using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MazeHound.Controllers.API
{
    [RoutePrefix("api/maze")]
    public class MazeController : ApiController
    {
        [Route("")]
        [HttpPost]
        public IHttpActionResult SolveMaze(int x1, int y1, int x2, int y2)
        {
            byte[] content;
            try
            {
                foreach (string file in HttpContext.Current.Request.Files)
                {
                    var fileContent = HttpContext.Current.Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        content = new byte[stream.Length];
                        stream.Read(content, 0, (int)stream.Length - 1);
                        Business.MazeHound mh = new Business.MazeHound();
                        if (mh.SolveMaze(System.Text.Encoding.UTF8.GetString(content), x1, y1, x2, y2, 5, 5))
                        {
                            return Ok(new ResponseStatus() { email = "norman.munoz@gmail.com", repo = "", solution = mh.Path });
                        }
                    }
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Ok(new ResponseStatus());
            }
            return Ok(new ResponseStatus() { email = "norman.munoz@gmail.com", repo = "", solution = "There is not solution" });

        }
    }
}
