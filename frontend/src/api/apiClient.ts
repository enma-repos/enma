import axios from "axios";

const defaultBaseUrl = "http://localhost:8080";

const baseURL = process.env.NEXT_PUBLIC_API_GATEWAY_URL ?? defaultBaseUrl;

export const apiClient = axios.create({
  baseURL,
  withCredentials: true,
  timeout: 30_000,
  headers: {
    Accept: "application/json",
  },
});

type RetriableAxiosConfig = {
  _retry?: boolean;
};

let refreshPromise: Promise<void> | null = null;

async function refreshTokens() {
  await apiClient.post("/api/auth/v1/refresh", {});
}

if (typeof window !== "undefined") {
  apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
      const status = error?.response?.status;
      const config = error?.config as (RetriableAxiosConfig & { url?: string }) | undefined;

      if (status !== 401 || !config || config._retry) {
        return Promise.reject(error);
      }

      if (config.url?.includes("/api/auth/v1/refresh")) {
        return Promise.reject(error);
      }

      config._retry = true;

      try {
        refreshPromise ??= refreshTokens().finally(() => {
          refreshPromise = null;
        });
        await refreshPromise;
        return apiClient(config);
      } catch {
        const returnTo = window.location.pathname + window.location.search;
        window.location.assign(`/auth?returnTo=${encodeURIComponent(returnTo)}`);
        return Promise.reject(error);
      }
    },
  );
}
