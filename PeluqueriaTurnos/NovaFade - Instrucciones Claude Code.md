# Tarea para Claude Code: nuevo diseño NovaFade en la rama `new-design`. No hacer push

## Pasos

1. `git checkout master && git pull && git checkout -b new-design`
2. Leé este archivo completo antes de tocar código. Revisá cómo están hoy `_Layout.cshtml`, `_LoginPartial.cshtml`, `wwwroot/css/site.css`, las vistas de `Views/Home` y `Views/Appointments`, y si existen las páginas Identity Login/Register.
3. Reescribí `wwwroot/css/site.css` con los tokens de abajo como variables CSS (`--nf-*`) y clases con prefijo `nf-` para cada componente (botones primario/secundario/destructivo, link subrayado, inputs, select, alerta de error, panel, tabla, header, footer). Cargá Newsreader desde Google Fonts en `_Layout.cshtml`.
4. Actualizá `_Layout.cshtml` y `_LoginPartial.cshtml`: header con link activo según la ruta actual, variantes visitante / cliente / Admin (Agenda solo para rol Admin), y footer.
5. Rehacé el markup de las 9 pantallas según la sección "Pantallas": Home/Index, Home/About, Home/Contact, Appointments/Create, Appointments/My, Appointments/Index, Appointments/Cancel, Identity Login y Register (scaffoldealas en `Areas/Identity/Pages/Account` si no existen; usan un layout propio a dos columnas, sin header/footer del sitio).
6. Usá los textos exactos indicados. Fechas con cultura es-UY.
7. En Create, agregá JS liviano (en `wwwroot/js`) que actualice el panel "Tu turno" y deshabilite "Confirmar turno" hasta que el formulario sea válido.
8. Aplicá el criterio responsive (breakpoint 900px).

## Restricciones

- NO cambies controladores, modelos, ViewModels, migraciones, rutas, reglas de validación, nombres de campos, `asp-for` / `asp-action` / `asp-controller` ni antiforgery. Solo presentación: Razor markup, CSS y JS de UI.
- Si el diseño muestra un campo que el ViewModel no tiene, no lo agregues: omitilo y anotalo.
- Los datos (servicios, precios, estilistas, contacto) siguen saliendo de la base / appsettings como hoy; no los hardcodees. Los valores de ejemplo de abajo son solo referencia.
- Las imágenes ya están en `wwwroot/img`. No agregues assets.
- Si el proyecto usa Bootstrap, puede quedar para grilla/utilidades, pero los componentes deben verse como este diseño, no como Bootstrap.

## Verificación y cierre

- `dotnet build` sin errores ni warnings nuevos.
- Corré la app y revisá las 9 pantallas como visitante, cliente y Admin, incluyendo: error de login, error de fecha pasada en Reservar, Mis turnos vacío y con turnos, turno cancelado en Agenda.
- Commits chicos y descriptivos (css/tokens, layout, cada grupo de vistas, identity).
- `git push -u origin new-design`
- Al final, resumí: archivos cambiados, campos del diseño omitidos y cualquier diferencia con esta especificación.

---

# Especificación visual

## Fidelidad

**Alta.** Colores, tipografía, espaciados y textos son finales. Recrear con precisión. Diseñado a 1280 px de ancho; ver "Responsive" para mobile.

## Tokens

### Colores
| Token | Valor | Uso |
|---|---|---|
| `--nf-bg` | `#000000` | Fondo de página |
| `--nf-bg-canvas` | `#0a0908` | `body` |
| `--nf-surface` | `#16110e` | Paneles laterales, tarjeta "próximo turno", sección "El local" |
| `--nf-ink` | `#f5f0e8` | Texto principal |
| `--nf-ink-2` | `#d6cfc4` | Texto de apoyo sobre foto / lead |
| `--nf-muted` | `#a49a8d` | Labels, texto secundario, links de nav inactivos |
| `--nf-faint` | `#6b6259` | Placeholders, pie de login |
| `--nf-brass` | `#b8863f` | Botón primario, acento de bordes, radio/checkbox |
| `--nf-brass-hi` | `#d9a24a` | Hover de primario, precios, focus, links |
| `--nf-brass-hi-2` | `#f0c885` | `a:hover` genérico |
| `--nf-danger` | `#e0705c` | Errores, cancelado, botón destructivo |
| `--nf-danger-hi` | `#ec8873` | Hover destructivo |
| Líneas | `rgba(245,240,232,.12)` | Filetes estándar |
| Líneas suaves | `rgba(245,240,232,.08)` | Filas de tabla |
| Línea fuerte | `rgba(245,240,232,.25)` | Encabezado de día en agenda |
| Borde input / botón secundario | `rgba(245,240,232,.35)` | |
| Primario deshabilitado | fondo `rgba(184,134,63,.25)`, texto `rgba(245,240,232,.5)` | |

