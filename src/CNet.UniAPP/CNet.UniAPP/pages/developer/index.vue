<template>
	<view class="content" @click="bodyClick($event)">

		<view class="paging-box">
			<z-paging ref="paging" v-model="dataList" @query="queryList" :auto-show-system-loading="true">
				<view class="top-box" slot="top">
					<!--关键字-->
					<view class="keyword-box">
						<u-search shape="square" placeholder="请输入关键字" bgColor="#fff" v-model="keyword" :showAction="false"
							@search="search"></u-search>
					</view>
					<!--排序和筛选-->

					<view class="uni-flex uni-row search-box">
						<view class="flex-item flex-item-V" @click.stop="sortShow=!sortShow">
							<text class="uni-bold">排序</text> <text>{{sorts[0][sortIndex].text}}</text>
							<!-- <picker type="default" :value="sortIndex" @change="sortChange" range-key="text"
								:range="sorts">
								<text>{{sorts[sortIndex].text}}</text>
							</picker> -->
							<view>
								<u-picker :show="sortShow" :columns="sorts" ref="uPicker" keyName="text"
									@confirm="sortChange" @cancel="sortShow=false">
								</u-picker>
							</view>


						</view>

						<view class="flex-item flex-item-V" :class="{moreActive:moreShow}" @click.stop="moreOpenClick">
							<i class="iconfont icon-filter uni-bold"> 筛选</i>
						</view>

					</view>

					<!-- @click.native.stop="moreFormClick" 阻止事件冒泡 -->
					<view style=" position:relative;" @click.native.stop="moreFormClick" v-if="moreShow" ref="more">
						<view style="position: absolute;top:0;background-color: #ecf5ff;z-index: 999;width:100%;">
							<form class="search-form"  @reset="moreReset" ref="form1">
								<view class="uni-form-item uni-column">
									<view class="title">followers数[K]</view>
									<view class="uni-flex">
										<u-number-box v-model="searchModel.followersFrom" min="0" border="none"
											class='input-num'>
										</u-number-box>
										<text style="margin:0px 20px;">至</text>
										<u-number-box v-model="searchModel.followersTo" min="0" border="none"
											class='input-num'>
										</u-number-box>
									</view>
								</view>
								<view class="uni-form-item uni-row" @click="selectCity">
									<view class="title" style="margin-top: -2px;">地区</view>
									
									<view style="width:220px;">
										<label>{{searchModel.SES_location}}</label>
									<!-- 	<u--input v-model="searchModel.SES_location" border="none"></u--input> -->
									</view>
									<u-icon disabled name="arrow-right">
									</u-icon>

								</view>
								<view class="uni-form-item uni-row" @click="languageShow = true;">
									<view class="title">语言</view>
									<view style="width:220px;">
										<u--input v-model="searchModel.SES_language" border="none"></u--input>
									</view>
									<u-icon disabled name="arrow-right">
									</u-icon>

								</view>
								<view class="uni-form-item">

									<button type="default" size="mini" form-type="reset"
										style="width: 40%;height:40px;line-height:40px ;">重置</button>
									<button type="primary" size="mini" id="btn-search" ref="btnSearch" form-type="submit"
										style="width: 40%;height:40px;line-height:40px ;" @click.stop="moreSubmit">搜索</button>
								</view>

							</form>
							
							
							<city ref="city" @onClick="cityClick"></city>
							<u-popup :show="languageShow" :round="10" mode="right" @close="languageShow = false;">
								<view style="min-width:200px;">
									<lang @onLangClick="languageClick"></lang>
								</view>
							</u-popup>
						</view>

					</view>
				</view>
				<!--分页数据-->
				<view v-for="(item,index) in dataList">
					<list-item :item="item" :moreShow="moreShow" :rank="index+1"></list-item>
				</view>
			</z-paging>
		</view>
	</view>
</template>
<style lang="scss">
	.moreActive {
		$border-style: 1px solid #ecf5ff !important;
		border-bottom: $border-style;
		border-left: 1px solid #d0d0d0;
	}

	@mixin border-split($boderColor:#d0d0d0) {
		border-bottom: 1px solid $boderColor;

	}

	.keyword-box-style {
		padding: 10px;
		@include border-split;
	}

	.top-box {
		background-color: #ecf5ff;

		//@extend .keyword-box;//.top-box 会继承keyword-box 的属性 多出来padding: 10px;
		.keyword-box {
			@extend .keyword-box-style;
		}

		.search-box {
			height: 80rpx;
			//@include border-split;
			line-height: 80rpx;

			.flex-item {
				flex: 1;
				display: flex;
				justify-content: center;
				@include border-split;
			}
		}
	}

	.search-form {
		padding: 5px;
		display: flex;
		justify-content: center;

		.title {
			padding-left: 0px;
			font-size: 0.8rem;
		}
	}

