import axios, { type AxiosInstance, type InternalAxiosRequestConfig } from "axios";

// Base API URL
const BASE_URL = "https://localhost:7131/api";

// Create Axios instance
const api: AxiosInstance = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Ensures cookies/token authentication works
});

// Request Interceptor: Attach Authorization Token
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error) // Handle request errors
);

// Response Interceptor: Handle Unauthorized (401) Errors
api.interceptors.response.use(
  (response) => response, // Return successful responses
  (error) => {
    if (error.response?.status === 401) {
      // If unauthorized, remove token and redirect to login
      localStorage.removeItem("token");
      window.location.href = "/login"; 
    }
    return Promise.reject(error); // Forward other errors
  }
);

export default api;


