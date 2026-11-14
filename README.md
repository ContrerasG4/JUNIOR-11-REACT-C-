<div align="center">

# Junior 11

**Juego de cuadrícula de fútbol: completa cada celda con el jugador que cumple los dos criterios.**

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-API%20REST-5C2D91?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Supabase-4169E1?logo=postgresql&logoColor=white)
![React](https://img.shields.io/badge/React-18-61DAFB?logo=react&logoColor=black)
![Vite](https://img.shields.io/badge/Vite-646CFF?logo=vite&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-06B6D4?logo=tailwindcss&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)

</div>

---


##  Descripción

**Junior 11** es una aplicación web compuesta por dos partes independientes:

| Proyecto | Descripción |
|---|---|
| **`JuniorGrid.Api`** | API REST en .NET 10 con Entity Framework Core y PostgreSQL (Supabase). Documentada con Swagger y lista para Docker. |
| **`junior-grid-frontend`** | Interfaz en React 18 con Vite y Tailwind CSS que consume la API. |

La aplicación permite jugar sobre una cuadrícula de criterios, validar los jugadores de cada celda y consultar rankings **Top 10** por criterio.

##  Características

-  **Cuadrícula interactiva** con selección de jugador por celda mediante modal.
-  **Vista Top 10** por criterio.
-  **Protecciones verificadas** con pruebas de inyección, carga y tamaño de payload.
-  **Protección de la API** con API Key, rate limiting y CORS restringido.
-  **Swagger / OpenAPI** para explorar y probar los endpoints.
-  **Dockerfile multi-etapa** para desplegar el backend de forma ligera.
-  **Frontend rápido** con Vite y estilos con Tailwind CSS.

##  Capturas del juego

La aplicación ofrece dos modos de juego.

###  Modo Grilla (3x3)

Cada fila y cada columna tiene un criterio (por ejemplo *Campeón (>=1 título)*, *+50 goles*, *Década 1970s* o *Extranjero*). El jugador debe escribir el futbolista que cumple los dos criterios de la celda. Si no es válido, la celda se marca en rojo con un mensaje. El botón **Nuevo Tablero** genera una combinación distinta de criterios.

<p align="center">
  <img src="docs/screenshots/modo-grilla.png" alt="Modo Grilla 3x3 de Junior Grid" width="600">
</p>

###  Modo Top 10

Se propone un reto (por ejemplo *Top 10 jugadores con más partidos en Junior, década de los 80*). Cada posición muestra solo pistas (nacionalidad y década) y el jugador escribe el nombre del futbolista para descubrirlo. El botón **Cambiar de Reto Top 10** cambia el desafío.

<p align="center">
  <img src="docs/screenshots/modo-top10.png" alt="Modo Top 10 de Junior Grid" width="450">
</p>

---

## Stack tecnológico

| Capa | Tecnologías |
|---|---|
| **Backend** | .NET 10, ASP.NET Core, Entity Framework Core |
| **Base de datos** | PostgreSQL / Supabase |
| **Frontend** | React 18, Vite, Tailwind CSS |
| **Infraestructura** | Docker |
| **Documentación** | Swagger (OpenAPI) |

##  Arquitectura

El backend sigue una arquitectura por capas, separando responsabilidades:

```
Cliente (React)
      │  HTTP / JSON
      ▼
┌──────────────────────────────────────────┐
│  Middleware (API Key, Rate Limit, CORS)  │
├──────────────────────────────────────────┤
│  Controllers  →  Services  →  Repositories│
├──────────────────────────────────────────┤
│        AppDbContext (EF Core)            │
└──────────────────────────────────────────┘
      │
      ▼
 PostgreSQL / Supabase
```

- **Controllers**: exponen los endpoints y validan la entrada.
- **Services**: contienen la lógica de negocio.
- **Repositories**: encapsulan el acceso a datos (genéricos y específicos).
- **DTOs**: desacoplan el contrato de la API de las entidades.

##  Estructura del proyecto

```
JuniorGrid.Api/
├── Controllers/         # GameGridController, Top10Controller
├── DTOs/                # Objetos de transferencia de datos
├── Data/                # AppDbContext (EF Core)
├── Models/              # Entidades: Jugador, Criterio, Top10Criterio
├── Repositories/        # Repositorios genéricos y específicos
├── Services/            # Lógica de negocio
├── ApiKeyMiddleware.cs  # Autenticación por API Key
├── Program.cs           # Rate limiting, CORS, Swagger, Kestrel
└── Dockerfile           # Imagen multi-etapa para .NET 10

junior-grid-frontend/
├── src/
│   ├── App.jsx
│   ├── api/
│   │   └── gameApi.js   # Cliente HTTP hacia la API
│   └── components/      # Grid, CellModal, Top10View
├── tailwind.config.js
└── vite.config.js
```

##  Requisitos previos

| Herramienta | Versión | Obligatoria |
|---|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0 | ✔️ |
| [Node.js](https://nodejs.org/) | 18 o superior | ✔️ |
| PostgreSQL o [Supabase](https://supabase.com/) | — | ✔️ |
| [Docker](https://www.docker.com/) | — | Opcional |

## Inicio rápido

```bash
# 1. Clona el repositorio
git clone https://github.com/ContrerasG4/JUNIOR-11-REACT-C-
cd <carpeta-del-repositorio>

# 2. Backend
cd JuniorGrid.Api
dotnet restore
dotnet run

# 3. Frontend (en otra terminal)
cd junior-grid-frontend
npm install
npm run dev
```

| Servicio | URL |
|---|---|
| Frontend | `http://localhost:5173` |
| API | `https://localhost:<puerto>` (el puerto se muestra al ejecutar `dotnet run`) |
| Swagger | `https://localhost:<puerto>/swagger` |

##  Configuración

### Backend

Configura la cadena de conexión en `JuniorGrid.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Conexion": "Host=...;Database=...;Username=...;Password=..."
  },
  "AllowedOrigins": [
    "http://localhost:5173"
  ]
}
```

| Clave | Descripción |
|---|---|
| `ConnectionStrings:Conexion` | Cadena de conexión a PostgreSQL / Supabase. |
| `AllowedOrigins` | Orígenes permitidos por CORS. En desarrollo se permiten por defecto `http://localhost:5173` y `http://localhost:3000`. |


### Frontend

El cliente HTTP vive en `junior-grid-frontend/src/api/gameApi.js`. Asegúrate de que apunte a la URL de tu API y de enviar la API Key que espera el middleware.

##  Ejecución con Docker

```bash
cd JuniorGrid.Api

# Construir la imagen
docker build -t juniorgrid-api .

# Ejecutar el contenedor
docker run -p 8080:8080 \
  -e ConnectionStrings__Conexion="Host=...;Database=...;Username=...;Password=..." \
  juniorgrid-api
```

La API quedará disponible en `http://localhost:8080`.

## Seguridad

Cada una de estas medidas fue validada con pruebas reales (ver [Pruebas de seguridad y robustez](#-pruebas-de-seguridad-y-robustez)).

| Medida | Detalle |
|---|---|
| **Rate limiting** | 60 peticiones por minuto por IP, con cola de 2. |
| **API Key** | Middleware de autenticación (`ApiKeyMiddleware.cs`). |
| **CORS** | Restringido a los orígenes configurados en `AllowedOrigins`. |
| **Límite de cuerpo** | Máximo 10 KB por petición. |
| **Kestrel** | Header `Server` oculto para no exponer información del servidor. |
| **Excepciones** | Manejo global de errores. |
| **HSTS** | Habilitado en producción. |

##  Pruebas de seguridad y robustez

Además de implementar las protecciones, las **verificamos con pruebas reales** contra el endpoint de validación:

```
POST /api/GameGrid/validar
{ "nombreJugador": "Carlos Bacca", "criterioFilaId": 1, "criterioColumnaId": 1 }
```

| # | Prueba | Resultado |
|---|---|---|
| 1 | Entradas maliciosas (XSS, SQL Injection, SSTI, Path Traversal, Unicode) | Tratadas como texto; la API responde "no encontrado" y el frontend no ejecuta nada |
| 2 | Rate limiting: 500 peticiones, 50 en paralelo |  62 respuestas `200` y 438 `429` |
| 3 | Cuerpo de 20.000 caracteres |  `413 Payload Too Large` con mensaje JSON controlado |

### 1. Inyección de entradas maliciosas

Se escribieron en las celdas del tablero payloads de ataque como `<script>alert(1)</script>`, `<img src=x onerror=alert(1)>`, `{{7*7}}`, `${7*7}`, `' OR 1=1 -- AND "; DROP TABLE jugadores;--`, `../../etc/passwd` y texto con emojis, kanji y `ñ`. Ninguno se ejecutó ni se interpretó: el backend los busca como un nombre cualquiera y devuelve *"Jugador '…' no encontrado"*, y el frontend lo muestra como texto plano.

<p align="center">
  <img src="docs/screenshots/test-inyeccion-entradas.png" alt="Prueba de inyección de entradas maliciosas" width="600">
</p>

### 2. Rate limiting bajo carga

Se lanzaron 500 peticiones con 50 hilos concurrentes en PowerShell. El resultado coincide con la configuración (60 peticiones por minuto + cola de 2 = 62 aceptadas); el resto fue rechazado con `429 Too Many Requests`.

```powershell
$url  = "http://localhost:5000/api/GameGrid/validar"
$body = '{"nombreJugador":"Carlos Bacca","criterioFilaId":1,"criterioColumnaId":1}'

$resultados = 1..500 | ForEach-Object -ThrottleLimit 50 -Parallel {
    try {
        $r = Invoke-WebRequest -Uri $using:url -Method POST -Body $using:body `
             -ContentType "application/json" -UseBasicParsing
        [int]$r.StatusCode
    } catch {
        if ($_.Exception.Response) { [int]$_.Exception.Response.StatusCode } else { "error de conexión" }
    }
}

$resultados | Group-Object | Select-Object Count, Name
```

<p align="center">
  <img src="docs/screenshots/test-rate-limiting.png" alt="Prueba de rate limiting con 500 peticiones" width="700">
</p>

### 3. Límite de tamaño del cuerpo

Se envió un `nombreJugador` de 20.000 caracteres. El servidor lo rechazó con `413 Payload Too Large` y un mensaje JSON controlado, sin exponer trazas ni detalles internos.

```powershell
$u = "http://localhost:5000/api/GameGrid/validar"
curl.exe -i -X POST $u -H "Content-Type: application/json" `
  -d ('{"nombreJugador":"' + ('A' * 20000) + '","criterioFilaId":1,"criterioColumnaId":1}')
```

<p align="center">
  <img src="docs/screenshots/test-payload-413.png" alt="Prueba de payload demasiado grande (413)" width="700">
</p>

---

## Documentación de la API

En desarrollo, Swagger está disponible en:

```
https://localhost:<puerto>/swagger
```

Allí puedes explorar los endpoints de `GameGridController` y `Top10Controller`, ver los esquemas de los DTOs y probar las peticiones directamente.

## 🩺 Solución de problemas

<details>
<summary><strong>Error de CORS en el navegador</strong></summary>

Verifica que el origen del frontend (por ejemplo `http://localhost:5173`) esté incluido en `AllowedOrigins`.
</details>

<details>
<summary><strong>Respuesta 401 / 403 al llamar a la API</strong></summary>

El middleware de API Key está rechazando la petición. Revisa que el frontend envíe la clave correcta en el header esperado.
</details>

<details>
<summary><strong>Respuesta 429 (Too Many Requests)</strong></summary>

Superaste el límite de 60 peticiones por minuto por IP. Espera unos segundos e inténtalo de nuevo.
</details>

<details>
<summary><strong>No conecta con la base de datos</strong></summary>

Comprueba la cadena de conexión, que el host sea accesible desde tu red y que las credenciales de Supabase/PostgreSQL sean correctas.
</details>
