<template>
	<view>
		<view class="container" v-if="isLogin">
			<form @submit="formSubmit">
				<view class="uni-form-item uni-column">
					<view class="title">昵称</view>
					<input class="uni-input" name="nickname" placeholder="输入昵称" v-model="userInfo.nickName" />
				</view>
				<view class="uni-form-item uni-column">
					<view class="title">电话</view>
					<text></text>
				</view>
				<view class="uni-form-item uni-column">
					<view class="title">邮箱</view>
					<input type="email"  class="uni-input" name="email" placeholder="输入邮箱" />
				</view>
				<view class="uni-btn-v">
					<button form-type="submit">Submit</button>
				</view>
			</form>
		</view>
	</view>
</template>

<script>
	//import auth from '@/common/auth.js'
	import {mapState, mapMutations} from 'vuex'
	export default {
		data() {
			return {
			}
		},
		onLoad() {
			//let userInfo=auth.login();
			this.login();
		},
		computed:{
			...mapState(['userInfo','isLogin'])
		},
		methods: {
			...mapMutations(['login']),
			formSubmit() {
				
			},
			Refresh(){
				
			}
		}
	}
</script>

<style>

</style>
