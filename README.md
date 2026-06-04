# PreciousStones - Employee & Department Management System

## Project Overview

**PreciousStones** (codenamed **Diamond**) is a modern **ASP.NET Core MVC** web application designed for managing organizational departments and employees. The system provides a complete CRUD (Create, Read, Update, Delete) interface for both departments and employees with comprehensive data validation, business rule enforcement, and a user-friendly Bootstrap-based UI.

## Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET Framework** | 9.0 | Modern web framework with latest C# features |
| **ASP.NET Core MVC** | 9.0 | Web application framework for building scalable applications |
| **Entity Framework Core** | 9.0.9 | ORM (Object-Relational Mapping) for database operations |
| **SQL Server** | Latest | Relational database management system |
| **Bootstrap** | 5.x | Responsive UI framework for frontend styling |
| **Bootstrap Icons** | Latest | Icon library for UI enhancement |
| **jQuery** | Latest | JavaScript library for DOM manipulation |
| **jQuery Validation** | Latest | Client-side form validation |

## Project Architecture

### Folder Structure

```
Diamond/
├── Controllers/              # MVC Controllers handling business logic
│   ├── HomeController.cs     # Home and error handling
│   ├── EmployeesController.cs # Employee CRUD operations
│   └── DepartmentsController.cs # Department CRUD operations
├── Models/                   # Data models with validation
│   ├── Employee.cs          # Employee entity with 12+ properties
│   ├── Department.cs        # Department entity
│   └── ErrorViewModel.cs    # Error handling model
├── Views/                   # Razor templates for rendering UI
│   ├── Employees/           # Employee views (CRUD)
│   ├── Departments/         # Department views (CRUD)
│   ├── Home/               # Homepage and privacy views
│   └── Shared/             # Shared layouts and components
├── Data/                    # Database context
│   └── ApplicationDbContext.cs # EF Core DbContext configuration
├── Migrations/              # Database schema versioning
│   ├── 20250927152805_InitialCreate.cs
│   ├── 20250927190350_AddingDataValidation.cs
│   ├── 20250927194603_AddingDepartmentsTable.cs
│   └── 20250929150143_AddingDepartmentsEmployeesRelation.cs
├── wwwroot/                 # Static assets (CSS, JS, images)
│   ├── css/
│   ├── js/
│   └── lib/                 # Client libraries
├── Properties/              # Launch settings and configurations
├── Program.cs              # Application startup configuration
├── appsettings.json        # Configuration settings
└── Diamond.csproj          # Project file with dependencies
```

## Core Features

### 1. Department Management

#### Data Model Properties
- **Id** (int): Primary key, auto-generated
- **Name** (string): Department name
  - Required field
  - 2-20 characters validation
- **Description** (string): Department details
- **Employees**: Navigation property (One-to-Many relationship)

#### Operations (CRUD)
- **Create**: Add new departments with name and description
- **Read**: View all departments or individual department details
- **Update**: Modify existing department information
- **Delete**: Remove departments from the system

#### Technical Implementation
- Uses **async/await** patterns for database operations
- Implements **ValidateAntiForgeryToken** for CSRF protection
- Uses **LINQ to Entities** for data queries
- Includes error handling with **DbUpdateConcurrencyException**

---

### 2. Employee Management

#### Data Model Properties with Validation Rules

| Property | Type | Validation Rules | Business Logic |
|----------|------|------------------|-----------------|
| **Id** | int | Primary Key | Auto-generated identifier |
| **FullName** | string | Required, 8-50 chars | Employee identification |
| **NationalId** | string | Exactly 14 chars | Unique identifier (Egypt format) |
| **PhoneNo** | string | Pattern: `^01\d{9}$` | Egyptian mobile format validation |
| **Position** | string | Min 5 characters | Job title/role |
| **Salary** | decimal | Range: 6,000 - 60,000 EGP | Monthly compensation with business boundaries |
| **DateOfBirth** | DateOnly | Age >= 18 years | Enforced on POST - employees must be adults |
| **HiringDateAndTime** | DateTime | Timestamp | Exact hiring moment |
| **AttendanceTime** | TimeOnly | Daily start time | Working hours tracking |
| **LeaveTime** | TimeOnly | Daily end time | Working hours tracking |
| **Email** | string | Email format | Communication channel |
| **Password** | string | Encrypted storage | Authentication credential |
| **IsActive** | bool | True/False | Employee status flag |
| **DepartmentId** | int | Foreign Key | Links to Department (required) |
| **Department** | Navigation | DeleteBehavior.NoAction | One-to-Many relationship |

#### Operations (CRUD)
- **Create**: Add new employees with comprehensive validation
  - Age verification (must be 18+ years old)
  - All required fields must be provided
