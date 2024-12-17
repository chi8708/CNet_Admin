<template>
	<view>
		<view class="nav-tab uni-flex uni-row">
			<view class="nav-tab-item flex-item" v-for="(item,index) in tabItems" :class="{active:index==checkIndex}"
				@click="tabItemClick(index)">
				<view class="nav-tab-item_label">
					<u-icon :name="item.name" :label="item.label"></u-icon>
				</view>
			</view>
		</view>
		<view class="container item-content">
			<view v-show="checkIndex==0">
				<view class="item-content-info1 card-box">
					<view>
						<view>
							<u-image class="author-img flex-item-WVcenter" width="40px" height="40px"
								:src="item.avatar_url" shape="circle">
							</u-image>
						</view>
						<view>
							<h4 class="text-title">{{item.name}}</h4>
							<small class="text-desc">{{item.login}} {{item.bio}}</small>
						</view>

					</view>
					<view class="item2 flex font-sm">
						<view>
							<i class="iconfont icon-dian font-sm"><label
									class="font-sm">{{item.location||'未知地区'}}</label></i>
						</view>
						<view class="flex">
							<i class="iconfont icon-wode1 font-sm"><label>{{item.followers}}</label></i>
						</view>
						<view style="text-align: right;">
							<span style="color: #000;">{{dateParse(item.created_at)}}</span>
						</view>
					</view>
					<view class="flex">
						<view class="flex-item-flex_1 flex-item-WVcenter" style="border-right: 1px solid #e0e0e0;"
							@click="saveStar">
							<u-icon :name="isStared?'star-fill':'star'" label="收藏" :color="isStared?'#ff0000':'#3c9cff'"
								labelColor="#3c9cff"></u-icon>
						</view>

						<view class="flex-item-flex_1 flex-item-WVcenter" @click="share">
							<button open-type="share" class="share">
								<u-icon name="share" label="分享" color="#3c9cff" labelColor="#3c9cff"></u-icon>
							</button>

						</view>
					</view>
				</view>
				<view class="item-content-info2">
					<u-cell-group title="其他">
						<u-cell title="博客" :value="item.blog||'未知'"></u-cell>
						<u-cell title="邮箱" :value="item.email||'未知'"></u-cell>
						<u-cell title="followers" :value="item.followers"></u-cell>
						<u-cell title="following" :value="item.following==0?'0':item.following"></u-cell>
						<u-cell title="创建日期" :value="item.created_at"></u-cell>
						<u-cell title="更新日期" :value="item.updated_at"></u-cell>
					</u-cell-group>
				</view>
				<ad unit-id="adunit-513e3d6ccfbe9960" bindload="adLoad" binderror="adError" bindclose="adClose"></ad>
			</view>

			<view v-show="checkIndex==1">
				<view v-for="(item,index) in Users">
					<list-item :item="item"></list-item>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		getModel,
		getRepos
	} from '@/api/developer.js'
	import {
		dateParse
	} from '@/common/date.js'
	import listItem from './detail-reposItem.vue'
	export default {
		components: {
			listItem
		},
		data() {
			return {
				cu_starsUser: [],
				isStared: false,
				item: {},
				Users: [],
				checkIndex: 0,
				tabItems: [{
						name: "info-circle",
						label: "基本信息"
					},
					{
						name: "file-text",
						label: "他的仓库"
					},
				]
			}
		},
		onLoad(e) {
			this.showDetail(decodeURIComponent(e.login));
		},
		mounted() {},
		methods: {
			async tabItemClick(i) {
				this.checkIndex = i;
				if (i == 1) {
					let login = this.item.login;
					let res = await getRepos(login);
					if (res.code == 1) {
						this.Users = res.data;
					}
				}
			},
			async showDetail(login) {
				let res = await getModel(login);
				if (res.code == 1) {
					this.item = res.data;
					this.getStars();
					this.saveViewHistory();
				}
			},
			getStars() {
				let that = this;
				uni.getStorage({
					key: 'cu_starsUser',
					success(res) {
						that.cu_starsUser = res.data;
						that.isStared = that.cu_starsUser.some(p => p.login == that.item.login);
					}
				});

			},
			//收藏
			saveStar() {
				this.isStared = !this.isStared;
				this.cu_starsUser = this.cu_starsUser.filter(p => p.login != this.item.login); //删除
				if (this.isStared) {
					this.cu_starsUser.unshift({
						login: this.item.login,
						createTime: new Date()
					});
					//this.cu_starsUser.push({login:this.item.login,createTime:new Date()});
				}
				if (this.cu_starsUser.length > 100) {
					this.cu_starsUser.pop();
				}
				uni.setStorageSync('cu_starsUser', this.cu_starsUser);
			},
			//保存浏览记录
			saveViewHistory() {
				let viewHistory = []
				var that = this;
				uni.getStorage({
					key: 'cu_viewHistory',
					success(res) {
						viewHistory = res.data;
					},
					fail() {

					},
					complete() {
						try {
							viewHistory.forEach((p, index) => {
								if (p.name == that.item.login && p.type == "user") {
									viewHistory.splice(index, 1); //删除
									throw new Error('LoopInterrupt'); //跳出循环
								}
							});
						} catch (e) {

						}
						if(viewHistory.length>100){
							viewHistory.splice(viewHistory.length-1, 1);
						}
						//插入
						viewHistory.unshift({
							name: that.item.login,
							createTime: new Date(),
							type: "user"
						});
						uni.setStorage({
							key: 'cu_viewHistory',
							data: viewHistory
						});
					}
				});
			},
			share() {

			},
			dateParse(data) {
				return dateParse(new Date(data), "yyyy-MM-dd");
			}
		}
	}
</script>
<!-- 背景色对应页面中的style样式表中设置，且不能有scoped属性 -->
<style>
	page {
		background-color: #f9f9f9;
	}
</style>
<style scoped lang="scss">
	.share {
		background-color: #fff;
		width: 100%;
		height: 100%;
		display: flex;
		justify-content: center;
		align-items: center;
	}

	.share::after {
		border: none;
		//background-color: #fff;
	}

	.nav-tab {
		&-item {
			padding: 10px;
			text-align: center;

			&_label {
				margin: 0 auto;
				width: fit-content;
			}
		}
	}

	.active {
		.nav-tab-item_label {
			border-bottom: 2px solid #2979ff;
		}

		/deep/.u-icon__icon,
		//样式穿透访问，设置子组件样式
		::v-deep.u-icon__label {
			color: #2979ff !important;

		}
	}

	.flex-item {
		flex: 1;
	}

	.item-content {
		height: 100%;

		&-info1 {
			background-color: #fff;

			>view:nth-of-type(1) {
				display: flex;

				>view {
					padding: 10px;
				}
			}

			>view:nth-of-type(2) {
				color: #00557f;
				justify-content: space-between;
				vertical-align: middle;
				padding: 10px;
				border-bottom: 1px solid #e0e0e0;

				>view {
					flex: 1;

					&:nth-of-type(1) {
						flex: 0 0 120px; // 第一个固定其余自动分配
					}
				}


			}

			>view:nth-of-type(3) {
				height: 40px;
			}



		}

		&-info2 {
			background-color: #fff;
			margin-top: 10px;
		}
	}

	/deep/.u-cell-group__title {
		background-color: #f9f9f9;
	}
</style>
