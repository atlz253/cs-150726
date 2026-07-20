export type Order = {
  orderNumber: number;
  senderCity: string;
  senderAddress: string;
  recipientCity: string;
  recipientAddress: string;
  weight: number;
  pickupDate: string;
  createdAt: string;
};

export type PagedOrders = {
  items: Order[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

export type CreateOrderPayload = Omit<
  Order,
  "orderNumber" | "createdAt"
>;
export type FieldErrors = Partial<Record<keyof CreateOrderPayload, string>>;
