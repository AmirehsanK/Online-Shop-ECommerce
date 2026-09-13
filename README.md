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

فروشگاه اینترنتی فارسی با ASP.NET Core MVC — سبد خرید، کیف پول، درگاه پرداخت و پنل مدیریت با سطح دسترسی.

![Browsing the store, signing in, adding to the basket, paying from the wallet, and the admin dashboard](docs/demo.gif)

## Security and correctness fixes

The shop worked in a happy-path demo but had holes that matter the moment it takes real
money. Each fix below has a test that fails against the old code.

| Problem | Impact | Fix |
| --- | --- | --- |
| The permission filter wrote a redirect but never set `context.Result`, so MVC ran the action anyway. The admin dashboard had no check at all. | **Any signed-in customer could perform every admin action**, and anyone could read tickets and contact messages. | Short-circuit with `ChallengeResult` / 403; dedicated `AdminPanel` permission. |
| Wallet checkout was `GET ?amount=…&userId=…`. | Pay any basket for 1 toman, or with **someone else's wallet**. | `POST` with antiforgery; the server computes the total for the signed-in user. |
| The wallet repository returned **every user's** transactions. | Each balance was the whole store's deposits. | Filter by user. |
| The payment callback trusted the user id in the request and closed whatever basket was open. | Topping up a wallet marked the basket as paid; a card purchase also credited the wallet. | Payments carry their `OrderId`; the callback works only from the stored transaction, verifies the recorded amount, and is idempotent. |
| Stock was decremented after adding to the basket, with no check. | Negative stock; sold-out items still sellable. | One conditional `UPDATE … WHERE Count > 0`. |
| The profile form was bound to the admin edit model. | Customers could post `IsAdmin=true` or another person's email; saving the profile **deactivated the account**. | Separate profile update touching only profile fields. |
| Two incompatible password hash formats; the edit form round-tripped the stored hash; admin-created users were stored in plain text. | Password reset and change never worked; editing a user locked them out. | One PBKDF2-SHA256 (600k) hasher that still reads both old formats and upgrades them at sign-in. |
| Password-reset links never expired; the form posted to a non-existent action. | Reset was broken, and an old email link worked forever. | Working form, single-use token, no account enumeration. |
| reCAPTCHA secret and SMTP credentials in source. | Secrets in a public repository. | Configuration only (user secrets / environment variables). |

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

- **Checkout and payments:** server-side totals, balance checks, wallet isolation between users, card payments not crediting the wallet, idempotent callbacks, a basket edited mid-payment.
- **Stock:** the last unit sells once; a colour from another product is rejected.
- **Passwords:** both legacy formats verify and upgrade; reset works and is single-use; admin-created users are hashed; the stored hash never reaches a form.
- **Over HTTP:** admin pages for anonymous users, customers and the administrator; profile mass assignment; wallet checkout ignoring a tampered amount and user id; antiforgery; a repeated gateway callback.

[CI](.github/workflows/ci.yml) runs the tests, checks that the migrations match the
model, and brings the Docker Compose stack up until `/health/ready` passes.

## Known limitations

- Discounts are shown on product cards but not yet applied to the basket total.
- The payment success page shows a placeholder order number.
