import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Login from '../views/Login.vue'
import Cadastro from '../views/Cadastro.vue'
import Curriculo from '../views/Curriculo.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/cadastro',
    name: 'Cadastro',
    component: Cadastro
  },
  {
    path: '/curriculo',
    name: 'Curriculo',
    component: Curriculo
  },
  {
    path: '/curriculo/visualizar/:id',
    name: 'CurriculoVisualizar',
    component: () => import('@/views/CurriculoVisualizar.vue')
  },
  {
    path: '/curriculo/editar/:id',
    name: 'CurriculoEditar',
    component: () => import('@/views/Curriculo.vue')
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

export default router