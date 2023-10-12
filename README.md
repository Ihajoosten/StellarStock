# StellarStock [![.NET](https://github.com/Ihajoosten/StellarStock/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Ihajoosten/StellarStock/actions/workflows/dotnet.yml) [![Docker Image CI](https://github.com/Ihajoosten/StellarStock/actions/workflows/docker-image.yml/badge.svg)](https://github.com/Ihajoosten/StellarStock/actions/workflows/docker-image.yml)

StellarStock is a robust inventory management system built on the .NET Core framework, employing the CQRS pattern and adhering to clean architecture principles. It provides a scalable and maintainable solution for businesses seeking efficient control over their stock and inventory.

## Features

- **CQRS Architecture:** StellarStock leverages the Command Query Responsibility Segregation (CQRS) pattern, separating read and write operations for improved scalability and performance.

- **Clean Architecture:** The project is structured following clean architecture principles, promoting a clear separation of concerns and making it easy to maintain and extend.

- **Microsoft Identity Integration:** Utilizing Microsoft Identity for seamless and secure authentication and authorization processes, ensuring a robust and reliable user management system.

- **Inventory Management:** StellarStock allows you to effortlessly manage your inventory, including creating, updating, and deleting inventory items. Query functionalities provide insights into stock levels.

## Folder Structure

- **Presentation Layer:** Contains the web API or MVC project for user interaction and authentication.
- **Application Layer:** Implements CQRS commands, queries, and handlers for business logic.
- **Domain Layer:** Holds the core business logic, entities, and domain services.
- **Infrastructure Layer:** Manages data access, external dependencies, and infrastructure-specific concerns.

## Getting Started

Follow these steps to get StellarStock up and running on your local machine:

1. Clone the repository: `git clone https://github.com/Ihajoosten/StellarStock.git`
2. Navigate to the `src` directory.
3. Open the solution in Visual Studio or your preferred IDE.
4. Build and run the application.

For more detailed instructions, refer to the [**Installation Guide**](docs/installation.md).

## Contribution Guidelines

We welcome contributions from the community. If you want to contribute to StellarStock, please read our [**Contribution Guidelines**](CONTRIBUTING.md) before getting started.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- The project structure is inspired by the principles of clean architecture and CQRS.
- Special thanks to the open-source community for their valuable contributions.

Feel free to explore the codebase and contribute to making StellarStock even better!
