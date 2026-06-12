# Ticket Management System

## Overview

The Ticket Management System is a backend application designed to manage user tickets with role-based access control, authentication, and optimized database operations. It supports ticket creation, updates, filtering, bulk insertion, and analytics.

---

## Features

### Authentication and Authorization

* JWT-based authentication
* Role-based authorization (Admin and User roles)

### User Management

* Create user with default role assignment
* Update user role (Admin only)
* Retrieve user details (Admin only)
* Soft delete user
* Fetch user by unique identifier

### Ticket Management

* Create ticket
* Update ticket
* Retrieve tickets with pagination and filtering
* Fetch ticket by unique identifier
* Soft delete ticket (Admin only)

### Analytics

* Retrieve total active tickets
* Priority-wise ticket distribution

### Performance Optimization

* Bulk ticket insertion using User Defined Table Types (UDT)
* Pagination using OFFSET-FETCH
* Stored procedures for database operations

---

## Architecture

The system follows a layered architecture:

Controller → Service → Repository → Database

* Controller: Handles HTTP requests and responses
* Service: Contains business logic
* Repository: Handles database interaction using ADO.NET
* Database: SQL Server with stored procedures

---

## Technology Stack

* Backend: ASP.NET Core Web API
* Database: SQL Server
* Authentication: JWT
* Data Access: ADO.NET
* Testing Tools: Postman, Swagger

---

## API Endpoints

### Authentication

POST /api/Auth/login

### User APIs

POST   /api/User/create
PUT    /api/User/update-role
GET    /api/User/details
GET    /api/User/{userGuid}
DELETE /api/User/delete/{userGuid}

### Ticket APIs

POST   /api/Ticket/create
PUT    /api/Ticket/update
GET    /api/Ticket/list
GET    /api/Ticket/{ticketGuid}
DELETE /api/Ticket/delete/{ticketGuid}

### Summary

GET /api/Ticket/summary

### Bulk Insert

POST /api/Ticket/bulk

---

## Database Design

* GUIDs used for external references
* Integer IDs used internally for performance
* Soft delete implemented using IsActive flag
* Business logic handled via stored procedures
* Default role assignment managed at database level

---

## Security

* JWT-based authentication
* Role-based access control
* Password hashing using BCrypt
* Internal identifiers are not exposed externally

---

## Testing

All API endpoints were tested using Postman to ensure proper functionality and reliability. Authentication was validated using JWT tokens, and secured endpoints were accessed by passing the token through the Authorization header. Different scenarios such as successful operations, invalid inputs, and edge cases were tested to verify the robustness of the system. Pagination, filtering, bulk insertion, and role-based access control were also thoroughly validated during testing.
