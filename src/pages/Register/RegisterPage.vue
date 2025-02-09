<template>
    <div class="flex h-screen">
        <!-- Left Section: Register Form -->
        <div class="w-full md:w-1/2 bg-white flex flex-col justify-center items-center p-6 md:p-12 overflow-y-auto">
            <div class="w-full max-w-md">
                <div class="text-center mb-6">
                    <div class="text-2xl md:text-3xl font-bold text-emerald-700">
                        📝 Create Your Noted Account
                    </div>
                </div>

                <RegisterForm />

                <div class="text-center mt-4 text-gray-600 text-sm">
                    Already have an account?
                    <router-link to="/login" class="text-emerald-600 font-bold hover:underline">Log in</router-link>
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
import RegisterForm from '@/components/RegisterForm.vue';
</script>
