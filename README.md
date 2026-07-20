# DeliveryFlow

Тестовое веб-приложение для оформления и просмотра заказов на доставку. Включает форму создания заказа, список заказов и карточку заказа в режиме чтения.

## Стек

- ASP.NET Core 9 Minimal API, Entity Framework Core и SQLite
- React, TypeScript, Vite и Tailwind CSS
- xUnit для backend-тестов и Vitest для frontend-тестов

## Требования

- .NET SDK 9.0+
- Node.js 20+

## Запуск

В первом терминале запустите API:

```powershell
cd backend/DeliveryOrders.Api
dotnet restore
dotnet run --urls http://localhost:5050
```

При первом запуске автоматически применяется EF Core-миграция и рядом с проектом создаётся файл `delivery-orders.db`.

Во втором терминале запустите клиент:

```powershell
cd frontend
npm install
npm run dev
```

Откройте http://localhost:5173. Swagger API доступен по адресу http://localhost:5050/swagger.

## Проверки

```powershell
dotnet test DeliveryOrders.sln
cd frontend; npm test; npm run build
```

## API

| Метод | Маршрут | Назначение |
| --- | --- | --- |
| `POST` | `/api/orders` | Создать заказ |
| `GET` | `/api/orders?page=1` | Получить страницу списка (по 20 заказов, сначала новые) |
| `GET` | `/api/orders/{orderNumber}` | Получить карточку заказа |

Номер заказа создаётся базой данных как последовательное целое число. Список возвращает `items` и метаданные пагинации: `page`, `pageSize`, `totalCount`, `totalPages`. Все поля обязательны; вес должен быть положительным.
