<template>
	<view>
		<view>
			<u-cell-group>
				<u-cell icon="trash-fill" title="清除收藏" @click="clearShow('是否清除收藏',1)"></u-cell>
				<u-cell icon="trash-fill" title="清除足迹" @click="clearShow('是否清除足迹',2)"></u-cell>
				<u-cell icon="trash-fill" title="清除缓存" @click="clearShow('是否清除缓存',3)"></u-cell>
			</u-cell-group>
		</view>
		<view style="margin:200rpx 10rpx 10rpx 10rpx" v-if="userInfo">
			<button type="default" @click="loginOut">退出登录</button>
		</view>


		<u-modal :show="Modal_Show" :title="Modal_Title" :showConfirmButton="true" :showCancelButton="true"
			@confirm="saveConfirm" @cancel="saveCancel">
			<view class="slot-content">
				{{Modal_Content}}
			</view>
		</u-modal>
	</view>
</template>

<script>
	import {
		mapState,
		mapActions
	} from 'vuex'
	export default {
		data() {
			return {
				oprType: 0,
				Modal_Show: false,
				Modal_Title: '操作确认',
				Modal_Content: ''
			}
		},
		computed: {
			//...mapState(['userInfo'])
			////计算属性响应式 login登录页面后会自动更新userInfo
			userInfo: {
				get() {
					return this.$store.state.user.userInfo;
				},
				set(val) {

				}
			}
		},
		methods: {
			...mapActions(['getUserInfo','clearUserInfo']),
			// Refresh() {
			// 	this.getLoginInfo();
			// },
			async getLoginInfo() {
				let userInfo = await this.getUserInfo();
				if (userInfo) {
					this.userInfo = userInfo;
				}
			},
			clearShow(content, oprType) {
				this.Modal_Content = content;
				this.Modal_Show = true;
				this.oprType = oprType;
			},
			clearStarConfirm() {
				uni.removeStorageSync('cu_starsRepo');
				uni.removeStorageSync('cu_starsUser');
				uni.showToast({
					title: '清除成功',
					icon: 'none'
				});
				this.Modal_Show = false;
			},
			clearViewHistoryConfirm() {
				uni.removeStorageSync('cu_viewHistory');
				uni.showToast({
					title: '清除成功',
					icon: 'none'
				});
				this.Modal_Show = false;
			},
			clearAllConfirm() {
				uni.removeStorageSync('cu_starsRepo');
				uni.removeStorageSync('cu_starsUser');
				uni.removeStorageSync('cu_viewHistory');
				uni.showToast({
					title: '清除成功',
					icon: 'none'
				});
				this.Modal_Show = false;
			},
			saveConfirm() {
				switch (this.oprType) {
					case 1:
						this.clearStarConfirm();
						break;
					case 2:
						this.clearViewHistoryConfirm();
						break;
					case 3:
						this.clearAllConfirm();
						break;
					default:
						break;
				}
			},
			saveCancel() {
				this.Modal_Show = false;
			},
			loginOut() {
					let b=this.clearUserInfo();
					b.then(r=>{
						if(r.code==1){
							uni.showToast({
								title: '退出成功',
								icon: 'none'
							});
						}
					})
			}

		}
	}
</script>

<style>

</style>
