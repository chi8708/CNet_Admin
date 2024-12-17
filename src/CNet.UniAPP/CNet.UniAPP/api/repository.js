//分页
import request from '@/common/request.js'
export const getPage = (data) => {
  return request.get('api/hubrepository',data)
 
}
//实体
export const getModel = (code) => {
  return request.get(`api/hubrepository/GetModel/`,{full_name:code},{},true);
}
//readMe
export const getReadMe = ( full_name, default_branch) => {
  return request.get(`api/hubrepository/getReadMe/`,{full_name, default_branch});
 
}
//代码浏览目录
export const GetContentsDir = (contents_url) => {
  return request.get(`api/hubrepository/GetContentsDir/`,{contents_url});
 
}
//代码浏览
export const GetCode = (download_url) => {
  return request.get(`api/hubrepository/GetCode/`,{download_url});
 
}

//代码浏览V2
export const GetCodeV2 = (download_url) => {
  return request.get(`api/hubrepository/GetCodeV2/`,{download_url});
 
}