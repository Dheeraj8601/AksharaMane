import {
  createContext,
  useContext,
  useMemo,
  useState
} from "react";
import { loginAdmin } from "../services/authService";
import {
  readJson,
  storageKeys
} from "../utils/storage";

const AuthContext = createContext(null);

function isAdminSessionValid(token, admin) {
  if (!token || !admin) {
    return false;
  }

  if (
    admin.role &&
    admin.role.toLowerCase() !== "admin"
  ) {
    return false;
  }

  if (
    admin.expiresAt &&
    new Date(admin.expiresAt) <= new Date()
  ) {
    return false;
  }

  return true;
}

export function AuthProvider({ children }) {
  const initialToken = localStorage.getItem(
    storageKeys.token
  );

  const initialAdmin = readJson(
    storageKeys.admin,
    null
  );

  const validInitialSession = isAdminSessionValid(
    initialToken,
    initialAdmin
  );

  const [token, setToken] = useState(
    validInitialSession ? initialToken : null
  );

  const [admin, setAdmin] = useState(
    validInitialSession ? initialAdmin : null
  );

  async function login(credentials) {
    const result = await loginAdmin(credentials);

    localStorage.setItem(
      storageKeys.token,
      result.token
    );

    localStorage.setItem(
      storageKeys.admin,
      JSON.stringify(result.admin)
    );

    setToken(result.token);
    setAdmin(result.admin);

    return result;
  }

  function logout() {
    localStorage.removeItem(storageKeys.token);
    localStorage.removeItem(storageKeys.admin);

    setToken(null);
    setAdmin(null);
  }

  const value = useMemo(
    () => ({
      token,
      admin,
      isAuthenticated: isAdminSessionValid(
        token,
        admin
      ),
      login,
      logout
    }),
    [token, admin]
  );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error(
      "useAuth must be used inside AuthProvider."
    );
  }

  return context;
}