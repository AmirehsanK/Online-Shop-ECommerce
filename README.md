# Online Shop

[![CI](https://github.com/AmirehsanK/Online-Shop-ECommerce/actions/workflows/ci.yml/badge.svg)](https://github.com/AmirehsanK/Online-Shop-ECommerce/actions/workflows/ci.yml)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-10-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED)

A Persian-language online store built with ASP.NET Core MVC: product catalogue with
colour variants and stock, discounts, basket and checkout with card payment or an
in-site wallet, customer panel with tickets and notifications, and an admin panel
with role-based permissions.


![Browsing the store, signing in, adding to the basket, paying from the wallet, and the admin dashboard](docs/demo.gif)

## What it does

**Storefront** — browse products through a category mega menu and category slider,
filter the product list, and open a product page with its gallery, colour variants
(each with its own price and stock), specifications, customer comments with ratings,
and questions and answers. Discounted products appear on the home page with a
countdown, and customers can keep a favourites list.

**Accounts** — sign up with email activation, sign in, and reset a forgotten password
by email. Sign-in, sign-up and the contact form are protected by Google reCAPTCHA.

**Basket and checkout** — add products in a chosen colour, review the basket, pick a
delivery address, then pay by card through the payment gateway or from the wallet.
Customers top up their wallet by card, and stock is reserved as items go into the
basket.

**Customer panel** — edit the profile, change the password, check the wallet balance,
open support tickets and follow their replies, and read notifications from the store.

**Admin panel** — manage products, categories, colours, specifications and galleries;
home page slider and banners; discounts, including assigning them to products or
users; users, roles and fine-grained permissions (each admin page and action is
guarded by its own permission); support tickets, contact messages, comments,
product questions, FAQs and notifications. The dashboard shows sales, paid orders,
open tickets and active discounts.

## How it is put together

| Project | Responsibility |
| --- | --- |
| `Domain` | Entities, view models, repository interfaces. |
| `Infra.Data` | EF Core `ApplicationDbContext`, repositories, migrations, permission seed data. |
| `Application` | Services with the business rules: checkout and payments, users and passwords, products, discounts, tickets. |
| `IOC` | Dependency registration; picks the email sender and payment gateway from configuration. |
| `Web` | MVC site with `Admin` and `UserPanel` areas, permission filter, demo data seeder. |
| `Tests` | xUnit: services against in-memory SQLite, and the whole site booted in memory and driven over HTTP. |

Payments go through `IPaymentGateway`: `NovinoPaymentGateway` for the real provider, or
`DemoPaymentGateway`, which shows a local "bank" page that posts to the same callback, so
checkout can be exercised end to end without a merchant account.

## Running it with Docker

```bash
docker compose up --build
```

| | |
| --- | --- |
| Shop | http://localhost:8080 (set `APP_PORT` to change) |
| Customer | `customer@shop.local` / `Customer#12345`, with wallet credit |
| Admin | `admin@shop.local` / `Admin#12345`, panel at `/Admin` |
| Telemetry | http://localhost:18888: traces, metrics and logs (OpenTelemetry, .NET Aspire dashboard) |
| Health | http://localhost:8080/health/ready |

This starts SQL Server 2022, applies the migrations, seeds demo products, and runs the
shop with the demo payment gateway. reCAPTCHA is off, and emails (activation, password
reset) are written to `docker compose logs app`.

## Running it locally

Requires the .NET 10 SDK and SQL Server. The connection string is in
`Web/appsettings.json`.

```bash
dotnet tool restore
```

```bash
dotnet dotnet-ef database update --project Infra.Data --startup-project Web
```

```bash
dotnet run --project Web
```

The `Development` environment uses the demo payment gateway and turns reCAPTCHA off. Add
`Database__SeedDemoData=true` for sample data. For anything real, set secrets outside the
repository:

```bash
dotnet user-secrets set "GoogleRecaptcha:SecretKey" "<key>" --project Web
```

The same goes for `GoogleRecaptcha:SiteKey`, `Smtp:Host`, `Smtp:UserName`,
`Smtp:Password`, and `Payment:MerchantId`. Without an SMTP host, emails are logged
instead of sent.

To run without SQL Server, set `Database__Provider=Sqlite` and
`ConnectionStrings__DefaultConnection="Data Source=shop.db"`. The schema is created
on start.

## Tests

```bash
dotnet test
```

- **Checkout and payments:** basket totals, wallet balances, card and wallet payments, wallet top-ups, repeated gateway callbacks, and a basket edited during payment.
- **Stock:** reserving the last unit, and rejecting a colour that belongs to another product.
- **Accounts:** password hashing, sign-in, single-use password reset links, and profile updates.
- **Over HTTP:** the whole site booted in memory — admin permissions for visitors, customers and administrators, the profile form, wallet checkout, and the card payment callback.

[CI](.github/workflows/ci.yml) runs the tests, checks that the migrations match the
model, and brings the Docker Compose stack up until `/health/ready` passes.

## Known limitations

- Discounts are shown on product cards but not yet applied to the basket total.
- The payment success page shows a placeholder order number.
