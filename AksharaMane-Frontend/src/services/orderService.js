import httpClient, {
  unwrapPagedResponse,
  unwrapResponse
} from "./httpClient";

function normalizeOrder(order) {
  return {
    ...order,

    id:
      order.id ??
      order.orderId,

    orderNumber:
      order.orderNumber || "",

    bookTitle:
      order.bookTitle ??
      order.book?.title ??
      "",

    quantity:
      Number(order.quantity || 0),

    unitPrice:
      Number(order.unitPrice || 0),

    totalAmount:
      Number(order.totalAmount || 0),

    status:
      normalizeStatus(order.status),

    paymentMethod:
      order.paymentMethod ||
      "CashOnDelivery",

    orderedAt:
      order.orderedAt ||
      order.createdAt,

    createdAt:
      order.createdAt ||
      order.orderedAt,

    canCancel:
      order.canCancel ?? false
  };
}

function normalizeStatus(status) {
  if (typeof status === "string") {
    return status;
  }

  const statuses = {
    0: "Placed",
    1: "Placed",
    2: "Confirmed",
    3: "Shipped",
    4: "Delivered",
    5: "Cancelled"
  };

  return statuses[status] || "Placed";
}

export async function createOrder(payload) {
  const response = await httpClient.post(
    "/orders",
    {
      bookId: Number(payload.bookId),
      quantity: Number(payload.quantity),
      customerName: payload.customerName.trim(),
      email: payload.email.trim(),
      phone: payload.phone.trim(),
      addressLine1:
        payload.addressLine1.trim(),
      addressLine2:
        payload.addressLine2?.trim() || null,
      city: payload.city.trim(),
      state: payload.state.trim(),
      pincode: payload.pincode.trim()
    }
  );

  return normalizeOrder(
    unwrapResponse(response)
  );
}

export async function trackOrder(payload) {
  const response = await httpClient.post(
    "/orders/track",
    {
      orderNumber:
        payload.orderNumber.trim(),

      emailOrPhone:
        payload.emailOrPhone.trim()
    }
  );

  return normalizeOrder(
    unwrapResponse(response)
  );
}

export async function cancelOrder(payload) {
  const response = await httpClient.post(
    "/orders/cancel",
    {
      orderNumber:
        payload.orderNumber.trim(),

      emailOrPhone:
        payload.emailOrPhone.trim(),

      reason:
        payload.reason?.trim() ||
        "Cancelled by customer"
    }
  );

  return normalizeOrder(
    unwrapResponse(response)
  );
}

export async function getOrders(
  filters = {}
) {
  const response = await httpClient.get(
    "/admin/orders",
    {
      params: {
        search:
          filters.search || undefined,

        status:
          filters.status || undefined,

        fromDate:
          filters.fromDate || undefined,

        toDate:
          filters.toDate || undefined,

        minimumAmount:
          filters.minimumAmount || undefined,

        maximumAmount:
          filters.maximumAmount || undefined,

        sortBy:
          filters.sortBy || "createdAt",

        sortDirection:
          filters.sortDirection || "desc",

        pageNumber:
          filters.pageNumber || 1,

        pageSize:
          filters.pageSize || 100
      }
    }
  );

  const paged = unwrapPagedResponse(response);

  return paged.items.map(normalizeOrder);
}

export async function getOrdersPaged(
  filters = {}
) {
  const response = await httpClient.get(
    "/admin/orders",
    {
      params: {
        ...filters
      }
    }
  );

  const result = unwrapPagedResponse(response);

  return {
    ...result,
    items: result.items.map(normalizeOrder)
  };
}

export async function getOrderById(id) {
  const response = await httpClient.get(
    `/admin/orders/${id}`
  );

  return normalizeOrder(
    unwrapResponse(response)
  );
}

export async function updateOrderStatus(
  orderId,
  status
) {
  const statusValue =
    typeof status === "number"
      ? status
      : statusNameToValue(status);

  const response = await httpClient.patch(
    `/admin/orders/${orderId}/status`,
    {
      status: statusValue
    }
  );

  return normalizeOrder(
    unwrapResponse(response)
  );
}

function statusNameToValue(status) {
  const values = {
    Placed: 1,
    Confirmed: 2,
    Shipped: 3,
    Delivered: 4,
    Cancelled: 5
  };

  const result = values[status];

  if (!result) {
    throw new Error(
      `Unsupported order status: ${status}`
    );
  }

  return result;
}