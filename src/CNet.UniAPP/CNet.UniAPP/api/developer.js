//分页
import request from '@/common/request.js'
export const getPage = (data) => {
  return request.get('api/hubUser',data)
 
}
export const getModel = (code) => {
  return request.get(`api/hubUser/${code}`,{},{},true)
}
export const getRepos = (code) => {
  return request.get(`api/hubUser/${code}/repos`,{},{},true)
}