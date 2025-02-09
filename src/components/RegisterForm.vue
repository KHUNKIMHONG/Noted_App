<template>
    <form @submit.prevent="register" class="space-y-4">
        <div>
            <label for="name" class="block text-gray-700">Full Name</label>
            <input v-model="name" type="text" id="name" class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-emerald-500" required>
        </div>

        <div>
            <label for="email" class="block text-gray-700">Email</label>
            <input v-model="email" type="email" id="email" class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-emerald-500" required>
        </div>

        <div>
            <label for="password" class="block text-gray-700">Password</label>
            <input v-model="password" type="password" id="password" class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-emerald-500" required>
        </div>

        <div>
            <label for="confirmPassword" class="block text-gray-700">Confirm Password</label>
            <input v-model="confirmPassword" type="password" id="confirmPassword" class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-emerald-500" required>
        </div>

        <button type="submit" class="w-full bg-emerald-700 text-white font-bold py-2 rounded-lg hover:bg-emerald-800 transition">
            Register
        </button>
    </form>
</template>

<script>
export default {
    data() {
        return {
            name: '',
            email: '',
            password: '',
            confirmPassword: '',
        };
    },
    methods: {
        register() {
            if (this.password !== this.confirmPassword) {
                alert("Passwords do not match!");
                return;
            }
            alert("Registered successfully! 🎉");
            // You can handle API submission here
        }
    }
};
</script>

