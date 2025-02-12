import { defineStore } from "pinia";
import api from '../services/api.ts';

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: null as any | null,
    token: localStorage.getItem("token") || null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token, // Check if user is logged in
  },
  actions: {
    async login(email: string, password: string) {
      try {
        const response = await api.post("/UserAuth/login", { email, password });
    
        // Ensure the token is always a string
        this.token = response.data.token || ""; // Fallback to empty string if null
    
        localStorage.setItem("token", this.token ?? "");
        await this.fetchUser();
        
        return response.data;
      } catch (error: any) {
        throw new Error(error.response?.data || "Login failed");
      }
    },
    
    async fetchUser() {
      try {
        if (!this.token) return;

        // Decode the token to get the user ID
        const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
        const userId = decodedToken?.id;

        if (!userId) throw new Error("User ID not found in token");

        const response = await api.get(`/UserAuth/getUser/${userId}`); 
        this.user = response.data.user; // Assuming the user object is inside 'data.user'
      } catch (error) {
        console.error("Failed to fetch user:", error);
        this.logout(); // Logout if fetching fails (e.g., invalid token)
      }
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
    },
  },
});



