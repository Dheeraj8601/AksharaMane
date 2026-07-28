import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import {
  createBook,
  deleteBook,
  getBooks,
  updateBook
} from "../../services/bookService";
import { getCategories } from "../../services/categoryService";
import { formatCurrency } from "../../utils/currency";

const emptyForm = {
  title: "",
  author: "",
  categoryId: "",
  price: "",
  stockQuantity: "",
  language: "English",
  pages: "",
  publisher: "",
  description: "",
  imageUrl: null,
  isActive: true
};

export default function AdminBooksPage() {
  const [books, setBooks] = useState([]);
  const [categories, setCategories] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editingId, setEditingId] = useState(null);

  async function load() {
    const [booksData, categoriesData] = await Promise.all([
      getBooks({ includeInactive: true }),
      getCategories({ includeInactive: true })
    ]);

    setBooks(booksData);
    setCategories(categoriesData);
  }

  useEffect(() => {
    load();
  }, []);

  function handleChange(event) {
    const {
      name,
      value,
      type,
      checked,
      files
    } = event.target;

    setForm((current) => ({
      ...current,

      [name]:
        type === "checkbox"
          ? checked
          : type === "file"
            ? files?.[0] || null
            : value
    }));
  }

  function startEdit(book) {
    setEditingId(book.id);
    setForm({
      title: book.title,
      author: book.author,
      categoryId: book.categoryId,
      price: book.price,
      stockQuantity: book.stockQuantity,
      language: book.language,
      pages: book.pages,
      publisher: book.publisher,
      description: book.description,
      imageUrl: null,
      isActive: book.isActive
    });
  }

  function resetForm() {
    setEditingId(null);
    setForm(emptyForm);
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      if (editingId) {
        await updateBook(editingId, form);
        toast.success("Book updated.");
      } else {
        await createBook(form);
        toast.success("Book created.");
      }

      resetForm();
      await load();
    } catch (error) {
      toast.error(error.message);
    }
  }

  async function handleDelete(id) {
    if (!window.confirm("Delete this book?")) return;

    try {
      await deleteBook(id);
      toast.success("Book deleted.");
      await load();
    } catch (error) {
      toast.error(error.message);
    }
  }

  return (
    <>
      <h1 className="section-title">Book Management</h1>

      <div className="classic-panel mb-3">
        <div className="classic-panel-header">
          {editingId ? "Edit Book" : "Add Book"}
        </div>

        <form className="p-3" onSubmit={handleSubmit}>
          <div className="row g-3">
            <div className="col-md-4">
              <label className="form-label">Title *</label>
              <input className="form-control" name="title" value={form.title} onChange={handleChange} required />
            </div>
            <div className="col-md-4">
              <label className="form-label">Author *</label>
              <input className="form-control" name="author" value={form.author} onChange={handleChange} required />
            </div>
            <div className="col-md-4">
              <label className="form-label">Category *</label>
              <select className="form-select" name="categoryId" value={form.categoryId} onChange={handleChange} required>
                <option value="">Select category</option>
                {categories.map((category) => (
                  <option value={category.id} key={category.id}>{category.name}</option>
                ))}
              </select>
            </div>

            <div className="col-md-3">
              <label className="form-label">Price *</label>
              <input className="form-control" type="number" min="1" name="price" value={form.price} onChange={handleChange} required />
            </div>
            <div className="col-md-3">
              <label className="form-label">Stock *</label>
              <input className="form-control" type="number" min="0" name="stockQuantity" value={form.stockQuantity} onChange={handleChange} required />
            </div>
            <div className="col-md-3">
              <label className="form-label">Language</label>
              <input className="form-control" name="language" value={form.language} onChange={handleChange} />
            </div>
            <div className="col-md-3">
              <label className="form-label">Pages</label>
              <input className="form-control" type="number" min="0" name="pages" value={form.pages} onChange={handleChange} />
            </div>

            <div className="col-md-4">
              <label className="form-label">Publisher</label>
              <input className="form-control" name="publisher" value={form.publisher} onChange={handleChange} />
            </div>
            <div className="col-md-8">
              <label className="form-label">
                Book Cover
              </label>

              <input
                className="form-control"
                type="file"
                name="image"
                accept=".jpg,.jpeg,.png,.webp"
                onChange={handleChange}
              />

              {editingId && (
                <div className="form-text">
                  Leave empty to keep the existing image.
                </div>
              )}
            </div>

            <div className="col-12">
              <label className="form-label">Description</label>
              <textarea className="form-control" rows="3" name="description" value={form.description} onChange={handleChange} />
            </div>

            <div className="col-12">
              <div className="form-check">
                <input className="form-check-input" type="checkbox" name="isActive" checked={form.isActive} onChange={handleChange} />
                <label className="form-check-label">Active</label>
              </div>
            </div>
          </div>

          <div className="d-flex gap-2 mt-3">
            <button className="primary-button" type="submit">
              {editingId ? "Update Book" : "Create Book"}
            </button>
            {editingId && (
              <button className="classic-button" type="button" onClick={resetForm}>
                Cancel
              </button>
            )}
          </div>
        </form>
      </div>

      <div className="classic-panel">
        <div className="classic-panel-header">Book List</div>

        <div className="table-responsive">
          <table className="table table-bordered table-sm align-middle">
            <thead>
              <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Category</th>
                <th>Price</th>
                <th>Stock</th>
                <th>Status</th>
                <th style={{ width: 150 }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {books.map((book) => (
                <tr key={book.id}>
                  <td>{book.title}</td>
                  <td>{book.author}</td>
                  <td>{book.categoryName}</td>
                  <td>{formatCurrency(book.price)}</td>
                  <td>{book.stockQuantity}</td>
                  <td>{book.isActive ? "Active" : "Inactive"}</td>
                  <td>
                    <button className="classic-button me-2" onClick={() => startEdit(book)}>Edit</button>
                    <button className="danger-button" onClick={() => handleDelete(book.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
