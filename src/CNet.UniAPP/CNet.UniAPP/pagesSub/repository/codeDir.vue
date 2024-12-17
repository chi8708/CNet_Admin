<template>
	<view>
		<view v-for="item in items" class="flex item" @click="fileClick(item)">
			<i class="iconfont uni-bold" :class="item.type=='file'?'icon-24gl-fileText':'icon-wenjianjia'"></i>
			<!-- 	<u-icon :name="item.type=='file'?'file-text':'email-fill'" class="flex-item"></u-icon> -->

			<label class="flex-item lb_break" style="flex:1;">{{item.path}}</label>

			<u-icon v-if="item.type=='dir'" name="arrow-right" class="flex-item" style=""></u-icon>
			<label v-else style="color:#bfbfbf;">{{item.size|changByte()}}</label>
		</view>

	</view>
</template>

<script>
	import {
		GetContentsDir
	} from "@/api/repository.js"
	import {
		isPic
	} from '@/common/util.js';
	export default {
		data() {
			return {
				content: "",
				items: []
			}
		},
		filters: {
			changByte: (size) =>
				size > 1024*1024 ? (size/(1024*1024)).toFixed(2)+'MB': (size>1024? (size / 1024).toFixed(2) + 'KB' : size + 'B'),
		},
		onLoad(e) {

			const contents_url = decodeURIComponent(e.contents_url);
			this.GetContentsDir(contents_url);
		},
		methods: {
			async GetContentsDir(contents_url) {
				uni.showLoading({
					title: "加载数据中....",
					mask: true
				})
				let res = await GetContentsDir(contents_url);
				if (res.code == 1) {
					this.items = res.data; //text.replace(reg, '')
				} else {
					uni.showToast({
						title: res.msg,
						icon: 'error'
					})
				}
				uni.hideLoading();
			},
			fileClick(i) {
				if (i.type == 'dir') {
					uni.navigateTo({
						url: `/pagesSub/repository/codeDir?contents_url=${encodeURIComponent(i.url)}`
					})
				} else {
					//获取最后一个.的位置
					let index = i.download_url.lastIndexOf(".");
					//获取后缀
					let ext = i.download_url.substr(index + 1);
					let docArr = ['doc', 'xls', 'ppt', 'pdf', 'docx', 'xlsx', 'pptx'];
					if (!i.download_url) return;
					if (!this.fileCheck(i)) return;

					if (docArr.some(p => p == ext)) {
						//this.openDocByUrl(i.download_url);//此方法不可用
						uni.showToast({
							title: "暂不支持此文件格式!",
							icon: "none"
						})
						return;
					}

					if (i.download_url)
					//var i.download_url
						uni.navigateTo({
							url: `/pagesSub/repository/codeView?download_url=${encodeURIComponent(i.url)}`
						})
				}

			},
			fileCheck(i) {
				if (isPic(i.download_url)) {
					uni.showToast({
						title: "暂不支持此文件格式!",
						icon: "none"
					})
					return false;
				}
				if (i.size > 1024 * 1024) {
					uni.showToast({
						title: "打开文件大小不能超1M!",
						icon: "none"
					})
					return false;
				}
				return true;
			},
			//通过url打开pdf文件
			openDocByUrl(docUrl) {
				// uni.navigateTo({url: "/pagesSub/webView/webView?contractUrl=" + docUrl,
				// });
				uni.showLoading({
					title: "下载中，请稍后...",
					mask: true,
				});
				uni.downloadFile({
					url: docUrl,
					success: function(res) {
						let filePath = res.tempFilePath;
						uni.openDocument({
							filePath: filePath,
							showMenu: true,
							success: function(res) {
								console.log('打开文档成功');
							}
						});
					},
					fail() {
						uni.showToast({
							title: "加载失败！",
							icon: "none"
						})
					},
					complete() {
						uni.hideLoading();
					}
				});
			}
		}
	}
</script>

<style scoped lang="scss">
	.item+.item {
		border-top: 1px solid #d0d0d0;
	}

	.item {
		margin-top: 2px;
		min-height: 25px;
		background-color: #fff;
		align-items: center;
		padding: 5px 10px;
	}

	.lb_break {
		padding: 5px;
		width: 75%;
		/*盒子宽自己设置想要的宽度*/
		height: auto;
		/*高度自动*/
		display: inline-block;
		/*转为行内块元素*/
		white-space: pre-wrap;
		/*处理元素内的空白,保留空白符序列，但是正常地进行换行*/
		word-wrap: break-word;
		/*允许长单词或 URL 地址换行到下一行,在长单词或 URL 地址内部进行换行*/

	}
</style>
