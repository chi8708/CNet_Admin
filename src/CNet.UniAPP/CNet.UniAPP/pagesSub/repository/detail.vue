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
					<view class="item-content-info1_1">
						<view @click="itemUserClick">
							<u-image class="author-img flex-item-WVcenter" width="40px" height="40px"
								:src="item.owner.avatar_url" shape="circle">
							</u-image>
						</view>
						<view>
							<h4 class="text-title">{{item.full_name}}</h4>
							<small class="text-desc">{{item.description}}</small>

							<view class="flex">
								<i class="iconfont icon-star font-sm"><label>{{item.stargazers_count}}</label></i>
								<i class="iconfont icon-codefork font-sm"><label>{{item.forks_count}}</label></i>
							</view>
						</view>

					</view>
					<view class="flex">
						<view class="flex-item-flex_1 flex-item-WVcenter" style="" @click="viewCodeDir">
							<u-icon name="eye" label="代码" color="#3c9cff" labelColor="#3c9cff" >
							</u-icon>
						</view>
						<view class="flex-item-flex_1 flex-item-WVcenter" style="" @click="saveStar()">
							<u-icon :name="isStared?'star-fill':'star'" label="收藏" :color="isStared?'#ff0000':'#3c9cff'"
								labelColor="#3c9cff">
							</u-icon>
						</view>

						<view class="flex-item-flex_1 flex-item-WVcenter">
							<button open-type="share" class="share">
								<u-icon name="share" label="分享" color="#3c9cff" labelColor="#3c9cff"></u-icon>
							</button>
						</view>
					</view>
				</view>
				<view class="item-content-info2">
					<u-cell-group title="仓库详细">
						<u-cell title="语言" :value="item.language||'未知语言'"></u-cell>
						<u-cell title="Watch" :value="item.watchers_count"></u-cell>
						<u-cell title="创建日期" :value="item.created_at"></u-cell>
						<u-cell title="提交日期" :value="item.pushed_at"></u-cell>
						<u-cell title="更新日期" :value="item.updated_at"></u-cell>
					</u-cell-group>
				</view>
				<ad unit-id="adunit-3cc2a087e503d723" ad-type="video" ad-theme="white" bindload="adLoad" binderror="adError" bindclose="adClose"></ad>
			</view>

			<view v-show="checkIndex==1">
				<u-parse :content="content" :tagStyle="parse_style" selectable></u-parse>
			</view>
			
		</view>
	</view>
</template>

<script>
	const {
		marked
	} = require('marked');
	import hljs from "highlight.js";
	import "highlight.js/scss/atom-one-dark.scss";
	import ResetStyle from "@/common/ResetStyle.js"
	import {
		getModel,
		getReadMe
	} from "@/api/repository.js"
	export default {
		data() {
			return {
				item: {},
				cu_starsRepo: [],
				isStared: false,
				content: '',
				parse_style: ResetStyle.uParseStyle,
				checkIndex: 0,
				readMeLoaded: false,
				tabItems: [{
						name: "info-circle",
						label: "摘要"
					},
					{
						name: "file-text",
						label: "介绍"
					},
				]
			}
		},
		onLoad(e) {
			//markdown 高亮参考https://www.xiaowo6.cn/uniapp/138.html

			if (e.full_name) {
				this.showDetail(e.full_name);
			} else {
				this.item = JSON.parse(decodeURIComponent(e.item));
				this.getStars();
				this.saveViewHistory();
			}
			this.initHighLight();

		},
		methods: {
			tabItemClick(i) {
				this.checkIndex = i;
				if (i == 1) {
					this.getReadMe();
				}
			},
			itemUserClick(item) {
				if (this.moreShow) {
					return;
				}
				uni.navigateTo({
					url: '/pagesSub/developer/detail?login=' + encodeURIComponent(this.item.owner.login)
				})
				return;
			},
			async showDetail(code) {
				let res = await getModel(code);
				if (res.code == 1) {
					this.item = res.data;
					this.getStars();
					this.saveViewHistory();
				}

			},
			async getReadMe() {
				if (this.readMeLoaded) {
					return;
				}
				uni.showLoading({
					title: "加载数据中....",
					mask: true

				})
				let res = await getReadMe(this.item.full_name, this.item.default_branch);
				if (res.code == 1) {
					let text = marked(res.data || "");
					let reg = /< img [^>]*src=['"]([^'"]+)[^>]*>/gi
					this.content = text; //text.replace(reg, '')
				}
				this.readMeLoaded = true;
				uni.hideLoading();
			},
			//代码浏览
			viewCodeDir() {
				uni.navigateTo({
					url: `/pagesSub/repository/codeDir?contents_url=${encodeURIComponent(this.item.contents_url)}`
				})
			},
			getStars() {
				let that = this;
				uni.getStorage({
					key: 'cu_starsRepo',
					success(res) {
						that.cu_starsRepo = res.data;
						that.isStared = that.cu_starsRepo.some(p => p.full_name == that.item.full_name);
					}
				});

			},
			//保存收藏
			saveStar() {
				this.isStared = !this.isStared;
				this.cu_starsRepo = this.cu_starsRepo.filter(p => p.full_name != this.item.full_name); //删除
				if (this.isStared) {
					this.cu_starsRepo.unshift({
						full_name: this.item.full_name,
						createTime: new Date()
					});
					//this.cu_starsRepo.push({full_name:this.item.full_name,createTime:new Date()});
				}
				if (this.cu_starsRepo.length > 500) {
					this.cu_starsRepo.pop();
				}
				uni.setStorageSync('cu_starsRepo', this.cu_starsRepo);
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
								if (p.name == that.item.full_name && p.type == "repo") {
									viewHistory.splice(index, 1); //删除
									throw new Error('LoopInterrupt'); //跳出循环
								}
							});
						} catch (e) {

						}
						if(viewHistory.length>100){
							viewHistory.splice(viewHistory.length-1, 1);
						}
						//插入,
						viewHistory.unshift({
							name: that.item.full_name,
							createTime: new Date(),
							type: "repo"
						});
						uni.setStorage({
							key: 'cu_viewHistory',
							data: viewHistory
						});
					}
				});
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
		}
	}
</script>
<!-- 背景色对应页面中的style样式表中设置，且不能有scoped属性 -->
<style>
	page {
		background-color: #f4f6f8;
	}

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
</style>
<style scoped lang="scss">
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
				padding: 10px 0px;
				border-bottom: 1px solid #e0e0e0;

				>view:nth-child(1) {
					padding: 10px;
				}
			}

			>view:nth-of-type(2) {
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

	//中割线
	.flex-item-flex_1+.flex-item-flex_1 {
		border-left: 1px solid #e0e0e0;
	}
</style>
