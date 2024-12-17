<!-- 滑动切换选项卡演示(标准写法) -->
<template>
	<!-- 使用z-paging-swiper为根节点可以免计算高度 -->
	<z-paging-swiper>
		<!-- 需要固定在顶部不滚动的view放在slot="top"的view中 -->
		<!-- 注意！此处的z-tabs为独立的组件，可替换为第三方的tabs，若需要使用z-tabs，请在插件市场搜索z-tabs并引入，否则会报插件找不到的错误 -->
		<template #top>
			<u-tabs :list="tabList" @click="tabsChange" :current=current :scrollable="false"></u-tabs>
		</template>
		<!-- swiper必须设置height:100%，因为swiper有默认的高度，只有设置高度100%才可以铺满页面  -->

		<repo-list v-if="current==0"></repo-list>
		<user-list v-if="current==1"></user-list>
	</z-paging-swiper>
	
</template>

<script>
	import repoList from './repo-list.vue';
	import userList from './user-list.vue';
	export default {
		data() {
			return {
				tabList: [{name: '仓库'}, {name: '开发者'}],
				current: 0, // tabs组件的current值，表示当前活动的tab选项
			};
		},
		components: {
			repoList,
			userList
		},
		methods: {
			//tabs通知swiper切换
			tabsChange(e) {
				this.current = e.index;
			}
		}
	}
</script>

<style>
	.swiper {
		height: 100%;
	}
</style>
