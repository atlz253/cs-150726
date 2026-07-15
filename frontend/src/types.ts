export type Order = {
  id: string;
  orderNumber: string;
  senderCity: string;
  senderAddress: string;
  recipientCity: string;
  recipientAddress: string;
  weight: number;
  pickupDate: string;
  createdAt: string;
};

export type CreateOrderPayload = Omit<
  Order,
  "id" | "orderNumber" | "createdAt"
>;
export type FieldErrors = Partial<Record<keyof CreateOrderPayload, string>>;
