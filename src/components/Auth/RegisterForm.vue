<template>
  <form @submit.prevent="register" class="space-y-4">
    <!-- Full Name -->
    <div class="flex flex-col">
      <label for="name" class="text-gray-700 font-medium mb-1">Full Name</label>
      <input 
        v-model="name" 
        type="text" 
        id="name" 
        class="w-full p-3 border border-gray-300 rounded-lg bg-white focus:border-emerald-500 focus:ring-2 focus:ring-emerald-500 outline-none" 
        required
        placeholder="Enter your Full Name"/>
    </div>

    <!-- Email -->
    <div class="flex flex-col">
      <label for="email" class="text-gray-700 font-medium mb-1">Email</label>
      <input 
        v-model="email" 
        type="email" 
        id="email" 
        class="w-full p-3 border border-gray-300 rounded-lg bg-white focus:border-emerald-500 focus:ring-2 focus:ring-emerald-500 outline-none" 
        required
        placeholder="Enter your Email"/>
    </div>

    <!-- Password -->
    <div class="flex flex-col">
      <label for="password" class="text-gray-700 font-medium mb-1">Password</label>
      <input 
        v-model="password" 
        type="password" 
        id="password" 
        class="w-full p-3 border border-gray-300 rounded-lg bg-white focus:border-emerald-500 focus:ring-2 focus:ring-emerald-500 outline-none" 
        required
        placeholder="Enter your Password"/>
    </div>

    <!-- Confirm Password -->
    <div class="flex flex-col">
      <label for="confirmPassword" class="text-gray-700 font-medium mb-1">Confirm Password</label>
      <input 
        v-model="confirmPassword" 
        type="password" 
        id="confirmPassword" 
        :class="{'border-red-500 focus:ring-red-500': passwordMismatch, 'focus:border-emerald-500 focus:ring-emerald-500': !passwordMismatch}" 
        class="w-full p-3 border border-gray-300 rounded-lg bg-white outline-none" 
        required
        placeholder="Enter your Confirm Password"/>
      <p v-if="passwordMismatch" class="text-red-500 text-sm mt-1">Passwords do not match!</p>
    </div>

    <!-- Register Button -->
    <button 
      type="submit" 
      class="w-full bg-emerald-700 text-white font-bold py-3 rounded-lg hover:bg-emerald-800 transition-all">
      Register
    </button>
  </form>
</template>

<script>
import api from "../../services/api"; 

export default {
  data() {
    return {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
    };
  },
  computed: {
    passwordMismatch() {
      return this.password && this.confirmPassword && this.password !== this.confirmPassword;
    },
    emailInvalid() {
      const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      return !emailPattern.test(this.email);
    },
  },
  methods: {
    async register() {
      if (this.passwordMismatch || this.emailInvalid || this.phoneNumberInvalid) {
        return;
      }

      try {
        // Send registration data as strings
        const response = await api.post("/UserAuth/register", {
          name: String(this.name),
          email: String(this.email),
          password: String(this.password),
          phoneNumber: String(this.phoneNumber),
        });

        // Handle successful registration
        alert("Registration successful! 🎉");
        console.log(response.data); 

        //Optionally redirect after successful registration
        this.$router.push("/login");

      } catch (error) {
        console.error("Registration failed:", error);
        alert("Registration failed. Please try again.");
      }
    }
  }
};
</script>




