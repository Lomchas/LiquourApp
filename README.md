# LiquourApp

Aplicación web para la gestión de una tienda de licores con autenticación de usuarios y verificación de edad.

## Características

- Autenticación de usuarios con JWT
- Verificación de edad para cumplir con regulaciones de venta de alcohol
- Base de datos PostgreSQL en la nube (ElephantSQL)
- Interfaz de usuario responsive

## Requisitos

- .NET 8.0
- PostgreSQL (a través de ElephantSQL)
- Navegador web moderno

## Configuración

### Base de Datos

La aplicación utiliza ElephantSQL como servicio de base de datos PostgreSQL en la nube:

1. Regístrate en [ElephantSQL](https://www.elephantsql.com/)
2. Crea una nueva instancia (plan gratuito)
3. Obtén los datos de conexión (host, nombre de base de datos, usuario y contraseña)
4. Actualiza el archivo `appsettings.Development.json` con tus credenciales

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=YOUR_ELEPHANTSQL_HOST;Database=YOUR_DB_NAME;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
}
```

### Autenticación JWT

La aplicación utiliza JWT para la autenticación de usuarios. Las claves JWT se generan dinámicamente cada 60 minutos por usuario.

## Estructura del Proyecto

El proyecto sigue la arquitectura MVC (Model-View-Controller):

- **Models**: Definiciones de entidades como User, Product, etc.
- **Views**: Interfaces de usuario en Razor
- **Controllers**: Lógica de manejo de peticiones HTTP
- **Services**: Lógica de negocio y autenticación
- **Data**: Contexto de base de datos y migraciones

## Desarrollo

### Ramas de Git

El proyecto utiliza tres ramas principales:

- **main**: Código de producción estable
- **test**: Pruebas de nuevas características
- **dev**: Desarrollo activo

### Flujo de Trabajo

1. Desarrolla nuevas características en la rama `dev`
2. Realiza pruebas en la rama `test`
3. Una vez aprobadas, integra los cambios a `main`

## Despliegue

Para desplegar la aplicación:

```bash
dotnet publish -c Release
```

## Migraciones de Base de Datos

Para crear y aplicar migraciones:

```bash
dotnet ef migrations add [NombreMigracion]
dotnet ef database update
```

## Licencia

[MIT](LICENSE)