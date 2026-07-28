import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import {
  createCategory,
  deleteCategory,
  getCategories,
  updateCategory
} from "../../services/categoryService";

const emptyForm = {
  name: "",
  description: "",
  image: null,
  isActive: true
};

export default function AdminCategoriesPage() {
  const [categories, setCategories] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editingId, setEditingId] = useState(null);

  async function load() {
    setCategories(await getCategories({ includeInactive: true }));
  }

  useEffect(() => {
    load();
  }, []);

  function startEdit(category) {
    setEditingId(category.id);

    setForm({
      name: category.name,
      description: category.description || "",
      image: null,
      isActive: category.isActive
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
        await updateCategory(editingId, form);
        toast.success("Category updated.");
      } else {
        await createCategory(form);
        toast.success("Category created.");
      }

      resetForm();
      await load();
    } catch (error) {
      toast.error(error.message);
    }
  }

  async function handleDelete(id) {
    if (!window.confirm("Delete this category?")) return;

    try {
      await deleteCategory(id);
      toast.success("Category deleted.");
      await load();
    } catch (error) {
      toast.error(error.message);
    }
  }

  return (
    <>
      <h1 className="section-title">Category Management</h1>

      <div className="row g-3">
        <section className="col-lg-4">
          <div className="classic-panel">
            <div className="classic-panel-header">
              {editingId ? "Edit Category" : "Add Category"}
            </div>

            <form className="p-3" onSubmit={handleSubmit}>
              <div className="mb-3">
                <label className="form-label">Name *</label>
                <input
                  className="form-control"
                  value={form.name}
                  onChange={(event) =>
                    setForm((current) => ({
                      ...current,
                      name: event.target.value
                    }))
                  }
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Description</label>
                <textarea
                  className="form-control"
                  rows="4"
                  value={form.description}
                  onChange={(event) =>
                    setForm((current) => ({
                      ...current,
                      description: event.target.value
                    }))
                  }
                />
              </div>

              <div className="form-check mb-3">
                <input
                  className="form-check-input"
                  type="checkbox"
                  checked={form.isActive}
                  onChange={(event) =>
                    setForm((current) => ({
                      ...current,
                      isActive: event.target.checked
                    }))
                  }
                />
                <label className="form-check-label">Active</label>
              </div>

              <div className="d-flex gap-2">
                <button className="primary-button" type="submit">
                  {editingId ? "Update" : "Create"}
                </button>
                {editingId && (
                  <button className="classic-button" type="button" onClick={resetForm}>
                    Cancel
                  </button>
                )}
              </div>
            </form>
          </div>
        </section>

        <section className="col-lg-8">
          <div className="classic-panel">
            <div className="classic-panel-header">Category List</div>

            <div className="table-responsive">
              <table className="table table-bordered table-sm align-middle">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Status</th>
                    <th style={{ width: 150 }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {categories.map((category) => (
                    <tr key={category.id}>
                      <td>{category.name}</td>
                      <td>{category.description}</td>
                      <td>{category.isActive ? "Active" : "Inactive"}</td>
                      <td>
                        <button
                          className="classic-button me-2"
                          onClick={() => startEdit(category)}
                        >
                          Edit
                        </button>
                        <button
                          className="danger-button"
                          onClick={() => handleDelete(category.id)}
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </section>
      </div>
    </>
  );
}
