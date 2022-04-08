import Vue from 'vue'
import Router from 'vue-router'
import routes from './routers'
import store from '@/store'
import iView from 'iview'
import {setToken, getToken, setTitle} from '@/libs/util'
import config from '@/config'

const {homeName} = config

Vue.use(Router)
const router = new Router({
  routes,
  mode: 'hash'
})
const LOGIN_PAGE_NAME = 'login'

const initRouters = (store) => {
  //这个人登录了已经
  if (store.state.user.hasGetInfo) {
    //路由加载过了
    if (store.state.app.hasGetRouter && store.state.app.routers && store.state.app.routers.length > 0) {
      console.log("已经加载过了路由")
    } else {
      //加载路由
      console.log("开始加载路由权限...")
      store.dispatch('getUserMenus').then(routers => {
        //此处routers已经是按照权限过滤后的路由了，没权限的，不加入路由，无法访问
        //路由重置一下把404放最后
        const newRouter = new Router({
          routes,
          mode: config.routerModel
        })
        router.matcher = newRouter.matcher;
       // router.addRoutes(routes);
        //把404加最后面，如果用router.push({name:'xxxx'})这种的话，404页面可能空白，用path:'/aa/bb'
        router.addRoutes(routers.concat([{
          path: '*',
          name: 'error_404',
          meta: {
            hideInMenu: true
          },
          component: () => import(/* webpackChunkName: "404" */'@/view/error-page/404.vue')
        }]))
      }).finally(() => {
      })
    }
  }
}
router.beforeEach((to, from, next) => {
  iView.LoadingBar.start()
  const token = getToken()
  if (!token && to.name !== LOGIN_PAGE_NAME) {
    // 未登录且要跳转的页面不是登录页
    next({
      name: LOGIN_PAGE_NAME // 跳转到登录页
    })
  } else if (!token && to.name === LOGIN_PAGE_NAME) {
    // 未登陆且要跳转的页面是登录页
    next() // 跳转
  } else if (token && to.name === LOGIN_PAGE_NAME) {
    // 已登录且要跳转的页面是登录页
    next({
      name: homeName // 跳转到homeName页
    })
  } else {
    if (store.state.user.hasGetInfo) {
      initRouters(store)
      next()
    } else {
      store.dispatch('getUserInfo').then(user => {
        initRouters(store)
        next()
      }).catch(() => {
        setToken('')
        next({
          name: 'login'
        })
      })
    }
  }
})

router.afterEach(to => {
  setTitle(to, router.app)
  iView.LoadingBar.finish()
  window.scrollTo(0, 0)
})

export default router
