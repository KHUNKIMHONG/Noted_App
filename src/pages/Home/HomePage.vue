<template>
    <div>
        <Navbar />
        <div class="bg-gradient-to-r from-blue-700 via-teal-500 to-blue-700 flex flex-col items-center justify-center min-h-screen p-6">
            <h1 class="text-3xl sm:text-4xl font-bold text-gray-800 dark:text-white text-center">
                Welcome to the Note App
            </h1>
            <p class="mt-4 text-gray-600 dark:text-gray-300 text-center">
                You need to login to create and manage notes.
            </p>
            <div class="mt-6 flex flex-col sm:flex-row space-x-0 sm:space-x-4 space-y-4 sm:space-y-0">
                <RouterLink
                    to="/login"
                    class="px-6 py-2 text-white bg-blue-600 border border-teal-500 shadow shadow-orange-400 rounded-lg hover:bg-blue-700 w-full sm:w-auto text-center"
                >
                    Login
                </RouterLink>
                <RouterLink
                    to="/register"
                    class="px-6 py-2 text-white bg-green-600 border border-teal-500 shadow shadow-orange-400 rounded-lg hover:bg-green-700 w-full sm:w-auto text-center"
                >
                    Register
                </RouterLink>
            </div>
        </div>
    </div>
</template>

<script setup>
import Navbar from '@/components/Navbar/Navbar.vue';
</script>