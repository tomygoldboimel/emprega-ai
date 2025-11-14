import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Login from '../views/Login.vue'
import Curriculo from '../views/Curriculo.vue'
import usuarioService from '@/services/usuarioService'

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
router.beforeEach(async (to, from, next) => {
  if (to.meta.requiresAuth) {
    try {
      const autenticado = await usuarioService.verificarSessao()
      
      if (autenticado) {
        next()
      } else {
        next('/login')
      }
    } catch (error) {
      next('/login')
    }
  } else {
    next()
  }
})
export default router