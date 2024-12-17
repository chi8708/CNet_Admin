import Vue from 'vue'
import Vuex from 'vuex'

import user from './user'
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    sessionAuthId: '', //微信缓存授权信息
	hasLogin: false,//存储用户当前是否登录，作为切换特效使用
	userInfo: {}, //用户信息存储
	 showLoginTip: false,//显示登录框
  },
  mutations: {
    sessionAuthId(state, payload) {
        state.sessionAuthId = payload
    },
	hasLogin(state, payload) {
	    state.hasLogin = payload
	},
	userInfo(state, userInfo) {
	    state.userInfo = userInfo
	},
	showLoginTip(state, payload) {
	    state.showLoginTip = payload
	},
  },
  actions: {
    //
  },
  getters: {
      userInfo: state => state.userInfo,
      hasLogin: state => state.hasLogin,
      sessionAuthId: state => state.sessionAuthId,
  },
  modules: {
    user
  }
})
