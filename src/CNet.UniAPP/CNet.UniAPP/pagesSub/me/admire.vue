<template>
	<view>
		<!-- <image src="../../static/img/zs111.jpg" style="width:100vw;" mode="widthFix" show-menu-by-longpress="true"  @click="previewImage" > -->
		</image>
	</view>
</template>

<script>
	export default {
		data() {
			return {

			}
		},
		methods: {
			previewImage(e) {
				uni.previewImage({
					// 需要预览的图片链接列表。若无需预览，可以注释urls
					urls: [e.target.src],
					// 为当前显示图片的链接/索引值
					current: e.target.src,
					// 图片指示器样式	
					indicator: 'default',
					// 是否可循环预览
					loop: false,
					// 长按图片显示操作菜单，如不填默认为保存相册
					longPressActions: {
						itemList: [this.l('发送给朋友'), this.l]
					},
					success: res => {
						console.log('previewImage res', res);
					},
					fail: err => {
						console.log('previewImage err', err);
					}
				});
			}
		}
	}
</script>

<style lang="scss" scoped>
	page {
		background-color: #f9f9f9;
	}
</style>
