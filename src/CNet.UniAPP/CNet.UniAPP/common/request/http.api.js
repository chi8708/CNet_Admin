
// 此处第二个参数vm，就是我们在页面使用的this，你可以通过vm获取vuex等操作，更多内容详见uView对拦截器的介绍部分：
// https://uviewui.com/js/http.html#%E4%BD%95%E8%B0%93%E8%AF%B7%E6%B1%82%E6%8B%A6%E6%88%AA%EF%BC%9F
const install = (Vue, vm) => {

    // 获取店铺配置
    //let shopConfig = (params = {}) => vm.$u.post('/Api/Common/GetConfig', params, { method: 'common.shopConfig', needToken: false });
    let shopConfigV2 = (params = {}) => vm.$u.post('/Api/Common/GetConfigV2', params, { method: 'common.shopConfigV2', needToken: false });
  
    vm.$u.api = {
        shopConfigV2
    };
}

export default {
    install
}