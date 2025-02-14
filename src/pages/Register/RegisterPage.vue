<template>
  <div class="flex h-screen justify-center items-center bg-blue-500">
    <!-- Section: Register Form -->
    <div class="bg-white p-8 md:p-12 w-full max-w-md rounded-lg shadow-xl">
      <div class="text-center">
        <h2
          class="text-2xl md:text-3xl font-bold text-gray-700 mb-6"
        >
          Register Account
        </h2>
      </div>

      <RegisterForm />

      <p class="text-center mt-4 text-gray-600 text-sm">
        Already have an account?
        <router-link
          to="/login"
          class="text-blue-600 font-bold hover:underline"
        >
          Log in
        </router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import RegisterForm from "@/components/Auth/RegisterForm.vue";
</script>
