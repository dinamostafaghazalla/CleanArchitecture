## Technologies

* ASP.NET Core 5
* Entity Framework Core 5
* Angular 10
* MediatR
* AutoMapper
* FluentValidation
* NUnit, FluentAssertions, Moq & Respawn

## Getting Started
Follow these steps to get your development environment set up:

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest [Node.js LTS](https://nodejs.org/en/)
3. Clone the repository
4. At the root directory, restore required packages by running:
    ```
    dotnet restore
    ```
5. Next, build the solution by running:
    ```
    dotnet build
    ```
6. Next, within the `\src\WebUI\ClientApp` directory, launch the front end by running:
    ```
    npm start
    ```
7. Once the front end has started, within the `\src\WebUI` directory, launch the back end by running:
    ```
    dotnet run
    ```
8. Launch [https://localhost:5001/](http://localhost:5001/) in your browser to view the Web UI front end (Angular)

9. Launch [https://localhost:5001/api](http://localhost:5001/api) in your browser to view the API Swagger Documentation

7. Launch https://localhost:5001/.well-known/openid-configuration to view identity server openid configuration


### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **WebUI/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/WebUI`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a single page application based on Angular 10 and ASP.NET Core 5. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

## License

This project is licensed with the [MIT license](LICENSE).
