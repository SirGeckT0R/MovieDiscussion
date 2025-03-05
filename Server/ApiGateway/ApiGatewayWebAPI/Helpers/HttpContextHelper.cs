using ApiGatewayWebAPI.Enums;

namespace ApiGatewayWebAPI.Helpers
{
    public static class HttpContextHelper
    {
        public static RequestMethod GetRequestMethod(HttpContext context)
        {
            return context.Request.Method switch
            {
                "GET" => RequestMethod.GET,
                "POST" => RequestMethod.POST,
                "PUT" => RequestMethod.PUT,
                "DELETE" => RequestMethod.DELETE,
                _ => RequestMethod.OTHER
            };
        }

        public static bool IsSuccess(HttpContext context)
        {
            return context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;
        }
    }
}
