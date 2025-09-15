# LiquourApp

Aplicación web para una tienda de venta de licores desarrollada con ASP.NET Core MVC.

## Descripción del Proyecto

LiquourApp es una aplicación web para una tienda de venta de licores en línea. El proyecto está desarrollado utilizando C# ASP.NET Core MVC con SQL Server como base de datos.

## Estructura del Proyecto

El proyecto sigue una arquitectura MVC (Modelo-Vista-Controlador) y está organizado de la siguiente manera:

- **Models**: Contiene las clases de modelo que representan los datos de la aplicación.
- **Views**: Contiene las vistas que se muestran al usuario.
- **Controllers**: Contiene los controladores que manejan las solicitudes HTTP.
- **Data**: Contiene las clases relacionadas con el acceso a datos y Entity Framework.
- **Services**: Contiene los servicios que implementan la lógica de negocio.
- **wwwroot**: Contiene archivos estáticos como CSS, JavaScript e imágenes.

## Sprints y Funcionalidades

### Sprint 1 (15 días)

Objetivo: Implementar el módulo de gestión de usuarios (Registro, Login y Verificación de Edad).

#### Historias de Usuario:

1. **HU1 - Inicio de Sesión**
   - Desarrollar el flujo de autenticación seguro (login + JWT), con persistencia de sesión en navegador.
   - Responsable: Daniel Losada
   - Tiempo máximo: 8 días

2. **HU2 - Registro de Usuario**
   - Implementar el flujo completo de registro de nuevos usuarios, asegurando validaciones y persistencia en BD.
   - Responsable: Sebastian Gutierrez
   - Tiempo máximo: 8 días

3. **HU3 - Verificación de Edad**
   - Implementar validación de mayoría de edad, integrando reglas de negocio en frontend y backend.
   - Responsable: Estiven Parra
   - Tiempo máximo: 9 días

## Requisitos Previos

- .NET 7.0 SDK
- SQL Server (Local o Express)
- Visual Studio 2022 o Visual Studio Code

## Configuración del Entorno de Desarrollo

1. Clonar el repositorio:
   ```
   git clone <url-del-repositorio>
   cd LiquourApp
   ```

2. Restaurar los paquetes NuGet:
   ```
   dotnet restore
   ```

3. Actualizar la cadena de conexión en `appsettings.json` según tu entorno local.

4. Aplicar las migraciones para crear la base de datos:
   ```
   dotnet ef database update
   ```

5. Ejecutar la aplicación:
   ```
   dotnet run
   ```

## Flujo de Trabajo Git

1. Crear una rama a partir de `main` para cada historia de usuario:
   ```
   git checkout -b feature/HU1-login
   git checkout -b feature/HU2-registro
   git checkout -b feature/HU3-verificacion-edad
   ```

2. Realizar commits frecuentes con mensajes descriptivos.

3. Al finalizar la historia de usuario, crear un Pull Request a `main`.

4. Solicitar revisión de código a otro miembro del equipo.

5. Una vez aprobado, hacer merge a `main`.

## Convenciones de Código

- Seguir las convenciones de nomenclatura de C# (PascalCase para clases y métodos, camelCase para variables).
- Utilizar comentarios para explicar la lógica compleja.
- Mantener los controladores ligeros, delegando la lógica de negocio a los servicios.
- Escribir pruebas unitarias para la lógica crítica.

## Contacto

Para cualquier duda o sugerencia, contactar a los responsables del proyecto:

- Daniel Losada
- Sebastian Gutierrez
- Estiven Parra