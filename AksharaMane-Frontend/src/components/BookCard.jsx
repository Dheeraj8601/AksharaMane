import { Link } from "react-router-dom";
import { formatCurrency } from "../utils/currency";
import {
  getImageUrl,
  setFallbackImage
} from "../utils/imageUrl";
import { FaShoppingCart, FaTimesCircle } from "react-icons/fa";

export default function BookCard({ book }) {

  console.log("book", book)
  return (
    <article className="book-card">
      <img
        className="book-cover"
        src={getImageUrl(book.imageUrl)}
        alt={book.title}
        onError={setFallbackImage}
      />
      <h3>{book.title}</h3>
      <div className="text-muted mb-1">{book.author}</div>
      <div className="small mb-2">{book.categoryName}</div>
      <div className="price-text mb-3">{formatCurrency(book.price)}</div>
      {book.stockQuantity > 0 ? (
        <span className="badge bg-success mb-3">
          In Stock
        </span>
      ) : (
        <span className="badge bg-danger mb-3">
          Out of Stock
        </span>
      )}
      <div className="d-flex gap-2">
        <Link
          className="classic-button flex-fill text-center"
          to={`/books/${book.id}`}
        >
          Details
        </Link>

        {book.stockQuantity > 0 ? (
          <Link
            className="primary-button flex-fill text-center"
            to={`/buy/${book.id}`}
          >
            <FaShoppingCart className="me-1" />
            Buy
          </Link>
        ) : (
          ''
        )}
      </div>
    </article>
  );
}
