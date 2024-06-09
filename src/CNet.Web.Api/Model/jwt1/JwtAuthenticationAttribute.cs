using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CNet.Web.Api
{
    /// <summary>
    /// 异常捕获
    /// </summary>
    public class JwtAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //App_UserTokenBLL tokenBLL = new App_UserTokenBLL();

            var httpContext = context.HttpContext;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var user = httpContext.User;

            var authorization = request.Headers.Authorization.FirstOrDefault();

            if (authorization == null || !authorization.StartsWith("Bearer"))
            {
                context.Result = new AuthenticationFailureResult("Invalid token Bearer", request,response);
                return;
            }
   

            if (user.Identity.IsAuthenticated)
            {
                //var userCode = user.GetCNetUser().UserCode;
                //App_UserToken userToken = tokenBLL.Get(userCode, "UserCode");
                //if (userToken == null)
                //{
                //    context.Result = new AuthenticationFailureResult("Invalid token in DB", request, response);
                //    return;
                //}

                ////判断token是否改变
                //var savedToken = userToken.Token;
                //var token= request.Headers["Authorization"];
                //if (token !=$"Bearer {savedToken}")
                //{
                //    context.Result = new AuthenticationFailureResult("Invalid token", request, response);
                //    return;
                //}
                ////判断token是否过期
                //var latestToken = userToken.EditTime;
                //double expireHours = Convert.ToDouble(AppConfigurtaionServices.Configuration["JwtSeetings:TokenExprieHours"]);//过期时长
                //if (latestToken.Value.AddHours(expireHours) < DateTime.Now)
                //{
                //    //过期
                //    context.Result = new AuthenticationFailureResult("Expired token", request, response);
                //    return;
                //}

                //userToken.EditTime = DateTime.Now;
                //Task.Run(() =>
                //{
                //    //更新用户token
                //    tokenBLL.Update(userToken);
                //});


                //通过Redis判断
                //var userCode = user.GetCNetUser().UserCode;
                //var tokenKey = Controllers.Constant.TOKEN_PRE + userCode;
                //string savedToken = Controllers.Constant.redis7.StringGet(tokenKey);
                //if (string.IsNullOrEmpty(savedToken))
                //{
                //    context.Result = new AuthenticationFailureResult("Miss token in DB", request, response);
                //    return;
                //}

                ////判断token是否改变
                //var token = request.Headers["Authorization"];
                //if (token != $"Bearer {savedToken}")
                //{
                //    context.Result = new AuthenticationFailureResult("Invalid token", request, response);
                //    return;
                //}

                //TimeSpan ts = TimeSpan.FromHours(Convert.ToInt32(Controllers.Constant.TOKEN_EXPRIE_HOURS));
                ////更新token过期时间
                //Controllers.Constant.redis7.KeyExpire(tokenKey, ts);

            }
            else
            {
                context.Result = new AuthenticationFailureResult("Missing Jwt Token", request, response);
            }
        }


    }
}