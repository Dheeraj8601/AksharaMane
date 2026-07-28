import httpClient, {
  unwrapResponse
} from "./httpClient";

export async function loginAdmin(credentials) {
  const response = await httpClient.post(
    "/admin/auth/login",
    {
      email: credentials.email.trim(),
      password: credentials.password
    }
  );

  const result = unwrapResponse(response);

  const token =
    result.accessToken ||
    result.token ||
    result.jwtToken;

  if (!token) {
    throw new Error(
      "The server did not return an authentication token."
    );
  }

  return {
    token,
    admin: {
      id: result.id ?? result.adminId,
      name:
        result.name ||
        result.fullName ||
        "AksharaMane Admin",
      email: result.email || credentials.email,
      role: result.role || "Admin",
      expiresAt:
        result.expiresAt ||
        result.expiration ||
        null
    }
  };
}