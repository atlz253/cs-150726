import { describe, expect, it } from "vitest";
import { validateOrder } from "./validation";

describe("validateOrder", () => {
  it("reports every required field for an empty form", () => {
    expect(
      Object.keys(
        validateOrder({
          senderCity: "",
          senderAddress: "",
          recipientCity: "",
          recipientAddress: "",
          weight: 0,
          pickupDate: "",
        }),
      ),
    ).toHaveLength(6);
  });
  it("accepts a complete order", () => {
    expect(
      validateOrder({
        senderCity: "Москва",
        senderAddress: "ул. Тверская, 1",
        recipientCity: "Казань",
        recipientAddress: "ул. Баумана, 2",
        weight: 12.5,
        pickupDate: "2026-08-01",
      }),
    ).toEqual({});
  });
});
