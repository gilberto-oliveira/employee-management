# Employee Management Application

Welcome to the Employment Management Application. This platform is designed to help you efficiently manage employee information. With it, you can easily add, delete, and update employee records. You can also view a list of all employees and search for specific employees by their first name, last name, or document number.

## Backend

The backend of this project is built using **.NET 8** with **C#** and **SQL Server**. Key features of the backend include:

- **Minimal API**: A lightweight approach to creating APIs in .NET Core, offering better performance and easier maintenance compared to traditional methods.
- **Repository Pattern**: Used to abstract database access and promote better maintainability by avoiding direct interaction with the database context.
- **Business Logic**: Services are created to manage the business logic, such as authentication and authorization.
- **CQRS Pattern**: Implements Command Query Responsibility Segregation (CQRS) to separate read and write operations, enhancing code clarity and maintainability, even in small projects.
- **JWT Authentication & Authorization**: Utilizes JWT for user authentication. While currently using internal logic for role management, it’s recommended to implement an Identity Provider (e.g., Keycloak) for more scalable user and role management. The system currently has three roles: `Employee`, `Leader` and `Director`.
- **Entity Framework Core**: Database operations are managed with Entity Framework Core, and migrations are handled through the `dotnet ef` CLI.
- **Unit Tests**: Written with **xUnit** and **NSubstitute**.

The backend is organized in two projects:

1. `EmployeeManagement.Api`: contains all the backend logic, with endpoints, repositories, models...
2. `EmployeeManagement.Test`: Contains the backend UnitTests

The project `EmployeeManagement.Api` contains four main folders for better maintainability and scalability, following **SOLID principles** and **Clean Architecture**:

1. **Features**: Contains API endpoints, commanders, queries and services (handlers).
2. **Models**: Houses domain models.
3. **Abstractions**: Contains the interfaces and helpers classes.
4. **Infrastructure**: Manages the database context, logging and repository 
implementations.

### Known Issues
- Passwords are being stored in the database insecurely.
- Unit tests do not cover all the code.

## Frontend

The frontend is a **Angula 18 standarlone** application built with **TypeScript**. To run the frontend, ensure you have **Node.js** installed.

- **API Requests**: Instead of using `axios`, a dedicated service manages API requests, making it easier to maintain in a small project.
- **Routing**: Uses **angular-routing** for routing.
- **UI**: Styled using **PrimeNG** components.

The folder structure follows Angular's component-based architecture, with organized directories for **auth** (implement the authentication logic and components), **modules** (core components).

### Known Issues
- Tests do not cover all the code.
- Some styles could be improved.

## Installation

To install the Employment Management Application:

1. Clone the repository to your local machine.
2. Install the necessary dependencies for both frontend and backend.

Both frontend and backend can run in Docker containers. Ensure **Docker** is installed, then use the provided `docker-compose.yml` file to start the project:

```bash
  docker-compose build && 
  docker-compose up -d
```

For testing, the data below can be used:

```bash
curl -X 'POST' \
  'http://localhost:7001/api/v1/employees/auth' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "docNumber": "1234567891",
  "password": "Test!@123456"
}'
```

The frontend application is hosted in **http://localhost:8080/auth/login** and the backend in **http://localhost:7001/swagger/index.html**

### Known Issues
- The container instance `employee-management-db` don´t have permission to access the **.sql-server-data** folder. To fix it, type in the terminal:

```bash
  docker-compose down
```

```bash
  sudo chmod -R 777 .sqlserver-data &&
  sudo chown -R $(whoami):$(whoami) .sqlserver-data &&
  sudo chown -R 10001:10001 .sqlserver-data
```

```bash
  docker-compose up -d
```
