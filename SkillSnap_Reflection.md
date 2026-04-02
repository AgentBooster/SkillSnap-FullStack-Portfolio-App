# SkillSnap - Project Reflection

## 1. Descripción de la aplicación y principales características

SkillSnap es un portafolio full-stack y rastreador de proyectos que permite a los usuarios gestionar y exhibir perfiles profesionales, proyectos realizados y habilidades técnicas. Funciona como un ecosistema donde la interacción del usuario final e interfaces de administración coexisten fluidamente.

**Tres características clave:**
1. **Gestión Integral de Portafolio (CRUD API & Blazor):** Capacidad transaccional completa sobre modelos de `Project` y `Skill`, interconectando el back-end robusto con un front-end reactivo mediante Blazor WebAssembly.
2. **Seguridad y Autenticación Bi-Capa basada en JWT:** Implementación profunda de ASP.NET Identity con emisión de Web Tokens (JWT) firmados, protegiendo tanto la integridad de las rutas en el SPA como el acceso a mutaciones de base de datos en la API, empleando RBAC (Role-Based Access Control).
3. **Alto Rendimiento con Estrategias de Caché y Estado:** Mitigación de carga en el proveedor de datos mediante almacenamiento en caché en la capa API (`IMemoryCache`) y una resolución ágil de componentes Blazor administrando el estado de sesión local.

## 2. Retos de desarrollo y soluciones

La transición entre la ejecución de una API *stateless* y una SPA (Single Page Application) acarrea retos clásicos en estado y latencia.

* **Reto:** Persistencia segura del token JWT y estado de autenticación tras recargas críticas en Blazor WebAssembly.
  * **Solución:** Se implementó un `AuthenticationStateProvider` personalizado junto con un `AuthService` que interactúa en el almacenamiento local (`localStorage`) del navegador. Esto interviene activamente las solicitudes `HttpClient` inyectando nativamente cabeceras de autorización, asegurando persistencia de sesión sin degradación en UX.
* **Reto:** Sobrecarga de base de datos interconectada y potencial sobrecarga útil (over-fetching).
  * **Solución:** Desacoplamiento de las cargas mediante la implementación estricta del patrón observador en Entity Framework Core. Evitamos el `N+1 Query Problem` acoplando `.Include()` con `.AsNoTracking()` en accesos read-only.
* **Rol de Copilot:** Actuó como revisor secundario para andamiar rápidamente *boilerplate code* (generación de controladores y scaffolding de JWT configs), permitiendo que nosotros nos centrásemos en la arquitectura troncal.

## 3. Estructuración de Lógica Empresarial, Persistencia y Estado

Aplicamos principios de diseño para asegurar que el código sobreviva a cambios en producción:
* **Persistencia de Datos (EF Core & SQLite):** Consolidado en `SkillSnapContext` aislando el ecosistema DB. Modelos (`PortfolioUser`, `Project`, `Skill`) respetan fuertemente la integridad referencial configurada a través de data annotations `[Key]` y `[ForeignKey]`.
* **Lógica Empresarial (API Controller Pattern):** Cada recurso se maneja bajo el ciclo de vida de controladores independientes (`ProjectsController`, `SkillsController`). La lógica transaccional de hidratación del dominio se realiza inyectando el `SkillSnapContext` con interfaces desacopladas.
* **Gestión de Estado (Blazor Front-End):** La vista abstrae en componentes con Single Responsibility (`ProfileCard`, `ProjectList`). Inyectamos servicios scope, como por ejemplo un `UserSessionService`, unificando un único origen de verdad para gestionar asíncronamente los datos de usuario a lo largo de toda la UI.

## 4. Implementación de Seguridad con Identity

Nuestra prioridad arquitectónica es denegar accesos por defecto (Zero-Trust):
* **ASP.NET Identity Integrado:** Reemplazamos abstracciones rústicas heredando con `ApplicationUser` la clase base `IdentityUser`, asegurando salting/hashing criptográfico sólido en la tabla subyacente de Entity Framework.
* **Gestión de Sesión vía JWT:** A través de un `AuthController` dedicado, se exponen puntos finales de inicio y registro que despachan tokens validados e intercambian aserciones perimetrales.
* **Autorización Estricta (RBAC y Claims):** Decoradores `[Authorize]` con validación por perfiles (ej. `[Authorize(Roles = "Admin")]`) filtran las capas de solicitud antes de penetrar el scope de inyección de ef-core, garantizando la inmutabilidad de la información crítica desde vectores no autorizados.

## 5. Mejoras de rendimiento y optimización aplicadas

Previniendo un escalamiento costoso futuro, intercedimos tempranamente en cuellos de botella:
1. **Almacenamiento en Caché (Server-Side):** `IMemoryCache` se inicializó sobre el middleware, incrustando políticas de TTL (Time-To-Live). Por ejemplo, métodos de listado general se encapsulan, logrando que llamados recursivos se resuelvan en fracciones de milisegundo (in-memory hits) sin re-interpretar sentencias SQL.
2. **Consultas Livianas (EF Core Tuning):** Aplicamos extensiones `.AsNoTracking()` para lecturas no mutables ahorrando costos volumétricos en la monitorización del contexto.
3. **Gestión Sensitiva Blazor:** Deprescindimos de renderizados DOM innecesarios usando verificaciones de actualización en hooks como `OnInitializedAsync`, consumiendo las recolecciones de memoria locales eficientemente.
4. **Validación Copilot:** Microsoft Copilot facilitó auditorías sobre variables huerfanas y métodos no invocados para condensar los payloads resultantes, manteniendo un bundle minimalista para WASM.
