import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function AdminLayout() {
  const { admin, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/admin/login");
  }

  return (
    <div className="app-window">
      <div className="desktop-titlebar d-flex justify-content-between">
        <span>AksharaMane Administration</span>
        <span className="fw-normal small">
          {admin?.name} |{" "}
          <button
            type="button"
            className="btn btn-link btn-sm p-0"
            onClick={handleLogout}
          >
            Logout
          </button>
        </span>
      </div>

      <div className="d-flex admin-shell">
        <aside className="admin-sidebar">
          <NavLink to="/admin" end>
            Dashboard
          </NavLink>
          <NavLink to="/admin/categories">Categories</NavLink>
          <NavLink to="/admin/books">Books</NavLink>
          <NavLink to="/admin/orders">Orders</NavLink>
          <NavLink to="/">Open Customer Site</NavLink>
        </aside>

        <main className="admin-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
