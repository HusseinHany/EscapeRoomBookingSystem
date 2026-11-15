# Escape Room Booking System

A Windows Forms application in C# for staff at an escape room facility: manage themed rooms, take bookings, and stop a room being booked twice for the same slot. Data is stored in SQL Server.

## What's in this repository

This repository holds part of the project's source, not the whole solution:

| File | What it is |
| --- | --- |
| `RoomsForm.cs` | The rooms screen: list all rooms, add, edit, delete, toggle availability |
| `AddEditRoomDialog.cs` | Dialog for adding or editing a room, with input validation |
| `Booking.cs` | The booking model (room, customer, phone, date, group size, payment status) |
| `ProjectReport.cs` | The written project report: problem statement, database design, forms, OOP analysis, testing notes |

The login, dashboard, bookings, filter and leaderboard forms, the `DatabaseHelper`, `Room`, `RoomManager` and `BookingManager` classes, and the Visual Studio project files are described in the report but are not in this repository, so it does not build as it stands.

## Design, from the project report

**Database**

- `Rooms`: name (unique), theme, difficulty 1-5, capacity, price, availability
- `Bookings`: room, customer name, phone, booking date, group size, payment status, completion time
- One room has many bookings

**Screens**

1. Login
2. Dashboard: today's bookings, today's revenue, room availability, recent bookings
3. Rooms: add, edit, delete, mark as unavailable
4. Bookings: add, edit, delete, with double-booking and capacity checks
5. Filter and search: by room, date range, payment status and customer name
6. Leaderboard: top five fastest escape times per room

**Rules the system enforces**

- A room cannot be booked twice for the same date
- Group size cannot exceed the room's capacity
- Room names are unique; capacity and price must be greater than zero
- All SQL is parameterized

## Built with

C#, Windows Forms, SQL Server (`SqlConnection`).
