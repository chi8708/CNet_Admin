using Microsoft.AspNetCore.Builder;

namespace CNet.Web.Api.Config
{
    /// <summary>
    /// 中间件
    /// </summary>
    public static class MiddlewareHelpers
    {
        
        /// <summary>
        /// Swagger授权中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerAuthorizedMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }

    }
}
