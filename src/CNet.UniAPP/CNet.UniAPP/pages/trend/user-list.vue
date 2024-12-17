<template>
	<view class="content">
		<!--:fixed="false"默认为true 高度固定后，头底部会有空间， :auto="true" 默认为true mounted后自动调用reload方法，如果为false需要在mounted中手动调用-->
		<!-- auto-show-system-loading="true" 默认为false，是否显示loading正在加载 -->
		<z-paging ref="paging" v-model="dataList" @query="queryList" :fixed="false"  :auto="true"  :auto-show-system-loading="true" :default-page-size="10">
			<!--分页数据-->
			<view v-for="(item,index) in dataList">
				<list-item :item="item" :moreShow="false" :rank="index+1"></list-item>
			</view>
		</z-paging>
	</view>
</template>
<style lang="scss">
</style>

<script>
	import {
		getPageUsers
	} from '@/api/trend.js'
	import listItem from '@/pages/developer/list-item.vue'
	import {
		dateParse
	} from '@/common/date.js'
	export default {
		components: {
			listItem
		},
		data() {
			return {
				searchModel: {},
				dataList: []
			}
		},
		onLoad() {
			//document.addEventListener('click', this.bodyClick); //接听点击事件 小程序无效
			//this.queryList(1);
		},
		mounted(){
			//this.queryList(1);
		},
		methods: {
			queryList(pageNo = 1, pageSize) {

				let reqModel = this.queryListGetReq();
				reqModel.page = pageNo;
				getPageUsers(reqModel).then(res => {
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
			queryListGetReq() {
				let reqModel = {
					q:"",
				}
				reqModel.query =null;
				return reqModel;
			}


		},
		options: {
			styleIsolation: 'shared'
		}
	}
</script>
<style>
	/* 注意:父节点需要固定高度fixed:true，z-paging的height:100%才会生效 ,fixed:true 后顶部会留空间*/
	.content {
		height: 100%;
	}
	
</style>