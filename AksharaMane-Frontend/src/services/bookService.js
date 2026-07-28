import httpClient, {
  unwrapPagedResponse,
  unwrapResponse
} from "./httpClient";

function normalizeBook(book) {
  return {
    ...book,

    id:
      book.id ??
      book.bookId,

    title:
      book.title ??
      book.bookTitle ??
      "",

    author:
      book.author ??
      book.authorName ??
      "",

    categoryId:
      book.categoryId,

    categoryName:
      book.categoryName ??
      book.category?.name ??
      book.category?.categoryName ??
      "",

    price:
      Number(book.price || 0),

    stockQuantity:
      Number(
        book.stockQuantity ??
        book.stock ??
        0
      ),

    pages:
      Number(book.pages || 0),

    imageUrl:
      book.imageUrl || "",

    isActive:
      book.isActive ??
      book.active ??
      true,

    isAvailable:
      book.isAvailable ??
      Number(book.stockQuantity || 0) > 0
  };
}

export async function getBooks({
  categoryId,
  search = "",
  includeInactive = false,
  minimumPrice,
  maximumPrice,
  isAvailable,
  sortBy = "title",
  sortDirection = "asc",
  pageNumber = 1,
  pageSize = 100
} = {}) {
  const endpoint = includeInactive
    ? "/admin/books"
    : "/books";

  const response = await httpClient.get(endpoint, {
    params: {
      categoryId:
        categoryId || undefined,

      search:
        search.trim() || undefined,

      minimumPrice:
        minimumPrice || undefined,

      maximumPrice:
        maximumPrice || undefined,

      isAvailable:
        isAvailable ?? undefined,

      sortBy,
      sortDirection,
      pageNumber,
      pageSize
    }
  });

  const paged = unwrapPagedResponse(response);

  return paged.items.map(normalizeBook);
}

export async function getBooksPaged(
  filters = {}
) {
  const response = await httpClient.get(
    filters.includeInactive
      ? "/admin/books"
      : "/books",
    {
      params: {
        categoryId:
          filters.categoryId || undefined,

        search:
          filters.search?.trim() || undefined,

        minimumPrice:
          filters.minimumPrice || undefined,

        maximumPrice:
          filters.maximumPrice || undefined,

        isAvailable:
          filters.isAvailable ?? undefined,

        sortBy:
          filters.sortBy || "title",

        sortDirection:
          filters.sortDirection || "asc",

        pageNumber:
          filters.pageNumber || 1,

        pageSize:
          filters.pageSize || 12
      }
    }
  );

  const result = unwrapPagedResponse(response);

  return {
    ...result,
    items: result.items.map(normalizeBook)
  };
}

export async function getBookById(id) {
  const response = await httpClient.get(
    `/books/${id}`
  );

  return normalizeBook(
    unwrapResponse(response)
  );
}

export async function createBook(payload) {
  const formData = createBookFormData(payload);

  const response = await httpClient.post(
    "/admin/books",
    formData
  );

  return normalizeBook(
    unwrapResponse(response)
  );
}

export async function updateBook(id, payload) {
  const formData = createBookFormData(payload);

  const response = await httpClient.put(
    `/admin/books/${id}`,
    formData
  );

  return normalizeBook(
    unwrapResponse(response)
  );
}

export async function toggleBookStatus(id) {
  const response = await httpClient.patch(
    `/admin/books/${id}/toggle-status`
  );

  return normalizeBook(
    unwrapResponse(response)
  );
}

export async function deleteBook(id) {
  await httpClient.delete(
    `/admin/books/${id}`
  );
}

function createBookFormData(payload) {
  const formData = new FormData();

  formData.append("Title", payload.title || "");
  formData.append("Author", payload.author || "");

  formData.append(
    "CategoryId",
    String(payload.categoryId || "")
  );

  formData.append(
    "Price",
    String(payload.price || 0)
  );

  formData.append(
    "StockQuantity",
    String(payload.stockQuantity || 0)
  );

  formData.append(
    "Language",
    payload.language || ""
  );

  formData.append(
    "Pages",
    String(payload.pages || 0)
  );

  formData.append(
    "Publisher",
    payload.publisher || ""
  );

  formData.append(
    "Description",
    payload.description || ""
  );

  formData.append(
    "IsActive",
    String(payload.isActive ?? true)
  );

  if (payload.isbn !== undefined) {
    formData.append(
      "Isbn",
      payload.isbn || ""
    );
  }

  if (payload.image instanceof File) {
    formData.append("Image", payload.image);
  }

  return formData;
}