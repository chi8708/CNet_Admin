
<template>
	<div class="pdf">
		<pdf v-for="i in numPages" :key="i" :src="src" :page="i"></pdf>
	</div>
</template>
<script>
import pdf from 'vue-pdf'
export default {
	components: {
		pdf
	},
	data() {
		return {
			currentPage: 0, // pdf文件页码
			pageCount: 0, // pdf文件总页数
			src: '', // pdf路径
			numPages: 0
		}
	},
	mounted() {
		var url = 'http://localhost:10910/api/WikiMain/GetPdfFile?filePath=/FileUpload/wiki/1.pdf' // 文件地址
		this.src = pdf.createLoadingTask(url)
		this.src.promise.then((pdf) => {
			this.numPages = pdf.numPages
		})
	}
}
</script>
<style scoped>
body {
  overflow: auto;
  margin: 0;
  padding: 0;
}
</style>