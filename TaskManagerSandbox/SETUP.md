# 🚀 Task Manager Sandbox - Setup Guide

This guide will help you get the Task Manager Sandbox application up and running on your local machine.

## 📋 Prerequisites

Before you begin, make sure you have the following installed:

### 1. .NET 8.0 SDK
- **Download**: [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Verify Installation**: Open a terminal/command prompt and run:
  ```bash
  dotnet --version
  ```
  You should see something like `8.0.xxx`

### 2. IDE (Choose one)
- **Visual Studio Code** (Recommended for beginners)
  - Download: [VS Code](https://code.visualstudio.com/)
  - Install the **C# Dev Kit** extension
- **Visual Studio 2022** (Community edition is free)
  - Download: [Visual Studio](https://visualstudio.microsoft.com/)

### 3. Database (Optional for development)
- **SQL Server LocalDB** (comes with Visual Studio)
- **SQL Server Express** (if you want a full database)
- **PostgreSQL** (alternative option)

## 🛠️ Installation Steps

### Step 1: Clone/Download the Project
1. Navigate to the `TaskManagerSandbox` directory
2. Open a terminal/command prompt in this directory

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build the Project
```bash
dotnet build
```

### Step 4: Run the Application
```bash
dotnet run
```

### Step 5: Access the Application
Once the application starts, you'll see output similar to:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
      Now listening on: http://localhost:5000
```

Open your web browser and navigate to:
- **API Tester**: `https://localhost:7001` or `http://localhost:5000`
- **Swagger Documentation**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`

## 🧪 Testing the Application

### Using the Web Interface
1. Open `https://localhost:7001` in your browser
2. You'll see a user-friendly interface to test all API endpoints
3. Try creating, reading, updating, and deleting tasks and users

### Using Swagger
1. Open `https://localhost:7001/swagger`
2. Explore the interactive API documentation
3. Test endpoints directly from the browser

### Using curl/Postman
```bash
# Get all tasks
curl -X GET "https://localhost:7001/api/tasks"

# Create a new task
curl -X POST "https://localhost:7001/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Task",
    "description": "This is a test task",
    "priority": "Medium",
    "assignedTo": "admin"
  }'

# Get task statistics
curl -X GET "https://localhost:7001/api/tasks/statistics"
```

## 📊 Sample Data

The application comes with pre-loaded sample data:

### Users
- **Admin User**: `admin` (Role: Admin)
- **Regular User**: `user` (Role: User)

### Categories
- **Work**: Blue (#007bff)
- **Personal**: Green (#28a745)
- **Urgent**: Red (#dc3545)

### Sample Tasks
- Complete API Documentation (High Priority, In Progress)
- Review Code Changes (Medium Priority, Pending)
- Plan Team Meeting (Low Priority, Completed)
- Buy Groceries (Medium Priority, Pending)
- Fix Critical Bug (Critical Priority, In Progress)
- Exercise (Low Priority, Pending)

## 🔧 Configuration

### Environment Variables
The application uses configuration from `appsettings.json`. Key settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "AppSettings": {
    "MaxTasksPerUser": 100,
    "DefaultPageSize": 10
  }
}
```

### Development vs Production
- **Development**: Uses in-memory database (no setup required)
- **Production**: Uses SQL Server/PostgreSQL (requires database setup)

## 🐛 Troubleshooting

### Common Issues

#### 1. "dotnet command not found"
- **Solution**: Install .NET 8.0 SDK
- **Verify**: Run `dotnet --version`

#### 2. Port already in use
- **Solution**: Change ports in `Properties/launchSettings.json`
- **Alternative**: Kill the process using the port

#### 3. SSL Certificate Issues
- **Solution**: Run `dotnet dev-certs https --trust`
- **Alternative**: Use HTTP instead of HTTPS

#### 4. Database Connection Issues
- **Development**: The app uses in-memory database by default
- **Production**: Ensure SQL Server is running and connection string is correct

### Getting Help
1. Check the console output for error messages
2. Verify all prerequisites are installed
3. Ensure you're in the correct directory
4. Try running `dotnet clean` then `dotnet build`

## 🎯 Next Steps

Once the application is running, you can:

1. **Explore the Code**: Understand the architecture and patterns used
2. **Add Features**: Follow the examples to add new functionality
3. **Modify Data Models**: Add new properties or entities
4. **Add Authentication**: Implement JWT authentication
5. **Add Database**: Switch from in-memory to SQL Server/PostgreSQL
6. **Add Tests**: Create unit and integration tests
7. **Deploy**: Deploy to Azure, AWS, or other cloud platforms

## 📚 Learning Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [REST API Design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

## 🤝 Contributing

This is a learning sandbox - feel free to:
- Experiment with the code
- Add new features
- Break things and fix them
- Share your improvements

Happy coding! 🎉 