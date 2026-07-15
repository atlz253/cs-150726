import type { ReactNode } from "react";

export function PageTitle({
  eyebrow,
  title,
  children,
}: {
  eyebrow: string;
  title: string;
  children?: ReactNode;
}) {
  return (
    <div className="mb-8 flex flex-wrap items-end justify-between gap-4">
      <div>
        <p className="mb-1 text-sm font-semibold uppercase tracking-widest text-teal-700">
          {eyebrow}
        </p>
        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          {title}
        </h1>
      </div>
      {children}
    </div>
  );
}

export function Field({
  label,
  error,
  children,
}: {
  label: string;
  error?: string;
  children: ReactNode;
}) {
  return (
    <label className="block text-sm font-medium text-slate-700">
      <span className="mb-1.5 block">
        {label} <span className="text-rose-600">*</span>
      </span>
      {children}
      {error && (
        <span role="alert" className="mt-1.5 block text-sm text-rose-600">
          {error}
        </span>
      )}
    </label>
  );
}

export const inputClass = (hasError?: boolean) =>
  `w-full rounded-lg border bg-white px-3 py-2.5 text-slate-900 outline-none transition placeholder:text-slate-400 focus:ring-3 ${hasError ? "border-rose-400 focus:border-rose-500 focus:ring-rose-100" : "border-slate-300 focus:border-teal-600 focus:ring-teal-100"}`;
