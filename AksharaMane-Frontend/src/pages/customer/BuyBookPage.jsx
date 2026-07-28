import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getBookById } from "../../services/bookService";
import { createOrder } from "../../services/orderService";
import { formatCurrency } from "../../utils/currency";
import {
  getImageUrl,
  setFallbackImage
} from "../../utils/imageUrl";

const initialForm = {
  customerName: "",
  email: "",
  phone: "",
  addressLine1: "",
  addressLine2: "",
  city: "",
  state: "Karnataka",
  pincode: "",
  quantity: 1
};

export default function BuyBookPage() {
  const { bookId } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState(null);
  const [form, setForm] = useState(initialForm);
  const [loading, setLoading] = useState(true);
  const [placing, setPlacing] = useState(false);

  useEffect(() => {
    getBookById(bookId)
      .then(setBook)
      .catch((error) => toast.error(error.message))
      .finally(() => setLoading(false));
  }, [bookId]);

  const total = useMemo(
    () => (book ? book.price * Number(form.quantity || 1) : 0),
    [book, form.quantity]
  );

  function handleChange(event) {
    const { name, value } = event.target;
    setForm((current) => ({ ...current, [name]: value }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (!/^[6-9]\d{9}$/.test(form.phone)) {
      toast.error("Enter a valid 10-digit Indian mobile number.");
      return;
    }

    if (!/^\d{6}$/.test(form.pincode)) {
      toast.error("Enter a valid 6-digit pincode.");
      return;
    }

    setPlacing(true);

    try {

      if (
        Number(form.quantity) >
        Number(book.stockQuantity)
      ) {
        toast.error(
          "The requested quantity is greater than the available stock."
        );

        return;
      }
      const order = await createOrder({
        ...form,
        bookId: Number(bookId),
        quantity: Number(form.quantity)
      });

      toast.success("Order placed successfully.");
      navigate("/order-success", { state: { order } });
    } catch (error) {
      toast.error(error.message);
    } finally {
      setPlacing(false);
    }
  }

  if (loading) return <LoadingSpinner />;

  return (
    <div className="row g-3">
      <section className="col-lg-4">
        <div className="classic-panel">
          <div className="classic-panel-header">Selected Book</div>
          <div className="p-3">
            <img
              className="book-cover"
              src={getImageUrl(book.imageUrl)}
              alt={book.title}
              onError={setFallbackImage}
            />
            <h1 className="h5 mt-3">{book.title}</h1>
            <div>{book.author}</div>
            <div className="price-text mt-2">{formatCurrency(book.price)}</div>
          </div>
        </div>
      </section>

      <section className="col-lg-8">
        <div className="classic-panel">
          <div className="classic-panel-header">
            Delivery Address - Cash on Delivery
          </div>

          <form className="p-3" onSubmit={handleSubmit}>
            <div className="row g-3">
              <div className="col-md-6">
                <label className="form-label">Full Name *</label>
                <input
                  className="form-control"
                  name="customerName"
                  value={form.customerName}
                  onChange={handleChange}
                  required
                  maxLength={100}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Email *</label>
                <input
                  className="form-control"
                  type="email"
                  name="email"
                  value={form.email}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Phone *</label>
                <input
                  className="form-control"
                  name="phone"
                  value={form.phone}
                  onChange={handleChange}
                  required
                  maxLength={10}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Quantity *</label>
                <input
                  className="form-control"
                  type="number"
                  min="1"
                  max={Math.max(1, book.stockQuantity)}
                  name="quantity"
                  value={form.quantity}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="col-12">
                <label className="form-label">Address Line 1 *</label>
                <input
                  className="form-control"
                  name="addressLine1"
                  value={form.addressLine1}
                  onChange={handleChange}
                  required
                  maxLength={200}
                />
              </div>

              <div className="col-12">
                <label className="form-label">Address Line 2</label>
                <input
                  className="form-control"
                  name="addressLine2"
                  value={form.addressLine2}
                  onChange={handleChange}
                  maxLength={200}
                />
              </div>

              <div className="col-md-4">
                <label className="form-label">City *</label>
                <input
                  className="form-control"
                  name="city"
                  value={form.city}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="col-md-4">
                <label className="form-label">State *</label>
                <input
                  className="form-control"
                  name="state"
                  value={form.state}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="col-md-4">
                <label className="form-label">Pincode *</label>
                <input
                  className="form-control"
                  name="pincode"
                  value={form.pincode}
                  onChange={handleChange}
                  required
                  maxLength={6}
                />
              </div>
            </div>

            <div className="classic-panel mt-4">
              <div className="p-3 d-flex flex-column flex-md-row justify-content-between align-items-md-center gap-2">
                <div>
                  <div><strong>Payment:</strong> Cash on Delivery</div>
                  <div><strong>Total:</strong> <span className="price-text">{formatCurrency(total)}</span></div>
                </div>

                <button className="primary-button" type="submit" disabled={placing}>
                  {placing ? "Placing Order..." : "Place Order (COD)"}
                </button>
              </div>
            </div>
          </form>
        </div>
      </section>
    </div>
  );
}
