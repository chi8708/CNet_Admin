
<template>
	<view class="big_out">
		<!-- 左上角返回按钮 -->
		<!-- 这里为什么要加4px，是因为这个左箭头<的高度为16rpx需要下降一半，就是8rpx=4px,可以自己试一下，我这里没有问题 -->
		<view class="back"  :style="'margin-top:'+ (titletop+4)+'px'"  @click="back_page"> 
			<view class="back_img">
			</view>
			<view class="back_text">
				<slot>
					{{backtext}}
				</slot>
			</view>
		</view>
	</view>
</template>
 
<script>
	export default {
		name: "backPages",
		props: {
			backtext: String,
		},
 
		data() {
			return {
				titleheight: 0,
				titletop: 0
			};
		},
        //这里原来使用的onLoad一直有bug（不重新进入页面就会错位），
        // 后来使用onShow  更改于2022/09/30  以后再修改
		onShow() {
			//加载时调用 getHeight
			this.getHeight();
		},
		methods: {
			//利用胶囊按钮定位宽高
			getHeight() {
				let res = uni.getMenuButtonBoundingClientRect();
				this.titletop = res.top;
				this.titleheight = res.height
			},
 
			//直接返回上一级
			back_page() {
				uni.navigateBack({
					delta: 1 // 返回的页面数
				})
			},
		}
 
	}
</script>
 
<style scoped>
	.big_out {
		position: fixed;
	    z-index: 999;
		/* background-color: red; */
	}
 
	.back {
		position: absolute;
		height: 50rpx;
		width: 120rpx;
	}
 
	.back_img {
		/* 用border值来控制箭头粗细 */
		border: 3px solid black;
		/* 上、右、下、左  四个边框的宽度 */
		border-width: 0px 2px 2px 0px;
		display: inline-block;
		/* padding值控制箭头大小 */
		padding: 5px;
		transform: rotate(135deg);
		-webkit-transform: rotate(135deg);
		margin-left: 30rpx;
	}
 
	.back_text {
		float: right;
	}
</style>