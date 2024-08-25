using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Common.Const
{
    public class AppSettingsConst
    {
        #region Swagger授权访问设置
        /// <summary>
        /// Swagger文档默认访问路由地址
        /// </summary>
        public static readonly string SwaggerRoutePrefix =AppConfigurtaionServices.Configuration["SwaggerConfig:RoutePrefix"];

        /// <summary>
        /// Swagger文档登录账号
        /// </summary>
        public static readonly string SwaggerUserName = AppConfigurtaionServices.Configuration["SwaggerConfig:UserName"];

        /// <summary>
        /// Swagger文档登录密码
        /// </summary>
        public static readonly string SwaggerPassWord = AppConfigurtaionServices.Configuration["SwaggerConfig:PassWord"];

        #endregion
    }
}
