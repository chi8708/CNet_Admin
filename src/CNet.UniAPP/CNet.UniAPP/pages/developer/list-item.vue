<template>
	<view class="item-box " @click="itemUserClick(item)">
		<view class="item1 flex">
			<view  style="position: relative;height:45px;">
				<view class="dot" style='position: absolute;top:0px;z-index: 111;'>{{rankNum}}</view>
				<view style='display:block; position: absolute;top:5px;left: 10px;'>
					<u-image  class="author-img" width="40px" height="40px" :src="item.avatar_url" shape="circle">
					</u-image>
				</view>

			</view>
			<!-- class="v-center" -->
			<view style="vertical-align: middle;line-height:45px; margin-left: 10px;">
				<h6 class="text-ellipsis-lines text-title">{{item.login}}</h6>
				<!-- <span class="text-ellipsis-lines text-desc">{{item.login}} {{item.bio}}</span> -->
			</view>

		</view>
	<!-- 	<view class="item2 flex font-sm">
			<view>
						<label class="font-sm">{{item.location||'未知地区'}}</label>
			</view>
			<view class="flex">
				<i class="iconfont icon-wode1 font-sm"><label>{{item.followers}}</label></i>
			</view>
			<view style="text-align: right;">
				<span style="color: #000;">{{item.created_at?dateParse(item.created_at):''}}</span>
			</view>
		</view> -->


	</view>
</template>

<script>
	import {
		dateParse
	} from '@/common/date.js'
	export default {
		props: {
			item: {},
			moreShow:false,
			rank:0
		},
		onLoad() {
			
		},
		mounted(){
			//console.log(this.rank);
			this.rankNum=this.rank;/*不使用data rank将双向绑定*/
		},
		data() {
			return {
				rankNum:1
			}
		},
		methods: {
			dateParse(data){
			   return dateParse( new Date(data), "yyyy-MM-dd");
			},
			itemUserClick(item) {
				if(this.moreShow){
					return;
				}
				uni.navigateTo({
					url: '/pagesSub/developer/detail?login=' + encodeURIComponent(item.login)
				})
				return;
			}
		}
	}
</script>

<style lang="scss" scoped>
	$item-height:60px;
	.flex {
		display: flex;
	}

	.item-box {
		min-height: $item-height;
		align-items: center;
		padding: 5px;
		background-color: #fff;
		border: 2px solid #ebeef5;
		border-radius: 5px;
		margin: 10px;
		 padding-top: 15px;
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
			vertical-align: middle;
			>view {
				flex: 1;

				&:nth-of-type(1) {
					flex: 0 0 120px; // 第一个固定其余自动分配
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
