<template>
    <form @submit.prevent="submitForm" class="space-y-4">
        <div>
            <label class="block text-gray-600 text-sm font-medium">Username</label>
            <input type="text" v-model="username"
                class="w-full p-3 border border-gray-300 rounded-md focus:ring-2 focus:ring-emerald-500 outline-none">
        </div>

        <div>
            <label class="block text-gray-600 text-sm font-medium">Password</label>
            <input type="password" v-model="password"
                class="w-full p-3 border border-gray-300 rounded-md focus:ring-2 focus:ring-emerald-500 outline-none">
        </div>

        <div class="flex items-center justify-between text-sm text-gray-600">
            <label class="flex items-center space-x-2">
                <input type="checkbox" class="rounded border-gray-300 focus:ring-purple-500">
                <span>Keep me logged in</span>
            </label>
            <router-link to="/forgot-password" class="text-purple-600 hover:underline">Forgotten?</router-link>
        </div>

        <button type="submit" class="w-full bg-emerald-700 text-white py-3 rounded-md font-bold hover:bg-emerald-800">
            Log in
        </button>
    </form>
</template>

<script setup>
import { ref } from 'vue';

const username = ref('');
const password = ref('');

const submitForm = () => {
    alert(`Logging in with username: ${username.value}`);
};
</script>