# Student Management System

A comprehensive full-stack application for managing student information with a modern .NET Core Web API backend and React frontend.

## Features

✅ **Backend (.NET Core Web API)**
- RESTful API with CRUD operations for Student management
- JWT Authentication & Authorization
- Global Exception Handling Middleware
- Structured Logging with Serilog
- Swagger/OpenAPI Documentation
- Clean Layered Architecture:
  - Controller Layer (API)
  - Service Layer (Business Logic)
  - Repository Layer (Data Access)
  - Entity Layer (Models & DTOs)
- SQL Server Database with Entity Framework Core
- Input Validation & Error Handling

✅ **Frontend (React)**
- Modern responsive UI
- Student listing, search, and pagination
- Add, edit, and delete student operations
- Authentication with JWT tokens
- Form validation
- Beautiful UI with CSS/Bootstrap

## Tech Stack

### Backend
- **.NET 8.0** - Web API Framework
- **Entity Framework Core 8.0.8** - ORM
- **SQL Server** - Database
- **JWT** - Authentication
- **Serilog** - Logging
- **Swagger/OpenAPI** - API Documentation

### Frontend
- **React 18** - UI Library
- **Axios** - HTTP Client
- **React Router** - Navigation
- **Bootstrap/CSS** - Styling

## Project Structure

```
StudentManagement/
├── StudentManagement.API/          # ASP.NET Core API
│   ├── Controllers/                # API Endpoints
│   ├── Authentication/             # JWT Token Generation
│   ├── Middleware/                 # Exception Handling
│   ├── Properties/                 # Configuration
│   ├── appsettings.json           # App Configuration
│   └── Program.cs                 # Application Startup
├── StudentManagement.Core/         # Core Models & DTOs
│   ├── Entities/                  # Database Models
│   ├── DTOs/                      # Data Transfer Objects
│   └── Models/                    # API Response Models
├── StudentManagement.Data/         # Data Access Layer
│   ├── Context/                   # DbContext
│   └── Repositories/              # Repository Pattern
├── StudentManagement.Service/      # Business Logic Layer
│   ├── Interfaces/                # Service Interfaces
│   └── Services/                  # Service Implementations
├── StudentManagement.Tests/        # Unit Tests
├── StudentManagement.UI/           # React Frontend
│   ├── src/
│   │   ├── components/            # React Components
│   │   ├── pages/                # Pages
│   │   ├── services/             # API Services
│   │   └── App.js                # Root Component
│   └── package.json              # Dependencies
├── README.md                       # This File
├── StudentManagement.sln          # Solution File
└── .gitignore                     # Git Ignore Rules
```

## Prerequisites

- **.NET 8.0 SDK** or higher
- **Node.js 16+** and **npm/yarn**
- **SQL Server** (LocalDB or Express)
- **Git**
- **VS Code** or **Visual Studio**

## Backend Setup

### 1. Clone Repository
```bash
git clone https://github.com/amey2612/StudentManagementSystem.git
cd StudentManagement
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Configure Database Connection
Edit `StudentManagement.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentManagementDb;Trusted_Connection=true;"
  }
}
```

### 4. Update & Migrate Database
```bash
cd StudentManagement.Data
dotnet ef database update
cd ..
```

### 5. Run API Server
```bash
cd StudentManagement.API
dotnet run
```

The API will be available at: `https://localhost:5001`

### 6. Access Swagger Documentation
Navigate to: `https://localhost:5001/`

## Frontend Setup

### 1. Create React App
```bash
cd StudentManagement.UI
npm install
```

### 2. Configure API URL
Edit `src/services/api.js`:
```javascript
const API_URL = "https://localhost:5001/api";
```

### 3. Run React App
```bash
npm start
```

The frontend will be available at: `http://localhost:3000`

## API Endpoints

### Authentication
```
POST /api/auth/login
Headers: Content-Type: application/json
Body: {
  "username": "demo",
  "password": "password"
}
Response: {
  "success": true,
  "data": {
    "access_token": "eyJhbGc...",
    "token_type": "Bearer",
    "expires_in": 3600
  }
}
```

