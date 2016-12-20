using System.Web.Mvc;

namespace ACS.MDB.Net.App.Common
{
    /// <summary>
    ///
    ///
    /// </summary>
    public class HttpStatusCodeAndErrorResult : HttpStatusCodeResult
    {
        public HttpStatusCodeAndErrorResult(int statusCode, string errorMsg)
            : base(statusCode, errorMsg)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var response = httpContext.Response;
            response.Write(StatusDescription);
            base.ExecuteResult(context);
        }
    }
}