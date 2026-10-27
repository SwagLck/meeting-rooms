# Meeting Room Booking

Web application for booking office meeting rooms: browse rooms, users list, manage
users reservations and book rooms with backend validation.

## Tech stack

- **Back end:** ASP.NET Core Web API (.NET 10), EF Core In-Memory database

- **Front end:** React + TypeScript (Vite), Material UI, React Router

## How to run

Back end (http://localhost:5248):

```
cd MeetingRooms.Api
dotnet run
```

Front end (http://localhost:5173) (in a seperate git bash terminal):

```
cd frontend
npm install
npm run dev
```

The in-memory database is seeded on startup with sample rooms, equipment,
users and reservations (past, currently happening and upcoming), so the app is usable immediately.
Data is lost when the back end stops.

## Features

- **Rooms:** List of rooms that can be filtered by minimum capacity, floor and equipment.

- **Users:** List of users with the number of active reservations.

- **Reservations:** List of reservations that can be filtered by room, organizer, status and date range.
Also you can create, cancel and end a meeting early.

## Booking rules

Every new reservation is validated in the back end. A broken rule returns
409 Conflict with { "code": "...", "message": "..." }, which the UI shows.

| Rule | Description                                                            |
|------|------------------------------------------------------------------------|
| R1   | Max duration 7 days, configurable                                      |
| R2   | No overlapping bookings in the same room                               |
| R3   | An organizer cannot have two overlapping meetings                      |
| R4   | Start must be in the future                                            |
| R5   | Attendees must not exceed room capacity                                |
| R6   | Start at most 90 days ahead, configurable                              |
| R7   | Inactive rooms cannot be booked                                        |

Limits for R1 and R6 are in appsettings.json (BookingRules section).

## Architecture decisions

- **Layers:** Thin controllers -> services -> EF Core AppDbContext.
Also dependency injection is used all over.

- **Strategy pattern for rules:** Each rule is a class implementing IBookingRule. ReservationService runs all registered rules, so adding a
new rule means adding a class and registering it, with no changes to the service.

- **DTOs:** Entities are never returned directly, DTOs avoid JSON cycles and 
decouple the API from the database model.

- **`ServiceResult<T>`:** Expected business errors are returned as values 
instead of exceptions, controllers map them to 404/409.

- **Equipment as a table (many-to-many)** Instead of a flags enum, so new equipment types
can be added as data without code changes.

- **Filtering in the database query (IQueryable)** Avoiding filters in the frontend, so all logic stays in the backend.

- **TimeProvider** Instead of DateTime.Now, so time-based rules are testable.

- **Front end:** API calls are isolated in src/api/ and types in src/types.ts.

## Assumptions

- Times are the server's local time.
- An "active reservation" is Booked with an end time in the future.
- R3 compares exact time ranges, not only dates, cancelled reservations do not block.
- Only the first violated rule is returned.
- "End early" is allowed only for a booked meeting currently in progress,
  it sets the end time to now and the status to Completed.
- The date range filter shows reservations that start on or after "from"
  and end on or before "to".
