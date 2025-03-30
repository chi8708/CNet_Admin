
//////此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-30 23:43:16
import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;

//获取所有
export const getRoles = () => {
  return axios.request({
    url: 'api/testgen/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//分页
export const getPage = ({pageNum ,  pageSize ,  field ,  order,query={}  }) => {
  const data = {
    pageNum:pageNum,
    pageSize:pageSize,
    field:field,
    order:order,
    query:query
  }
  return axios.request({
    url: 'api/testgen/GetPage',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
 
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/testgen/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/testgen/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/testgen/Remove/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}