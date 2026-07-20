import {
  ArrowLeft,
  CalendarDays,
  CheckCircle2,
  LoaderCircle,
  MapPin,
  Package,
} from "lucide-react";
import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import { ordersApi } from "../api";
import { PageTitle } from "../components";
import type { Order } from "../types";

const date = (value: string) =>
  new Intl.DateTimeFormat("ru-RU", { dateStyle: "long" }).format(
    new Date(`${value}T00:00:00`),
  );
function Point({
  title,
  city,
  address,
}: {
  title: string;
  city: string;
  address: string;
}) {
  return (
    <div className="rounded-xl border border-slate-200 p-5">
      <p className="text-sm font-medium text-slate-500">{title}</p>
      <div className="mt-3 flex gap-3">
        <MapPin className="mt-0.5 size-5 shrink-0 text-teal-600" />
        <div>
          <p className="font-semibold">{city}</p>
          <p className="mt-0.5 text-sm text-slate-600">{address}</p>
        </div>
      </div>
    </div>
  );
}
export function OrderDetailsPage() {
  const { id } = useParams();
  const location = useLocation();
  const [order, setOrder] = useState<Order | null>(null);
  const [error, setError] = useState("");
  useEffect(() => {
    if (id)
      ordersApi
        .get(id)
        .then(setOrder)
        .catch((e) => setError(e.message));
  }, [id]);
  if (error)
    return (
      <div className="py-16 text-center">
        <h1 className="text-xl font-bold">{error}</h1>
        <Link
          className="mt-4 inline-flex items-center gap-2 text-teal-700"
          to="/orders"
        >
          <ArrowLeft size={16} />К списку заказов
        </Link>
      </div>
    );
  if (!order)
    return (
      <div className="grid min-h-60 place-items-center">
        <LoaderCircle className="size-7 animate-spin text-teal-600" />
      </div>
    );
  return (
    <>
      <PageTitle eyebrow="Заказ" title={String(order.orderNumber)}>
        <Link
          to="/orders"
          className="inline-flex items-center gap-2 text-sm font-medium text-slate-600 hover:text-teal-700"
        >
          <ArrowLeft size={16} />
          Все заказы
        </Link>
      </PageTitle>
      {location.state?.created && (
        <div className="mb-6 flex items-center gap-2 rounded-xl border border-teal-200 bg-teal-50 px-4 py-3 text-sm font-medium text-teal-800">
          <CheckCircle2 size={18} />
          Заказ успешно создан.
        </div>
      )}
      <div className="grid gap-6 lg:grid-cols-3">
        <section className="space-y-4 lg:col-span-2">
          <Point
            title="Отправитель"
            city={order.senderCity}
            address={order.senderAddress}
          />
          <Point
            title="Получатель"
            city={order.recipientCity}
            address={order.recipientAddress}
          />
        </section>
        <aside className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
          <h2 className="font-semibold">Параметры груза</h2>
          <dl className="mt-5 space-y-5">
            <div className="flex gap-3">
              <Package className="size-5 text-teal-600" />
              <div>
                <dt className="text-sm text-slate-500">Вес</dt>
                <dd className="mt-0.5 font-semibold">{order.weight} кг</dd>
              </div>
            </div>
            <div className="flex gap-3">
              <CalendarDays className="size-5 text-teal-600" />
              <div>
                <dt className="text-sm text-slate-500">Дата забора</dt>
                <dd className="mt-0.5 font-semibold">
                  {date(order.pickupDate)}
                </dd>
              </div>
            </div>
          </dl>
        </aside>
      </div>
    </>
  );
}
