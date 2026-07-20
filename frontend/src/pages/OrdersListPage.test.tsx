import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter, useLocation } from "react-router-dom";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { OrdersListPage } from "./OrdersListPage";

const { list } = vi.hoisted(() => ({ list: vi.fn() }));

vi.mock("../api", () => ({ ordersApi: { list } }));

function LocationSearch() {
  return <output data-testid="location-search">{useLocation().search}</output>;
}

describe("OrdersListPage", () => {
  beforeEach(() => {
    list.mockImplementation((page: number) =>
      Promise.resolve({
        items: [
          {
            orderNumber: page,
            senderCity: "Москва",
            senderAddress: "Тверская, 1",
            recipientCity: "Казань",
            recipientAddress: "Баумана, 2",
            weight: 1,
            pickupDate: "2026-08-01",
            createdAt: "2026-07-20T10:00:00Z",
          },
        ],
        page,
        pageSize: 20,
        totalCount: 21,
        totalPages: 2,
      }),
    );
  });

  it("loads the page from the URL and updates the URL when navigating", async () => {
    render(
      <MemoryRouter initialEntries={["/orders?page=2"]}>
        <OrdersListPage />
        <LocationSearch />
      </MemoryRouter>,
    );

    await waitFor(() => expect(list).toHaveBeenCalledWith(2));
    expect(screen.getByRole("button", { name: /вперёд/i })).toBeDisabled();

    fireEvent.click(screen.getByRole("button", { name: /назад/i }));

    await waitFor(() => expect(list).toHaveBeenLastCalledWith(1));
    expect(screen.getByTestId("location-search")).toHaveTextContent("");
  });
});
