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

  
