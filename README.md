# NovaFade Studio — Sistema de Reserva de Turnos

Sistema web para la gestión de turnos de una peluquería/barbería. Permite a los clientes registrarse, elegir un servicio y estilista, y reservar un turno en línea. Los administradores pueden visualizar y gestionar todos los turnos desde un panel dedicado.

## Caracteristicas principales

- Registro e inicio de sesion con roles (Admin / Cliente)
- Catalogo de servicios con nombre, descripcion, precio y duracion
- Perfiles de estilistas con los servicios que ofrecen
- Reserva de turnos: seleccion de servicio, estilista, fecha y hora
- Historial de turnos del cliente con opcion de cancelacion
- Panel de administracion para ver y cancelar cualquier turno
- Paginas publicas: inicio, estudio y contacto
- Datos de contacto del salon configurables via `appsettings.json`

## Tech Stack

| Capa | Tecnologia |
|---|---|
| Backend | ASP.NET Core 10.0 (MVC + Razor Pages) |
| Lenguaje | C# |
| ORM | Entity Framework Core 10.0 |
| Base de datos | SQLite |
| Autenticacion | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap 5.3.3 |
| Fuentes | Google Fonts — Poppins & Inter |
