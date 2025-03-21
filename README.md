# Edu Quest Platform 🎓

**Edu Quest Platform** is a comprehensive solution designed to revolutionize the way users create, share, and engage with educational content by integrating gamification and interactive tools. The platform empowers educators and learners to explore educational material in an engaging, dynamic environment with features such as quizzes, challenges, and user progress tracking.

---

## Project Structure 📂

The **Edu Quest Platform** follows **Clean Architecture** and **CQRS (Command Query Responsibility Segregation)** patterns to ensure separation of concerns, maintainability, and scalability. Below is a breakdown of the folder structure and the purpose of each project in the solution.

### 1. **EduQuest_API** 🌐
This project is responsible for the **API layer**, exposing endpoints that the client applications can consume. It handles incoming HTTP requests, calls the application services, and returns appropriate responses.

- **Controllers**: Defines API controllers that manage HTTP requests and responses.
- **Dependencies**: Contains the dependency injection setup for the application services and repositories.
- **Middleware**: Custom middleware to handle things like logging, exception handling, and authentication.
- **Template**: Contains basic templates or structure for API responses.
- **Program.cs**: The entry point of the application.

### 2. **EduQuest_Application** ⚙️
This project represents the **application layer**. It holds the core business logic of the platform and manages user actions and requests. The focus here is on implementing **CQRS** principles, where commands and queries are handled separately.

- **Abstractions**: Defines interfaces for the application layer services.
- **Behaviour**: Contains the business logic and behaviors used in application commands and queries.
- **DTO (Data Transfer Objects)**: Represents the data structures used for communication between layers.
- **ExternalServices**: Defines integration with external services like payment providers, email services, etc.
- **Helper**: Utility classes used throughout the application layer.
- **Mappings**: Contains logic for mapping between domain models and DTOs.
- **UseCases**: Defines the application-specific use cases, grouped by commands and queries.
- **DependencyInjection.cs**: Sets up dependency injection for the application services.

### 3. **EduQuest_Domain** 🏢
The domain layer holds the core business models, entities, and domain logic. It represents the heart of the application where the rules of the platform are defined.

- **Constants**: Defines constant values used across the domain layer.
- **Enums**: Defines enums for various platform configurations (e.g., user roles, subscription types).
- **Entities**: Contains the core domain entities, representing the business objects (e.g., User, Course, Subscription).
- **Models**: Defines domain models and related logic.

### 4. **EduQuest_Infrastructure** 🛠️
This project is responsible for the **data access layer** and other infrastructure-related components, such as third-party integrations. It provides implementations for the repository interfaces defined in the domain layer.

- **Dependencies**: Manages infrastructure-specific services and configurations.
- **Configurations**: Contains configuration settings related to the database and other services.
- **Extensions**: Provides extension methods that add functionality to various classes.
- **ExternalServices**: Manages integration with third-party services and APIs.
- **Migrations**: Contains the Entity Framework migrations for the database schema.
- **Persistence**: Implements the repository pattern for accessing the database.
- **Repository**: Contains repository interfaces and implementations for interacting with the data store.

### 5. **EduQuest_Test** 🔧
The **test project** ensures the integrity of the platform by providing unit and integration tests for all components in the application. It is structured to test both the business logic (application layer) and the infrastructure (data access).

---

## Getting Started 🚀

To get started with the Edu Quest Platform, follow these steps:

### Prerequisites 🔑

- **.NET SDK** (version 6.0 or higher)
- **PostgreSQL** (or any database service you prefer)
- **Docker** (optional, for containerization)
