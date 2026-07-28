import { Navigate, Route, Routes } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import CustomerLayout from "./layouts/CustomerLayout";
import AdminLayout from "./layouts/AdminLayout";
import ProtectedRoute from "./routes/ProtectedRoute";
import HomePage from "./pages/customer/HomePage";
import CategoriesPage from "./pages/customer/CategoriesPage";
import BooksPage from "./pages/customer/BooksPage";
import BookDetailsPage from "./pages/customer/BookDetailsPage";
import BuyBookPage from "./pages/customer/BuyBookPage";
import OrderSuccessPage from "./pages/customer/OrderSuccessPage";
import AdminLoginPage from "./pages/admin/AdminLoginPage";
import DashboardPage from "./pages/admin/DashboardPage";
import AdminCategoriesPage from "./pages/admin/AdminCategoriesPage";
import AdminBooksPage from "./pages/admin/AdminBooksPage";
import AdminOrdersPage from "./pages/admin/AdminOrdersPage";
import NotFoundPage from "./pages/NotFoundPage";
import TrackOrderPage from "./pages/customer/TrackOrderPage";

export default function App() {
  return (
    <>
      <Routes>
        <Route element={<CustomerLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/books" element={<BooksPage />} />
          <Route path="/books/:id" element={<BookDetailsPage />} />
          <Route path="/buy/:bookId" element={<BuyBookPage />} />
          <Route path="/order-success" element={<OrderSuccessPage />} />
          <Route path="/track-order" element={<TrackOrderPage />}/>
        </Route>

        <Route path="/admin/login" element={<AdminLoginPage />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<DashboardPage />} />
            <Route path="categories" element={<AdminCategoriesPage />} />
            <Route path="books" element={<AdminBooksPage />} />
            <Route path="orders" element={<AdminOrdersPage />} />
          </Route>
        </Route>

        <Route path="/home" element={<Navigate to="/" replace />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>

      <ToastContainer position="top-right" autoClose={2500} />
    </>
  );
}
