import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getCategories } from "../../services/categoryService";

export default function CategoriesPage() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getCategories()
      .then(setCategories)
      .finally(() => setLoading(false));
  }, []);

  return (
    <section className="classic-panel">
      <div className="classic-panel-header">Book Categories</div>
      <div className="p-3">
        {loading ? (
          <LoadingSpinner />
        ) : (
          <div className="row g-3">
            {categories.map((category) => (
              <div className="col-md-6 col-lg-4" key={category.id}>
                <div className="classic-panel p-3 h-100">
                  <h2 className="h5 text-primary">{category.name}</h2>
                  <p>{category.description}</p>
                  <Link
                    className="classic-button d-inline-block"
                    to={`/books?categoryId=${category.id}`}
                  >
                    View Books
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </section>
  );
}
