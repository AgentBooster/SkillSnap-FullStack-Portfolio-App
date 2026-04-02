# Contexto:

**Actividad Introducción**

Estás construyendo **SkillSnap**, tu propio portafolio full-stack y rastreador de proyectos. En esta parte del capstone, construirás los cimientos de tu aplicación. Definirás los modelos de datos que soportan los perfiles de usuario, proyectos y habilidades. También configurará su API y el contexto de EF Core, y establecerá componentes Blazor de marcador de posición para su diseño.

En esta actividad

* Crear modelos de datos y configurar la base de datos  
* Crear la estructura de la API  
* Diseñar componentes de diseño estático en Blazor  
* Utilizar Microsoft Copilot para organizar y optimizar el código

Esta es la primera de cinco actividades. En futuras partes, añadirá interactividad, seguridad, mejoras de rendimiento y depuración asistida por Copilot.

### **Instrucciones de la actividad**

**Paso 1: Configure su proyecto Full Stack**

* Crea una nueva carpeta de solución llamada SkillSnap  
  * Crea una nueva API web llamada SkillSnap.Api  
  * Crea el proyecto Blazor WebAssembly llamado SkillSnap.Client  
* Agrega SkillSnap.Api y SkillSnap.Client a tu solución:

**Paso 2: Crear los Modelos de datos**

En una carpeta **SkillSnap.Api/Models**, agrega tres clases:

**PortfolioUser.cs**

* Id (int)  
* Nombre (string)  
* Bio (cadena)  
* ProfileImageUrl (cadena)  
* Navegación: Lista\<Proyecto\>, Lista\<Habilidad\>

**Proyecto.cs**

* Id (int)  
* Título (cadena)  
* Descripción (cadena)  
* ImageUrl (cadena)  
* PortfolioUserId (Clave foránea)

**Habilidad.cs**

* Id (int)  
* Nombre (cadena)  
* Nivel (cadena)  
* PortfolioUserId (Clave foránea)

Utilice los atributos **\[Key\]** y **\[ForeignKey\]** según sea necesario. Utilice Copilot para ayudar a generar el código y las relaciones del modelo.

**Paso 3: Configurar EF Core**

En **SkillSnap.Api**, cree **SkillSnapContext.cs**:

public class SkillSnapContext : DbContext

{

    public SkillSnapContext(DbContextOptions\<SkillSnapContext\> options) : base(options) { }

    public DbSet\<PortfolioUser\> PortfolioUsers { get; set; }

    public DbSet\<Project\> Projects { get; set; }

    public DbSet\<Skill\> Skills { get; set; }

}

En **Program.cs**, registre el contexto y configure SQLite:

builder.Services.AddDbContext\<SkillSnapContext\>(options \=\>

    options.UseSqlite("Data Source=skillsnap.db"));

Run EF Core commands:dotnet ef migrations add InitialCreate

dotnet ef database update

**Paso 4: Rellenar la base de datos con datos de ejemplo**

Ahora que su base de datos está configurada, añada algunos datos de muestra que pueda utilizar para probar sus componentes.

#### **1\. Añada un nuevo controlador para sembrar datos**

En la carpeta **SkillSnap.Api/Controllers**, cree un nuevo archivo llamado **SeedController.cs**. Pegue el siguiente código:

using Microsoft.AspNetCore.Mvc;

using SkillSnap.Api.Models;

namespace SkillSnap.Api.Controllers

{

    \[ApiController\]

    \[Route("api/\[controller\]")\]

    public class SeedController : ControllerBase

    {

        private readonly SkillSnapContext \_context;

        public SeedController(SkillSnapContext context)

        {

            \_context \= context;

        }

        \[HttpPost\]

        public IActionResult Seed()