- **Read**: 
  - List all employees with search functionality
  - View individual employee details with department information
  - Search by name or position
- **Update**: Modify employee information with re-validation
- **Delete**: Remove employees from the system

#### Special Features
- **Search Functionality**: Search employees by:
  - Full Name (substring match)
  - Position (substring match)
  - Real-time search with ViewBag state preservation
- **Age Validation**: Custom business logic ensures minimum 18 years old at hiring
- **Department Association**: Every employee must belong to a department

#### Technical Implementation
- **Custom Model Validation**: Age calculation and verification in controller
- **Include() for Relationships**: Eagerly loads Department data when needed
- **ModelState Validation**: Server-side form validation with custom error messages
- **Search Query**: Uses LINQ `.Where()` with `.Contains()` for filtering

---

### 3. Database Architecture

#### Entity Relationship Diagram
```
Department (1) ──────────── (Many) Employee
   Id                           Id
   Name                         FullName
   Description                  NationalId
   ↓                            PhoneNo
   [Employees]                  Position
                                Salary
                                DateOfBirth
                                HiringDateAndTime
                                AttendanceTime
                                LeaveTime
                                Email
                                Password
                                IsActive
                                DepartmentId (FK) → Department.Id
                                [Department]
```

#### Migration History

**Migration 1: InitialCreate** (2025-09-27 15:28:05)
- Created initial database schema
- Established base Employee and Department tables

**Migration 2: AddingDataValidation** (2025-09-27 19:03:50)
- Enhanced models with validation attributes
- Added business rule constraints to properties

**Migration 3: AddingDepartmentsTable** (2025-09-27 19:46:03)
- Created Departments table as separate entity
- Established infrastructure for departments

**Migration 4: AddingDepartmentsEmployeesRelation** (2025-09-29 15:01:43)
- Added `DepartmentId` foreign key to Employees table
- Created one-to-many relationship
- Added database index on `IX_Employees_DepartmentId`
- Set `DeleteBehavior.NoAction` for referential integrity

#### Connection String
```
Server=AMR-HASSAN;
Database=PreciousStones;
Trusted_Connection=True;
TrustServerCertificate=True;
```

---

## Controllers & Routing

### HomeController
Handles general application navigation:
- **Index**: Homepage/landing page
- **Privacy**: Privacy policy page
- **Error**: Centralized error handling with `ErrorViewModel`
- **Response Caching**: Error page disables caching

### EmployeesController
Manages employee operations with mixed action patterns:

| HTTP Verb | Action | Purpose |
|-----------|--------|---------|
| **GET** | `Index()` | List all employees |
| **GET** | `GetIndexView(search)` | Search employees (supports filtering) |
| **GET** | `Create()` | Display employee creation form |
| **POST** | `AddNew(Employee)` | Save new employee with validation |
| **GET** | `Details(id)` | Show employee details with department info |
| **GET** | `GetDetailsView(id)` | Alternative details view method |
| **GET** | `Edit(id)` | Display employee edit form |
| **POST** | `EditCurrent(Employee)` | Update employee with validation |
| **GET** | `Delete(id)` | Show delete confirmation |
| **GET** | `GetDeleteView(id)` | Alternative delete view method |
| **POST** | `DeleteCurrent(id)` | Permanently delete employee |

#### Key Implementation Details
- **Duplicate action methods**: Some operations have both standard and custom-named routes (e.g., `GetIndexView` vs `Index`)
- **DbContext**: Instantiated directly in controller (could be improved with dependency injection)
- **Include() relationships**: Used to load related Department data when needed

### DepartmentsController
Manages department operations with async patterns:

| HTTP Verb | Action | Purpose |
|-----------|--------|---------|
| **GET** | `Index()` | List all departments (async) |
| **GET** | `Create()` | Display department creation form |
| **POST** | `Create(Department)` | Save new department with anti-forgery token |
| **GET** | `Details(id)` | Show department details (async) |
| **GET** | `Edit(id)` | Display department edit form (async) |
| **POST** | `Edit(id, Department)` | Update department with validation |
| **GET** | `Delete(id)` | Show delete confirmation (async) |
| **POST** | `DeleteConfirmed(id)` | Permanently delete department (async) |

#### Key Implementation Details
- **Async/Await**: Uses `Task<IActionResult>` for async database operations
- **ValidateAntiForgeryToken**: CSRF protection on POST operations
- **Bind Attribute**: Whitelist binding to prevent over-posting attacks
- **Concurrency Handling**: Catches `DbUpdateConcurrencyException`

---

## Data Access Layer (DAL)

