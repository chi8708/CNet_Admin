import {
	GetModelByWXCode
} from "@/api/customer.js"
const state = { //要设置的全局访问的state对
	userInfo: null,
	isLogin: false,
};

const getters = { //实时监听state值的变化(最新状态)

}

const mutations = {
	 writeUserInfo(state,user) {
		uni.setStorageSync('isLogin', true);
		uni.setStorageSync('userInfo', user);
		state.isLogin = true;
		state.userInfo = user;
		return user;
	},
	clearUserInfo(state){
		uni.setStorageSync('isLogin', false);
		uni.removeStorageSync('userInfo');
		state.isLogin=false;
		state.userInfo=null;
	}
};

//vuex官方API还提供了一个actions，这个actions也是个对象变量，
// 最大的作用就是里面的Action方法 可以包含任意异步操作，这里面的方法是用来异步触发mutations里面的方法，
// actions里面自定义的函数接收一个context参数和要变化的形参，
// context与store实例具有相同的方法和属性，所以它可以执行context.commit(' '),然后也不要忘了把它也扔进
const actions = {
	getUserInfo({
		state,
		commit
	}) { //自定义触发mutations里函数的方法，context与store 实例具有相同方法和属性
		return new Promise((resolve, reject) => {
			let userInfo = uni.getStorageSync('userInfo');
			if (!userInfo) {
				resolve(null);
			} else {
				if(state.isLogin){
					return state.userInfo;
				}
				GetModelByWXCode(userInfo.wX_MiniOpenId).then(res => {
					if (res.code == 1) {
						commit('writeUserInfo', res.data);
						resolve(state.userInfo);
					}
				}).catch(err => {
					reject(err)
				});
			}

		});
	},
	writeUserInfo(context,userInfo) { 
	   return new Promise((resolve, reject) => {
		   context.commit('writeUserInfo',userInfo);
		   resolve({code:1,state:"ok"});
	   }) 
	},
	clearUserInfo(context) {
	   return new Promise((resolve, reject) => {
		   context.commit('clearUserInfo');
		   resolve({code:1,state:"ok"});
	   }) 
	},
	
};

const user = {
	state: () => ({
		...state
	}),
	mutations: mutations,
	actions: actions,
	getters: getters
}

export default user
