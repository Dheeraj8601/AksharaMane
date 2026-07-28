
import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { toast } from "react-toastify";

import BookCard from "../../components/BookCard";
import CategorySidebar from "../../components/CategorySidebar";
import LoadingSpinner from "../../components/LoadingSpinner";

import { getBooksPaged } from "../../services/bookService";
import { getCategories } from "../../services/categoryService";

export default function BooksPage() {
  const [searchParams, setSearchParams] =
    useSearchParams();

  const [categories, setCategories] =
    useState([]);

  const [books, setBooks] =
    useState([]);

  const [search, setSearch] = useState(
    searchParams.get("search") || ""
  );

  const [loading, setLoading] =
    useState(true);

  const [totalPages, setTotalPages] =
    useState(1);

  const [pageNumber, setPageNumber] =
    useState(1);

  const categoryId =
    searchParams.get("categoryId") || "";

  const appliedSearch =
    searchParams.get("search") || "";

  useEffect(() => {
    async function loadCategories() {
      try {
        const result = await getCategories();
        setCategories(result);
      } catch (error) {
        toast.error(
          error.message ||
            "Unable to load categories."
        );
      }
    }

    loadCategories();
  }, []);

  useEffect(() => {
    async function loadBooks() {
      try {
        setLoading(true);

        const result = await getBooksPaged({
          categoryId:
            categoryId || undefined,

          search:
            appliedSearch || undefined,

          pageNumber,
          pageSize: 12,
          isAvailable: true
        });

        setBooks(result.items || []);
        setTotalPages(
          result.totalPages || 1
        );
      } catch (error) {
        setBooks([]);
        setTotalPages(1);

        toast.error(
          error.message ||
            "Unable to load books."
        );
      } finally {
        setLoading(false);
      }
    }

    loadBooks();
  }, [
    categoryId,
    appliedSearch,
    pageNumber
  ]);

  function selectCategory(id) {
    const next = new URLSearchParams(
      searchParams
    );

    if (id) {
      next.set("categoryId", id);
    } else {
      next.delete("categoryId");
    }

    setPageNumber(1);
    setSearchParams(next);
  }

  function submitSearch(event) {
    event.preventDefault();

    const next = new URLSearchParams(
      searchParams
    );

    const normalizedSearch = search.trim();

    if (normalizedSearch) {
      next.set("search", normalizedSearch);
    } else {
      next.delete("search");
    }

    setPageNumber(1);
    setSearchParams(next);
  }

  function clearSearch() {
    const next = new URLSearchParams(
      searchParams
    );

    next.delete("search");

    setSearch("");
    setPageNumber(1);
    setSearchParams(next);
  }

  function goToPreviousPage() {
    setPageNumber((current) =>
      Math.max(current - 1, 1)
    );
  }

  function goToNextPage() {
    setPageNumber((current) =>
      Math.min(current + 1, totalPages)
    );
  }

  return (
    <div className="row g-3">
      <div className="col-lg-3">
        <CategorySidebar
          categories={categories}
          selectedCategoryId={categoryId}
          onSelect={selectCategory}
        />
      </div>

      <section className="col-lg-9">
        <div className="classic-panel">
          <div className="classic-panel-header">
            Book List
          </div>

          <div className="p-3 border-bottom">
            <form
              className="d-flex gap-2"
              onSubmit={submitSearch}
            >
              <input
                className="form-control"
                value={search}
                onChange={(event) =>
                  setSearch(event.target.value)
                }
                placeholder="Search by title or author"
              />

              <button
                className="primary-button"
                type="submit"
              >
                Search
              </button>

              {appliedSearch && (
                <button
                  className="btn btn-outline-secondary"
                  type="button"
                  onClick={clearSearch}
                >
                  Clear
                </button>
              )}
            </form>
          </div>

          <div className="p-3">
            {loading ? (
              <LoadingSpinner />
            ) : books.length === 0 ? (
              <div className="empty-state">
                No books found.
              </div>
            ) : (
              <>
                <div className="row g-3">
                  {books.map((book) => (
                    <div
                      className="col-sm-6 col-xl-4"
                      key={book.id}
                    >
                      <BookCard book={book} />
                    </div>
                  ))}
                </div>

                {totalPages > 1 && (
                  <div className="d-flex justify-content-center align-items-center gap-3 mt-4">
                    <button
                      type="button"
                      className="btn btn-outline-secondary"
                      onClick={goToPreviousPage}
                      disabled={pageNumber === 1}
                    >
                      Previous
                    </button>

                    <span>
                      Page {pageNumber} of{" "}
                      {totalPages}
                    </span>

                    <button
                      type="button"
                      className="btn btn-outline-secondary"
                      onClick={goToNextPage}
                      disabled={
                        pageNumber === totalPages
                      }
                    >
                      Next
                    </button>
                  </div>
                )}
              </>
            )}
          </div>
        </div>
      </section>
    </div>
  );
}