### ApplicationDbContext
The Entity Framework Core context managing database operations:

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=AMR-HASSAN;Database=PreciousStones;..." 
        );
    }
}
```

#### Features
- **DbSet Properties**: Expose both entities for querying and manipulation
- **SQL Server Provider**: Configured for SQL Server database
- **Connection String**: Embedded in `OnConfiguring()` method
- **Trusted Connection**: Uses Windows authentication (no credentials required)
- **TrustServerCertificate**: Accepts SSL certificates without validation

#### Considerations
- Connection string is hardcoded (security concern - should use configuration)
- No connection pooling explicitly configured
- No change tracking optimization

---

## Views & User Interface

### View Organization

#### Employees Views
- **Index.cshtml**: 
  - Lists all employees in a Bootstrap table
  - Search form for name/position filtering
  - Create button to add new employees
  - Action buttons (View Details, Edit, Delete) for each row
  - Uses Bootstrap Icons for visual indicators

- **Create.cshtml**: 
  - Form for adding new employees
  - Validation messages displayed inline
  - Department dropdown selection
  - Date/Time pickers for hiring information

- **Edit.cshtml**: 
  - Form for modifying employee data
  - Pre-populated with existing values
  - Department selection dropdown (loaded from ViewBag)
  - Same validation rules as Create

- **Details.cshtml**: 
  - Read-only display of employee information
  - Shows associated department details
  - Navigation links to edit or delete

- **Delete.cshtml**: 
  - Delete confirmation page
  - Displays employee information before deletion
  - Confirmation button for final deletion

#### Departments Views
- **Index.cshtml**: Lists all departments in table format
- **Create.cshtml**: Form for adding new departments
- **Edit.cshtml**: Form for modifying department info
- **Details.cshtml**: Department details view
- **Delete.cshtml**: Delete confirmation for departments

#### Shared Views
- **_Layout.cshtml**: Master layout template
  - Navigation bar with links to Home, Employees, Departments, Privacy
  - Bootstrap responsive styling
  - Includes Bootstrap CSS and Icons
  - Footer with default message
  - Support for responsive mobile design

- **_ValidationScriptsPartial.cshtml**: Client-side validation scripts
- **Error.cshtml**: Error page display with RequestId

### Frontend Technologies
- **Bootstrap 5.x**: Responsive layout and component styling
- **Bootstrap Icons**: Icon library for UI elements
- **jQuery Validation**: Client-side form validation
- **jQuery Unobtrusive Validation**: ASP.NET Core integration for validation
- **Razor Templating**: ASP.NET Core view engine for dynamic HTML generation

---

## Key Concepts & Design Patterns

### 1. Model-View-Controller (MVC) Architecture
- **Models**: Entity classes with validation attributes
- **Views**: Razor templates for UI rendering
- **Controllers**: Handle user requests and coordinate model/view interaction

### 2. Entity Framework Core (Code-First ORM)
- **DbContext**: Central abstraction for database access
- **DbSet<T>**: Collections representing database tables
- **Navigation Properties**: Object relationships (Department/Employee)
- **Migrations**: Version control for database schema changes
- **LINQ to Entities**: Strongly-typed queries with deferred execution

### 3. Data Validation
- **Model Validation Attributes**: 
  - `[Required]`: Mandatory fields
  - `[MinLength] / [MaxLength]`: String length constraints
  - `[Range]`: Numeric boundaries
  - `[RegularExpression]`: Pattern matching
  - `[DataType]`: Semantic type information
- **Custom Validation**: Server-side age verification logic
- **ModelState**: Tracks validation errors on the server

### 4. One-to-Many Relationship
- **Foreign Key**: `DepartmentId` on Employee table
- **Navigation Properties**: 
  - `Employee.Department` (reference navigation)
  - `Department.Employees` (collection navigation)
- **DeleteBehavior.NoAction**: Prevents cascade deletion
- **Eager Loading**: `.Include(e => e.Department)` to load related data

### 5. Asynchronous Programming (DepartmentsController)
- **async/await patterns** for non-blocking database calls
- **Task<T>**: Return type for async operations
- Improves scalability for concurrent requests

### 6. Security Features
- **ValidateAntiForgeryToken**: CSRF (Cross-Site Request Forgery) protection
- **Bind Attribute**: Prevents over-posting attacks
- **Windows Authentication**: Trusted database connections
- **Input Validation**: Prevents malicious data entry

### 7. Search & Filtering
- **LINQ Query**: `.Where()` with string `.Contains()`
- **ViewBag State**: Preserves search context across requests
- **Substring Matching**: Enables flexible search on multiple fields

---

## Database Schema

### Departments Table
```sql
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(20) NOT NULL,
    Description NVARCHAR(MAX)
);
```

### Employees Table
```sql
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(50) NOT NULL,
    NationalId NVARCHAR(14),
    PhoneNo NVARCHAR(11),
    Position NVARCHAR(MAX),
    Salary DECIMAL(18,2),
    DateOfBirth DATE,
    HiringDateAndTime DATETIME2,
    AttendanceTime TIME,
    LeaveTime TIME,
    Email NVARCHAR(MAX),
    Password NVARCHAR(MAX),
    IsActive BIT,
    DepartmentId INT NOT NULL,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
