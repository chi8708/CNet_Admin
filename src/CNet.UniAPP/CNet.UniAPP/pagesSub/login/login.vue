<template>
	<view class="content">
		<view>
			<image src="../../static/img/logo.png" style="width: 80px;height: 80px;"></image>
			<view>
				<text class="font-sm">登录开发者将获取一些权限</text><br />
				<text>获取您的公开信息（昵称、头像等）</text>
			</view>

		</view>
		<view>
			<!-- open-type="getUserInfo" -->
			<button style="margin: 10px 100px;" @click="getUserInfo">微信快捷登录</button>
		</view>
	</view>
</template>

<script>
	import {
		WX_loginByCode
	} from "@/api/customer.js"
	import{mapActions} from 'vuex'
	export default {
		data() {
			return {
				redirect: '',
				redirectIsTab: false
			}
		},
		onLoad(e) {
			this.redirect = e.redirectUrl ? decodeURIComponent(e.redirectUrl) : '';
			this.redirectIsTab = e.redirectIsTab || false;
			// this.WX_loginByCode({
			// 	'code': '123123'
			// });
		},
		methods: {
			...mapActions(['writeUserInfo']),
			getUserInfo() {
				//1wx.getUserProfile弹出授权页面获取昵称、头像 2.wx.login登录获取code。3.通过code获取openId。
				let that = this;
				wx.getUserProfile({
					desc: '获取个人资料',
					success(res) {
						wx.login({
							success(resLogin) {
								if (resLogin.code) {
									res.userInfo.code = resLogin.code;
									that.WX_loginByCode(res.userInfo);
								} else {
									console.log('登录失败！' + resLogin.errMsg);
								}
							}
						})
					},
					fail() {
						uni.showToast({
							title: '授权失败',
							icon: 'error'
						});
					}
				})
			},
			async WX_loginByCode(req) {
				//发起网络请求
				let WX_loginRes = await WX_loginByCode(req);
				if (WX_loginRes.code == 1) {
					uni.setStorageSync('isLogin', true);
					uni.setStorageSync('userInfo', WX_loginRes.data.userInfo);
					let b=this.writeUserInfo(WX_loginRes.data.userInfo);
					b.then(r=>{
						console.log(r);
					})
					let pages=getCurrentPages();
					let prevPage = pages[pages.length - 2]; //上一页页面实例
					if (typeof(prevPage) == "undefined") {
						//没上一页面
						return;
					}
					uni.navigateBack({
						success() {
							//prevPage.$vm.Refresh(); 
						}
					});
					//跳转页面
					// if (this.redirectUrl) {
					// 	if (this.redirectIsTab) {
					// 		uni.switchTab({
					// 			url: redirectUrl
					// 		})
					// 	} else {
					// 		uni.navigateTo({
					// 			url: this.redirectUrl
					// 		})
					// 	}

					// } else {
					// 	uni.switchTab({
					// 		url: '/pages/me/index'
					// 	})
					// }

				} else {
					uni.showToast({
						title: '远程登录失败',
						icon: 'none'
					});
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	.content {
		>view {
			text-align: center;
			margin-top: 100rpx;
		}
	}
</style>
