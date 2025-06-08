# Assignment Library System

## Overview
The Assignment Library System is a console and web-based application designed for making and manageing assignments like a planner.

## Features
- Add, update, search, and delete assignments
- Add or update notes per assignment
- Mark assignments as complete
- Moq-based unit testing via xUnit
- Test coverage includes service logic and private UI methods
- Console UI and Web API controller for CRUD operations

## Technologies/Dependencies
- Visual Studio 2022
- .NET 9
- .NET 7 / .NET 8 (depending on branch)
- C#
- ASP.NET Core (Web API)
- Moq + xUnit for testing
- Swagger (OpenAPI) for API interaction

s
## Setup

### Prerequisites
- [.NET SDK 7.0 or 8.0](https://dotnet.microsoft.com/download)
- Visual Studio 2022 or later

### Getting Started
1. Clone the repository:
   ```bash
   https://github.com/Kaiamanthe/AssignmentLabs.git

2. Build the solution
    a. Console
    b. Api for swagger
    
### Project Structure
- AssignmentLibrary/        # Business logic and domain models
- AssignmentLibrary.Tests/  # Unit tests with Moq and xUnit
- AssignmentCLI/            # Console interface
- AssignmentAPI/            # Web API interface with Swagger
