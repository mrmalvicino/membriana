# Membriana

- [Membriana](#membriana)
  - [Features](#features)
  - [Screenshots](#screenshots)
  - [Setup Instructions](#setup-instructions)
    - [Configuration Parameters](#configuration-parameters)
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

### Configuration Parameters

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
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\SQLEXPRESS; Initial Catalog=membriana_db; Integrated Security=True",
    "LOCAL_SERVER_NAME": "Data Source=CUSTOM_PATH\\SQLEXPRESS; Initial Catalog=membriana_db; Integrated Security=True",
    "EXTERNAL_SERVER_NAME": "Data Source=SERVER_ADDRESS_OR_IP; Initial Catalog=membriana_db; User ID=USERNAME; Password=PASSWORD; Connect Timeout=30; TrustServerCertificate=True;"
  }
}
```

&nbsp;
The following table details the parameters which need to be modified:

| Parameter | Section | Details |
|-|-|-|
| `LOCAL_SERVER_NAME` | connectionStrings | Name of the local server string. |
| `CUSTOM_PATH` | connectionStrings | Path where SQL Express was installed. |
| `EXTERNAL_SERVER_NAME` | connectionStrings | Name of the external server string. |
| `SERVER_ADDRESS_OR_IP` | connectionStrings | URL address or IP of the external server. |
| `USERNAME` | connectionStrings | Username to access the external server. |
| `PASSWORD` | connectionStrings | Password to access the external server. |

> [!TIP]
&nbsp;
You can replace `DefaultConnection` in [Program.cs](./src/Mvc/Program.cs) by the choosen `LOCAL_SERVER_NAME` or `EXTERNAL_SERVER_NAME`. But you may also remove the whole default connection line from `appsettings.json` and replace the `LOCAL_SERVER_NAME` or `EXTERNAL_SERVER_NAME` by `DefaultConnection`.

### NuGet Packages

> [!IMPORTANT]
&nbsp;
Install each of the following **NuGet Packages** for the respective projects according to the following table:

| Projects | NuGet Package | Purpose |
|-|-|-|
| [Domain](./src/Domain/) | [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/) | ORM for database generation. |
| [Domain](./src/Domain/) | [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/) | Enables Microsoft SQL Server. |
| [Domain](./src/Domain/) | [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/) | Enables migrations. |
| [Mvc](./src/Mvc/) | [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/) | Enables migrations. |
| [Domain](./src/Domain/), [Infrastructure](./src/Infrastructure/) | [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore/) | User authentication. |
| [Domain](./src/Domain/) | [Microsoft.AspNetCore.Mvc.Core](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/) | ValidateNever annotation. |

>[!TIP]
&nbsp;
In order to install the packages using Visual Studio interface, open the [solution](./src/Membriana.sln), right click the project, select `Manage NuGet Packages...` and install each package.

## License and Contributions

&nbsp;
This is an open source project licensed under the [General GNU Public License](./LICENSE).
Contributions are welcome! Please fork the repository and create a pull request with your improvements or suggestions.
Make sure to check the [contibution guide](./CONTRIBUTING.md) before.