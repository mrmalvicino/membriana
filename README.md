# Membriana

- [Membriana](#membriana)
  - [Features](#features)
  - [Screenshots](#screenshots)
  - [Setup Instructions](#setup-instructions)
    - [Configuration Parameters: Frontend](#configuration-parameters-frontend)
    - [Configuration Parameters: Backend](#configuration-parameters-backend)
    - [NuGet Packages](#nuget-packages)
  - [License and Contributions](#license-and-contributions)

## Features

&nbsp;
Membriana offers the following functionalities:
- **Web-Based Platform**: Accessible from any device with an internet connection.
- **Member Management**: Register, update, and track members.
- **Payment Tracking**: Record payments, due dates, and payment statuses.
- **Automated Billing**: Send notifications and reminders for pending payments.
- **Pricing Management**: Configure different membership types with custom pricing.
- **Multi-User Access**: Role-based access for admins, operators, and other users.
- **Payment Gateway Integration**: Connect with online payment platforms.

## Screenshots

![ERD](https://i.imgur.com/fAL0oJb.png)

## Setup Instructions

### Configuration Parameters: Frontend

> [!IMPORTANT]
&nbsp;
Create a file named `appsettings.json` in the [Mvc](./src/Mvc/) project directory, modifying the following template code:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiBaseUrl": "API_BASE_URL"
}
```

&nbsp;
The following table details the parameters which need to be modified:

| Parameter | Key | Details | Example |
|-|-|-|-|
| `API_BASE_URL` | `ApiBaseUrl` | URL where the API is running. | https://localhost:7076 |

### Configuration Parameters: Backend

> [!IMPORTANT]
&nbsp;
Create a file named `appsettings.json` in the [Api](./src/Api/) project directory, modifying the following template code:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\SQLEXPRESS; Initial Catalog=membriana_db; Integrated Security=True",
    "LOCAL_SERVER_NAME": "Data Source=CUSTOM_PATH\\SQLEXPRESS; Initial Catalog=membriana_db; Integrated Security=True",
    "EXTERNAL_SERVER_NAME": "Data Source=SERVER_ADDRESS_OR_IP; Initial Catalog=membriana_db; User ID=USERNAME; Password=PASSWORD; Connect Timeout=30; TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "SECRET_KEY",
    "Issuer": "Membriana.Api",
    "Audience": "Membriana.Mvc",
    "ExpireMinutes": 60
  }
}
```

&nbsp;
The following table details the parameters which need to be modified:

| Parameter | Key | Details |
|-|-|-|
| `LOCAL_SERVER_NAME` | `ConnectionStrings` | Name of the local server string. |
| `CUSTOM_PATH` | `ConnectionStrings` | Path where SQL Express was installed. |
| `EXTERNAL_SERVER_NAME` | `ConnectionStrings` | Name of the external server string. |
| `SERVER_ADDRESS_OR_IP` | `ConnectionStrings` | URL address or IP of the external server. |
| `USERNAME` | `ConnectionStrings` | Username to access the external server. |
| `PASSWORD` | `ConnectionStrings` | Password to access the external server. |
| `SECRET_KEY` | `Jwt` | Random string for signing JWTs. |

> [!TIP]
&nbsp;
You can replace `DefaultConnection` in [Program.cs](./src/Mvc/Program.cs) by the choosen `LOCAL_SERVER_NAME` or `EXTERNAL_SERVER_NAME`. But you may also remove the whole default connection line from `appsettings.json` and replace the `LOCAL_SERVER_NAME` or `EXTERNAL_SERVER_NAME` by `DefaultConnection`.

### NuGet Packages

> [!IMPORTANT]
&nbsp;
Install each of the following **NuGet Packages** for the respective projects according to the following table:

| Projects | NuGet Package | Version | Purpose |
|-|-|-|-|
| [Domain](./src/Domain/) | [EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/) | Latest | ORM for database generation. |
| [Domain](./src/Domain/) | [EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/) | Latest | Enables Microsoft SQL Server. |
| [Domain](./src/Domain/) | [EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/) | Latest | Enables migrations. |
| [Domain](./src/Domain/) | [AspNetCore.Mvc.Core](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/) | Latest | ValidateNever annotation. |
| [Domain](./src/Domain/), [Infrastructure](./src/Infrastructure/) | [Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore/) | Latest | User authentication. |
| [Api](./src/Api/) | [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/) | 8.0.16 | JWT Authentication. |
| [Api](./src/Api/) | [Tokens.Jwt](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt) | 8.11.0 | JWT Authentication. |
| [Mvc](./src/Mvc/) | [EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/) | 9.0.5 | Enables migrations. |
| [Application](./src/Application/), [Mvc](./src/Mvc/) | [AutoMapper](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/) | 12.0.1 | DTOs and Views mapping. |

>[!TIP]
&nbsp;
In order to install the packages using Visual Studio interface, open the [solution](./src/Membriana.sln), right click the project, select `Manage NuGet Packages...` and install each package.

## License and Contributions

&nbsp;
This is an open source project licensed under the [General GNU Public License](./LICENSE).
Contributions are welcome! Please fork the repository and create a pull request with your improvements or suggestions.
Make sure to check the [contibution guide](./CONTRIBUTING.md) before.