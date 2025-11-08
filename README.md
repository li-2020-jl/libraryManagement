# Library Management System

A comprehensive web-based library management system built with ASP.NET Core MVC, featuring book management, customer management, library branch management, and real-time chat functionality.

## Features

### Core Functionality
- **Book Management**: Create, view, and manage books with author and branch associations
- **Author Management**: Manage authors and view their book collections
- **Customer Management**: Register and manage library customers
- **Library Branch Management**: Manage multiple library branches
- **Book Borrowing**: Track which customers have borrowed which books
- **User Authentication**: Secure login system with ASP.NET Core Identity
- **OAuth Integration**: Optional Google and GitHub authentication (configurable)
- **Real-time Chat**: SignalR-powered chat functionality for users
- **REST API**: Complete RESTful API with Swagger documentation
- **Error Handling**: Comprehensive error handling and exception management

### Technical Features
- **Database**: SQLite database with Entity Framework Core
- **Migrations**: Database migrations for schema management
- **Seed Data**: Pre-populated with sample authors, branches, and books
- **API Documentation**: Swagger/OpenAPI documentation (available in Development mode)
- **Responsive Design**: Modern UI with Bootstrap

## Technology Stack

- **Framework**: ASP.NET Core 9.0 (MVC)
- **Database**: SQLite
- **ORM**: Entity Framework Core 9.0.3
- **Authentication**: ASP.NET Core Identity
- **OAuth Providers**: Google, GitHub (optional)
- **Real-time Communication**: SignalR
- **API Documentation**: Swashbuckle (Swagger)
- **Frontend**: Bootstrap, jQuery, Razor Views

## Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- A code editor (Visual Studio, VS Code, or Rider)
- Git (for cloning the repository)

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/li-2020-jl/libraryManagement.git
cd libraryManagement
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Update Database

The database will be automatically created on first run. To apply migrations manually:

```bash
dotnet ef database update
```

### 4. Configure Authentication (Optional)

If you want to enable Google or GitHub OAuth authentication, update `appsettings.json`:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    },
    "GitHub": {
      "ClientId": "YOUR_GITHUB_CLIENT_ID",
      "ClientSecret": "YOUR_GITHUB_CLIENT_SECRET"
    }
  }
}
```

**Note**: The application will run perfectly fine without OAuth credentials. OAuth providers are only configured if valid credentials are provided.

### 5. Run the Application

```bash
dotnet run
```

The application will be available at:
- HTTP: `http://localhost:5291`
- HTTPS: `https://localhost:7048`
- Swagger UI (Development only): `https://localhost:7048/swagger`

## Project Structure

```
libraryManagement/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs      # Authentication (Login, Register, OAuth)
│   ├── BookController.cs         # Book management
│   ├── AuthorController.cs       # Author management
│   ├── CustomerController.cs     # Customer management
│   ├── LibraryBranchController.cs # Branch management
│   └── Api/                      # REST API Controllers
│       ├── AuthorApiController.cs
│       ├── BookApiController.cs
│       ├── CustomerApiController.cs
│       └── LibraryBranchApiController.cs
├── Models/               # Data Models
│   ├── Book.cs
│   ├── Author.cs
│   ├── Customer.cs
│   └── Branch.cs
├── Views/                # Razor Views
│   ├── Account/          # Login, Register views
│   ├── Book/             # Book management views
│   ├── Author/           # Author views
│   ├── Customer/         # Customer management views
│   ├── LibraryBranch/    # Branch management views
│   └── Home/             # Home page, Chat
├── ViewModels/           # View Models for data binding
├── Data/                 # Database Context
│   └── AppDbContext.cs
├── Migrations/           # Entity Framework Migrations
├── wwwroot/              # Static files (CSS, JS, images)
├── Program.cs            # Application entry point
└── appsettings.json      # Configuration file
```

## Database Models

### Book
- `BookId`: Primary key
- `Title`: Book title
- `AuthorId`: Foreign key to Author
- `LibraryBranchId`: Foreign key to LibraryBranch
- `CustomerId`: Foreign key to Customer (nullable, for borrowed books)

