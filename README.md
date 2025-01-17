# hfsolutions-desafio-dotnet

Proyecto de proceso de selección, como Desarrollador .NET.

## 1.- Pasos para ejecutar el Proyecto

### 1.1.- Visual Studio

Para ejecutar el proyecto se requiere realizar cambios en el archivo **appsettings.json** para modificar la cadena de conexión a la base de datos con las credenciales que se utilizarán.

### 1.2.- IIS

Queda a su disposición la forma de utilizar los sitios, el proyecto no utiliza alguna variable de entorno para funcionar. Se omitió la prueba de este paso, ya que no se cuenta con licencia de Windows Server.

## 2.- Pasos para configurar la base de datos

Se requiere crear un usuario para utilizar la base de datos de tareas de usuarios y configurar la aplicación como se menciona en el punto *1.1*.

Para la base de datos se utilizó la versión de SQL Server 2022 en su última versión:

**Microsoft SQL Server 2022 (RTM-CU16) (KB5048033) - 16.0.4165.4 (X64)**

El script de creación de base de datos intentará crear una base de datos con el nombre "UserTasks" y un usuario que es dueño de la misma con nombre y contraseña "hfsolutions",

también asignará el rol *db_owner* a la misma, por lo tanto se necesita que se ejecute con un usuario con los permisos suficientes para no resultar con errores.

El proceso de creación de la base de datos fue mediante la técnica *Code First*, ya que por tiempo y dado el tamaño del modelado de datos, fue la opción más rápida, así que el script resultante es a base de las migraciones realizadas en código.

El nombre del script de la base de datos es: *user_tasks_hf_solutions.sql*.

## 3.- Información adicional

### 3.1.- General

- La documentación del código no se realizó de forma exhaustiva, por temas de tiempo, pero se documentaron los controladores principales de la aplicación, estos son: *UserController* y *UserTaskController*.

### 3.2.- Endpoints

- A Excepción del enpoint *api/User/Login*, todos los enpoints requieren de un usuario autenticado.

- Se crearon endpoints de CRUD para usuarios y tareas de usuarios.

- No se eliminó el ejemplo de API WeatherForecast, dado que permite una prueba más rápida de una ejecución correcta de la API.

- El endpoint utilizado en las tareas de usuario con filtros y paginación es el *api/UserTask/All*. El endpoint hermano, siendo la versión anterior a este es *api/UserTask/AllTasks*, no se eliminó para usos prácticos.

### 3.3.- Tests unitarios

Se realizaron dos test unitarios, uno para inicio de sesión y otro para obtener las tareas de un usuario logueado.

- Para poder ejecutar correctamente las pruebas, en el archivo *appsettings.json* del proyecto *HFSolutions.TestDotNet.Tests* se debe cambiar la url de la API en la sección **ApiUrls:Base**.

- También de los tests *LoginTests* y *UserTasksTests* se debe cambiar el usuario y contraseña de la data en línea con los que se ejecutan. Por ejemplo:

```csharp
//HFSolutions.TestDotNet.Tests.UserTests.LoginTests.cs Línea 25
[Theory]
[InlineData("<USUARIO>", "<CONTRASEÑA>")]
public async Task Login_Should_Get_JWT_Token(string username, string password)
{
    string token = await _userServiceTests.Login(username, password);

    Assert.NotEmpty(token);
}
```