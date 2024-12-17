import clickOutside from './clickOutside'
// 自定义指令
const directives = {
  clickOutside,
}

export default {
  install(Vue) {
    Object.keys(directives).forEach((key) => {
      Vue.directive(key, directives[key])
    })
  },
}