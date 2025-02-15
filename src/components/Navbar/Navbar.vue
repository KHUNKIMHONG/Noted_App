<template>
    <nav class="bg-gradient-to-r from-blue-700 via-teal-500 to-blue-700 text-white p-4 fixed w-full top-0 z-10">
        <div class="container mx-auto flex justify-between items-center">
            <h1 class="text-xl font-bold hover:text-blue-400">Noted Application</h1>

            <!-- Check if user is logged in -->
            <div v-if="isAuthenticated" class="flex items-center gap-4">
                <span class="font-medium hover:text-blue-400">👤 {{ user?.name }}</span>
                <button @click="logout"
                    class="px-4 py-2 bg-red-500 text-white border border-teal-500 shadow-sm shadow-orange-400 rounded hover:bg-red-600">
                    Logout
                </button>
            </div>

            <!-- Show Login and Register when NOT logged in -->
            <div v-else class="flex items-center space-x-4">
                <router-link to="/login"
                    class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-300">
                    Login
                </router-link>
                <router-link to="/register"
                    class="px-4 py-2 bg-white text-blue-600 rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-300">
                    Register
                </router-link>
            </div>
        </div>
    </nav>
</template>

<script lang="ts" setup>
import { computed } from "vue";
import { useAuthStore } from "../../stores/auth";

const authStore = useAuthStore();

// Computed properties for authentication state
const isAuthenticated = computed(() => authStore.isAuthenticated);
const user = computed(() => authStore.user);

// Logout function
const logout = () => {
    authStore.logout();
};
</script>