</style>

<script>
	import {
		getPage
	} from '@/api/developer.js'
	import {
		dateParse
	} from '@/common/date.js'
	import listItem from './list-item.vue'
	import lang from '@/pages/repository/language.vue'
	import city from '@/components/city.vue'

	export default {
		components: {
			listItem,
			lang,
			city
		},
		//重设按钮分享内容
		onShareAppMessage(res) {
			return {
				title: 'github热门排行,手机端看源码',
				content: 'github仓库开发者排行，github中文',
				path: getCurrentPages()[0].$page.fullPath,
				//imageUrl: '/static/index.jpg'
			}
		},
		data() {
			return {
				searchModel: {SES_language:"",SES_location:""},
				isSubmit:false,
				dataList: [],
				keyword: '',
				moreShow: false,
				languageShow: false,
				sortShow: false,
				sortIndex: 0,
				sortSelected: '',
				sorts: [
					[{
							"value": "",
							"text": "默认"
						},
						{
							"value": "followers",
							"text": "follower最多"
						},
						{
							"value": "repositories",
							"text": "仓库最多"
						},
						{
							"value": "joined",
							"text": "joined最多"
						},

					]
				]
			}
		},
		onLoad() {
			//document.addEventListener('click', this.bodyClick); //接听点击事件 小程序无效
			//this.queryList(1);
			//uni.showShareMenu();
		},
		methods: {
			updateTimeClick(name) {
				this.updateTimes.map((item, index) => {
					item.checked = index === name ? true : false
					if (item.checked) {
						this.searchModel.S_updateTime = item.value;
					}

				})
			},
			sortChange(e) {
				this.sortIndex = e.indexs[0];
				this.sortSelected = this.sorts[0][this.sortIndex];
				this.sortShow = false;
				this.search();
			},
			moreFormClick(e) {
				this.moreShow =true;
			},
			bodyClick(e) {
				this.moreShow = false;
			},
			moreOpenClick() {
				this.moreShow = !this.moreShow;
			},
			moreSubmit(e) {
				this.moreShow = false;
				this.search();
			},
			moreReset(e) {
				//this.$refs.form1.resetFields();
				this.searchModel = {};
				this.updateTimes.map(i => i.checked = false);
				this.updateTimes[0].checked = true;
			},
			selectCity(){
				this.$refs.city.open();
			},
			languageClick(value) {
				this.searchModel.SES_language = value;
				this.languageShow = false;
			},
			cityClick(value){
				this.searchModel.SES_location=value;
				//this.$refs.city.close();
			},
			search() {
				this.$refs.paging.reload();
			},
			async queryList(pageNo = 1, pageSize) {
				let reqModel = await this.queryListGetReq();
				reqModel.page = pageNo;
				await getPage(reqModel).then(res => {
					//请勿在网络请求回调中给dataList赋值！！只需要调用complete就可以了
					if (res.code == 1) {
						//this.dataList = res.data.items;
						this.$refs.paging.complete(res.data.items);
					} else {
						uni.showToast({
							title: '请求失败:' + res.msg,
							icon: 'fail'
						});
					}

				}).catch(res => {
					//如果请求失败写this.$refs.paging.complete(false)，会自动展示错误页面
					//注意，每次都需要在catch中写这句话很麻烦，z-paging提供了方案可以全局统一处理
					//在底层的网络请求抛出异常时，写uni.$emit('z-paging-error-emit');即可
					//this.$refs.paging.complete(false);
				})
			},
			async queryListGetReq() {
				
				let reqModel = {
					q: this.keyword,
					order: !this.sortSelected ? null : 'desc',
					sort: !this.sortSelected ? null : `${this.sortSelected.value}`,
					query: this.searchModel
				}
				if (reqModel.query.followersFrom) {
					reqModel.query.SEGT_followers = reqModel.query.followersFrom * 1000;
				}
				if (reqModel.query.followersTo) {
					reqModel.query.SELT_followers = reqModel.query.followersTo * 1000;
				}
				reqModel.SES_location=reqModel.SES_location||"";
				
				if (reqModel.query) {
					reqModel.query = JSON.stringify(reqModel.query);
				}

				return reqModel;
			}


		},
		options: {
			styleIsolation: 'shared'
		}
	}
</script>
