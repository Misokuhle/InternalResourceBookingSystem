# InternalResourceBookingSystem

An ASP.NET Core MVC application for managing internal resource bookings — allows users to add, view, edit, and manage resource reservations.

## Features

- Create, update, and delete resources
- Book resources with time conflict validation
- View upcoming and today's bookings
- Dashboard with resource and booking summaries
- Basic search and filter for resources and bookings

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **SQL Server Express (SQLEXPRESS)** installed and running
- Visual Studio 2022 / Visual Studio Code 

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/YourUsername/InternalResourceBookingSystem.git
cd InternalResourceBookingSystem

2. Configure the Database Connection
In appsettings.json, make sure your connection string uses SQLEXPRESS:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=InternalResourceBookingSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

3. Restore Dependencies

dotnet restore
4. Apply Migrations and Create the Database

dotnet ef database update
If dotnet ef is not recognized, install the EF tools:

dotnet tool install --global dotnet-ef
5. Run the Application

dotnet run
Then open the browser and go to:

arduino
https://localhost:7248
Make sure the port matches what appears in your terminal.
