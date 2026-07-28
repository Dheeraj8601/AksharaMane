export const ORDER_STATUSES = [
  "Placed",
  "Confirmed",
  "Shipped",
  "Delivered",
  "Cancelled"
];

export const ORDER_STATUS_VALUES = {
  Placed: 1,
  Confirmed: 2,
  Shipped: 3,
  Delivered: 4,
  Cancelled: 5
};

export function getStatusClass(
  status = ""
) {
  return `status-${String(status)
    .toLowerCase()
    .replaceAll(" ", "-")}`;
}