        {

            if (\_context.PortfolioUsers.Any())

            {

                return BadRequest("Sample data already exists.");

            }

            var user \= new PortfolioUser

            {

                Name \= "Jordan Developer",

                Bio \= "Full-stack developer passionate about learning new tech.",

                ProfileImageUrl \= "https://example.com/images/jordan.png",

                Projects \= new List\<Project\>

                {

                    new Project { Title \= "Task Tracker", Description \= "Manage tasks effectively", ImageUrl \= "https://example.com/images/task.png" },

                    new Project { Title \= "Weather App", Description \= "Forecast weather using APIs", ImageUrl \= "https://example.com/images/weather.png" }

                },

                Skills \= new List\<Skill\>

                {

                    new Skill { Name \= "C\#", Level \= "Advanced" },

                    new Skill { Name \= "Blazor", Level \= "Intermediate" }

                }

            };

            \_context.PortfolioUsers.Add(user);

            \_context.SaveChanges();

            return Ok("Sample data inserted.");

        }

    }

}

#### **2\. Llamar al Endpoint para Insertar Datos**

Inicie su proyecto API, luego haga una solicitud **POST** a este punto final:

https://localhost:\<your-port\>/api/seed

Puede utilizar una extensión de navegador como **REST Client**, **Postman**, o una herramienta de línea de comandos como **curl**:

curl \-X POST https://localhost:\<your-port\>/api/seed

Consejo: Sustituye **\<your-port\>** por el número de puerto real que aparece en tu terminal cuando se ejecuta la aplicación.

Recibirás un mensaje de confirmación si los datos se han añadido correctamente. Si lo intentas de nuevo más tarde, te devolverá un mensaje indicando que los datos ya existen.

**Paso 5: Scaffold Static Blazor Layouts**

En **SkillSnap.Client**, añada componentes de marcador de posición:

* **ProfileCard.razor**: Nombre, Biografía, Foto de Perfil  
* **ProjectList.razor**: Bucle de proyectos ficticios  
* **SkillTags.razor**: Habilidades en una lista de etiquetas

Utilice Copilot para ayudar a andamiaje cada componente .razor. Éstos se conectarán a la API en la siguiente actividad.

**Paso 6: Utilice Microsoft Copilot para refinar el código**

Pruebe instrucciones como:

* "Cree un componente Blazor para mostrar un perfil con nombre, biografía e imagen"  
* "Genere relaciones EF Core de uno a muchos con PortfolioUser y Project"  
* "Sugerir formas de limpiar los registros del servicio Program.cs"

Refactorice cualquier lógica o marcado basado en las sugerencias de Copilot.

### **Lista de comprobación de presentación**

* Solución con API y proyectos cliente añadidos  
* Modelos para PortfolioUser, Project y Skill creados  
* Base de datos creada correctamente con EF Core  
* Componentes de diseño estático en Blazor  
* Copilot utilizado para apoyar y mejorar la implementación  
* Proyecto guardado para su continuación en la Parte 2

**Introducción a la Actividad**

Ahora que tu aplicación SkillSnap tiene modelos de datos y componentes marcadores de posición, es hora de conectar tu front-end a la API. En esta actividad, construirás las rutas CRUD en la API y las vincularás a los componentes Blazor. Utilizarás la inyección de dependencias para crear servicios, enviar solicitudes HTTP y vincular los resultados a la IU.

En esta actividad:

* Construir puntos finales GET y POST para Proyectos y Habilidades  
* Crear servicios Blazor para obtener y enviar datos  
* Vincular datos a componentes mediante **@code** y **@inject**  
* Utilizar Copilot para ayudar a gestionar el flujo de datos y detectar errores

Esta es la segunda de cinco actividades. Al final de esta parte, su aplicación comenzará a funcionar como un sitio de cartera real.

### **Instrucciones de la actividad**

**Paso 1: Crear Controladores API**

En la carpeta **SkillSnap.Api/Controllers**, crear:

* **ProjectsController.cs**  
* **SkillsController.cs**

Cada controlador debe incluir:

* **\[HttpGet\]** para devolver todos los elementos  
* **\[HttpPost\]** para añadir un elemento  
* Utilice la inyección de constructores para **SkillSnapContext**

Ejemplo de solicitud de ayuda de Copilot:

