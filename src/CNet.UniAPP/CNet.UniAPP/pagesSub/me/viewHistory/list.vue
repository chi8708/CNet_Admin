<template>
	<view class="content">
		<view style="margin-top: 10px;">
			<u-swipe-action v-for="(item,index) in items" :key="item.name">
				<u-swipe-action-item :options="options1" @click="removeHistory(item)">
					<view class="swipe-action u-border-top u-border-bottom">
						<view class="swipe-action__content" @click="gotoDetail(item)">
							<u-row>
								<u-col span="9">
									<view class="swipe-action__content__text">
										<!-- icon-xiangmu-copy -->
										<i class="iconfont" :class="{'icon-xiangmu-copy':item.type=='repo', 'icon-kaifazhezhongxin-copy':item.type=='user'}">{{item.name}}</i>
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
			this.items = uni.getStorageSync("cu_viewHistory");
			//this.queryList();//不适用分页
		},
		methods: {
			//跳转详细
			gotoDetail(item) {
				if(item.type=='repo'){
					uni.navigateTo({
						url: `../../repository/detail?full_name=${item.name}`
					})
				}
				else if(item.type=='user'){
					uni.navigateTo({
						url: `../../developer/detail?login=${item.name}`
					})
				}

			},
			//取消
			removeHistory(item) {
				this.items = this.items.filter(p => p.name != item.name)
				uni.setStorageSync("cu_viewHistory", this.items);
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
	.icon-xiangmu-copy{
		
	}
</style>
