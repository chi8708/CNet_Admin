import request from '@/common/request.js'
export const WX_loginByCode = (req) => {

	return request.post(`api/WX_Login`, req,{}, true)
}

export const GetModelByWXCode = (wxCode) => {

	return request.post(`api/Cu_User/GetModelByWXCode/${wxCode}`, null,{}, true)
}

