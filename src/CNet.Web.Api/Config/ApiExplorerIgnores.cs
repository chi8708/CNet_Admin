using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CNet.Web.Api.Config
{
    public class ApiExplorerIgnores : IActionModelConvention
    {
        /// <summary>
        /// 自带的Controller与swagger3.0冲突，在此排除扫描
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action)
        {
            //冲突的Ocelot.Raft.RaftController
            if (action.Controller.ControllerName == "WxOfficialOAuth" || action.Controller.ControllerName == "WxOpenOAuth")
                action.ApiExplorer.IsVisible = false;
            //Ocelot.Cache.OutputCacheController
            if (action.Controller.ControllerName == "AliPay")
                action.ApiExplorer.IsVisible = false;

            if (action.Controller.ControllerName == "WeChatPay")
                action.ApiExplorer.IsVisible = false;
        }
    }
}
