import { NavLink, Outlet } from "react-router-dom";

export default function CustomerLayout() {
  return (
    <div className="app-window d-flex flex-column">
      <div className="desktop-titlebar">📖 AksharaMane Book Store</div>

      <header className="container-fluid py-3 border-bottom bg-light">
        <div className="d-flex flex-column flex-md-row justify-content-between align-items-md-end gap-3">
          <div>
            <div className="top-brand">AksharaMane</div>
            <div className="top-subtitle">The House of Books</div>
          </div>

          <ul className="nav nav-tabs">
            <li className="nav-item">
              <NavLink className="nav-link" to="/" end>
                Home
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/categories">
                Categories
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/books">
                Books
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/track-order">
                Track Order
              </NavLink>
            </li>
          </ul>
        </div>
      </header>

      <main className="container-fluid flex-grow-1 py-3">
        <Outlet />
      </main>

      <footer className="footer-bar d-flex justify-content-between">
        <span>AksharaMane Book Store</span>
        <span>Cash on Delivery Only</span>
      </footer>
    </div>
  );
}