```

---

## Configuration Files

### appsettings.json
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=AMR-HASSAN;Database=PreciousStones;..."
    }
}
```

### Program.cs Configuration
```csharp
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware Pipeline
app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
```

---

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- SQL Server (local or remote)
- Visual Studio 2022 or Visual Studio Code

### Installation Steps

1. **Clone/Open Project**
   ```bash
   cd PreciousStones
   ```

2. **Update Database Connection**
   - Edit `appsettings.json` or `ApplicationDbContext.cs`
   - Update connection string for your SQL Server instance

3. **Apply Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

5. **Run Application**
   ```bash
   dotnet run
   ```

6. **Access Application**
   - Navigate to `https://localhost:7033` (or configured port)
   - Default route: Home page

---

## Future Improvements & Recommendations

### 1. Architecture Enhancements
- [ ] Implement **Dependency Injection** for DbContext
- [ ] Create **Repository Pattern** for data access abstraction
- [ ] Implement **Unit of Work Pattern** for transaction management
- [ ] Add **Service Layer** for business logic separation

### 2. Security Improvements
- [ ] Move connection strings to **User Secrets** or **Key Vault**
- [ ] Implement **Password Hashing** (bcrypt, PBKDF2)
- [ ] Add **Authorization/Authentication** (Identity framework)
- [ ] Implement **Logging** for audit trails
- [ ] Add **SQL Injection** prevention validation

### 3. Performance Optimization
- [ ] Implement **Connection Pooling**
- [ ] Add **Caching Layer** (Redis, in-memory)
- [ ] Use **Compiled Queries** for frequently used queries
- [ ] Implement **Pagination** for large datasets
- [ ] Add **Lazy Loading** or **Explicit Loading** strategies

### 4. Data & Validation
- [ ] Add **Unique Constraints** (Email, NationalId)
- [ ] Implement **Data Encryption** for sensitive fields
- [ ] Add **Concurrency Tokens** for optimistic locking
- [ ] Implement **Soft Deletes** instead of hard deletes
- [ ] Add **Audit Fields** (CreatedAt, UpdatedAt, CreatedBy)

### 5. User Experience
- [ ] Add **Pagination** to employee/department lists
- [ ] Implement **Advanced Filtering** options
- [ ] Add **Export to Excel/CSV** functionality
- [ ] Implement **Dashboard** with statistics
- [ ] Add **Batch Operations** (bulk delete, update)
- [ ] Implement **Real-time Search** with autocomplete

### 6. Testing & Documentation
- [ ] Add **Unit Tests** for business logic
- [ ] Add **Integration Tests** for controllers
- [ ] Implement **API Documentation** (Swagger/OpenAPI)
- [ ] Add **Code Comments** for complex logic
- [ ] Create **User Manual** and **Admin Guide**

### 7. Infrastructure
- [ ] Implement **Logging Framework** (Serilog, NLog)
- [ ] Add **Error Handling** middleware
- [ ] Implement **Health Checks**
- [ ] Add **Rate Limiting**
- [ ] Containerize with **Docker**

---

## Project Statistics

| Metric | Value |
|--------|-------|
| **Framework** | .NET 9.0 |
| **Project Type** | ASP.NET Core MVC |
| **Database** | SQL Server |
| **Controllers** | 3 |
| **Models** | 3 |
| **Views** | 13+ |
| **Migrations** | 4 |
| **NuGet Packages** | 4 core dependencies |
| **Lines of Code** | ~500+ |

---

## Summary

**PreciousStones** is a well-structured ASP.NET Core MVC application demonstrating:
- ✅ Proper entity relationships and foreign keys
- ✅ Comprehensive data validation and business rules
- ✅ CRUD operations with both sync and async patterns
- ✅ Search and filtering capabilities
- ✅ Clean separation of concerns (MVC pattern)
- ✅ Bootstrap-based responsive UI
- ✅ Entity Framework Core best practices
- ✅ Security considerations (CSRF protection)

The application provides a solid foundation for organizational employee and department management and can be extended with additional features, security enhancements, and performance optimizations as outlined in the Future Improvements section.

---

## License

This project is part of ITI .NET training curriculum.

---

## Author & Date

- **Created**: September 27, 2025 - September 29, 2025
- **Last Updated**: June 4, 2026
- **Status**: Learning Project ✓
