<template>
	<view class="item-box " @click="itemClick(item)">
		<view class="item1 flex">
			<view @click.native.stop="itemUserClick(item)">
				<u-image class="author-img" width="40px" height="40px" :src="item.owner.avatar_url" shape="circle">
				</u-image>
			</view>
			<!-- class="v-center" -->
			<view>
				<h6 class="text-ellipsis-lines text-title">{{item.full_name}}</h6>
				<span class="text-ellipsis-lines text-desc">{{item.description}}</span>
			</view>

		</view>
		<view class="item2 flex font-sm">
			<view>
				<i class="iconfont icon-dian font-sm"><label
						class="font-sm">{{item.language||'未知语言'}}</label></i>
			</view>
			<view class="flex">
				<i class="iconfont icon-star font-sm"><label>{{item.stargazers_count}}</label></i>
				<i class="iconfont icon-codefork font-sm"><label>{{item.forks_count}}</label></i>
				<!-- <u-icon name="star"></u-icon><label>{{item.stargazers_count}}</label>
				<u-icon name="share" ></u-icon><label>{{item.forks_count}}</label> -->
			</view>
			<view style="text-align: right;">
				<span style="color: #000;">{{item.pushed_at}}</span>
			</view>
		</view>


	</view>
</template>

<script>
	export default {
		props: {
			item: {},
			moreShow:false
		},
		data() {
			return {}
		},
		methods: {
			itemClick(item) {
				if(this.moreShow){
					return;
				}
				uni.navigateTo({
					url: '/pagesSub/repository/detail?item=' + encodeURIComponent(JSON.stringify(item))
				})
				return;
			},
			itemUserClick(item) {
				if(this.moreShow){
					return;
				}
				uni.navigateTo({
					url: '/pagesSub/developer/detail?login=' + encodeURIComponent(item.owner.login)
				})
				return;
			}
		}
	}
</script>

<style lang="scss" scoped>
	$item-height:80px;

	.flex {
		display: flex;
	}

	.item-box {
		min-height: $item-height;
		padding: 5px;
		background-color: #fff;
		border: 2px solid #ebeef5;
		border-radius: 5px;
		margin: 10px;
		box-shadow: rgba(0, 0, 0, 0.08) 0px 0px 3px 1px;
		.item1 {
			view:nth-of-type(1) {
				flex: 0 0 50px;
			}
		}

		.item2 {
			color: #00557f;
			margin: 5px;
			justify-content: space-between;

			>view {
				flex: 1;

				&:nth-of-type(1) {
					flex: 0 0 80px; // 第一个固定其余自动分配
				}
			}

		}
	}

	.text-ellipsis-lines {
		-webkit-line-clamp: 2;
	}

	.font-sm {
		font-size: 0.6rem;
	}
</style>
