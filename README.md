# C# .NET Web Application Sandbox

A comprehensive task management web application built with ASP.NET Core, designed to help you learn how to add features to a C# application.

## 🚀 Getting Started

### Prerequisites
1. **Install .NET 8.0 SDK** from [Microsoft's official website](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Install Visual Studio Code** (recommended) or Visual Studio
3. **Install C# extension** for VS Code if using VS Code

### Quick Setup
```bash
# Navigate to the project directory
cd TaskManagerSandbox

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will be available at: `https://localhost:7001` and `http://localhost:5000`

## 🏗️ Project Architecture

This sandbox follows a clean architecture pattern with the following structure:

```
TaskManagerSandbox/
├── Controllers/          # API Controllers (Tasks, Users)
├── Models/              # Data Models (Task, User, Category)
├── Services/            # Business Logic Layer
├── Data/                # Data Access Layer (DbContext, Seed Data)
├── DTOs/                # Data Transfer Objects
├── Middleware/          # Custom Middleware (Exception Handling, Logging)
├── Configuration/       # AutoMapper Profiles
├── Validators/          # FluentValidation Rules
└── wwwroot/            # Static Files (HTML API Tester)
```

## 📚 Learning Features

This sandbox includes examples of:

### ✅ Already Implemented
- **Basic CRUD Operations** - Create, Read, Update, Delete tasks
- **RESTful API Design** - Proper HTTP methods and status codes
- **Data Validation** - Input validation and error handling
- **Dependency Injection** - Service registration and usage
- **Configuration Management** - App settings and environment variables
- **Logging** - Structured logging with Serilog
- **Error Handling** - Global exception handling middleware

### 🎯 Features You Can Add (Learning Opportunities)
1. **Authentication & Authorization**
   - User registration and login
   - JWT token authentication
   - Role-based access control

2. **Database Integration**
   - Entity Framework Core
   - SQL Server/PostgreSQL integration
   - Database migrations

3. **Advanced Features**
   - File upload/download
   - Email notifications
   - Real-time updates with SignalR
   - Caching with Redis
   - Background jobs with Hangfire

4. **Testing**
   - Unit tests with xUnit
   - Integration tests
   - API testing with Postman/Swagger

5. **Frontend Integration**
   - React/Angular/Vue.js frontend
   - Blazor Server/WebAssembly
   - Static file serving

## 🔧 Development Workflow

1. **Add a new feature**:
   - Create models in `Models/`
   - Add DTOs in `DTOs/`
   - Implement business logic in `Services/`
   - Create controllers in `Controllers/`
   - Update configuration if needed

2. **Testing your changes**:
   ```bash
   dotnet test
   ```

3. **Database migrations** (when adding EF Core):
   ```bash
   dotnet ef migrations add MigrationName
   dotnet ef database update
   ```

## 📖 API Documentation

Once the application is running, visit:
- **Swagger UI**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`

## 🛠️ Tools & Extensions

Recommended VS Code extensions:
- C# Dev Kit
- C# Extensions
- REST Client
- Thunder Client (API testing)

## 📝 Next Steps

1. Install .NET 8.0 SDK
2. Open the project in your preferred IDE
3. Run the application
4. Explore the code structure
5. Start adding features following the examples provided

## 🤝 Contributing

This is a learning sandbox - feel free to experiment, break things, and rebuild them! Each feature addition is a learning opportunity.

## 📚 Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [.NET Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/) 