import httpClient, {
  unwrapPagedResponse,
  unwrapResponse
} from "./httpClient";

function normalizeCategory(category) {
  return {
    ...category,

    id:
      category.id ??
      category.categoryId,

    name:
      category.name ??
      category.categoryName ??
      "",

    description:
      category.description ?? "",

    imageUrl:
      category.imageUrl ?? "",

    isActive:
      category.isActive ??
      category.active ??
      true
  };
}

export async function getCategories({
  includeInactive = false,
  search = "",
  pageNumber = 1,
  pageSize = 100
} = {}) {
  const endpoint = includeInactive
    ? "/admin/categories"
    : "/categories";

  const response = await httpClient.get(endpoint, {
    params: {
      search: search || undefined,
      pageNumber,
      pageSize
    }
  });

  const paged = unwrapPagedResponse(response);

  return paged.items.map(normalizeCategory);
}

export async function getCategoryById(id) {
  const response = await httpClient.get(
    `/categories/${id}`
  );

  return normalizeCategory(
    unwrapResponse(response)
  );
}

export async function createCategory(payload) {
  //const formData = createCategoryFormData(payload);

  const response = await httpClient.post(
    "/admin/categories",
    payload
  );

  return normalizeCategory(
    unwrapResponse(response)
  );
}

export async function updateCategory(
  id,
  payload
) {
  //const formData = createCategoryFormData(payload);

  const response = await httpClient.put(
    `/admin/categories/${id}`,
    payload
  );

  return normalizeCategory(
    unwrapResponse(response)
  );
}

export async function toggleCategoryStatus(id) {
  const response = await httpClient.patch(
    `/admin/categories/${id}/toggle-status`
  );

  return normalizeCategory(
    unwrapResponse(response)
  );
}

export async function deleteCategory(id) {
  await httpClient.delete(
    `/admin/categories/${id}`
  );
}

function createCategoryFormData(payload) {
  const formData = new FormData();

  formData.append(
    "CategoryName",
    payload.name || payload.categoryName || ""
  );

  formData.append(
    "Description",
    payload.description || ""
  );

  formData.append(
    "IsActive",
    String(payload.isActive ?? true)
  );

  if (payload.image instanceof File) {
    formData.append("Image", payload.image);
  }

  return formData;
}