* "Escribir un controlador para gestionar los datos del proyecto con GET y POST"

**Paso 2: Habilitar CORS en la API**

En **Program.cs**, añada:

builder.Services.AddCors(options \=\>  
{  
    options.AddPolicy("AllowClient", policy \=\>  
    {  
        policy.WithOrigins("https://localhost:5001")  
              .AllowAnyMethod()  
              .AllowAnyHeader();  
    });  
});  
app.UseCors("AllowClient");

Esto asegura que el cliente Blazor puede hacer peticiones a la API.

**Paso 3: Crear servicios en Blazor**

En **SkillSnap.Client/Services**, añada:

* **ProjectService.cs**  
* **SkillService.cs**

Cada servicio debe utilizar **HttpClient** para:

* **GetProjectsAsync()**  
* **AddProjectAsync(Project newProject)**

Registrarlos en **Program.cs** en el proyecto cliente:

builder.Services.AddScoped\<ProjectService\>();  
builder.Services.AddScoped\<SkillService\>();

**Paso 4: Conectar Servicios a Componentes**

En **ProjectList.razor**, añada:

@inject ProjectService ProjectService  
@code {  
    private List\<Project\> projects;  
    protected override async Task OnInitializedAsync()  
    {  
        projects \= await ProjectService.GetProjectsAsync();  
    }  
}

Repítalo para **SkillTags.razor**. Utilice botones o formularios de marcador de posición para probar la funcionalidad POST.

**Paso 5: Utilice Microsoft Copilot para mejorar la integración**

Pruebe instrucciones como:

* "Escriba un componente Blazor que muestre una lista de elementos de una API"  
* "Añada validación de formularios a un formulario Blazor"  
* "Sugerir la gestión de errores para llamadas HTTP fallidas en Blazor"

### **Lista de envío**

* Controladores API para Proyectos y Habilidades añadidos  
* CORS configurado para permitir la comunicación con el cliente  
* Servicios en Blazor creados y registrados  
* Componentes de IU conectados a fuentes de datos  
* Copilot utilizado para generar, probar o refinar la lógica  
* Datos cargados y mostrados en la interfaz de Blazor

**Introducción a la actividad**

Ahora que SkillSnap está mostrando datos, es hora de asegurar tu aplicación. En esta actividad, implementarás autenticación y autorización. Construirás rutas de inicio de sesión y registro utilizando ASP.NET Identity, asegurarás los puntos finales sensibles de la API y añadirás la funcionalidad de inicio de sesión/cierre de sesión en el front-end Blazor.

En esta actividad, deberá:

* Configurar ASP.NET Identity para la gestión de usuarios  
* Crear API de registro e inicio de sesión  
* Utilizar la autorización basada en funciones para restringir el acceso a la API  
* Implementar la funcionalidad de inicio/cierre de sesión en Blazor  
* Utilizar Copilot para verificar la lógica de autenticación

Esta es la tercera de cinco actividades. Al final, sólo los usuarios autenticados podrán modificar el contenido de tu aplicación.

### **Instrucciones de la actividad**

**Paso 1: Añadir ASP.NET Identity a la API**

Instale los paquetes Identity:

1

2

Cree una clase **ApplicationUser** que herede de **IdentityUser**. Actualice **SkillSnapContext**:

1

En **Program.cs**, registre Identity:

2

1

Ejecutar migraciones:

1

2

**Paso 2: Crear AuthController**

En **SkillSnap.Api/Controllers**, crear **AuthController.cs**. Añadir endpoints:

* **\[HttpPost\] /api/auth/register** \- registrar nuevo usuario  
* **\[HttpPost\] /api/auth/login** \- autenticar y devolver JWT

Utilice **UserManager**, **SignInManager**, y la lógica de generación de token.

Utilice las instrucciones de Copilot:

* "Generar controlador de inicio de sesión ASP.NET Identity que devuelva un JWT"  
* "Registrar un usuario de forma segura utilizando ASP.NET Identity"

