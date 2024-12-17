<template>
	<view class="u-page" style="padding: 40px 0px;">
		<u-list>
			<u-cell :title="`${item}`" v-for="(item, index) in indexList" :key="index" @click="lanClick(item)" >
			</u-cell>
		</u-list>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				indexList: [
					'C',
					'C#',
					'C+',
					'CoffeeScript',
					'CSS',
					'Dart',
					'Document',
					'Elixir',
					'Go',
					'Groovy',
					'HTML',
					'Java',
					'JavaScript',
					'Kotlin',
					'Objective-C',
					'Perl',
					'PHP',
					'PowerShell',
					'Python',
					'Ruby',
					'Rust',
					'Scala',
					'Shell',
					'Swift',
					'TypeScript'
				]
			}
		},
		onLoad() {},
		methods: {
			lanClick(item) {
				this.$emit("onLangClick",item)
			}
		},
	}
</script>
