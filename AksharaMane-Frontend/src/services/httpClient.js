import axios from "axios";
import { appConfig } from "../config/appConfig";
import { storageKeys } from "../utils/storage";

const httpClient = axios.create({
  baseURL: appConfig.apiBaseUrl,
  timeout: 30000,
  headers: {
    Accept: "application/json"
  }
});

httpClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(storageKeys.token);

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem(storageKeys.token);
      localStorage.removeItem(storageKeys.admin);

      if (
        window.location.pathname.startsWith("/admin") &&
        window.location.pathname !== "/admin/login"
      ) {
        window.location.href = "/admin/login";
      }
    }

    const responseData = error.response?.data;

    let message =
      responseData?.detail ||
      responseData?.message ||
      responseData?.title ||
      error.message ||
      "Something went wrong.";

    if (responseData?.errors) {
      const validationErrors = Object.values(
        responseData.errors
      ).flat();

      if (validationErrors.length > 0) {
        message = validationErrors[0];
      }
    }

    const apiError = new Error(message);

    apiError.status = error.response?.status;
    apiError.data = responseData;

    return Promise.reject(apiError);
  }
);

export function unwrapResponse(response) {
  return response?.data?.data ?? response?.data;
}

export function unwrapPagedResponse(response) {
  const result = unwrapResponse(response);

  if (Array.isArray(result)) {
    return {
      items: result,
      totalCount: result.length,
      pageNumber: 1,
      pageSize: result.length,
      totalPages: 1
    };
  }

  return {
    items:
      result?.items ||
      result?.data ||
      result?.results ||
      [],

    totalCount:
      result?.totalCount ??
      result?.totalRecords ??
      result?.items?.length ??
      0,

    pageNumber:
      result?.pageNumber ??
      result?.currentPage ??
      1,

    pageSize:
      result?.pageSize ??
      result?.items?.length ??
      10,

    totalPages:
      result?.totalPages ??
      Math.ceil(
        (result?.totalCount || 0) /
          (result?.pageSize || 10)
      )
  };
}

export default httpClient;