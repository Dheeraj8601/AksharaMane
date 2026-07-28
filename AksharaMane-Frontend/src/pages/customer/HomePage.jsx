import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import BookCard from "../../components/BookCard";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getBooks } from "../../services/bookService";

export default function HomePage() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getBooks({
      pageNumber: 1,
      pageSize: 4,
      isAvailable: true
    })
      .then(setBooks)
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="row g-3">
      <section className="col-lg-9">
        <div className="classic-panel p-4 mb-3">
          <h1 className="section-title">Welcome to AksharaMane</h1>
          <p>
            Discover books, view their details, and place a Cash on Delivery
            order directly. No customer account is required.
          </p>
          <Link to="/books" className="primary-button d-inline-block">
            Browse Books
          </Link>
        </div>

        <div className="classic-panel">
          <div className="classic-panel-header d-flex justify-content-between">
            <span>Featured Books</span>
            <Link to="/books">View All</Link>
          </div>

          <div className="p-3">
            {loading ? (
              <LoadingSpinner />
            ) : (
              <div className="row g-3">
                {books.map((book) => (
                  <div className="col-sm-6 col-xl-3" key={book.id}>
                    <BookCard book={book} />
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </section>

      <aside className="col-lg-3">
        <div className="classic-panel mb-3">
          <div className="classic-panel-header">How It Works</div>
          <ol className="m-0 p-3 ps-4">
            <li className="mb-2">Choose a book.</li>
            <li className="mb-2">Enter delivery details.</li>
            <li className="mb-2">Place the COD order.</li>
            <li>Pay when the book arrives.</li>
          </ol>
        </div>

        <div className="classic-panel">
          <div className="classic-panel-header">Important</div>
          <div className="p-3">
            <p className="mb-2">Payment Method:</p>
            <strong>Cash on Delivery only</strong>
          </div>
        </div>
      </aside>
    </div>
  );
}
