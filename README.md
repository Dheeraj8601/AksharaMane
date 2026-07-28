# 📚 AksharaMane

AksharaMane is a full-stack online bookstore application built with **ASP.NET Core 9 Web API**, **React**, and **SQL Server**. It allows customers to browse books, place Cash on Delivery (COD) orders, and track orders, while administrators can manage books, categories, and orders through a secure dashboard.

## 🚀 Features

### Customer

* Browse books by category
* Search books
* View book details
* Place COD orders
* Track and cancel eligible orders

### Admin

* Secure JWT-based login
* Dashboard
* Manage categories
* Manage books
* Manage orders
* Update order status

## 🛠️ Tech Stack

### Backend

* ASP.NET Core 9 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Clean Architecture

### Frontend

* React
* React Router
* Axios
* Bootstrap
* React Toastify

## 📂 Project Structure

```text
AksharaMane
│
├── Backend
│   ├── API
│   ├── Application
│   ├── Domain
│   ├── Infrastructure
│   └── Persistence
│
└── Frontend
    └── React
```

## ⚙️ Getting Started

### Backend

```bash
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
npm install
npm run dev
```

## 🔐 Authentication

* JWT Authentication
* Role-based Authorization
* Protected Admin Routes


## 📄 License

This project is created for learning and portfolio purposes.
