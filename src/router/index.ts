import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/auth.ts'; // Ensure this store exists

// Import Pages
import HomePage from '../pages/Home/HomePage.vue';
import LoginPage from '../pages/Login/LoginPage.vue';
import RegisterPage from '../pages/Register/RegisterPage.vue';
import NotesView from '../pages/Note/NotedPage.vue';

// Define Routes
const routes = [
  { path: '/', name: 'Home', component: HomePage },
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
router.beforeEach(async (to, _from, next) => {
  const authStore = useAuthStore();

  // If the user is authenticated and tries to access login or register, redirect to the notes page or another route
  if (authStore.isAuthenticated && (to.path === '/' || to.path === '/login' || to.path === '/register')) {
    next('/notes');  // Redirect to /notes (or another authenticated route)
  }
  // If the route requires authentication and the user is not authenticated, redirect to login
  else if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/');  // Redirect to login page if the user is not authenticated
  } 
  else {
    next();  // Otherwise, allow access to the requested route
  }
});

export default router;



