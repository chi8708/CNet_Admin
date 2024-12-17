<template>
	<view class="content">
		<view class="head">
			<view class="head-container" @click="gotoUserInfo">

				<view class="head-avatar"
					style="border: 0px solid #78b9ff; padding: 2rpx; border-radius: 5%;margin-right: 10rpx;">
					<u-avatar :src="userInfo?userInfo.avatarUrl:'/static/img/tabbar/meactive.png'" shape="square"></u-avatar>
				</view>
				<view v-if="userInfo">
					<text>{{userInfo.nickName}}</text>
				</view>
				<view v-else>
					<u-button type="primary" text="登录/注册" size="mini"></u-button>
				</view>
			</view>
		</view>
		<view class="group1">
			<u-cell-group customStyle="{'background-color':'#fff'}">
				<u-cell icon="star" title="收藏" :isLink="true" @click="gotoStar"></u-cell>
				<u-cell icon="map" title="足迹" :isLink="true" @click="gotoViewHistory"></u-cell>
			</u-cell-group>
		</view>
		<view class="group2">
			<u-cell-group customStyle="{'background-color':'#fff'}">
				<!-- <u-cell icon="thumb-up" title="赞赏" :isLink="true" @click="gotoAdmire"></u-cell> -->
				<button open-type="share" class="share btn-cell">
					<u-cell icon="share" title="分享" :isLink="true">
					</u-cell>
				</button>
				<button open-type="feedback" class="feedback btn-cell">
					<u-cell icon="edit-pen" title="意见反馈" :isLink="true">
					</u-cell>
				</button>
				<!-- 	<u-cell icon="file-text" title="更新日志" :isLink="true"></u-cell> -->
				<u-cell icon="setting" title="设置" :isLink="true" @click="gotoSetting"></u-cell>
			</u-cell-group>
		</view>
		<ad unit-id="adunit-3cc2a087e503d723" ad-type="video" ad-theme="white" bindload="adLoad" binderror="adError" bindclose="adClose" style="margin-top: 50px;"></ad>
		
	</view>
</template>

<script>
	import {mapState, mapActions} from 'vuex'
	export default {
		data() {
			return {
			}

		},
		computed:{
			//...mapState(['userInfo'])
			////计算属性响应式 login登录页面后会自动更新userInfo
			userInfo:{
				get(){
					return this.$store.state.user.userInfo;
				},
				set(val){
					
				}
			}
		},
		//重设按钮分享内容
		onShareAppMessage(res) {
			if (res.from === 'button') { // 来自页面内分享按钮
				console.log(res.target)
			}
			return {
				title: 'github热门排行,手机端看源码',
				path: '/pages/trend/index',
				content: 'github仓库开发者排行，github中文',
				imageUrl: '/static/index.jpg'
			}
		},
		//分享到朋友圈 要同时设置发送朋友 才能使用 
		onShareTimeline() {
			return {
				title: 'github热门排行,手机端看源码',
				path: '/pages/trend/index',
				content: 'github仓库开发者排行，github中文',
				imageUrl: '/static/index.jpg'
			}
		},
		onLoad() {},
		mounted() {
			this.getLoginInfo();
		},
		methods: {
		   ...mapActions(['getUserInfo']),
			// Refresh() {
			// 	this.getLoginInfo();
			// },
			async getLoginInfo() {
				let userInfo =await this.getUserInfo();
				if (userInfo) {
					this.userInfo = userInfo;
				}
			},
			gotoStar() {
				uni.navigateTo({
					url: '/pagesSub/me/star/list'
				})
			},
			gotoViewHistory() {
				uni.navigateTo({
					url: '/pagesSub/me/viewHistory/list'
				})
			},
			gotoSetting() {
				uni.navigateTo({
					url: '/pagesSub/me/setting/index'
				})
			},
			gotoLogin(){
				uni.navigateTo({
					url: '/pagesSub/login/login'
				})
			},
			gotoUserInfo() {
				if(this.userInfo){
					// uni.navigateTo({
					// 	url: '/pagesSub/me/userinfo/index'
					// })
				}
				else{
					this.gotoLogin();
				}
			},
			gotoAdmire(){
				uni.navigateTo({
					url:'/pagesSub/me/admire'
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	//分享按钮样式处理
	.btn-cell {
		background-color: #fff;
		width: 100%;
		height: 100%;
		padding: 0;
		text-align: left;

		&::after {
			border: none;
			border-bottom: 1px solid rgb(214, 215, 217)
		}
	}

	.head {
		display: flex;
		align-items: center;
		//background-color: #3c9cff;
		background-image: linear-gradient(#3c9cff 20%, #ecf5ff); //设置透明度防止有连接色
		padding: 20px;
		padding-bottom: 0px;

		&-container {
			padding: 10px;
			height: 50px;
			display: flex;
			align-items: center;
			width: 100%;
			border-top-left-radius: 10px;
			border-top-right-radius: 10px;
			background-color: #fff;
			margin-bottom: -25px;
		}

		&-avatar {
			width: 40px;
			height: 40px;
			background-color: #fff;
			//opacity:1;
			border-radius: 50%;
		}
	}

	@mixin group_container_style {
		margin: 10px;
		background-color: #fff;
		margin-top: 15px;
		box-shadow: 0px 0px 3px 2px rgba(0, 0, 0, 0.08);
		border-radius: 10px;
	}

	//移除group上下边框
	::v-deep .u-cell-group__wrapper {
		:first-child {
			border: 0px !important;
		}

		.u-cell:last-of-type .u-line {
			border: 0px !important;
		}


	}

	.group1 {
		@include group_container_style;
	}

	.group2 {
		@include group_container_style;
	}
</style>
