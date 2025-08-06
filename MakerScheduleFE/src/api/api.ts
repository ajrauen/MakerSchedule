import axios from "axios";
import { SERVER_URI } from "@ms/common/env-constants";
import type { AxiosRequestConfig, AxiosResponse } from "axios";

const AxiosInstance = axios.create({
  baseURL: SERVER_URI,
  withCredentials: true,
});

AxiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

let isRefreshing = false;
let failedQueue: {
  resolve: (value: unknown) => void;
  reject: (reason?: unknown) => void;
}[] = [];

const processQueue = (error: unknown, token: string | null = null) => {
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
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (originalRequest.url?.includes("/refresh")) {
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
        processQueue(null, newAccessToken);

        originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
        return await AxiosInstance(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError, null);

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
