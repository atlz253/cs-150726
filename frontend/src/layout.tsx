import { ClipboardList, Plus } from "lucide-react";
import { Link, NavLink, Outlet } from "react-router-dom";

export function Layout() {
  const navClass = ({ isActive }: { isActive: boolean }) =>
    `rounded-lg px-3 py-2 text-sm font-medium transition ${isActive ? "bg-teal-50 text-teal-700" : "text-slate-600 hover:bg-slate-100 hover:text-slate-900"}`;
  return (
    <div className="min-h-screen">
      <header className="border-b border-slate-200 bg-white">
        <div className="mx-auto flex max-w-6xl items-center justify-between gap-4 px-4 py-4 sm:px-6">
          <Link
            to="/orders"
            className="flex items-center gap-2 font-semibold tracking-tight text-slate-900"
          >
            <span className="grid size-9 place-items-center rounded-xl bg-teal-600 text-white">
              <ClipboardList size={19} />
            </span>
            DeliveryFlow
          </Link>
          <nav className="flex items-center gap-1">
            <NavLink to="/orders" className={navClass}>
              Заказы
            </NavLink>
            <NavLink
              to="/orders/new"
              className="inline-flex items-center gap-1 rounded-lg bg-teal-600 px-3 py-2 text-sm font-medium text-white shadow-sm transition hover:bg-teal-700"
            >
              <Plus size={16} />
              Новый заказ
            </NavLink>
          </nav>
        </div>
      </header>
      <main className="mx-auto max-w-6xl px-4 py-8 sm:px-6">
        <Outlet />
      </main>
    </div>
  );
}
