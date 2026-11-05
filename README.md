# SimulacaoBanco README

## Overview

SimulacaoBanco is a .NET console application that simulates banking operations. The project provides a modular architecture with services for banking operations and a user interface layer to interact with the banking system.

The application demonstrates object-oriented programming principles and dependency injection patterns within the .NET ecosystem.

## Prerequisites

To build and run this application, you need:

- [.NET SDK](https://dotnet.microsoft.com/download) version 10.0 or higher
- A .NET compatible development environment (Visual Studio, VS Code with .NET extension, or command line)

## Project Structure

The project consists of the following key components:

- **Program.cs** - Entry point of the console application
- **SimulacaoBanco.csproj** - Project configuration file
- **SimulacaoBanco/Services/** - Business logic and data access layers
- **SimulacaoBanco/UI/** - User interface components for interacting with the system

## Building and Running

### Build the Project

Open a terminal or command prompt in the project directory and run:

```bash
dotnet build
```

This will compile the application and restore any dependencies (though this project has no external dependencies).

### Run the Application

After building, you can run the application using:

```bash
dotnet run
```

## Usage

The application provides a simple banking simulation interface:

1. **Initialization**: The system starts with "Banco do Brasil" as the bank name
2. **Menu System**: A menu interface is displayed to interact with banking operations
3. **Core Features**: The application supports basic banking operations such as:
   - Account management
   - Transaction processing
   - User interactions

### Example Usage

When you run the application, you'll see a menu interface where you can:

```
Welcome to Banco do Brasil Simulation!
Please select an option:
1. Create Account
2. Deposit Money
3. Withdraw Money
4. Check Balance
5. Exit
```

Select an option by entering the corresponding number and follow the prompts.

## Project Details

- **Target Framework**: .NET 10.0
- **Output Type**: Console Application (Exe)
- **Nullable Analysis**: Enabled
- **Implicit Usings**: Enabled

## Dependencies

This project has no external dependencies beyond the .NET SDK. It uses only the standard .NET libraries provided with the SDK.

## Notes

- The application is designed as a simulation and does not connect to real banking systems
- All data is in-memory and resets with each application run
- The project follows .NET best practices for console applications

## Troubleshooting

### .NET SDK Not Found

If you encounter issues with the .NET SDK, ensure you have installed version 10.0 or higher from the official Microsoft website.

### Build Errors

If you encounter build errors, check that you have a compatible version of the .NET SDK installed and that all files are properly saved.

### Runtime Errors

For runtime errors, ensure that all required files are present in the project directory and that the application configuration is correct.

## License

This project is provided as-is without any explicit license. For specific licensing information, please contact the project maintainer.

## Support

For issues or questions, please refer to the project documentation or contact the project maintainer.