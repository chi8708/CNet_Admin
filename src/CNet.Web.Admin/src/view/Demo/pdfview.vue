<template>
	<vue-office-pdf :src="pdf" @rendered="renderedHandler" @error="errorHandler" />
</template>

<script>
//引入VueOfficePdf组件
import VueOfficePdf from '@vue-office/pdf'

export default {
	components: {
		VueOfficePdf
	},
	data() {
		return {
			//pdf: 'http://static.shanhuxueyuan.com/test.pdf' //设置文档地址
			pdf: this.$route.query.filePath
		}
	},
	methods: {
		renderedHandler() {
			console.log("渲染完成")
		},
		errorHandler() {
			console.log("渲染失败")
		}
	}
}
</script>
<style>
/* 只显示一页BUG处理。 因为在APP.vue中做了全局处理 */
#app {
	overflow: auto ;
}

body {
	overflow: auto;
	margin: 0;
	padding: 0;
}
</style>