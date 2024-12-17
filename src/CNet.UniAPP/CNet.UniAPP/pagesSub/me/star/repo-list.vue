<template>
	<view class="content">
		<!-- 	<z-paging ref="paging" v-model="dataList" @query="queryList" :fixed="false" :auto="false"
			:auto-show-system-loading="true">
			<view style="margin-top: 10px;">
				<u-swipe-action v-for="(item,index) in dataList">
					<u-swipe-action-item :options="options1">
						<view class="swipe-action u-border-top u-border-bottom">
							<view class="swipe-action__content">
								<text class="swipe-action__content__text">{{item.full_name}}</text>
							</view>
						</view>
					</u-swipe-action-item>
				</u-swipe-action>
			</view>
		</z-paging> -->
		<view style="margin-top: 10px;">
			<u-swipe-action v-for="(item,index) in items" :key="item.full_name">
				<u-swipe-action-item :options="options1" @click="removeStar(item)">
					<view class="swipe-action u-border-top u-border-bottom">
						<view class="swipe-action__content" @click="gotoDetail(item)">
							<u-row>
								<u-col span="9">
									<view class="swipe-action__content__text">
										<i class="iconfont icon-xiangmu-copy">{{item.full_name}}</i>
									</view>
								</u-col>
								<u-col span="3" style="text-align:right;padding-right: 20rpx;">
									<text class="font-sm">{{item.createTime.substring(0,10)}}</text>
								</u-col>
							</u-row>

						</view>
					</view>
				</u-swipe-action-item>
			</u-swipe-action>
		</view>
		<u-empty mode="list" icon="http://cdn.uviewui.com/uview/empty/list.png" v-if="!items||items.length<=0">
		</u-empty>
	</view>
</template>
<style lang="scss">
</style>

<script>
	import {
		dateParse
	} from '@/common/date.js'
	export default {
		components: {
			//listItem
		},
		data() {
			return {
				searchModel: {},
				dataList: [],
				items: [],
				options1: [{
					text: '删除',
					style: {
						backgroundColor: '#f56c6c'
					}
				}]
			}
		},
		onLoad() {},
		mounted() {
			this.items = uni.getStorageSync("cu_starsRepo");
			//this.queryList();//不适用分页
		},
		methods: {
			queryList(pageNo = 1, pageSize = 10) {
				let items = this.items;
				let pageItems = [];
				for (let i = 0; i < items.length; i++) {
					if (i >= pageNo * pageSize) {
						break;
					} else if (i >= (pageNo - 1) * pageSize && i <= (pageNo * pageSize - 1)) {
						pageItems.push(items[i]);
					}
				}
				this.$refs.paging.complete(pageItems);
			},
			//跳转详细
			gotoDetail(item) {
				uni.navigateTo({
					url: `../../repository/detail?full_name=${item.full_name}`
				})
			},
			//取消
			removeStar(item) {
				this.items = this.items.filter(p => p.full_name != item.full_name)
			 uni.setStorageSync("cu_starsRepo", this.items);
			}

		}
	}
</script>
<style lang="scss">
	/* 注意:父节点需要固定高度fixed:true，z-paging的height:100%才会生效 ,fixed:true 后顶部会留空间*/
	.content {
		height: 100%;
	}

	.u-page {
		padding: 0;
	}

	.u-demo-block__title {
		padding: 10px 0 2px 15px;
	}

	.swipe-action {
		&__content {
			padding: 25rpx 0;

			&__text {
				font-size: 15px;
				color: $u-main-color;
				padding-left: 20rpx;
			}
		}
	}
</style>
