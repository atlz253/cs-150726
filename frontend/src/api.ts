import type { CreateOrderPayload, FieldErrors, Order } from "./types";

const baseUrl = import.meta.env.VITE_API_URL ?? "http://localhost:5050/api";

class ApiError extends Error {
  constructor(
    message: string,
    readonly fields: FieldErrors = {},
  ) {
    super(message);
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  let response: Response;
  try {
    response = await fetch(`${baseUrl}${path}`, init);
  } catch {
    throw new ApiError(
      "Не удалось связаться с сервером. Проверьте, что backend запущен.",
    );
  }
  if (response.ok) return response.json() as Promise<T>;
  if (response.status === 404) throw new ApiError("Заказ не найден.");
  const body = (await response.json().catch(() => null)) as {
    detail?: string;
    errors?: Record<string, string[]>;
  } | null;
  const fields = Object.fromEntries(
    Object.entries(body?.errors ?? {}).map(([key, messages]) => [
      key,
      messages[0],
    ]),
  );
  throw new ApiError(
    body?.detail ?? "Не удалось выполнить запрос. Повторите попытку.",
    fields,
  );
}

export const ordersApi = {
  list: () => request<Order[]>("/orders"),
  get: (id: string) => request<Order>(`/orders/${id}`),
  create: (payload: CreateOrderPayload) =>
    request<Order>("/orders", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    }),
};

export { ApiError };
