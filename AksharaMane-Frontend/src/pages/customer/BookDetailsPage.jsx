import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getBookById } from "../../services/bookService";
import { formatCurrency } from "../../utils/currency";
import {
  getImageUrl,
  setFallbackImage
} from "../../utils/imageUrl";

export default function BookDetailsPage() {
  const { id } = useParams();
  const [book, setBook] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getBookById(id)
      .then(setBook)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <LoadingSpinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;

  return (
    <section className="classic-panel">
      <div className="classic-panel-header">Book Details</div>
      <div className="p-4">
        <div className="row g-4">
          <div className="col-md-4 col-lg-3">
            <img
              className="book-cover"
              src={getImageUrl(book.imageUrl)}
              alt={book.title}
              onError={setFallbackImage}
            />
          </div>

          <div className="col-md-8 col-lg-9">
            <h1 className="section-title">{book.title}</h1>

            <table className="table table-bordered w-auto">
              <tbody>
                <tr><th>Author</th><td>{book.author}</td></tr>
                <tr><th>Category</th><td>{book.categoryName}</td></tr>
                <tr><th>Language</th><td>{book.language}</td></tr>
                <tr><th>Pages</th><td>{book.pages}</td></tr>
                <tr><th>Publisher</th><td>{book.publisher}</td></tr>
                <tr><th>Available</th><td>
                  {book.stockQuantity > 0 ?
                    <span className="badge bg-success">
                      In Stock
                    </span>
                    :
                    <span className="badge bg-danger">
                      Out of Stock
                    </span>
                  }
                </td></tr>
                <tr><th>Price</th><td className="price-text">{formatCurrency(book.price)}</td></tr>
              </tbody>
            </table>

            <h2 className="h5 mt-4">Description</h2>
            <p>{book.description}</p>

            {book.stockQuantity > 0 &&
              <Link className="primary-button d-inline-block" to={`/buy/${book.id}`}>
                Buy Book - COD
              </Link>
            }
          </div>
        </div>
      </div>
    </section>
  );
}
