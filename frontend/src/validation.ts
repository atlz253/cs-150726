import type { CreateOrderPayload, FieldErrors } from "./types";

export function validateOrder(values: CreateOrderPayload): FieldErrors {
  const errors: FieldErrors = {};
  if (!values.senderCity.trim())
    errors.senderCity = "Укажите город отправителя";
  if (!values.senderAddress.trim())
    errors.senderAddress = "Укажите адрес отправителя";
  if (!values.recipientCity.trim())
    errors.recipientCity = "Укажите город получателя";
  if (!values.recipientAddress.trim())
    errors.recipientAddress = "Укажите адрес получателя";
  if (!Number.isFinite(values.weight) || values.weight <= 0)
    errors.weight = "Введите вес больше 0 кг";
  if (!values.pickupDate) errors.pickupDate = "Выберите дату забора";
  return errors;
}
