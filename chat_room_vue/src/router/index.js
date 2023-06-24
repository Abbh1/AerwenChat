import {
  createWebHistory,
  createRouter
} from 'vue-router'

// 公共路由
export const constantRoutes = [{
    path: '/login',
    component: () => import('../views/Login.vue')
  },
  {
    path: '/',
    component: () => import('../views/Card.vue')
  },
  {
    path: '/load',
    component: () => import('../views/Loader.vue')
  },
  {
    path: '/chat',
    component: () => import('../views/chat/Chat.vue')
  },
  // {
  //   path: '/main',
  //   component: () => import('../views/Main.vue'),
  //   children: [{
  //       path: '/main/page1',
  //       component: () => import('../views/Page1.vue')
  //     },
  //     {
  //       path: '/main/page2',
  //       component: () => import('../views/Page2.vue')
  //     },
  //   ]
  // }
];

const router = createRouter({
  history: createWebHistory(),
  routes: constantRoutes
})

export default router;