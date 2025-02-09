import { createRouter, createWebHistory } from 'vue-router';
import NotesView from '../pages/Note/NotedPage.vue';
import { useAuthStore } from '../stores/auth.ts'; // Ensure this store exists
import HomePage from '../pages/Home/HomePage.vue';
import BoxPage from '../pages/Home/box.vue';
import LoginPage from '../pages/Login/LoginPage.vue';
import RegisterPage from '../pages/Register/RegisterPage.vue';

const routes = [
  { path: '/', name: 'Home', component: HomePage },
  { path: '/box', name: 'Box', component: BoxPage },
  { path: '/login', name: 'Login', component: LoginPage },
  { path: '/register', name: 'Register', component: RegisterPage },
  { 
    path: '/notes', 
    name: 'Notes', 
    component: NotesView, 
    beforeEnter: (to, from, next) => {
      const authStore = useAuthStore()
      if (!authStore.user) {
        next('/login') // Redirect if not logged in
      } else {
        next()
      }
    }
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;


