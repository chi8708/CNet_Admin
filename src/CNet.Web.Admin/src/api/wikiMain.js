import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;

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
    url: 'api/wikiMain/GetPage',
   // headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/WikiMain/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/WikiMain/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/WikiMain/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//导出word
export const getDocxFile=(path)=>{
  return axios.request({
    url: 'api/WikiMain/GetDocxFile/'+path,
    method: 'post',
    data,
    responseType: 'blob'
  })
}
