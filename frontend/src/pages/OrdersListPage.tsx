import {
  ArrowRight,
  ClipboardList,
  LoaderCircle,
  Plus,
  RefreshCw,
} from "lucide-react";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ordersApi } from "../api";
import { PageTitle } from "../components";
import type { Order } from "../types";

const formatDate = (date: string) =>
  new Intl.DateTimeFormat("ru-RU", { dateStyle: "medium" }).format(
    new Date(`${date}T00:00:00`),
  );
export function OrdersListPage() {
  const [orders, setOrders] = useState<Order[] | null>(null);
  const [error, setError] = useState("");
  const load = () => {
    setError("");
    setOrders(null);
    ordersApi
      .list()
      .then(setOrders)
      .catch((e) => setError(e.message));
  };
  useEffect(load, []);
  return (
    <>
      <PageTitle eyebrow="Все заказы" title="Доставки">
        <Link
          to="/orders/new"
          className="inline-flex items-center gap-2 rounded-lg bg-teal-600 px-4 py-2.5 text-sm font-medium text-white shadow-sm hover:bg-teal-700"
        >
          <Plus size={17} />
          Создать заказ
        </Link>
      </PageTitle>
      {orders === null && !error && (
        <div className="grid min-h-60 place-items-center">
          <LoaderCircle className="size-7 animate-spin text-teal-600" />
        </div>
      )}
      {error && (
        <div className="rounded-2xl border border-rose-200 bg-rose-50 p-6 text-rose-800">
          <p className="font-semibold">Не удалось загрузить заказы</p>
          <p className="mt-1 text-sm">{error}</p>
          <button
            onClick={load}
            className="mt-4 inline-flex items-center gap-2 rounded-lg border border-rose-300 bg-white px-3 py-2 text-sm font-medium"
          >
            <RefreshCw size={15} />
            Повторить
          </button>
        </div>
      )}
      {orders?.length === 0 && (
        <div className="rounded-2xl border border-dashed border-slate-300 bg-white px-6 py-16 text-center">
          <span className="mx-auto grid size-12 place-items-center rounded-xl bg-teal-50 text-teal-700">
            <ClipboardList size={24} />
          </span>
          <h2 className="mt-4 text-lg font-semibold">Заказов пока нет</h2>
          <p className="mt-1 text-sm text-slate-500">
            Создайте первый заказ на доставку.
          </p>
          <Link
            to="/orders/new"
            className="mt-5 inline-flex items-center gap-2 text-sm font-semibold text-teal-700 hover:text-teal-800"
          >
            Оформить заказ <ArrowRight size={16} />
          </Link>
        </div>
      )}
      {orders && orders.length > 0 && (
        <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          <div className="hidden grid-cols-[1.2fr_1fr_1fr_0.6fr_0.8fr] gap-4 border-b border-slate-200 bg-slate-50 px-5 py-3 text-xs font-semibold uppercase tracking-wide text-slate-500 md:grid">
            <span>Номер заказа</span>
            <span>Откуда</span>
            <span>Куда</span>
            <span>Вес</span>
            <span>Дата забора</span>
          </div>
          <div>
            {orders.map((order) => (
              <Link
                key={order.orderNumber}
                to={`/orders/${order.orderNumber}`}
                className="group block border-b border-slate-100 px-5 py-4 last:border-0 hover:bg-teal-50/40"
              >
                <div className="grid gap-2 md:grid-cols-[1.2fr_1fr_1fr_0.6fr_0.8fr] md:items-center md:gap-4">
                  <span className="font-semibold text-teal-700 group-hover:underline">
                    {order.orderNumber}
                  </span>
                  <span className="text-sm">
                    <span className="md:hidden text-slate-500">Откуда: </span>
                    {order.senderCity}
                  </span>
                  <span className="text-sm">
                    <span className="md:hidden text-slate-500">Куда: </span>
                    {order.recipientCity}
                  </span>
                  <span className="text-sm">{order.weight} кг</span>
                  <span className="text-sm text-slate-600">
                    {formatDate(order.pickupDate)}
                  </span>
                </div>
              </Link>
            ))}
          </div>
        </div>
      )}
    </>
  );
}
