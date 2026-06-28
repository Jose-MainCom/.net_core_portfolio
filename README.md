# Fredric Portfolio

A modern portfolio website and content management system (CMS) built with **ASP.NET Core MVC**, **.NET 10 LTS**, **Entity Framework Core**, **SQL Server**, and **ASP.NET Core Identity**.

The application combines a responsive public portfolio with a complete administration dashboard, allowing all content to be managed without modifying the source code.

---

# Features

## Public Website

* Responsive design
* Home page
* About page
* Experience / Timeline
* Skills page
* Portfolio projects
* Project detail pages
* Project image gallery
* Lightbox image viewer
* Project links with icons
* CV download
* Contact page
* Custom 404 and 500 error pages

---

## Administration Dashboard

The entire website can be managed through an administration panel.

### Dashboard

* Portfolio statistics
* Quick navigation
* Project overview
* Skills overview
* Timeline overview
* Documents overview

### Projects

* Create projects
* Edit projects
* Delete projects
* Featured projects
* Categories
* Technologies used
* Multiple project links
* Link types
* Multiple project images
* Cover image selection
* Image ordering
* Gallery management

### Skills

* Create
* Edit
* Delete

### Timeline

* Create
* Edit
* Delete

### Portfolio Profile

* Edit personal information
* Update presentation text

### Documents

* Upload CV
* Download latest CV

---

# Account Management

Built with ASP.NET Core Identity.

Supports:

* Login
* Logout
* Remember Me
* Change email
* Change password
* Password reset
* One password reset request per day

Password reset currently generates a secure reset URL directly in the application console, making it ideal for self-hosted deployments without requiring an SMTP or external email provider.

---

# Technologies

* ASP.NET Core MVC (.NET 10 LTS)
* C# 13
* Entity Framework Core 10
* SQL Server
* ASP.NET Core Identity
* Razor Views
* Dependency Injection
* Repository Pattern
* Clean Architecture
* HTML5
* CSS3
* JavaScript

---

# Architecture

```text
Solution
│
├── Domain
│   ├── Entities
│   └── Enums
│
├── Application
│   ├── Interfaces
│   └── Services
│
├── Infrastructure
│   ├── Data
│   ├── Identity
│   └── Repositories
│
└── Presentation
    ├── Controllers
    ├── ViewModels
    ├── Views
    ├── wwwroot
    └── Program.cs
```

---

# Project Structure

## Domain

Contains:

* Entities
* Enums
* Domain models

---

## Application

Contains:

* Repository interfaces
* Business abstractions

---

## Infrastructure

Contains:

* Entity Framework Core DbContext
* Identity implementation
* Repository implementations
* Database seeding

---

## Presentation

Contains:

* MVC Controllers
* Razor Views
* ViewModels
* CSS
* JavaScript
* Static assets

---

# How to Run

## Requirements

* Visual Studio 2022 (17.14 or later)
* .NET 10 SDK
* SQL Server LocalDB or SQL Server

---

## Clone the Repository

```bash
git clone https://github.com/<your-github-username>/<repository-name>.git
```

Open the solution in Visual Studio.

---

## Configure the Database

Open:

```text
Presentation/appsettings.json
```

Update the connection string if necessary.

Example:

```json
"ConnectionStrings": {
  "PortfolioDatabase": "Server=(localdb)\\MSSQLLocalDB;Database=FredricPortfolio;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Create the Database

Open **Package Manager Console**

**Default Project**

```text
Infrastructure
```

Run:

```powershell
Update-Database -StartupProject Presentation
```

If the project does not already contain migrations, create one first:

```powershell
Add-Migration InitialCreate -StartupProject Presentation
Update-Database -StartupProject Presentation
```

---

## Run the Application

Press:

```text
F5
```

or

```text
Ctrl + F5
```

The database is automatically seeded during startup.

---

# Default Administrator

The initial administrator account is created automatically during database seeding.

Administrator credentials can be changed in:

```text
Infrastructure/Data/PortfolioDbSeeder.cs
```

For production deployments you should change the default credentials before publishing.

---

# Project Images

Uploaded project images are stored in:

```text
Presentation/wwwroot/images/projects
```

They are managed entirely from the administration dashboard.

---

# Security

The application uses:

* ASP.NET Core Identity
* Role-based authorization
* Password hashing
* Anti-forgery protection
* Entity Framework Core
* Repository Pattern

Administration pages require authentication.

---

# Future Improvements

Potential future additions:

* SMTP email service
* Rich text editor
* Drag & Drop image sorting
* Image optimization
* Audit logging
* Dashboard analytics
* SEO improvements
* Caching

---

# Screenshots

Add screenshots here once the portfolio contains real projects.

Suggested screenshots:

* Home page
* Projects page
* Project details
* Administration dashboard
* Project management
* Image gallery

---

# Learning Goals

This project demonstrates knowledge of:

* ASP.NET Core MVC
* Clean Architecture
* Entity Framework Core
* SQL Server
* Repository Pattern
* Dependency Injection
* ASP.NET Core Identity
* CRUD operations
* Authentication & Authorization
* Responsive Web Design
* File Uploads
* Image Gallery Management
* Razor Views
* Modern C#
