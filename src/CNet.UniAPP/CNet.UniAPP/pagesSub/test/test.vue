<template>
	<view>
      <u-swipe-action>
        <u-swipe-action-item
          :options="options1"
        >
          <view class="swipe-action u-border-top u-border-bottom">
            <view class="swipe-action__content">
              <text class="swipe-action__content__text">基础使用</text>
            </view>
          </view>
        </u-swipe-action-item>
      </u-swipe-action>
	</view>
</template>

<script>
	export default {
		data() {
			return {
                options1: [{
                    text: '删除'
                }]
			};
		},
	};
</script>

<style lang="scss">
    .u-page {
        padding: 0;
    }

    .u-demo-block__title {
        padding: 10px 0 2px 15px;
    }

    .swipe-action {
        &__content {
             padding: 25rpx 0;
    
            &__text {
                 font-size: 15px;
                 color: $u-main-color;
                 padding-left: 30rpx;
             }
        }
    }
</style>