//分页
import request from '@/common/request.js'
export const getPageRepos = (data) => {
  return request.get('api/hubtrend/repos',data)
 
}
export const getPageUsers = (data) => {
  return request.get('api/hubtrend/users',data)
 
}