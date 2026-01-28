RPelis - Sistema de Gestión de Películas con ASP.NET Core 9

RPelis es una plataforma integral para la administración de catálogos cinematográficos. Es mi primer proyecto desarrollado con el patrón MVC (Model-View-Controller), donde implementé un flujo completo de trabajo, desde la persistencia de datos hasta el despliegue en producción.

Funcionalidades Destacadas
- Seguridad e Identidad: Implementación de ASP.NET Core Identity para el manejo de usuarios.
- Roles de acceso (Administrador / Usuario).
- Sistema de registro y login.
- Flujo de confirmación de email y recuperación de contraseña (vía SMTP).
- Dashboard Interactivo: Visualización dinámica de estadísticas del catálogo utilizando Chart.js.
- Recomendaciones: Algoritmo que sugiere películas similares del mismo género.
- Reseñas: Sistema de comentarios y calificaciones para cada título.
- Listado de peliculas(Admin): Listado optimizado con DataTables.js (paginado, búsqueda y ordenado del lado del cliente).
- CRUD completo de películas, generos y plataformas con relaciones de base de datos.
- Arquitectura: Migraciones con Entity Framework Core siguiendo el enfoque Code First.

Stack Tecnológico
- Backend: C# | .NET 9 | ASP.NET Core MVC
- Persistencia: Entity Framework Core | SQL Server
- Frontend: Bootstrap 5 | JavaScript | Chart.js | DataTables.js
- Seguridad: ASP.NET Core Identity
- Despliegue: MonsterASP.NET

Lo que aprendí en este proyecto
Este proyecto marcó mi inicio en el desarrollo web profesional. Algunos de los desafíos superados fueron:
- Configuración de servicios de terceros para el envío de correos electrónicos.
- Gestionar roles y autorizaciones para proteger áreas críticas de la aplicación.
- Consumo y transformación de datos desde el controlador para alimentar gráficos en el frontend.
- Resolución de conflictos en migraciones y despliegue en un entorno de hosting real.

  <p align="center">
<img width="45%" width="1277" height="1263" alt="Inicio" src="https://github.com/user-attachments/assets/7ae68efe-5654-4dee-bc26-8457e14f0373" />

<img width="45%" width="1277" height="1263" alt="Panel admin" src="https://github.com/user-attachments/assets/f3ad076a-f5ce-4297-90c3-9f96c6fffcea" />

<img width="45%" width="1278" height="1268" alt="Detalle pelicula" src="https://github.com/user-attachments/assets/077442ae-f317-4466-b5c4-b39181453a6b" />

<img width="45%" width="1277" height="1268" alt="Paginas admin" src="https://github.com/user-attachments/assets/b28740bc-693c-4433-add1-bf4d07228203" />

<img width="45%" width="881" height="617" alt="Envio de emails" src="https://github.com/user-attachments/assets/086eb2b7-ed8f-49ae-aa08-023666153ff4" />
</p>