**Paso 3: Proteger los puntos finales de la API**

Añada **\[Authorize\]** a los métodos **ProjectsController** y **SkillsController** que modifican datos. Utilícelos:

**\[Authorize(Roles \= "Admin")\]**

para rutas que requieran privilegios elevados.

**Paso 4: Añadir Login/Logout a la IU de Blazor**

En **SkillSnap.Client**, cree:

* **Login.razor**  
* **Register.razor**  
* **AuthService.cs** para gestionar el almacenamiento del token y el estado del usuario

Almacene el token en el almacenamiento local y adjúntelo a las solicitudes de API.

Utilice Copilot para ayudar con:

* "Crear formulario de inicio de sesión Blazor con almacenamiento de token"  
* "Persistencia de la sesión de usuario tras recargar la página"

**Paso 5: Pruebe el flujo de autenticación completo**

* Registre un usuario  
* Inicie sesión y reciba un token  
* Utilice el token para acceder a rutas protegidas  
* Intente realizar solicitudes no autorizadas y confirme que están bloqueadas

### **Lista de comprobación de presentación**

* Incorporación de ASP.NET Identity a la API  
* Base de datos actualizada con el esquema de identidad  
* Funcionamiento de los puntos finales de la API de inicio de sesión y registro  
* Rutas API protegidas mediante **\[Authorize\]** y roles  
* Creación de la IU de inicio/cierre de sesión de Blazor  
* Uso de Copilot para orientación y depuración  
* Flujo de autenticación completo verificado

**Introducción a la actividad**

A medida que tu aplicación crece, el rendimiento se vuelve crítico. En esta actividad, implementarás el almacenamiento en caché en tu API para reducir la carga de la base de datos y mejorar la velocidad. También gestionarás el estado de la sesión en el front-end para mejorar la experiencia del usuario. Utilizarás Copilot para encontrar y aplicar optimizaciones en todo el proceso.

En esta actividad:

* Implementar el almacenamiento en caché en memoria en la API  
* Optimizar la lógica del controlador y reducir las consultas redundantes  
* Almacenar la sesión del usuario o la información de rol en el estado de Blazor  
* Utilizar Copilot para guiar las decisiones de almacenamiento en caché y optimización

Esta es la cuarta de cinco actividades. Su aplicación será más fluida, más rápida y estará mejor preparada para su uso en el mundo real.

### **Instrucciones de la actividad**

**Paso 1: Añadir el almacenamiento en caché en memoria a la API**

En **Program.cs**, registre el servicio:

**builder.Services.AddMemoryCache();**

En **ProjectsController** o **SkillsController**, utilice **IMemoryCache** para almacenar en caché las consultas comunes:

public class ProjectsController : ControllerBase  
{  
    private readonly SkillSnapContext \_context;  
    private readonly IMemoryCache \_cache;  
    public ProjectsController(SkillSnapContext context, IMemoryCache cache)  
    {  
        \_context \= context;  
        \_cache \= cache;  
    }  
    \[HttpGet\]  
    public async Task\<IActionResult\> GetProjects()  
    {  
        if (\!\_cache.TryGetValue("projects", out List\<Project\> projects))  
        {  
            projects \= await \_context.Projects.ToListAsync();  
            \_cache.Set("projects", projects, TimeSpan.FromMinutes(5));  
        }  
        return Ok(projects);  
    }  
}

Utilice las indicaciones de Copilot:

* "Añadir almacenamiento en caché en memoria al controlador ASP.NET Core"  
* "Implementar almacenamiento en caché con lógica de caducidad y fallback"

**Paso 2: Optimizar las consultas y la lógica del controlador**

En su API:

* Añada **.AsNoTracking()** donde no se requieran actualizaciones  
* Utilice **.Include()** para reducir los viajes de ida y vuelta de los datos relacionados

Pregunte a Copilot con:

* "Optimice las consultas del controlador de EF Core para mejorar el rendimiento"

**Paso 3: Gestionar el estado en Blazor Front End**

