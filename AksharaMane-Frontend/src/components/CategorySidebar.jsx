export default function CategorySidebar({
  categories,
  selectedCategoryId,
  onSelect
}) {
  return (
    <aside className="classic-panel">
      <div className="classic-panel-header">Categories</div>
      <div className="category-list">
        <button
          type="button"
          className={!selectedCategoryId ? "active" : ""}
          onClick={() => onSelect("")}
        >
          All Categories
        </button>

        {categories.map((category) => (
          <button
            key={category.id}
            type="button"
            className={
              Number(selectedCategoryId) === category.id ? "active" : ""
            }
            onClick={() => onSelect(category.id)}
          >
            {category.name}
          </button>
        ))}
      </div>
    </aside>
  );
}
