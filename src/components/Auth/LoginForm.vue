<template>
  <form @submit.prevent="submitForm" class="space-y-4">
    <!-- Email -->
    <div>
      <label class="block text-gray-600 text-sm font-medium">Email</label>
      <input
        type="email"
        v-model="email"
        class="w-full p-3 border border-gray-300 rounded-md focus:ring-2 focus:ring-emerald-500 outline-none"
        required
        placeholder="Enter your Email"
      />
    </div>

    <!-- Password -->
    <div>
      <label class="block text-gray-600 text-sm font-medium">Password</label>
      <input
        type="password"
        v-model="password"
        class="w-full p-3 border border-gray-300 rounded-md focus:ring-2 focus:ring-emerald-500 outline-none"
        required
        placeholder="Enter your Password"
      />
    </div>

    <!-- Remember me and Forgot password link -->
    <div class="flex items-center justify-between text-sm text-gray-600">
      <label class="flex items-center space-x-2">
        <input
          type="checkbox"
          class="rounded border-gray-300 focus:ring-purple-500"
        />
        <span>Keep me logged in</span>
      </label>
      <router-link to="/forgot-password" class="text-purple-600 hover:underline">
        Forgotten?
      </router-link>
    </div>

    <!-- Submit button -->
    <button
      type="submit"
      class="w-full bg-emerald-700 text-white py-3 rounded-md font-bold hover:bg-emerald-800"
      :disabled="loading"
    >
      {{ loading ? "Logging in..." : "Log in" }}
    </button>

    <!-- Error message -->
    <p v-if="errorMessage" class="text-red-500 text-sm">{{ errorMessage }}</p>
  </form>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { useAuthStore } from "../../stores/auth";
import router from "../../router";

// Initialize authentication store
const authStore = useAuthStore();

// Define reactive variables for form data
const email = ref<string>("");
const password = ref<string>("");
const errorMessage = ref<string | null>(null);
const loading = ref<boolean>(false);

// Define the submit function to handle login
const submitForm = async () => {
  errorMessage.value = null; // Reset error message
  loading.value = true; // Show loading state

  try {
    // Perform login action using the store
    await authStore.login(email.value, password.value);

    // Redirect to /notes page after successful login
      // Redirect to /notes page after successful login
    router.push("/notes");

  } catch (error: any) {
    // Handle the error by setting the error message
    errorMessage.value = error.message || "Login failed, please try again.";
  } finally {
    // Hide loading state regardless of success or failure
    loading.value = false;
  }
};
</script>



