import axios from '@/libs/api.request'
import store from '@/store'
var token=store.state.user.token;
//添加
export const uploadFile=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/UpLoadFile/SaveFile',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

export const mergeFile=(model)=>{
    const data=model;
    return axios.request({
      url: 'api/UpLoadFile/mergeFile',
      headers: {Authorization:"Bearer "+token},
      data,
      method: 'post'
    })
  }