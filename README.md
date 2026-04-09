# Tasneem Shop | Enterprise E-Commerce Solution

## Overview
**Tasneem Shop** is a robust, full-featured e-commerce platform built with **ASP.NET Core MVC**. The project is engineered using **N-Tier Architecture ** and the **Generic Repository Pattern** to ensure a strict separation of concerns, high maintainability, and scalability. It provides a seamless shopping experience for users and a powerful management dashboard for administrators.

---

## Architecture & Design Patterns
The solution is decoupled into specialized layers to follow the **N-Tier Architecture** principle:

###  Tasneem_Shop.Entities (Core Layer)
* **Domain Models:** Contains core business objects like `Product`, `Category`, `ApplicationUser`, `OrderHeader` ,`OrderDetail`,`ProductImage` and `ShoppingCart`.
* **Interfaces:** Defines the contract for data operations (`IUnitOfWork`, `IGenericRepository` ,`IProductRepository`,`ICategoryRepository`,`IOrderHeaderRepository`,`IOrderDetailRepository`,`IShoppingCartRepository`and `IUserRepository`).
* **ViewModels:** Defines the transport data operations (`HomeVM`, `CartVM` ,`CartItemVM`,`OrderVM`,`ProductVM`and`UserVM`).

###  Tasneem_Shop.DataAccess (Data Layer)
* **Context:** Bridge between the application and the database (`ApplicationDbContext`).
* **DbInitializer:** Seeding the database with initial data. (`IDbInitializer` and `DbInitializer`).
* **Implementation:** Actual logic for repository. (`UnitOfWork`, `GenericRepository` ,`ProductRepository`,`CategoryRepository`,`OrderHeaderRepository`,`OrderDetailRepository`,`ShoppingCartRepository`and `UserRepository`).
* **Migrations:** C# files that keep the SQL Server database in sync with your Domain Models.


###  Tasneem_Shop.Web (Presentation Layer)
* **Areas Strategy:** organized into dedicated Areass(`Admin`, `Customer` ,`Identity`).
* **MVC Logic:** Handles user interactions with a clean separation between Logic and Controllers.
* **Responsive UI:** Mobile-first design optimized for all screen sizes .

---

##  Key Features

### Customer Experience
* **Guest Shopping:** Browse products and manage a cookie based shopping cart without an initial login.
* **Secure Checkout:** Fully integrated with **Stripe API** for real-time and secure payment processing.
* **Order Management:** Customers can track their order status in real-time from "Pending" to "Shipped."

### Administrative Dashboard
* **Product & Category CRUD:** Full control creation, update, and deletion and make hotdeals for any product .
* **User Authority:** Advanced user management including the ability to **Lock/Unlock** accounts for security.
* **Order Mangement:** A dedicated system to process orders, manage shipping logistics, and handle **Automated Refunds** via Stripe for cancelled orders.

---

## Technologies Used
* **Backend:** .NET 10.0, ASP.NET Core MVC.
* **Database:** SQL Server with EF Core (Code First Approach).
* **Security:** ASP.NET Core Identity for secure Authentication & Authorization.
* **Payments:** Stripe Payment Gateway Integration.
* **FrontEnd :** Bootstrap, JavaScript, and Custom CSS (optimized for mobile responsiveness).

---

## 🚀🚀🚀🚀 Getting Started
1. **Clone the Repo:** `git clone https://github.com/MustafaElshehawy/E-commerce_Shop.git`
2. **Configure Secrets:** Set your **Stripe API Keys** in **User Secrets** (Local) or Environment Variables.
3. **Update Database:** Run `Update-Database` in the Package Manager Console.
4. **Run Project:** Press `F5` in Visual Studio.

---
*Developed  by Mostafa Elshehawy*
