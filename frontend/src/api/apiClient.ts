import axios from "axios";

const defaultBaseUrl = "http://localhost:8080";

const baseURL = process.env.NEXT_PUBLIC_API_GATEWAY_URL ?? defaultBaseUrl;

export const apiClient = axios.create({
  baseURL,
  timeout: 30_000,
  headers: {
    Accept: "application/json",
  },
});

