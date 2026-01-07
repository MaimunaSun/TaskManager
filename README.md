# Task Management System

A full-stack task orchestration platform featuring a React-driven "Smart Dashboard" and a robust .NET REST API. This application goes beyond basic CRUD by implementing automated priority logic and intelligent task views.

## 🚀 Key Features

### 🧠 Smart Dashboard (React)
* **Dynamic Priority Engine:** Automatically calculates and color-codes task priority (High/Medium/Low) based on real-time due date proximity.
* **Intelligent Filtering:** Includes "Smart Views" to instantly toggle between "Top 3 Urgent" tasks and "Due This Week."
* **Automated Overdue Detection:** Visually flags overdue tasks using conditional styling and background highlights.
* **Tagging System:** Categorizes tasks into "Work" or "Personal" for better organization and filtered browsing.

### ⚙️ Robust Backend (.NET & SQL Server)
* **Persistent Storage:** Uses Entity Framework Core (Code-First) for reliable data management in SQL Server.
* **RESTful Architecture:** Clean separation of concerns using a modular Service-Interface-Controller pattern.
* **API Documentation:** Fully integrated Swagger UI for seamless endpoint testing and client integration.
* **Code Reliability:** Backend logic validated through xUnit unit testing.

---

## 🛠️ Tech Stack

* **Frontend:** React, Axios, CSS3 (Conditional Styling)
* **Backend:** ASP.NET Core Web API, C#
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Testing:** xUnit

---

## 📂 Project Structure

* `taskmanager-ui/` - React frontend with logic for state management and smart views.
* `TaskManagerAPI/` - C# Web API with persistent database models and services.

---

## ⚙️ Getting Started

### Prerequisites
* [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
* [Node.js & npm](https://nodejs.org/)
* [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### Installation

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/MaimunaSun/TaskManager.git](https://github.com/MaimunaSun/TaskManager.git)