En **Program.cs**, registre los servicios contenedores de estado de **Scoped** (por ejemplo, **UserSessionService**).

Cree una clase de servicio que almacene

* **UserId**  
* **Role**  
* Cualquier estado de edición/proyecto actual

Inyéctelo en componentes para persistir la información del usuario a través de componentes sin recargar.

Utilice Copilot para:

* "Crear un servicio de gestión de estado Blazor para la información del usuario conectado"

**Paso 4: Medir y verificar las mejoras**

* Utilice **Stopwatch** para medir la duración de las solicitudes  
* Pruebe manualmente el tiempo de carga antes y después de los cambios  
* Registre los aciertos y errores de la caché para confirmar la eficacia

### **Lista de comprobación**

* Caché en memoria implementada en la API  
* Mejoras de rendimiento aplicadas en la lógica de consulta  
* Gestión del estado de Blazor activada  
* Sugerencias de Copilot utilizadas y aplicadas  
* Rendimiento verificado mediante pruebas o registros

**Introducción a la actividad**

En este paso final, completarás la aplicación SkillSnap asegurando que todas las funcionalidades principales funcionen como se espera. Validarás los flujos de usuarios, las actualizaciones de datos, la autenticación y las mejoras de rendimiento. También perfeccionarás la experiencia del usuario y aplicarás las últimas mejoras de código basadas en Copilot. Este trabajo preparará su proyecto para la presentación a los compañeros.

En esta actividad

* Probar la autenticación y los Controles de acceso  
* Verificar las operaciones CRUD en toda la IU  
* Confirmar que el almacenamiento en caché y la lógica de estado funcionan según lo previsto  
* Refactorizar el código con la ayuda de Copilot  
* Documentar su arquitectura y características para la revisión por pares

### **Instrucciones de la actividad**

**Paso 1: Validar el flujo completo de la aplicación**

* Empezar desde el registro/inicio de sesión y probar el acceso de los usuarios  
* Añada, actualice y elimine proyectos o competencias  
* Confirme que sólo los usuarios autenticados pueden editar  
* Compruebe que las acciones no autorizadas están bloqueadas

Utilice Copilot para validar las rutas:

* "Revisar la lógica de punto final seguro en ASP.NET Core"  
* "Confirmar el uso del token Blazor en HttpClient"

**Paso 2: Verificar la Coherencia de los datos y el almacenamiento en caché**

* Actualizar los datos después de las operaciones CRUD  
* Comprobar si los datos almacenados en caché se actualizan según lo esperado  
* Registrar el uso de la caché (aciertos/errores) en la API para su verificación

**Paso 3: Revisión y refactorización con Copilot**

Utilice Copilot para

* Eliminar código y servicios no utilizados  
* Sugerir mejoras de nomenclatura o estructura  
* Generar comentarios de código o métodos de ayuda

Ejemplos de instrucciones:

* "Revise este componente en busca de lógica redundante"  
* "Sugerir mejoras para esta clase de servicio Blazor"

**Paso 4: Pulido final de UX**

* Ajuste el diseño, el espaciado o el tamaño de las fuentes en los componentes de Blazor  
* Añade imágenes de marcador de posición o datos de muestra para facilitar la lectura  
* Probar la interfaz en escritorio y móvil si es posible

**Paso 5: Preparación para la revisión por pares**

Documente lo siguiente para su presentación:

* Resumen del proyecto (qué hace tu aplicación)  
* Características principales (CRUD, seguridad, almacenamiento en caché, estado)  
* Su proceso de desarrollo y el uso de Copilot  
* Cualquier problema conocido o futuras mejoras

Guarde este documento para enviarlo junto con el código de su proyecto.

### **Lista de comprobación para la presentación**

* Todos los componentes de la aplicación funcionan y están conectados  
* CRUD, autenticación y estado verificados  
* Aplicación de las recomendaciones de Copilot para perfeccionar el código  
* Revisión del diseño y la UX de la aplicación  
* Resumen del proyecto listo para la revisión por pares

