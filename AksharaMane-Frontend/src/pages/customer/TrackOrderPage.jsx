import { useState } from "react";
import { toast } from "react-toastify";
import {
  cancelOrder,
  trackOrder
} from "../../services/orderService";
import { formatCurrency } from "../../utils/currency";
import {
  getStatusClass
} from "../../utils/status";

export default function TrackOrderPage() {
  const [form, setForm] = useState({
    orderNumber: "",
    emailOrPhone: ""
  });

  const [order, setOrder] = useState(null);
  const [loading, setLoading] =
    useState(false);

  function handleChange(event) {
    const { name, value } = event.target;

    setForm((current) => ({
      ...current,
      [name]: value
    }));
  }

  async function handleTrack(event) {
    event.preventDefault();
    setLoading(true);

    try {
      const result = await trackOrder(form);
      setOrder(result);
    } catch (error) {
      setOrder(null);
      toast.error(error.message);
    } finally {
      setLoading(false);
    }
  }

  async function handleCancel() {
    const confirmed = window.confirm(
      "Are you sure you want to cancel this order?"
    );

    if (!confirmed) {
      return;
    }

    setLoading(true);

    try {
      const result = await cancelOrder({
        orderNumber: order.orderNumber,
        emailOrPhone: form.emailOrPhone,
        reason: "Cancelled by customer"
      });

      setOrder(result);
      toast.success(
        "Order cancelled successfully."
      );
    } catch (error) {
      toast.error(error.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <section
      className="classic-panel mx-auto"
      style={{ maxWidth: 800 }}
    >
      <div className="classic-panel-header">
        Track Your Order
      </div>

      <div className="p-4">
        <form
          className="row g-3"
          onSubmit={handleTrack}
        >
          <div className="col-md-6">
            <label className="form-label">
              Order Number
            </label>

            <input
              className="form-control"
              name="orderNumber"
              value={form.orderNumber}
              onChange={handleChange}
              required
            />
          </div>

          <div className="col-md-6">
            <label className="form-label">
              Email or Phone
            </label>

            <input
              className="form-control"
              name="emailOrPhone"
              value={form.emailOrPhone}
              onChange={handleChange}
              required
            />
          </div>

          <div className="col-12">
            <button
              className="primary-button"
              disabled={loading}
            >
              {loading
                ? "Checking..."
                : "Track Order"}
            </button>
          </div>
        </form>

        {order && (
          <div className="classic-panel mt-4">
            <div className="classic-panel-header">
              Order Details
            </div>

            <div className="p-3">
              <table className="table table-bordered">
                <tbody>
                  <tr>
                    <th>Order Number</th>
                    <td>{order.orderNumber}</td>
                  </tr>

                  <tr>
                    <th>Book</th>
                    <td>{order.bookTitle}</td>
                  </tr>

                  <tr>
                    <th>Quantity</th>
                    <td>{order.quantity}</td>
                  </tr>

                  <tr>
                    <th>Total</th>
                    <td>
                      {formatCurrency(
                        order.totalAmount
                      )}
                    </td>
                  </tr>

                  <tr>
                    <th>Status</th>
                    <td>
                      <span
                        className={`status-badge ${getStatusClass(
                          order.status
                        )}`}
                      >
                        {order.status}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>

              {order.canCancel && (
                <button
                  type="button"
                  className="danger-button"
                  onClick={handleCancel}
                  disabled={loading}
                >
                  Cancel Order
                </button>
              )}
            </div>
          </div>
        )}
      </div>
    </section>
  );
}