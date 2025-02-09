<template>
    <div class="flex h-screen">
        <!-- Left Section: Login Form -->
        <div class="w-full md:w-1/2 bg-white flex flex-col justify-center items-center p-6 md:p-12 overflow-y-auto">
            <div class="w-full max-w-md">
                <div class="text-center mb-6">
                    <div class="text-2xl md:text-3xl font-bold text-emerald-700">
                        📄 Welcome To Noted Application
                    </div>
                </div>

                <div class="my-4 flex items-center">
                    <hr class="flex-grow border-gray-300">
                    <span class="mx-2 text-gray-500 text-sm">OR LOGIN WITH EMAIL</span>
                    <hr class="flex-grow border-gray-300">
                </div>

                <LoginForm />

                <div class="text-center mt-4 text-gray-600 text-sm">
                    Don't have an account yet?
                    <router-link to="/register" class="text-emerald-600 font-bold hover:underline">Sign up</router-link>
                </div>
            </div>
        </div>

        <!-- Right Section: Illustration -->
        <div class="hidden md:flex w-full md:w-1/2 bg-emerald-700 items-center justify-center">
            <img src="@/assets/images/image.png" alt="Illustration" class="w-3/4 md:w-2/3 lg:w-1/2">
        </div>
    </div>
</template>

<script setup>
import LoginForm from '@/components/LoginForm.vue';
</script>