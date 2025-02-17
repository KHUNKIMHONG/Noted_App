<template>
    <div class="flex items-center justify-center min-h-screen bg-gray-900">
        <div class="text-center">
            <h1 class="text-6xl font-bold text-gray-200">
                {{ statusCode || "Error" }}
            </h1>
            <p class="mt-4 text-lg text-gray-400">
                {{ errorMessage }}
            </p>
            <router-link
                to="/"
                class="mt-6 inline-block px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600"
            >
                Go Home
            </router-link>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
const props = defineProps({
    error: {
        type: Object,
        default: () => ({ statusCode: 404 })  // Default to 500 if error object is not provided
    },
});

const errorMessages = {
    404: "Oops! The page you're looking for doesn't exist.",
    500: "Internal Server Error. Please try again later.",
    403: "You don't have permission to access this page.",
    401: "Unauthorized. Please login first."
};

const statusCode = computed(() => props.error.statusCode);

const errorMessage = computed(() => {
    // Use map to find the correct message
    const message = Object.keys(errorMessages)
        .map(code => (code === statusCode.value.toString()) ? errorMessages[code] : null)
        .filter(Boolean)
        .join();

    return message || "An unexpected error occurred.";
});
</script>

  
