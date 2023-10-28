import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;
export const getSorts = () => {
  return axios.request({
    url: 'api/WikiSort/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}
//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/WikiSort/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/WikiSort/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/WikiSort/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}