### Tipografía
- **Titulares, precios, logo:** `'Newsreader', Georgia, serif`, weight 400. Google Fonts:
  `https://fonts.googleapis.com/css2?family=Newsreader:ital,opsz,wght@0,6..72,400;0,6..72,500;1,6..72,400&display=swap` (con preconnect a fonts.googleapis.com y fonts.gstatic.com).
- **Todo lo demás (texto, nav, formularios, botones):** `Helvetica, Arial, sans-serif`.

| Rol | Tamaño / line-height |
|---|---|
| H1 hero Inicio | 76px / 1.02, letter-spacing -.01em |
| H1 El Studio (sobre foto) | 64px / 1.05 |
| H1 páginas internas | 52px / 1.05 |
| H1 Cancelar / Login / Registro | 44px / 1.1 |
| H2 secciones | 44px / 1.1 |
| Precio en lista de servicios | 40px / 1, color brass-hi |
| Fecha próximo turno | 40px / 1.1 |
| Estado vacío | 38px / 1.15 |
| Total en resumen / contador agenda | 34px |
| H3 servicio | 32px |
| Títulos de panel, día en agenda | 26px |
| Logo header | 24px, letter-spacing .01em |
| Logo footer | 19px |
| Lead | 22px / 1.55 (Studio), 18px / 1.6 (hero), 17px / 1.65 |
| Cuerpo | 16px / 1.65–1.75; 15px / 1.6 |
| Inputs | 17px |
| Labels | 14px, muted |
| Meta / ayuda / errores | 13px |
| Tagline del logo | 11px, letter-spacing .06em, muted |

### Layout y forma
- Contenedor: `max-width:1160px; margin:0 auto; padding:0 40px`.
- Radio: **2px, solo en botones**. Nada más tiene radio.
- Sin sombras. Separación con filetes de 1px solo donde separan datos.
- Paneles destacados: fondo `#16110e`, `border-top:2px solid #b8863f`, padding `32px 30px`.

## Componentes

**Botón primario:** fondo `#b8863f`, texto `#000`, 16px bold, padding `15px 30px` (16px en ancho completo), radio 2px. Hover `#d9a24a`. Focus `outline:2px solid #d9a24a` (o `#f5f0e8` en formularios), offset 3px. Deshabilitado: ver tokens, `cursor:not-allowed`.

**Botón secundario:** transparente, borde 1px `rgba(245,240,232,.35)`, texto `#f5f0e8` 14px, padding `10px 20px` / `12px 22px`, radio 2px. Hover: borde y texto `#d9a24a`.

**Botón destructivo:** fondo `#e0705c`, texto `#000`, bold, padding `15px 28px`. Hover `#ec8873`.

**Link subrayado:** 15px `#f5f0e8`, `padding-bottom:4px; border-bottom:1px solid rgba(245,240,232,.35)`. Hover: borde y texto `#d9a24a`.

**Input / select:** fondo `#000`, sin borde salvo `border-bottom:1px solid rgba(245,240,232,.35)`, 17px, padding `10px 0`, ancho 100%. Focus: sin outline, `border-bottom-color:#d9a24a`. Error: `border-bottom-color:#e0705c` + mensaje 13px `#e0705c` debajo (margin-top 8px), enlazado con `aria-describedby`. Select con `appearance:none`, `padding-right:24px` y chevron a la derecha: `background-image:url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23a49a8d' stroke-width='1.5' d='m2 5 6 6 6-6'/%3e%3c/svg%3e"); background-repeat:no-repeat; background-position:right 2px center; background-size:14px`. Label arriba, 14px muted, margin-bottom 10px.

**Radio / checkbox:** `accent-color:#b8863f`, 16×16.

**Alerta de error (login):** `border-left:2px solid #e0705c; background:rgba(224,112,92,.08); padding:12px 16px`, texto 14px `#f5f0e8`.

