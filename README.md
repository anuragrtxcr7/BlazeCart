# 🛒 BlazeCart - Blazor Server App with Dapper and Azure SQL

**BlazeCart** is a modern e-commerce web application built with **Blazor Server (.NET 9)** and **Dapper** for data access. It supports user authentication, product browsing, shopping cart functionality, and order placement. The app is deployed on **Azure App Service** with **Azure SQL Database** as the backend.

---

## 🚀 Tech Stack

- **Frontend:** Blazor Server (.NET 9)
- **Backend:** Dapper (Micro ORM)
- **Database:** Azure SQL
- **Authentication:** ASP.NET Core Identity
- **Cloud Hosting:** Azure App Service
- **ORM:** Manual SQL scripts + Dapper

---

## ✨ Features

- 🔐 User Registration & Login
- 🛒 Shopping Cart Functionality
- 📦 Order Placement & Order History
- 🛠️ Admin Panel for Product Management
- ⚡ Optimized SQL queries with Dapper
- ☁️ Hosted on Azure App Service with SQL Database

---

## 📂 Project Structure

```plaintext
BlazeCart/
│
├── Components/          # Razor components (UI)
├── Data/                # Entity Modals and Migrations
├── Repositories/        # Dapper-based repositories
├── Services/            # Contains Extensions for Javascript that needs to be injected
├── Utility/             # Contains the Static Details (Variables and Methods)
├── wwwroot/             # Static files (images, CSS, etc.)
├── appsettings.json     # Configuration (connection strings)
├── Program.cs           # App startup and DI setup
└── BlazeCart.csproj     # Project file
```

---

## 🧑‍💻 Getting Started (Local Development)

### 🔧 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022 or VS Code
- SQL Server or Azure SQL instance

---

### ⚙️ 1. Configure the Connection String

In your `appsettings.json`, update the `DefaultConnection` string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
}
```

For Azure SQL, it should look like:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=BlazeCartDB;Persist Security Info=False;User ID=your_user;Password=your_password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

> ℹ️ Replace `your_user`, `your_password`, and `yourserver` with your actual Azure SQL credentials.

---

### 🗃️ 2. Create SQL Tables Manually

Since Dapper doesn't generate tables automatically, you need to create them manually in your SQL Server.

Run the following SQL script on your database:

```sql
-- Users (for authentication if not using Identity directly)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL
);

-- Categories
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(255) NOT NULL,
);

-- Products
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    ImageUrl NVARCHAR(500),
    Category NVARCHAR(100)
);

-- Shopping Cart Items
CREATE TABLE ShoppingCartItems (
    Id INT PRIMARY KEY IDENTITY,
    UserId NVARCHAR(450) NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Orders
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId NVARCHAR(450) NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18, 2) NOT NULL
);

-- Order Items
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

---

### ▶️ 3. Run the App

```bash
dotnet run
```

Open your browser at:

```
https://localhost:5001
```

---

## ☁️ Azure Deployment

### ✅ Hosting Details

- **Frontend:** Azure App Service
- **Database:** Azure SQL Database

### 🔄 Steps to Deploy

1. Push your code to GitHub.
2. Create an Azure App Service for a .NET 9 app.
3. Set up deployment via:
   - **GitHub Actions** *(recommended)*, or
   - **Publish profile** from Visual Studio

### 🔑 Set Connection String in Azure

Go to:

```
Azure Portal → App Service → Configuration → Connection Strings
```

| Name              | Value                                                                                                  | Type     |
|-------------------|--------------------------------------------------------------------------------------------------------|----------|
| DefaultConnection | Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=BlazeCartDB;User ID=...;Password=...   | SQLAzure |

Also set the following application setting:

| Name                  | Value      |
|-----------------------|------------|
| ASPNETCORE_ENVIRONMENT | Production |

> ✅ Ensure your Azure SQL firewall allows Azure services.

---

## 🧪 Testing

- ✅ Register/Login Flow
- ✅ Add to Cart and Quantity Update
- ✅ Place Order and View Order History
- ✅ Admin Add/Edit/Delete Products
- ✅ Dapper SQL Parameterization Verified

---

## 🔐 Security Practices

- Uses parameterized SQL queries via Dapper (prevents SQL injection)
- Encrypted HTTPS enforced in Azure
- Passwords should be hashed before storing (if not using Identity)

---

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 🙌 Acknowledgements

- Microsoft ASP.NET Team
- Azure App Service & SQL Docs
- Dapper Contributors
