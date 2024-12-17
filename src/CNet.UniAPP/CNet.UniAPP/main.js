import App from './App.vue'

// #ifndef VUE3
import Vue from 'vue'
import uView from "uview-ui";
import Directives from './common/directives'
import store from './store'//引入store
//上传方法
import * as Upload from '@/common/utils/uploadHelper.js'
//常用方法库
import * as Common from '@/common/utils/commonHelper.js'
//本地存储封装
import * as Db from '@/common/utils/dbHelper.js'


Vue.use(Directives)
Vue.use(uView)
// 引入uView对小程序分享的mixin封装
let mpShare = require('@/node_modules/uview-ui/libs/mixin/mpShare.js');
Vue.mixin(mpShare)

Vue.config.productionTip = false
App.mpType = 'app'

Vue.prototype.$upload = Upload;
Vue.prototype.$common = Common;
Vue.prototype.$db = Db;
Vue.prototype.$store = store;




const app = new Vue({
	...App,
	store
})


// http拦截器 uview 2.0+不支持
// import httpInterceptor from '@/common/request/http.interceptor.js'
// // 这里需要写在最后，是为了等Vue创建对象完成，引入"app"对象(也即页面的"this"实例)
// Vue.use(httpInterceptor, app)
// http接口API集中管理引入部分
// import httpApi from '@/common/request/http.api.js'
// Vue.use(httpApi, app)


app.$mount()
// #endif

// #ifdef VUE3
import {
	createSSRApp
} from 'vue'
export function createApp() {
	const app = createSSRApp(App)
	return {
		app
	}
}
// #endif