**Header** (`_Layout` + `_LoginPartial`): borde inferior 1px línea estándar, padding `18px 40px`. Izquierda: logo "NovaFade" (serif 24px) + tagline "Peluquería & barbería · Montevideo". Derecha, gap 28px, 14px:
- Links: Inicio, El Studio, Contacto; con sesión se suman Reservar turno y Mis turnos; rol Admin suma Agenda.
- Activo: `#f5f0e8` con `border-bottom:1px solid #b8863f` (padding-bottom 3px). Inactivo: muted, hover `#f5f0e8`.
- Visitante: "Ingresar" (link muted) + botón primario chico "Reservar turno" (padding `9px 18px`).
- Con sesión: nombre del usuario (muted) + botón secundario chico "Cerrar sesión" (13px, padding `8px 14px`).

**Footer:** borde superior 1px, padding 40px vertical. Izquierda: "NovaFade Studio" serif 19px + "Montevideo, Uruguay · 2026" 14px muted. Derecha: links muted 14px "Agendá tu turno", "Instagram", "WhatsApp", hover `#d9a24a`.

## Pantallas

Todas usan fondo `#000` y texto `#f5f0e8`. Usar los textos exactos indicados.

1. **Inicio** (`Views/Home/Index.cshtml`)
   - Hero 620px de alto, foto `img/hero/hero-barber.jpg` a sangre (`object-position:60% 40%`) con degradado `linear-gradient(94deg, rgba(0,0,0,.94) 0%, rgba(0,0,0,.86) 38%, rgba(0,0,0,.35) 68%, rgba(0,0,0,.55) 100%)`. Texto alineado a la izquierda, max-width 560px, centrado vertical: H1, lead (max-width 440), botón primario "Elegir día y hora" + link subrayado "Ver servicios y precios".
   - Franja de 3 columnas con filetes verticales: "Solo con turno", "Café, agua y Wi-Fi", "Pagás como quieras" (15px título + 13px muted).
   - Servicios (padding `88px 0 96px`): cabecera con H2 a la izquierda y texto de 15px alineado a la derecha (max 360). Lista editorial: cada servicio es un grid `320px 1fr auto`, gap 40, padding 28px 0, filete superior. Foto 320×190 cover · nombre (H3) + descripción + "{duración} de sillón" · precio serif 40px brass-hi + botón secundario "Reservar {barba|corte|fade}". Filete de cierre al final. Datos desde la base (SeedData).
   - "El local, sobre Montevideo": fondo `#16110e`, grid 1fr 1fr; texto (padding `88px 64px 88px 0`) con dl de Dónde / Estilistas / Turnos y link "Conocer el Studio"; a la derecha `img/hero/local.jpg` a sangre, min-height 520.
2. **El Studio** (`About.cshtml`): cabecera de 420px con `local.jpg` y degradado a 90° (.92 → .6 → .2), H1 abajo a la izquierda (margin-bottom 48). Cuerpo grid `1fr 420px`, gap 80: lead 22px + dos párrafos muted + botón primario; panel "Cómo funciona" con lista de 4 pasos separados por filete.
3. **Contacto** (`Contact.cshtml`): grid `420px 1fr`, gap 80. Izquierda: H1, texto, dl de Teléfono y WhatsApp / Mail / Dónde estamos (valores desde `appsettings.json` como hoy; label 13px muted arriba, valor 17px, filetes entre filas) y botón "Reservar turno online". Derecha: el iframe de Google Maps actual, 520px de alto, borde 1px, `filter:grayscale(1) contrast(1.05) brightness(.85)`.
4. **Reservar turno** (`Appointments/Create.cshtml`): grid `1fr 380px`, gap 72.
   - Servicio: lista de radios (una fila por servicio: radio · nombre 17px · duración 14px muted · precio 17px brass-hi), filetes entre filas, hover `rgba(184,134,63,.07)`.
   - Grid de 2 columnas (gap 28): Estilista (select), Día y hora (datetime-local), Nombre para el turno, Teléfono de contacto (placeholder "Para avisarte si hay un cambio"). Mantener los campos que ya tenga el ViewModel; si alguno de estos no existe, no agregarlo al modelo.
   - Error de validación bajo el campo (ej. fecha pasada: "Ese día ya pasó. Elegí una fecha desde hoy, {dd/MM}, en adelante.").
   - Panel "Tu turno" (sticky opcional): dl Servicio / Estilista / Día y hora / Duración (valor faltante en `#e0705c` "Sin definir"), total serif 34px brass-hi "Total a pagar en el local", botón "Confirmar turno" ancho completo (deshabilitado hasta que el formulario sea válido) y nota 13px. El resumen se actualiza con JS liviano al cambiar los campos.
