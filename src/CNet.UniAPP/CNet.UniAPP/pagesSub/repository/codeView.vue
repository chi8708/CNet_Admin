<template>
	<view>
		<view>
			<u-parse :content="content" :tagStyle="parse_style" selectable></u-parse>
		</view>
	</view>
</template>

<script>
	import {
		GetCode,GetCodeV2
	} from "@/api/repository.js"
	const {
		marked
	} = require('marked');
	import hljs from "highlight.js";
	import "highlight.js/scss/atom-one-dark.scss";
	import ResetStyle from "@/common/ResetStyle.js"
	export default {
		data() {
			return {
				content: "",
				parse_style: ResetStyle.uParseStyle,
			}
		},
		onLoad(e) {

			const download_url = decodeURIComponent(e.download_url);
			this.initHighLight();
			this.GetCode(download_url);
		},
		methods: {
			async GetCode(download_url) {
				uni.showLoading({
					title: "加载数据中....",
					mask: true
				})
				let res = await GetCodeV2(download_url);
				if (res.code == 1) {
					let resData=res.data;
					if(download_url.indexOf('.md')<0)
					{
						resData="```"+ res.data; //text.replace(reg, '')
					}
					this.content =marked(resData);
				} else {
					uni.showToast({
						title: res.msg,
						icon: 'error'
					})
				}
				uni.hideLoading();
			},
			initHighLight() {
				hljs.configure({
					useBR: true,
					tabReplace: " ",
				});
				marked.setOptions({
					renderer: new marked.Renderer(),
					gfm: true,
					tables: true,
					breaks: false,
					pedantic: false,
					highlight: function(code, lang) {
						if (lang && hljs.getLanguage(lang)) {
							return hljs.highlight(lang, code, true).value;
						} else {
							return hljs.highlightAuto(code).value;
						}
					},
				});
			}
		},
	}
</script>

<style scoped lang="scss">


</style>
