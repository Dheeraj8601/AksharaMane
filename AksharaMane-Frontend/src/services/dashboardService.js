import httpClient, {
  unwrapResponse
} from "./httpClient";

export async function getDashboard() {
  const response = await httpClient.get(
    "/admin/dashboard"
  );

  return unwrapResponse(response);
}