5. **Mis turnos** (`My.cshtml`): cabecera con H1 + subtítulo y botón primario "Reservar otro turno" a la derecha.
   - Si hay próximo turno: tarjeta surface con borde superior brass, "Tu próximo turno" 13px, fecha serif 40px ("Lunes 28 de septiembre, 14:00", cultura es-UY), línea "Servicio con Estilista · duración · precio", botón secundario "Cancelar este turno" + nota "Podés cancelar hasta la hora del turno".
   - "Historial": tabla sin bordes verticales; th 13px muted weight 400; filas 15px con filete `.08`; columnas Fecha, Servicio, Estilista (muted), Estado (muted), Precio (derecha; "—" si cancelado).
   - Estado vacío: bloque entre filetes, grid 1fr 1fr: "Todavía no reservaste tu primer turno." serif 38px, texto, botón "Elegir día y hora"; imagen `corte-clasico.jpg` 260px de alto.
6. **Agenda (Admin)** (`Appointments/Index.cshtml`): H1 "Agenda" + subtítulo; a la derecha contador serif 34px brass-hi + "turnos confirmados esta semana". Turnos **agrupados por día** (próximos primero; al final grupo "Pasados"). Encabezado de grupo: día serif 26px + cantidad 13px muted, filete `.25`. Fila: grid `90px 1fr 180px 150px auto`, gap 24, padding 18px 0: hora 17px · cliente 16px · servicio 15px muted · estilista 15px muted · botón "Cancelar" (secundario, hover en rojo) o, si está cancelado, el estado en `#e0705c` ("Cancelado por el cliente" / "Cancelado por el salón").
7. **Cancelar turno** (`Cancel.cshtml`): columna centrada max-width 560, padding `96px 40px 120px`. H1 "¿Cancelamos este turno?", texto, dl de 3 filas (label 150px): Día y hora / Servicio / Estilista. Botón destructivo "Sí, cancelar el turno" + link "Volver a Mis turnos".
8. **Ingresar** (Identity `Login`): sin header/footer del sitio. Grid 1fr 1fr, min-height 760. Izquierda (padding `56px 64px`, flex columna): logo arriba, formulario max-width 380 (H1 "Entrá para reservar", texto, alerta de error si falla, Mail, Contraseña, checkbox "Mantener la sesión abierta", botón "Ingresar" ancho completo, "¿Todavía no tenés cuenta? Crear una cuenta"), pie "NovaFade Studio · Montevideo, Uruguay" 13px faint. Derecha: `img/services/skin-fade.jpg` a sangre. Mensaje de error de credenciales: "Ese mail y esa contraseña no coinciden. Revisá la contraseña o creá una cuenta nueva."
9. **Crear cuenta** (Identity `Register`): misma estructura que Login, imagen `img/services/barba.jpg`. Campos Mail, Contraseña (ayuda fija "Mínimo 6 caracteres, con una mayúscula, un número y un símbolo." — ajustar al `PasswordOptions` real), Repetir contraseña; botón "Crear cuenta y reservar"; "¿Ya tenés cuenta? Ingresar".

## Interacción y estados
- Transiciones: ninguna obligatoria; si se agregan, `150ms ease` en color/borde, y respetar `prefers-reduced-motion`.
- Focus visible en todos los interactivos (ver botones/inputs).
- Errores de ModelState: usar `asp-validation-for` con los estilos de error descritos; el `asp-validation-summary` de Login se muestra como la alerta de error.
- Globales en `site.css`: `body{margin:0;background:#0a0908;color:#f5f0e8;font-family:Helvetica,Arial,sans-serif}`, `a{color:#d9a24a}`, `a:hover{color:#f0c885}`, `::placeholder{color:#6b6259}`.

## Responsive (no diseñado; criterio)
Bajo 900px: grids de 2 columnas pasan a 1 (panel lateral debajo del formulario; imagen de login/registro oculta o arriba con 200px), lista de servicios pasa a foto arriba + texto + precio, filas de agenda en 2 líneas, nav colapsa en menú. H1 hero ~44px, H1 internos ~36px. Padding lateral 20px.

## Assets
Ya están en el repo, no hay que agregar nada: `wwwroot/img/hero/hero-barber.jpg`, `wwwroot/img/hero/local.jpg`, `wwwroot/img/services/barba.jpg`, `corte-clasico.jpg`, `skin-fade.jpg`. Fuente Newsreader vía Google Fonts.
