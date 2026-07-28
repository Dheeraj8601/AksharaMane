import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { getOrders, updateOrderStatus } from "../../services/orderService";
import { formatCurrency } from "../../utils/currency";
import { getStatusClass, ORDER_STATUSES } from "../../utils/status";

export default function AdminOrdersPage() {
  const [orders, setOrders] = useState([]);
  const [updatingId, setUpdatingId] = useState(null);

  async function load() {
    setOrders(await getOrders());
  }

  useEffect(() => {
    load();
  }, []);

  async function changeStatus(orderId, status) {
    setUpdatingId(orderId);

    try {
      await updateOrderStatus(
        orderId,
        status
      );

      toast.success(
        "Order status updated. The customer notification will be processed."
      );
      await load();
    } catch (error) {
      toast.error(error.message);
    } finally {
      setUpdatingId(null);
    }
  }

  return (
    <>
      <h1 className="section-title">Order Management</h1>

      <div className="classic-panel">
        <div className="classic-panel-header">Order List</div>

        <div className="table-responsive">
          <table className="table table-bordered table-sm align-middle">
            <thead>
              <tr>
                <th>Order</th>
                <th>Customer</th>
                <th>Book</th>
                <th>Address</th>
                <th>Total</th>
                <th>Status</th>
                <th style={{ minWidth: 180 }}>Change Status</th>
              </tr>
            </thead>
            <tbody>
              {orders.length === 0 ? (
                <tr>
                  <td colSpan="7" className="empty-state">No orders found.</td>
                </tr>
              ) : (
                orders.map((order) => (
                  <tr key={order.id}>
                    <td>
                      <strong>{order.orderNumber}</strong>
                      <div className="small text-muted">
                        {order.createdAt || order.orderedAt
                          ? new Date(
                            order.createdAt || order.orderedAt
                          ).toLocaleString("en-IN")
                          : "-"}
                      </div>
                    </td>
                    <td>
                      {order.customerName}
                      <div className="small">{order.email}</div>
                      <div className="small">{order.phone}</div>
                    </td>
                    <td>
                      {order.bookTitle}
                      <div className="small">Qty: {order.quantity}</div>
                    </td>
                    <td>
                      {order.addressLine1}
                      {order.addressLine2 ? `, ${order.addressLine2}` : ""}
                      <div>{order.city}, {order.state} - {order.pincode}</div>
                    </td>
                    <td>{formatCurrency(order.totalAmount)}</td>
                    <td>
                      <span className={`status-badge ${getStatusClass(order.status)}`}>
                        {order.status}
                      </span>
                    </td>
                    <td>
                      <select
                        className="form-select form-select-sm"
                        value={order.status}
                        disabled={
                          updatingId === order.id ||
                          order.status === "Cancelled"
                        }
                        onChange={(event) =>
                          changeStatus(order.id, event.target.value)
                        }
                      >
                        {ORDER_STATUSES.map((status) => (
                          <option key={status} value={status}>
                            {status}
                          </option>
                        ))}
                      </select>

                      <div className="small text-muted mt-1">
                        {order.status === "Cancelled"
                          ? "Cancelled order cannot be modified."
                          : "Buyer will receive an email."}
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
