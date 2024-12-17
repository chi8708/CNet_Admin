<template>
	<view id="web-info">
		<web-view :src="src"></web-view>
	</view>
</template>
<script>
	export default {
		data() {
			return {
				src: '',
			}
		},
		onLoad(option) {
			this.src = option.contractUrl
		}
	}
</script>
