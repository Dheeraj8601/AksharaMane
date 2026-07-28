export const appConfig = {
  apiBaseUrl:
    import.meta.env.VITE_API_BASE_URL ||
    "https://localhost:7120/api",

  fileBaseUrl:
    import.meta.env.VITE_FILE_BASE_URL ||
    "https://localhost:7120"
};