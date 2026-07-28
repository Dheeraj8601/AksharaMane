import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getDashboard } from "../../services/dashboardService";
import { formatCurrency } from "../../utils/currency";

export default function DashboardPage() {
  const [dashboard, setDashboard] =
    useState(null);

  const [loading, setLoading] =
    useState(true);

  useEffect(() => {
    async function loadDashboard() {
      try {
        const result = await getDashboard();
        setDashboard(result);
      } catch (error) {
        toast.error(error.message);
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, []);

  if (loading) {
    return <LoadingSpinner />;
  }

  const cards = [
    [
      "Categories",
      dashboard?.totalCategories ?? 0
    ],
    [
      "Active Categories",
      dashboard?.activeCategories ?? 0
    ],
    [
      "Books",
      dashboard?.totalBooks ?? 0
    ],
    [
      "Active Books",
      dashboard?.activeBooks ?? 0
    ],
    [
      "Orders",
      dashboard?.totalOrders ?? 0
    ],
    [
      "Placed Orders",
      dashboard?.placedOrders ?? 0
    ],
    [
      "Delivered Orders",
      dashboard?.deliveredOrders ?? 0
    ],
    [
      "Total Revenue",
      formatCurrency(
        dashboard?.totalRevenue ?? 0
      )
    ],
    [
      "Today Revenue",
      formatCurrency(
        dashboard?.todayRevenue ?? 0
      )
    ],
    [
      "Low Stock",
      dashboard?.lowStockCount ?? 0
    ],
    [
      "Out of Stock",
      dashboard?.outOfStockCount ?? 0
    ]
  ];

  return (
    <>
      <h1 className="section-title">
        Admin Dashboard
      </h1>

      <div className="row g-3 mb-4">
        {cards.map(([label, value]) => (
          <div
            className="col-sm-6 col-xl-3"
            key={label}
          >
            <div className="classic-panel h-100">
              <div className="classic-panel-header">
                {label}
              </div>

              <div className="p-4 display-6">
                {value}
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="row g-3">
        <div className="col-lg-6">
          <div className="classic-panel">
            <div className="classic-panel-header">
              Low Stock Books
            </div>

            <div className="table-responsive">
              <table className="table table-bordered mb-0">
                <thead>
                  <tr>
                    <th>Book</th>
                    <th>Author</th>
                    <th>Stock</th>
                  </tr>
                </thead>

                <tbody>
                  {dashboard?.lowStockBooks?.length ? (
                    dashboard.lowStockBooks.map(
                      (book) => (
                        <tr key={book.id}>
                          <td>{book.title}</td>
                          <td>{book.author}</td>
                          <td>{book.stockQuantity}</td>
                        </tr>
                      )
                    )
                  ) : (
                    <tr>
                      <td
                        colSpan="3"
                        className="text-center"
                      >
                        No low-stock books.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div className="col-lg-6">
          <div className="classic-panel">
            <div className="classic-panel-header">
              Recent Orders
            </div>

            <div className="table-responsive">
              <table className="table table-bordered mb-0">
                <thead>
                  <tr>
                    <th>Order</th>
                    <th>Customer</th>
                    <th>Total</th>
                    <th>Status</th>
                  </tr>
                </thead>

                <tbody>
                  {dashboard?.recentOrders?.length ? (
                    dashboard.recentOrders.map(
                      (order) => (
                        <tr key={order.id}>
                          <td>{order.orderNumber}</td>
                          <td>{order.customerName}</td>
                          <td>
                            {formatCurrency(
                              order.totalAmount
                            )}
                          </td>
                          <td>{order.status}</td>
                        </tr>
                      )
                    )
                  ) : (
                    <tr>
                      <td
                        colSpan="4"
                        className="text-center"
                      >
                        No recent orders.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}