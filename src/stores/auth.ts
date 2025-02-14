import { defineStore } from "pinia";
import api from "../services/api";
import { jwtDecode } from "jwt-decode";

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
        console.error("Login error:", error);
        throw new Error(error.response?.data || "Login failed");
      }
    },

    async fetchUser() {
      try {
        if (!this.token) {
          throw new Error("Token not found");
        }

        // Decode the token to get the user ID using jwt-decode
        const decodedToken: any = jwtDecode(this.token);
        console.log(decodedToken); // Debug to see token contents

        const userId = decodedToken.id || decodedToken.sub; // Use 'sub' if 'id' is missing

        if (!userId) {
          throw new Error("User ID not found in token");
        }

        const response = await api.get(`/UserAuth/getUser/${userId}`);
        this.user = response.data.user; // Assuming the user object is inside 'data.user'
        
      } catch (error) {
        console.error("Failed to fetch user:", error);
        this.logout(); // Logout if fetching fails (e.g., invalid token)
        throw error; // Optional: rethrow the error to handle it elsewhere in the app
      }
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
    },
  },
});
