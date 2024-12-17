import {
	GetModelByWXCode
} from "@/api/customer.js"
export default {
	redirectUrl:'',
	userInfo:'',
	async getUserInfo(){
		let userInfo = uni.getStorageSync('userInfo');
		if(!userInfo){
			return null;
		}
		else{
			let res=await GetModelByWXCode(userInfo.WX_MiniOpenId);
			if(res.code==1){
				uni.setStorageSync('isLogin', true);
				uni.setStorageSync('userInfo', WX_loginRes.data);
				this.userInfo=WX_loginRes.data;
				return this.userInfo;
			}
			return null;
		}
	},
    async writeUserInfo(){
       let userInfo=this.getUserInfo();
	   if(!userInfo){
		   this.gotoLoginPage();
	   }
	   return userInfo;
	},
	async login() {
		let userInfo = uni.getStorageSync('userInfo');
		if (userInfo){
			await this.writeUserInfo();
			return this.userInfo;
		}
		else{
			this.gotoLoginPage();
		}
	},
	gotoLoginPage(){
		uni.redirectTo({
			url:`/pagesSub/login/login`,
			//redirectUrl=${encodeURI(this.redirectUrl)}
		});
	}
	
}
