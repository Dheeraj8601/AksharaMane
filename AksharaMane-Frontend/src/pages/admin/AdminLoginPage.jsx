import { useState } from "react";
import { Navigate, useLocation, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useAuth } from "../../context/AuthContext";

export default function AdminLoginPage() {
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [form, setForm] = useState({
    email: "admin@aksharamane.com",
    password: "Admin@123"
  });
  const [loading, setLoading] = useState(false);

  if (isAuthenticated) {
    //alert("hi")
    return <Navigate to="/admin" replace />;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setLoading(true);

    try {
      await login(form);
      toast.success("Admin login successful.");
      navigate(location.state?.from || "/admin", { replace: true });
    } catch (error) {
      toast.error(error.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="app-window">
      <div className="desktop-titlebar">AksharaMane Admin Login</div>
      <div className="container py-5">
        <div className="classic-panel mx-auto" style={{ maxWidth: 460 }}>
          <div className="classic-panel-header">Administrator Login</div>

          <form className="p-4" onSubmit={handleSubmit}>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                className="form-control"
                type="email"
                value={form.email}
                onChange={(event) =>
                  setForm((current) => ({
                    ...current,
                    email: event.target.value
                  }))
                }
                required
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Password</label>
              <input
                className="form-control"
                type="password"
                value={form.password}
                onChange={(event) =>
                  setForm((current) => ({
                    ...current,
                    password: event.target.value
                  }))
                }
                required
              />
            </div>

            <button className="primary-button w-100" disabled={loading}>
              {loading ? "Signing in..." : "Login"}
            </button>

            <div className="alert alert-info mt-3 mb-0 small">
              Demo: admin@aksharamane.com / Admin@123
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
