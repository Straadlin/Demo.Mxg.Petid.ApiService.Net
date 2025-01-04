# Vet Store

## Getting Started

Thank you for taking the time to know my work. To continue you'll see this simple and easy backend application about a Vet Store (CRUD actions). I'll continue building This project because I haven't finished it. But I hope it can be useful for you.

## Description
This backend Rest API privide different resources to be consumed by differents clients (mobile apps and web applications). In this case this backend could work with a mobile and web application.

## Features

* **Pets Operations**: Provide CRUD operations to work with user cases about Pets domain.
    * Search a product by identificator.
* **Product Operations**:  Provide CRUD operations to work with user cases about Product domain.
    * Get list of products.
* **Account Operations**:  Provide CRUD operations to work with user cases about Account domain.
    * Authenticate user (Sign-In), using its own domain.
    * Register new users (Sign-Up), using its own domain.
    * Refresh Tokens from a expired token, using its own domain.

### Screenshoots

<img width="1280" alt="2025-05-06_12-34-23" src="https://github.com/user-attachments/assets/1515b15c-33af-4fe4-a6f0-314d6a4a96e6" />

## Other Features
* **Encrypt in database user sensible information**:  Sensible and personal information are encrypted as Name, Birthdate, etc. With this action avoid to shared or use maliciously the private information of evryone.

## Tecnhologies Used
* **Clean Architecture**: This project is built usiing Clean Architecture to separete in 4 different layers.
    * Application: Contains user cases (commands & queries).
    * Domain: Contains models and domain rules.
    * Infraestructure: Contains all implementations of every service.
    * Presentation: Contains the public functionalities to be consumed for different clients.
* **Solid Principles**: This project works with Xunit, in this case the unit test were applied only to Application layer.
    * This project apply every principle to have a scalable and maintainable application.
* **Microsoft Net**: C# and Net is the principal language which this application was built.
* **Windows Service**: This project can be launched as a Windows Service using Kestrel Web Server.
* **SQL Server databases**: This project relies on sql server database to get or persist information in the repositories.
* **Entity Framework**: This ORM is the main way to connect with repositories.
* **Patterns**: This project implements , Repository(Unit Of Work), Singleton, Specification, Dependency Injection, principally.
* **Unit Tests**: This project works with Xunit, in this case the unit test were applied only to Application layer.
* **Swagger**: This project works with Xunit, in this case the unit test were applied only to Application layer.


All rights reserved. This repository is shared for demonstration purposes only. Redistribution or use of this code in any form is not permitted without explicit permission from the author.
