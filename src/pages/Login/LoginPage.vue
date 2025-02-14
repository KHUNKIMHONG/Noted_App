<template>
  <div class="flex h-screen justify-center items-center bg-blue-500">
    <!-- Login Form Section -->
    <div
      class="bg-white p-8 md:p-12 w-full max-w-md rounded-lg shadow-xl"
    >
      <div class="text-center mb-6">
        <h1 class="text-3xl font-extrabold text-gray-800">
          Welcome To Noted Application
        </h1>
        <p class="text-gray-500 mt-2">Sign in to continue</p>
      </div>

      <LoginForm />

      <div class="text-center mt-6 text-gray-600 text-sm">
        Don't have an account yet?
        <router-link
          to="/register"
          class="text-blue-600 font-semibold hover:underline"
          >Sign up</router-link
        >
      </div>
    </div>
  </div>
</template>

<script setup>
import LoginForm from "@/components/Auth/LoginForm.vue";
</script>
