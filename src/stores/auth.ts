import { defineStore } from "pinia";
import api from "../services/api";
import { jwtDecode } from "jwt-decode";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user") || "null") as any | null,
    token: localStorage.getItem("token") || null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
    async login(email: string, password: string) {
      try {
        const response = await api.post("/UserAuth/login", { email, password });

        this.token = response.data.token || "";
        localStorage.setItem("token", this.token?? "");

        await this.fetchUser();

        return response.data;
      } catch (error: any) {
        console.error("Login error:", error);
        throw new Error(error.response?.data || "Login failed");
      }
    },

    async fetchUser() {
      try {
        if (!this.token) throw new Error("Token not found");

        const decodedToken: any = jwtDecode(this.token);
        const userId = decodedToken.id || decodedToken.sub;

        if (!userId) throw new Error("User ID not found in token");

        const response = await api.get(`/UserAuth/getUser/${userId}`);
        this.user = response.data.user;

        localStorage.setItem("user", JSON.stringify(this.user));
      } catch (error) {
        console.error("Failed to fetch user:", error);
        this.logout();
        throw error;
      }
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },

    initializeAuth() {
      const storedToken = localStorage.getItem("token");

      if (storedToken) {
        this.token = storedToken;
        this.fetchUser().catch(() => this.logout());
      }
    },
  },
});

