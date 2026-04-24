# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Solution Architecture

This is a .NET 6 full-stack web application with the following structure:

- **CNet.Web.Api** - ASP.NET Core Web API backend (Main API server on port 8916)
- **CNet.Web.Admin** - Vue.js 2.x frontend with ViewUI (iView) components
- **CNet.App.Api** - Additional API project
- **CNet.BLL** - Business Logic Layer (services)
- **CNet.DAL** - Data Access Layer (repositories using Dapper)
- **CNet.Model** - Entity models and DTOs
- **CNet.Common** - Shared utilities (Redis, Excel, Logging, AutoMapper)
- **CNet.DBUtility** - Database helper classes supporting MySQL, SQLite, SQL Server
- **CNet.UniAPP** - UniApp mobile project
- **CNet.CodeGen** - Code generation tool for scaffolding

## Common Commands

### Backend (.NET 6)

```bash
# Build the solution
dotnet build CNet.sln

# Run the main Web API
dotnet run --project CNet.Web.Api

# Restore packages
dotnet restore

# Run with specific configuration
dotnet run --project CNet.Web.Api --configuration Release
```

### Frontend (Vue.js)

```bash
cd CNet.Web.Admin
npm install
npm run dev    # Development server with hot-reload
npm run build  # Production build
```

## Architecture Patterns

### 3-Tier Data Access Pattern
- **Controller** → **BLL (Service)** → **DAL (Repository)** → **Database**
- Controllers in `CNet.Web.Api/Controllers/` handle HTTP requests
- Services in `CNet.BLL/Main/` or `CNet.BLL/T4.DapperExt/Main/` contain business logic
- Repositories in `CNet.DAL/Main/` or `CNet.DAL/T4.DapperExt/Main/` handle data operations

### Base Classes
- `ServiceMain<T>` - Base class for services (inherits from `RepositoryMain<T>`)
- `RepositoryMain<T>` - Base class for repositories (inherits from `BaseDataDapperContribMySql<T>`)
- `BaseController` - Base controller with common functionality

### Dependency Injection
- Uses Autofac container (`Startup.ConfigureContainer()`)
- Manual registration in `CNet.BLL/DI_ServiceInit.cs` implementing `IModuleInitializer`
- Services use constructor injection

## Code Generation with T4 Templates

The solution uses T4 templates for scaffolding CRUD operations:

- `CNet.Model/T4.DapperExt/Main/1Entity.tt` - Generates entity models from database
- `CNet.DAL/T4.DapperExt/Main/1Repository.tt` - Generates repository classes
- `CNet.BLL/T4.DapperExt/Main/1Service.tt` - Generates service classes

Run T4 templates to regenerate code from database schema changes.

## Database Configuration

Connection strings are in `CNet.Web.Api/appsettings.json`:
- Default: MySQL (`connStr_Main`)
- Supports SQL Server and SQLite via `DBHelperFactory`

The `Connection` class provides `MainStr` for the main connection string.

## Key Technologies

- **Dapper + Dapper.Contrib** - Micro-ORM for data access
- **Autofac** - Dependency injection container
- **JWT Bearer** - Authentication
- **Swagger/Swashbuckle** - API documentation at `/doc` route
- **log4net** - Logging with multiple repository types
- **Redis** - Caching (optional)
- **NPOI** - Excel operations
- **AutoMapper/TinyMapper** - Object mapping

## Frontend Notes

- Vue 2.6.x with ViewUI 4.x component library
- Axios for HTTP requests
- Vuex for state management
- Vue Router for routing
- Supports Office documents (@vue-office), rich text editing (@wangeditor)

## Special Considerations

1. **Synchronous IO**: Kestrel configured to allow synchronous IO for request body reading
2. **JWT Lifetime Validation**: Disabled (`ValidateLifetime = false`) - tokens don't expire
3. **CORS**: Allows all origins by default (`AllowAnyOrigin()`)
4. **Swagger Authorization**: Protected with credentials in `appsettings.json` (only in non-development)
5. **Database Provider**: Defaults to MySQL but supports SQL Server and SQLite