### Students (Requires JWT Token)
```
GET /api/students                    # Get all students
GET /api/students/{id}               # Get student by ID
POST /api/students                   # Create student
PUT /api/students/{id}               # Update student
DELETE /api/students/{id}            # Delete student
```

### Student Request/Response

**Create/Update Request:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20,
  "course": "Computer Science"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20,
    "course": "Computer Science",
    "createdDate": "2026-04-06T10:30:00Z"
  }
}
```

## Git Workflow

### Create Feature Branch
```bash
git checkout -b feature/student-features
# Make changes
git add .
git commit -m "Add student features"
git push origin feature/student-features
```

### Merge to Develop
```bash
git checkout develop
git merge feature/student-features
git commit -m "Merge feature into develop"
```

### Merge to Main (Release)
```bash
git checkout main
git merge develop
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin main --tags
```

## Testing

### Run Unit Tests
```bash
cd StudentManagement.Tests
dotnet test
```

### Test with Swagger
1. Navigate to `https://localhost:5001`
2. Click "Authorize" button
3. Login to get JWT token
4. Test endpoints using Swagger UI

## Docker Support

### Build Docker Image
```bash
docker build -t student-management-api:latest -f Dockerfile .
```

### Run in Container
```bash
docker run -p 5001:5001 student-management-api:latest
```

## Security Features

- **JWT Authentication** - Secure token-based authentication
- **CORS Policy** - Configured for frontend domain
- **Password Hashing** - (To be implemented)
- **SQL Injection Prevention** - Using EF Core parameterized queries
- **XSS Prevention** - React built-in XSS protection
- **Input Validation** - Server-side validation on all endpoints

## Logging

Logs are configured with Serilog and saved to:
- **Console** - Real-time output
- **File** - Daily rotating files in `logs/` directory

## Configuration

### Environment Variables
Create `.env` file in project root:
```
JWT_SECRET_KEY=your-secret-key-here
DATABASE_URL=connection-string
```

### Application Settings
All configuration in `appsettings.json`:
- JWT Settings
- Database Connection
- Serilog Configuration
- CORS Policy

## Deployment

### Azure Deployment
```bash
az login
dotnet publish -c Release
```

### Docker Deployment
```bash
docker push student-management-api:latest
docker pull student-management-api:latest
docker run -d -p 5001:5001 student-management-api:latest
```

## Troubleshooting

### Database Connection Issues
```bash
# Check SQL Server is running
# Verify connection string in appsettings.json
# Run migrations again
dotnet ef database update
```

### JWT Token Errors
- Ensure token is included in Authorization header
- Format: `Authorization: Bearer {token}`
- Check token expiration
- Verify secret key matches in configuration

### CORS Errors
- Check CORS policy in Program.cs
- Verify frontend URL is whitelisted
- Ensure credentials mode in fetch/axios calls

## Performance Optimization

- Database indexing on Email field
- Async/await for non-blocking operations
- Paging support in list endpoints
- Caching strategies (to be implemented)

## Contribution Guidelines

1. Create feature branch from `develop`
2. Write clean, documented code
3. Add unit tests for new features
4. Ensure all tests pass: `dotnet test`
5. Submit pull request for review
6. Merge to develop after approval
7. Create release from develop to main

## Developers

- **Lead Developer**: Zest India IT Team
- **Framework**: .NET Core 8.0 & React
- **Database**: SQL Server
- **Version**: 1.0.0

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
- GitHub Issues: [GitHub Repository]
- Email: support@zestindia.com
- Documentation: Check README.md

## Changelog

### Version 1.0.0 (2026-04-06)
- Initial project setup
- Student CRUD operations
- JWT Authentication
- Swagger Documentation
- Global Exception Handling
- Serilog Logging
- Entity Framework Core Integration
- React Frontend
- Repository & Service Pattern

---

**Build with ❤️ by Zest India IT**
