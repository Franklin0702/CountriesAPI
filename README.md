# Countries API

A sample **.NET Web API** implementing **IDesign architectural principles** to enforce a clean separation of responsibilities across application layers.

The project exposes CRUD operations for **Countries** and **Cities** while maintaining clear architectural boundaries between layers such as **Host**, **Manager**, **Engine**, **Access**, and a shared **Utility layer**.

The main objective of the project is to demonstrate a **maintainable layered architecture** where responsibilities are clearly defined and dependencies flow in a single direction.

---

# Architecture Overview

This solution follows the **IDesign Method**, which promotes strict separation of responsibilities between application layers.

Each layer performs a **single well-defined role**, preventing logic from leaking across boundaries and keeping the system maintainable over time.

```
Host → Manager → Engine → Access
          ↓
        Utility
```

Dependency flow always moves **downwards**, while the **Utility layer** is shared across layers for reusable helpers.

---

# Solution Structure

```
CountriesAPI
│
├── App.Host        → API entry point
├── App.Manager     → Application orchestration
├── App.Engine      → Business rules and validation
├── App.Access      → Data access and persistence
├── App.Utility     → Shared helpers and cross-layer utilities
└── App.Tests       → Tests
```

Each layer exists in its own project to enforce **clear boundaries and dependency control**.

---

# Layer Responsibilities

## Host Layer (App.Host)

The **Host layer** is responsible for exposing the API and configuring the application.

Responsibilities:

* ASP.NET Controllers
* Dependency Injection configuration
* Application startup
* HTTP concerns (routing, serialization, status codes)

Example structure:

```
Controllers/
  CityController.cs
  CountryController.cs

Program.cs
DependencyInjectionConfig.cs
```

Controllers should remain **thin** and should only call **Managers**.
They should not include business rules or persistence logic.

---

## Manager Layer (App.Manager)

Managers orchestrate application use cases.

Responsibilities:

* Coordinate operations
* Call validation engines
* Call the access layer for persistence
* Transform entities into API models
* Implement application workflows

Example structure:

```
Countries/
  CountryManager.cs
  ICountryManager.cs

Cities/
  CityManager.cs
  ICityManager.cs
```

Typical flow:

```
Controller
   ↓
Manager
   ↓
Engine
   ↓
Access
```

Managers coordinate the flow but should **not contain business validation logic**.

---

## Engine Layer (App.Engine)

The **Engine layer** implements business rules and domain validation.

Responsibilities:

* Business rule validation
* Domain consistency checks
* Policy enforcement
* Domain-level calculations

Example structure:

```
Countries/
  CountryValidationEngine.cs

Cities/
  CityValidationEngine.cs
```

Engines operate purely on **data passed to them** and do not interact with infrastructure concerns directly.

---

## Access Layer (App.Access)

This layer handles **all persistence operations**.

Responsibilities:

* Database communication
* Entity Framework DbContext
* Data queries
* Migrations
* Entity persistence

Example structure:

```
DbContext.cs

Countries/
  CountryAccess.cs
  ICountryAccess.cs

Cities/
  CityAccess.cs
  ICityAccess.cs
```

Entities are defined in this layer:

```
Entities/
  Country.cs
  City.cs
```

Only the **Access layer interacts with the database**.

---

## Utility Layer (App.Utility)

The **Utility layer** provides reusable functionality shared across layers.

Responsibilities:

* Common helpers
* Shared extensions
* Reusable infrastructure utilities
* Cross-cutting helpers used across the system

Examples of what typically belongs here:

```
Extensions
Mapping helpers
Guard clauses
Reusable validation helpers
General utility classes
```

This layer is designed to **avoid duplication across the application**.

---

# Dependency Rules

The architecture enforces **strict dependency direction**.

Allowed dependencies:

```
Host → Manager → Engine → Access
        ↓
      Utility
```

The Utility layer may be referenced by any other layer.

Forbidden dependencies:

```
Access → Manager
Access → Engine
Engine → Manager
Host → Access
```

These rules prevent architectural erosion.

---

# Dependency Injection

Each layer exposes a **DI helper** to register its dependencies.

Example:

```
App.Access.DIHelper
App.Engine.DIHelper
App.Manager.DIHelper
```

The **Host layer acts as the composition root**, registering all dependencies during startup.

```
Program.cs
   ↓
DependencyInjectionConfig
   ↓
Register layers
```

This ensures dependency wiring remains centralized.

---

# Example Request Flow

Example: Creating a Country

```
HTTP POST /countries
        │
        ▼
CountryController
        │
        ▼
CountryManager
        │
        ▼
CountryValidationEngine
        │
        ▼
CountryAccess
        │
        ▼
Database
```

Each layer handles a **specific responsibility**, keeping the architecture predictable.

---

# Trade-offs Made for Simplicity

This project intentionally simplifies some aspects to keep the architecture clear and easy to understand.

### Entities Located in Access Layer

Entities are stored in the **Access layer** rather than a separate domain project.

Trade-off:

* Simpler project layout
* Faster implementation

In larger systems, entities often live in a dedicated **Domain layer**.

---

### Manual DTO Mapping

Managers currently handle **mapping between entities and API models manually**.

Trade-off:

* Reduces external dependencies
* Keeps mapping explicit

More complex systems often introduce **mapping frameworks or dedicated mapping services**.

---

### Lightweight Validation Engines

Engines currently perform **simple validation logic**.

Trade-off:

* Easier to follow
* Less domain complexity

Real-world systems may include:

* richer domain policies
* domain services
* rule engines

---

### SQLite for Persistence

SQLite is used for simplicity.

Trade-off:

* Easy setup
* Minimal configuration

Production systems usually use:

* SQL Server
* PostgreSQL
* MySQL

---

# Possible Future Improvements

The following improvements would increase robustness and scalability if the system were expanded.

### Introduce a Domain Layer

Separating domain entities from persistence entities allows stronger domain modeling.

```
Domain/
  Entities
  ValueObjects
  Aggregates
```

Benefits:

* persistence independence
* richer domain modeling

---

### Introduce Domain Events

For more complex workflows:

```
CountryCreatedEvent
CityCreatedEvent
```

These events allow decoupled business processes.

---

### Introduce API Versioning

Versioning allows the API to evolve safely.

```
/api/v1/countries
/api/v2/countries
```

---

### Improve DTO Separation

DTOs could be separated into:

```
Requests
Responses
ViewModels
```

This helps keep API contracts stable over time.

---

### Introduce Caching

Caching improves performance for read-heavy endpoints.

Possible implementations:

```
MemoryCache
Redis
```

---

### Introduce Structured Logging

Structured logging helps monitor and diagnose production systems.

Examples:

```
Serilog
OpenTelemetry
```

---

# Running the Project

```
dotnet restore
dotnet build
dotnet run --project App.Host
```

The API will start and expose endpoints for managing **Countries** and **Cities**.

---

# Key Design Goals

This project demonstrates:

* Clear separation of responsibilities
* Layered architecture
* Controlled dependency direction
* Maintainable application structure

The architecture is designed so that the application can **evolve without becoming tightly coupled or difficult to maintain**.
