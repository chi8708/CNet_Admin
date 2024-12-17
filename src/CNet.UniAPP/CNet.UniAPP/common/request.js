import config from "@/config/index.js"
export default {
	common: {
		header: {
			"Content-Type": "application/x-www-form-urlencoded;charset=utf-8"
		},
		method: 'GET',
		needToken:true,
		data: {}
	},
	request(options = {}, isloading = false) {
		// options.url =  options.url
		// options.method = options.method || this.common.method
		// options.data = options.data || {}
		// options.header = options.header || this.common.header
		// console.log('options.data', options.data)
		options = {
			...this.common,
			...options
		}
		return new Promise((resolve, reject) => {
			
			
			if (isloading) {
				uni.showLoading({
					title: '加载中...',
					mask: true
				});
			}

			uni.request({
				...options,
				success: (result) => {
					// 请求失败
					if (result.statusCode !== 200) {
						uni.showToast({
							title: result.data.msg || '请求失败',
							icon: 'none'
						});
						return reject(result.data)
					}
					// 请求成功
					return resolve(result.data)
				},
				fail: (error) => {
					if (options.data.page) {
						uni.$emit('z-paging-error-emit');
					} else {
						uni.showToast({
							title: '请求失败',
							icon: 'none'
						});
					}


					return reject(error)
				},
				complete() {
					if (isloading) {
						uni.hideLoading();
					}
				}
			})
		})

	},
	get(url, data = {}, options = {}, isloading = false) {
		options.url = `${config.api.BaseUrl}${url}`;
		options.data = data
		options.method = 'GET'
		return this.request(options, isloading)
	},
	post(url, data = {}, options = {}, isloading = false, isjson = true) {
		options.url = config.api.BaseUrl + url
		options.data = data
		options.method = 'POST'
		if (isjson) {
			options.header = {
				"Content-Type": "application/json;charset=utf-8"
			};
		}
		console.log('下边的参数options.data', data)
		return this.request(options)
	},
	upload(url, options = {}, isloading = false) {
		options.url = config.api.BaseUrl + url
		options.filePath = options.filePath || ''
		options.header = options.header || {}
		options.name = options.name || 'userpic'


		return new Promise((resolve, reject) => {
			if (isloading) {
				uni.showLoading({
					title: '加载中...'
				});
			}
			uni.uploadFile({
				...options,
				success: (uploadFileRes) => {
					if (uploadFileRes.statusCode !== 200) {
						return uni.showToast({
							title: '上传失败',
							icon: 'none'
						});
					}
					return resolve(JSON.parse(uploadFileRes.data))
				},
				complete() {
					if (isloading) {
						uni.hideLoading();
					}
				}
			});
		})

	}

}
