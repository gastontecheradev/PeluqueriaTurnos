# NovaFade Studio — Appointment Booking System

Web system for managing appointments in a hair salon/barbershop. It allows clients to register, choose a service and stylist, and book an appointment online. Administrators can view and manage all appointments from a dedicated dashboard.

🔗 **Live site:** [NovaFade](https://novafadestudio.somee.com/)

## Main Features

- Registration and login with roles (Admin / Client)
- Service catalog with name, description, price, and duration
- Stylist profiles with the services they offer
- Appointment booking: service, stylist, date, and time selection
- Client appointment history with cancellation option
- Admin panel to view and cancel any appointment
- Public pages: home, studio, and contact
- Salon contact details configurable via `appsettings.json`

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 10.0 (MVC + Razor Pages) |
| Language | C# |
| ORM | Entity Framework Core 10.0 |
| Database | SQLite |
| Authentication | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap 5.3.3 |
| Fonts | Google Fonts — Poppins & Inter |
