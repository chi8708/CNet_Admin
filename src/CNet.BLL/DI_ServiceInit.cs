using CNet.Main.BLL;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;


namespace CNet.BLL
{
    //正常应将此类放在BLL。自己注入，不是使用者注入。
    public class DI_ServiceInit : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<Pub_UserBLL>();
        }
    }
}
