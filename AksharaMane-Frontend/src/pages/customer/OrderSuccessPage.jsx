import { Link, useLocation } from "react-router-dom";
import { formatCurrency } from "../../utils/currency";

export default function OrderSuccessPage() {
  const { state } = useLocation();
  const order = state?.order;

  if (!order) {
    return (
      <div className="classic-panel p-4 text-center">
        <h1 className="section-title">No recent order found</h1>
        <Link className="primary-button d-inline-block" to="/books">
          Browse Books
        </Link>
      </div>
    );
  }

  return (
    <section className="classic-panel mx-auto" style={{ maxWidth: 700 }}>
      <div className="classic-panel-header">Order Confirmed</div>
      <div className="p-4">
        <h1 className="section-title">Thank you, {order.customerName}!</h1>
        <p>Your Cash on Delivery order was placed successfully.</p>

        <table className="table table-bordered">
          <tbody>
            <tr><th>Order Number</th><td>{order.orderNumber}</td></tr>
            <tr><th>Book</th><td>{order.bookTitle}</td></tr>
            <tr><th>Quantity</th><td>{order.quantity}</td></tr>
            <tr><th>Total</th><td>{formatCurrency(order.totalAmount)}</td></tr>
            <tr><th>Status</th><td>{order.status}</td></tr>
            <tr><th>Payment</th><td>Cash on Delivery</td></tr>
          </tbody>
        </table>

        <p>
          Order status updates will be sent to <strong>{order.email}</strong>.
        </p>

        <Link className="primary-button d-inline-block" to="/books">
          Continue Browsing
        </Link>
      </div>
    </section>
  );
}
