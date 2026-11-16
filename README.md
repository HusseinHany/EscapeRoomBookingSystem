# Escape Room Management System

A Windows desktop app for the staff of an escape room business: manage the themed rooms, take bookings, filter them, and see revenue reports and a customer leaderboard. Built with C# Windows Forms and SQL Server.

## Screens

| Screen | What staff can do |
| --- | --- |
| **Login** | Sign in with a staff account; the password can be shown or hidden |
| **Dashboard** | Today's bookings, today's revenue, available rooms, the most recent bookings, and a live clock; buttons to every other screen |
| **Rooms** | Add, edit and delete rooms (name, theme, difficulty, capacity, price per session, availability), with a live search by name |
| **Bookings** | Add, edit and delete bookings; the price is filled in from the room and the group size; live search by customer name |
| **Filter** | Find bookings by customer name, room, payment status and date range |
| **Leaderboard** | Customers ranked by how much they have spent on paid bookings, with search |
| **Reports** | Revenue and booking counts (paid, pending, cancelled) for a chosen date range, with a booking list |

## Database

Three tables in `EscapeRoomDB`:

- **Staff:** full name, username, password, role, active flag
- **Rooms:** name, theme, difficulty, capacity, price per session, availability
- **Bookings:** room, customer name and phone, date, group size, payment status, total price (each booking belongs to one room)

`database-setup.sql` creates the database and the tables, and fills them with 20 sample rooms and 10 sample bookings.

## Run it

You need Windows, Visual Studio 2022 or later with the ".NET desktop development" workload, and SQL Server (Express or LocalDB is fine).

1. Open `database-setup.sql` in SQL Server Management Studio and run the whole script.
2. Open `EscapeRoomMS.slnx` in Visual Studio. NuGet restores the packages on the first build.
3. If your SQL Server is not the default instance, change `Data Source` in `Database/DatabaseHelper.cs` (for example `.\SQLEXPRESS` or `(localdb)\MSSQLLocalDB`).
4. Press F5 and sign in with the demo account **admin / admin123**.

## Built with

C# · .NET Framework 4.7.2 · Windows Forms · SQL Server · ADO.NET (`System.Data.SqlClient`)

## Notes

- Searches and filters use SQL parameters, so names with an apostrophe (such as "Pharaoh's Tomb") work.
- Passwords are stored as plain text, as in the original coursework; a real deployment would hash them. The team members' accounts in the setup script use the placeholder password `ChangeMe1`.
- An earlier, partial version of this project (four source files and a written report) is in this repository's history.

## Team

Hussein Hany, Ahmed Walid, Eslam Mohamed, Begad Sherif.