### Author
- `AuthorId`: Primary key
- `Name`: Author name
- `Books`: Collection of books by this author

### Customer
- Extends `IdentityUser` from ASP.NET Core Identity
- `Name`: Customer name
- `BorrowedBooks`: Collection of books borrowed by this customer

### LibraryBranch
- `LibraryBranchId`: Primary key
- `BranchName`: Name of the library branch

## API Endpoints

The application provides RESTful API endpoints for all entities. Access the Swagger documentation at `/swagger` in Development mode.

### Book API
- `GET /api/Book` - Get all books
- `GET /api/Book/{id}` - Get book by ID
- `POST /api/Book` - Create a new book
- `PUT /api/Book/{id}` - Update a book
- `DELETE /api/Book/{id}` - Delete a book

### Author API
- `GET /api/Author` - Get all authors
- `GET /api/Author/{id}` - Get author by ID
- `POST /api/Author` - Create a new author
- `PUT /api/Author/{id}` - Update an author
- `DELETE /api/Author/{id}` - Delete an author

### Customer API
- `GET /api/Customer` - Get all customers
- `GET /api/Customer/{id}` - Get customer by ID
- `POST /api/Customer` - Create a new customer
- `PUT /api/Customer/{id}` - Update a customer
- `DELETE /api/Customer/{id}` - Delete a customer

### LibraryBranch API
- `GET /api/LibraryBranch` - Get all branches
- `GET /api/LibraryBranch/{id}` - Get branch by ID
- `POST /api/LibraryBranch` - Create a new branch
- `PUT /api/LibraryBranch/{id}` - Update a branch
- `DELETE /api/LibraryBranch/{id}` - Delete a branch

## Seed Data

The database is pre-populated with the following seed data:

### Authors
- George Orwell
- J.K. Rowling
- Jane Austen

### Library Branches
- Main Library
- East Side Branch

### Books
- "1984" by George Orwell (Main Library)
- "Harry Potter and the Philosopher's Stone" by J.K. Rowling (Main Library)
- "Pride and Prejudice" by Jane Austen (East Side Branch)

## Authentication

### Registration
Users can register with:
- Username
- Email
- Phone Number
- Password (minimum 3 characters, requires at least one digit)

### Login
Users can log in using:
- Username and password
- Google OAuth (if configured)
- GitHub OAuth (if configured)

### Password Requirements
- Minimum length: 3 characters
- Requires at least one digit
- No requirement for uppercase, lowercase, or special characters

## Real-time Chat

The application includes a SignalR-powered chat feature accessible at `/Home/Chat`. Users can send and receive messages in real-time. The chat displays the user's name (or email if name is not available).

## Development

### Running in Development Mode

```bash
dotnet run --environment Development
```

This enables:
- Swagger UI at `/swagger`
- Detailed error pages
- Development-specific logging

### Creating Migrations

```bash
dotnet ef migrations add MigrationName
```

### Applying Migrations

```bash
dotnet ef database update
```

## Security Notes

- **OAuth Credentials**: OAuth client IDs and secrets are stored in `appsettings.json` with placeholder values. Replace them with your actual credentials for OAuth to work.
- **Database**: The SQLite database file (`library.db`) is included in `.gitignore` and will be created automatically on first run.
- **Secrets**: Never commit sensitive information like OAuth secrets to version control. The repository includes placeholder values for security.

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is part of an academic course (CSCI6809) and is provided as-is for educational purposes.

## Author

**Jialu Li**
- GitHub: [@li-2020-jl](https://github.com/li-2020-jl)
- Email: annyjialuli01@gmail.com

## Acknowledgments

- Built with ASP.NET Core
- UI components from Bootstrap
- Icons and styling from modern web standards

## Support

For issues, questions, or contributions, please open an issue on the GitHub repository.

---

**Note**: This is an academic project for CSCI6809. The application is designed for educational purposes and demonstrates modern web development practices with ASP.NET Core.

