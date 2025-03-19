
# ENSEK Technical Test

## Introduction

This is a technical test for Ensek. The task is to create a simple WebAPI that allows a user to upload meter readings files.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Building the Project](#building-the-project)
- [Running the Project](#running-the-project)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## Installation

1. **Clone the repository:**
2. **Ensure the following folders are created:**
- `C:\Temp`
- `C:\Temp\FileUploads`

3. **Copy the `Test_Accounts.csv` to:**
- `C:\Temp`

## Building the Project

To build the project, run the following command in the root directory:

```bash
dotnet build
```

This will compile the application and its dependencies.

## Running the Project
To run the project, use the following command:
```bash
dotnet run
```

By default, the API will be available at http://localhost:5000/swagger and https://localhost:7000/swagger.

## Testing
To run the tests, use the following command:
```bash
dotnet test
```

This will execute all the unit tests in the project.

## Contributing

We welcome contributions! Please follow these steps:

- Fork the repository.
- Create a new branch (git checkout -b feature-branch).
- Make your changes.
- Commit your changes (git commit -m 'Add some feature').
- Push to the branch (git push origin feature-branch).
- Open a pull request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
