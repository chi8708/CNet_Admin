import { createRouter, createWebHistory } from 'vue-router'
import Home from './components/Home.vue'
import CodeGenerator from './views/CodeGenerator.vue'
// =import CodeGenerator from './components/CodeGenerator.vue'
const routes = [
  {
    path: '/',
    name: 'Home',
    redirect: '/code-generator',
    component: Home
  },
  {
    path: '/code-generator',
    name: 'CodeGenerator',
    component: CodeGenerator
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router 