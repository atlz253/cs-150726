import {
  ArrowRight,
  ChevronLeft,
  ChevronRight,
  ClipboardList,
  LoaderCircle,
  Plus,
  RefreshCw,
} from "lucide-react";
import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { ordersApi } from "../api";
import { PageTitle } from "../components";
import type { PagedOrders } from "../types";

const formatDate = (date: string) =>
  new Intl.DateTimeFormat("ru-RU", { dateStyle: "medium" }).format(
    new Date(`${date}T00:00:00`),
  );
export function OrdersListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [ordersPage, setOrdersPage] = useState<PagedOrders | null>(null);
  const [error, setError] = useState("");
  const [reloadKey, setReloadKey] = useState(0);
  const requestedPage = Number(searchParams.get("page") ?? "1");
  const page = Number.isInteger(requestedPage) && requestedPage > 0 ? requestedPage : 1;

  useEffect(() => {
    let cancelled = false;
    setError("");
    setOrdersPage(null);
    ordersApi
      .list(page)
      .then((result) => {
        if (cancelled) return;
        if (result.items.length === 0 && result.totalPages > 0 && page > result.totalPages) {
          setSearchParams({ page: String(result.totalPages) }, { replace: true });
          return;
        }
        setOrdersPage(result);
      })
      .catch((e) => {
        if (!cancelled) setError(e.message);
      });
    return () => {
      cancelled = true;
    };
  }, [page, reloadKey, setSearchParams]);

  const orders = ordersPage?.items;
  const changePage = (nextPage: number) => {
    setSearchParams(nextPage === 1 ? {} : { page: String(nextPage) });
  };
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
            onClick={() => setReloadKey((key) => key + 1)}
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
      {orders && orders.length > 0 && ordersPage && (
        <>
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
          <nav className="mt-5 flex items-center justify-between" aria-label="Пагинация заказов">
            <span className="text-sm text-slate-600">
              Страница {ordersPage.page} из {ordersPage.totalPages}
            </span>
            <div className="flex gap-2">
              <button
                onClick={() => changePage(ordersPage.page - 1)}
                disabled={ordersPage.page === 1}
                className="inline-flex items-center gap-1 rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm font-medium disabled:cursor-not-allowed disabled:opacity-50"
              >
                <ChevronLeft size={16} /> Назад
              </button>
              <button
                onClick={() => changePage(ordersPage.page + 1)}
                disabled={ordersPage.page === ordersPage.totalPages}
                className="inline-flex items-center gap-1 rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm font-medium disabled:cursor-not-allowed disabled:opacity-50"
              >
                Вперёд <ChevronRight size={16} />
              </button>
            </div>
          </nav>
        </>
      )}
    </>
  );
}
