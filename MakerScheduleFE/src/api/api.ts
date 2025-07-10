import axios from "axios";
import { SERVER_URI } from "@ms/common/env-constants";
import type { AxiosRequestConfig, AxiosResponse } from "axios";

// import { store } from "@ms/app/store";
// import { setAccessToken } from "@ms/features/auth/auth.slice";

const AxiosInstance = axios.create({
  baseURL: SERVER_URI,
  withCredentials: true, // This is crucial for sending HttpOnly cookies
});

// Interceptor to add the access token to every request
AxiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  // const accessToken = store.getState().auth.accessToken;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor to handle token refresh on 401 errors
let isRefreshing = false;
let failedQueue: {
  resolve: (value: unknown) => void;
  reject: (reason?: any) => void;
}[] = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

AxiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;
    // Check if the error is 401 and it's not a retry request
    if (error.response?.status === 401 && !originalRequest._retry) {
      // If the failed request is the refresh endpoint itself, do not retry
      if (originalRequest.url?.includes("/refresh")) {
        // Optionally: clear tokens, trigger logout, etc.
        return Promise.reject(error);
      }
      if (isRefreshing) {
        return new Promise(function (resolve, reject) {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers["Authorization"] = "Bearer " + token;
            return AxiosInstance(originalRequest);
          })
          .catch((err) => {
            return Promise.reject(err);
          });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const { data } = await AxiosInstance.post("/api/auth/refresh");
        const newAccessToken = data.accessToken;
        localStorage.setItem("accessToken", newAccessToken);
        // store.dispatch(setAccessToken({ accessToken: newAccessToken }));
        processQueue(null, newAccessToken);

        originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
        return await AxiosInstance(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError, null);
        // Handle final refresh failure (e.g., logout user)
        // store.dispatch(setAccessToken({ accessToken: null }));
        // Optionally redirect to login page
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

const sendAxiosRequest = async <T>(
  req: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  try {
    const response = await AxiosInstance(req);
    return response;
  } catch (error) {
    console.log("error");
    throw error;
  }
};

export { sendAxiosRequest };
