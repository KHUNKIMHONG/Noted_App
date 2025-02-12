import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/auth.ts'; // Ensure this store exists

// Import Pages
import HomePage from '../pages/Home/HomePage.vue';
import BoxPage from '../pages/Home/Box.vue';
import LoginPage from '../pages/Login/LoginPage.vue';
import RegisterPage from '../pages/Register/RegisterPage.vue';
import NotesView from '../pages/Note/NotedPage.vue';

// Define Routes
const routes = [
  { path: '/', name: 'Home', component: HomePage },
  { path: '/box', name: 'Box', component: BoxPage },
  { path: '/login', name: 'Login', component: LoginPage },
  { path: '/register', name: 'Register', component: RegisterPage },
  { 
    path: '/notes', 
    name: 'Notes', 
    component: NotesView, 
    meta: { requiresAuth: true } // Protect this route
  }
];

// Create Router
const router = createRouter({
  history: createWebHistory(),
  routes,
});

// 🔹 Global Navigation Guard for Authentication
router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore();
  
  if (to.meta.requiresAuth && !authStore.user) {
    next('/login'); // Redirect to login if not authenticated
  } else {
    next(); // Allow access
  }
});

export default router;



