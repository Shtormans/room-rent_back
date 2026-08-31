# Room Booking API

An ASP.NET Core 8 REST API for managing conference rooms, optional services, reservations, time-based pricing, and operational reports. It supports a conference-room rental business: customers can find a room that fits their event and make a non-overlapping reservation, while administrators maintain the catalogue and analyse demand.

## Business objectives

The API helps the business:

- maintain a catalogue of rooms, capacities, hourly rates, and available services;
- prevent two bookings from occupying the same room at overlapping times;
- calculate a transparent reservation price from the duration, selected services, and an active time discount;
- identify popular booking hours and the services used for each room;
- separate public catalogue/booking operations from administrator-only management operations.

The required initial catalogue can be created through the API:

| Resource | Suggested data |
| --- | --- |
| Rooms | Room A — 50 people, UAH 2,000/hour; Room B — 100 people, UAH 3,500/hour; Room C — 30 people, UAH 1,500/hour |
| Services | Projector — UAH 500; Wi-Fi — UAH 300; Sound — UAH 700 |
| Time rules | Morning 06:00–09:00: 10% discount; evening 18:00–23:00: 20% discount |

> The database is initially empty. Create services first, then create rooms and associate their service IDs. Time discounts are configured through the discounts API. A peak-hour surcharge is a business rule from the original brief, but the current implementation supports discounts only (0–100%), not positive surcharges.

## Technical solution

The solution follows a layered, Clean Architecture-inspired design:

```text
WebApi          HTTP controllers, request/response models, JWT and Swagger
Application     Commands, queries, business workflows (MediatR)
Domain          Entities, value objects, result/error types, repository contracts
Infrastructure  Entity Framework Core, SQL Server repositories, database mappings
```

Key design decisions:

- **CQRS with MediatR:** commands change state; queries retrieve data.
- **Domain validation:** value objects and command handlers validate key business rules before persistence.
- **Repository and unit-of-work abstractions:** application logic does not depend directly on Entity Framework Core.
- **SQL Server with EF Core:** room, booking, service, and association data are persisted using migrations.
- **Concurrency protection by availability check:** a room is bookable only when no existing booking overlaps the requested interval (`start < existingEnd && end > existingStart`).
- **JWT role authorization:** changes to rooms, services, discounts, bookings, and reports require the `Admin` role.
- **Swagger/OpenAPI:** interactive API documentation is enabled in the Development environment.

## Pricing

For a reservation, the API calculates:

```text
subtotal = (room hourly rate × duration in whole hours) + sum(selected service prices)
total    = subtotal − (subtotal × active discount percentage)
```

Selected services are charged once per booking, not once per hour. Only services assigned to the chosen room can be selected. The discount is selected by the booking **start time**, so a booking that crosses a time boundary still receives one discount for its full duration.

## API overview

Base URL during local development: `http://localhost:5082/api/v1`.

| Area | Endpoint | Description | Admin |
| --- | --- | --- | --- |
| Authentication | `POST /auth/create-token` | Creates a JWT for the supplied username and role | No |
| Rooms | `GET /rooms`, `GET /rooms/{id}` | List rooms or get room details | No |
| Rooms | `POST /rooms`, `PUT /rooms/{id}`, `DELETE /rooms/{id}` | Maintain rooms and their available services | Yes |
| Services | `GET /services`, `GET /services/{id}` | List services or get service details | No |
| Services | `POST /services`, `PUT /services/{id}`, `DELETE /services/{id}` | Maintain optional services | Yes |
| Availability | `GET /bookings/search` | Find rooms with sufficient capacity and no overlap | No |
| Bookings | `POST /bookings/book-room` | Create a reservation and return its total price | No |
| Bookings | `GET /bookings`, `GET /bookings/{id}` | List reservations or get one reservation | No |
| Bookings | `DELETE /bookings/{id}` | Cancel a reservation | Yes |
| Discounts | `GET /discounts` | List configured time discounts | Yes |
| Discounts | `POST /discounts`, `PUT /discounts/{id}`, `DELETE /discounts/{id}` | Maintain time discounts | Yes |
| Reports | `GET /reports/room-services` | Booking count and service-use counts for a room and date range | Yes |
| Reports | `GET /reports/rush-hours` | Occupied-room count for each hour in a date range | Yes |

For an admin endpoint, send the token as:

```http
Authorization: Bearer <token>
```

### Example workflow

1. Create an administrator token:

```http
POST /api/v1/auth/create-token
Content-Type: application/json

{
  "username": "admin",
  "role": "Admin"
}
```

2. Create a service and retain the returned GUID:

```http
POST /api/v1/services
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Projector",
  "price": 500
}
```

3. Create a room, assigning the service ID:

```http
POST /api/v1/rooms
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Room A",
  "capacity": 50,
  "baseRentalRate": 2000,
  "services": ["<projector-service-guid>"]
}
```

4. Search availability:

```http
GET /api/v1/bookings/search?date=2026-09-10&startTime=10:00:00&endTime=14:00:00&capacity=50
```

5. Book a returned room:

```http
POST /api/v1/bookings/book-room
Content-Type: application/json

{
  "roomId": "<room-guid>",
  "start": "2026-09-10T10:00:00",
  "durationHours": 4,
  "services": ["<projector-service-guid>"]
}
```

The booking response contains the `bookingId` and calculated `price`.

## Reports

- **Room services report** — `GET /api/v1/reports/room-services?roomId=<guid>&startDate=2026-09-01&endDate=2026-09-30` returns the number of reservations for a room and how often each of its services was selected. This helps measure upsell performance.
- **Rush-hours report** — `GET /api/v1/reports/rush-hours?startDate=2026-09-01&endDate=2026-09-30` returns occupied-room counts grouped by hour (00:00–23:00). This helps identify demand peaks and plan staffing or promotions.

## Run locally

### Prerequisites

- .NET SDK 8.0 (the repository pins the 8.0 SDK family in `global.json`)
- SQL Server or SQL Server LocalDB
- EF Core CLI tools, if they are not already installed: `dotnet tool install --global dotnet-ef`

### Setup

1. Update the `ConnectionStrings:Database` value in `WebApi/appsettings.json` if your SQL Server instance differs from the local default.
2. Apply the database migration:

```bash
dotnet ef database update --project Infrastructure --startup-project WebApi
```

3. Start the API:

```bash
dotnet run --project WebApi
```

4. In the Development environment, open `http://localhost:5082/swagger` to explore and execute the API.

## Security and production notes

- Configuration currently contains a development JWT key. Store the production connection string and JWT key in environment variables, a secret manager, or a key vault—never in source control.
- `POST /auth/create-token` issues a token for any requested role and does not authenticate a user. It is suitable only for local development/demo use; production must replace it with real identity management and server-side role assignment.
- Token lifetime validation is currently disabled. Enable expiration and set a finite token lifetime before deployment.
- For high-traffic production use, enforce overlap protection atomically at the database/transaction level in addition to the application availability check.

## Swagger

Swagger is available only when `ASPNETCORE_ENVIRONMENT=Development`. It documents request shapes, response types, and the Bearer-token authorization scheme. Generate a token first, click **Authorize**, and paste the token value without the `Bearer` prefix.
