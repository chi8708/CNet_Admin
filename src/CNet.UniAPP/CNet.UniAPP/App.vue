<script>
export default {
	onLaunch: function() {
		console.log('App Launch');

		setTimeout(() => {
			// uni.setTabBarBadge({
			// 	index: 1,
			// 	text: '31'
			// });
			// uni.showTabBarRedDot({
			// 	index: 3
			// });
		}, 1000);
	},
	onShow: function() {
		console.log('App Show');
	},
	onHide: function() {
		console.log('App Hide');
	}
};

</script>
<style>
	@import '@/common/uni.css';
</style>

<style lang="scss">
    /* 注意要写在第一行，同时给style标签加入lang="scss"属性 */
     @import "uview-ui/index.scss";
	 @import '@/common/globalClass.scss';
</style>