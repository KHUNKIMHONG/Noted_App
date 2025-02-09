<template>
    <nav class="bg-gradient-to-r from-emerald-700 via-teal-700 to-emerald-700 text-white p-4 fixed w-full top-0 left-0 z-10">
        <div class="container mx-auto flex justify-between items-center">
            <h1 class="text-xl font-bold">📒 Noted Web</h1>
            <div>
                <router-link to="/login" class="px-4 py-2 hover:bg-blue-500 rounded">Login</router-link>
                <router-link to="/register" class="px-4 py-2 bg-white text-blue-600 rounded hover:bg-gray-200">Register</router-link>
            </div>
        </div>
    </nav>
</template>
