import { ArrowRight, CheckCircle2, LoaderCircle } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApiError, ordersApi } from "../api";
import { Field, PageTitle, inputClass } from "../components";
import type { CreateOrderPayload, FieldErrors } from "../types";
import { validateOrder } from "../validation";

const initialValues: CreateOrderPayload = {
  senderCity: "",
  senderAddress: "",
  recipientCity: "",
  recipientAddress: "",
  weight: 0,
  pickupDate: "",
};

export function NewOrderPage() {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState<FieldErrors>({});
  const [submitError, setSubmitError] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const navigate = useNavigate();
  const setValue = <K extends keyof CreateOrderPayload>(
    field: K,
    value: CreateOrderPayload[K],
  ) => {
    setValues((v) => ({ ...v, [field]: value }));
    setErrors((e) => ({ ...e, [field]: undefined }));
  };
  async function submit(event: React.FormEvent) {
    event.preventDefault();
    const clientErrors = validateOrder(values);
    if (Object.keys(clientErrors).length) {
      setErrors(clientErrors);
      return;
    }
    setSubmitting(true);
    setSubmitError("");
    try {
      const order = await ordersApi.create(values);
      navigate(`/orders/${order.id}`, { state: { created: true } });
    } catch (error) {
      if (error instanceof ApiError) {
        setErrors(error.fields);
        setSubmitError(error.message);
      } else setSubmitError("Не удалось создать заказ.");
    } finally {
      setSubmitting(false);
    }
  }
  return (
    <>
      <PageTitle eyebrow="Новый заказ" title="Оформление доставки" />
      <form
        onSubmit={submit}
        noValidate
        className="mx-auto max-w-3xl rounded-2xl border border-slate-200 bg-white p-5 shadow-sm sm:p-8"
      >
        <div className="grid gap-x-6 gap-y-5 sm:grid-cols-2">
          <section className="contents">
            <div className="sm:col-span-2">
              <h2 className="text-base font-semibold">Откуда забрать</h2>
              <p className="mt-1 text-sm text-slate-500">
                Укажите точку отправления груза.
              </p>
            </div>
            <Field label="Город отправителя" error={errors.senderCity}>
              <input
                className={inputClass(!!errors.senderCity)}
                value={values.senderCity}
                onChange={(e) => setValue("senderCity", e.target.value)}
                placeholder="Например, Москва"
              />
            </Field>
            <Field label="Адрес отправителя" error={errors.senderAddress}>
              <input
                className={inputClass(!!errors.senderAddress)}
                value={values.senderAddress}
                onChange={(e) => setValue("senderAddress", e.target.value)}
                placeholder="Улица, дом"
              />
            </Field>
          </section>
          <div className="sm:col-span-2 my-1 border-t border-slate-100" />
          <section className="contents">
            <div className="sm:col-span-2">
              <h2 className="text-base font-semibold">Куда доставить</h2>
              <p className="mt-1 text-sm text-slate-500">
                Укажите точку назначения.
              </p>
            </div>
            <Field label="Город получателя" error={errors.recipientCity}>
              <input
                className={inputClass(!!errors.recipientCity)}
                value={values.recipientCity}
                onChange={(e) => setValue("recipientCity", e.target.value)}
                placeholder="Например, Санкт-Петербург"
              />
            </Field>
            <Field label="Адрес получателя" error={errors.recipientAddress}>
              <input
                className={inputClass(!!errors.recipientAddress)}
                value={values.recipientAddress}
                onChange={(e) => setValue("recipientAddress", e.target.value)}
                placeholder="Проспект, дом"
              />
            </Field>
          </section>
          <div className="sm:col-span-2 my-1 border-t border-slate-100" />
          <Field label="Вес груза, кг" error={errors.weight}>
            <input
              className={inputClass(!!errors.weight)}
              type="number"
              min="0.001"
              max="1000000"
              step="0.001"
              value={values.weight || ""}
              onChange={(e) => setValue("weight", Number(e.target.value))}
              placeholder="0.000"
            />
          </Field>
          <Field label="Дата забора груза" error={errors.pickupDate}>
            <input
              className={inputClass(!!errors.pickupDate)}
              type="date"
              min={new Date().toISOString().slice(0, 10)}
              value={values.pickupDate}
              onChange={(e) => setValue("pickupDate", e.target.value)}
            />
          </Field>
        </div>
        {submitError && (
          <div
            role="alert"
            className="mt-6 rounded-lg bg-rose-50 p-3 text-sm text-rose-700"
          >
            {submitError}
          </div>
        )}
        <div className="mt-8 flex items-center justify-between border-t border-slate-100 pt-6">
          <p className="hidden text-sm text-slate-500 sm:block">
            <CheckCircle2 className="mr-1 inline size-4 text-teal-600" />
            Все поля обязательны
          </p>
          <button
            disabled={submitting}
            className="ml-auto inline-flex items-center gap-2 rounded-lg bg-teal-600 px-4 py-2.5 font-medium text-white shadow-sm transition hover:bg-teal-700 disabled:cursor-not-allowed disabled:opacity-70"
          >
            {submitting && <LoaderCircle className="size-4 animate-spin" />}
            Создать заказ
            <ArrowRight size={17} />
          </button>
        </div>
      </form>
    </>
  );
}
