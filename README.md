# CoinsApp

CoinsApp is a local **console application for coin collectors** and a practical learning project for building a database-driven .NET application.

The project demonstrates:

- C# / .NET 10
- SQL Server
- Dapper
- Repository-based data access
- Business/application services
- ViewModels
- Dependency Injection
- Stored procedures
- SQL Server Database Project / SSDT
- DACPAC deployment
- Global reference-data seeding
- Development reset and seed workflows
- Password hashing

The application is intentionally console-based. A graphical UI is **not part of the current scope**.

---

## Features

### Collection

- Collections
- Coins
- Coin details and metadata
- Coin images
- Price history
- Purchases
- Sales

Price history, purchases, sales and images are accessed from the corresponding coin workflow.

### Reference data

- Countries
- Currencies
- Country/Currency relationships
- Denominations
- Mints
- Materials

Several reference entities support active/inactive state management.

### Catalogs and contacts

- Catalogs
- Catalog entries
- Contacts

Catalog entries can be listed by coin or by catalog.

### Users

The current Users menu supports:

- List users
- User details
- Create user
- Update user
- Set password
- Delete user

Passwords are hashed with **PBKDF2-SHA256** using a random salt and 600,000 iterations.

There is currently **no login, authentication, registration, session management, roles or authorization**. These are planned features.

### Database administration

The Administration menu provides:

- Install Database
- Reset Database
- Database Status

The UI project builds the SQL database project and copies the required DACPAC and development reset/seed scripts to the application output.

---

## Architecture

The application follows a practical three-layer structure:

```text
CoinsApp.UI
    Console presentation
    Menus / input / DI composition root
          |
          v
CoinsApp.BLL
    Services / ViewModels / validation
          |
          v
CoinsApp.DAL
    Repositories / Dapper / SQL Server access
          |
          v
SQL Server

CoinsApp.DB
    SQL database project
    tables / indexes / stored procedures / seeds
    deployment and reset scripts
```

### Important architectural note

The current project is a practical **3-Schichten-Architektur** example, not a strict Clean Architecture implementation.

`CoinsApp.BLL` currently references `CoinsApp.DAL`, and `CoinsApp.UI` references both BLL and DAL. The UI composition root registers concrete repository implementations with Dependency Injection.

This is intentional for the current educational scope. A future refactoring could introduce a stricter application/infrastructure boundary, but that is not required for the current project.

### Projects

| Project | Responsibility |
|---|---|
| `CoinsApp.UI` | Console menus, input handling and application composition root |
| `CoinsApp.BLL` | Feature services, ViewModels, validation, password hashing and technical application services |
| `CoinsApp.DAL` | Repositories, Dapper, SQL Server client and database installation/reset/status infrastructure |
| `CoinsApp.DB` | SQL Server schema, indexes, stored procedures, global seed data, development seed and deployment/reset scripts |

---

## Technology Stack

- **.NET 10**
- **C#**
- **SQL Server**
- **SQL Server Express** is suitable for a local installation
- **Dapper 2.1.79**
- **Microsoft.Data.SqlClient 7.0.2**
- **Microsoft.SqlServer.DacFx 170.4.83**
- **Microsoft.Extensions.DependencyInjection 10.0.11**
- **Microsoft.Extensions.Configuration 10.0.11**
- SQL Server Database Project / SSDT
- SQL Server stored procedures

The database project targets the SQL Server `Sql160` schema provider.

---

## Requirements

For local development on Windows:

1. **.NET 10 SDK**
2. **SQL Server**  
   SQL Server Express is sufficient for a local installation.
3. **Visual Studio** with SQL Server Data Tools / SQL database project support
4. Git, if cloning the repository

The SQL database project is an SSDT-style `.sqlproj`, so Visual Studio with the required SQL database project tooling is the recommended build environment.

---

## Getting the Project

Clone the repository:

```bash
git clone <repository-url>
cd CoinsApp-master
```

Or download and extract the repository ZIP.

Open:

```text
CoinsApp.slnx
```

in Visual Studio.

---

## Configure SQL Server

The current development configuration is stored in:

```text
CoinsApp.UI/appsettings.json
```

The repository currently contains a machine-specific SQL Server Express example:

```text
DESKTOP-S63OFET\SQLEXPRESS
```

On another machine, change the connection string to the SQL Server instance that is actually installed there.

Example for SQL Server Express:

```json
{
  "ConnectionStrings": {
    "CoinsApp": "Data Source=.\\SQLEXPRESS;Initial Catalog=CoinsApp;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Command Timeout=0;"
  }
}
```

The application uses:

```text
Integrated Security=True
```

Therefore the Windows account running CoinsApp must be able to connect to SQL Server and deploy the database.

---

## Build

The recommended way to build the complete solution is **Visual Studio**, because `CoinsApp.UI` invokes the `CoinsApp.DB` SQL database project during its build.

From Visual Studio:

1. Open `CoinsApp.slnx`.
2. Restore NuGet packages.
3. Build the solution.

The `CoinsApp.UI` build process additionally:

1. builds `CoinsApp.DB`;
2. produces `CoinsApp.DB.dacpac`;
3. copies the DACPAC to:

```text
bin\<Configuration>\Database\CoinsApp.DB.dacpac
```

4. copies:

```text
ResetDevelopment.sql
DevelopmentSeed.sql
```

to:

```text
bin\<Configuration>\Reset\
```

If the SQL database project cannot be built, verify that the SQL database project/SSDT workload is installed in Visual Studio.

---

## First Run

Start the `CoinsApp.UI` project.

The main menu contains:

```text
A. Administration
B. Coins
C. Collections
D. Contacts
E. Catalogs
F. Catalog Entries
G. Countries
H. Currencies
I. Country Currencies
J. Mints
K. Denominations
L. Materials
M. Users
```

### 1. Install the database

Open:

```text
Administration
    -> Install Database
```

The application loads:

```text
Database/CoinsApp.DB.dacpac
```

from its output directory and deploys/upgrades the database named:

```text
CoinsApp
```

using the configured connection string.

### 2. Check database status

Open:

```text
Administration
    -> Database Status
```

The application reports whether the database is available and, when available, displays:

- SQL Server name
- database name
- SQL Server version
- edition
- product level

---

## Reset the Development Database

For development/testing, use:

```text
Administration
    -> Reset Database
```

The application loads:

```text
Reset/ResetDevelopment.sql
Reset/DevelopmentSeed.sql
```

and executes both scripts in one SQL transaction.

The reset workflow is **destructive** and is intended for development/testing. Do not use it against data that must be preserved.

---

## Seed Data

The project separates reusable global reference data from disposable development data.

### Global reference data

The database deployment seeds reference data such as:

- Countries
- Currencies
- Country/Currency relationships
- Materials
- Mints
- Denominations
- Catalogs

This is handled by the database project's post-deployment process.

### Development data

Development data is stored in:

```text
CoinsApp.DB/Development/Seed/DevelopmentSeed.sql
```

It is **not** executed by the normal database post-deployment script.

The development seed creates data used for local testing, including a development user, collection, contacts and sample coin-related data.

The development user contains a placeholder password hash and is **not an authentication credential**. There is currently no login functionality.

---

## Using the Application

### Coins

The Coins menu provides:

- List coins
- Coin details
- Create coin
- Update coin
- Delete coin
- Price history
- Purchases
- Sales
- Images

A coin can reference:

- Collection
- Country
- Currency
- Denomination
- Mint
- Material

Additional coin data includes physical properties and other collector information supported by the current database model.

### Collections

Create, view, update and delete collections.

Collections are associated with users in the current data model, but ownership enforcement is not yet implemented through authentication/authorization.

### Price History

Price history records belong to coins.

`PriceHistory` is the source of truth for historical prices. The `Coins` table also contains materialized current-price fields. During database deployment, these fields are synchronized from the latest price-history record.

### Purchases and Sales

Purchases and sales are managed from the corresponding coin workflow.

They contain the transaction information supported by the current database model and use contacts where applicable.

### Coin Images

Create and manage image metadata associated with coins.

### Catalogs and Catalog Entries

Manage catalogs and their entries. Catalog entries can be queried by coin or by catalog.

### Reference Data

Countries, currencies, country/currency relationships, denominations, mints and materials provide the reference data used by the coin model.

Several of these reference entities support activation/deactivation rather than deletion.

### Contacts

Manage contacts used by collection transaction workflows.

### Users

The current user-management workflow is administrative CRUD:

```text
List
Details
Create
Update
Set Password
Delete
```

Passwords are never stored as plaintext by the BLL. `PasswordHasher` uses:

```text
PBKDF2
SHA-256
16-byte random salt
600,000 iterations
32-byte derived hash
```

Password **verification is not implemented yet**, because authentication itself is not implemented.

---

## Database

The SQL project contains the main application tables:

```text
Users
Collections
Coins
CoinImages
PriceHistory
Purchases
Sales
Contacts
Catalogs
CatalogEntries
Countries
Currencies
CountryCurrencies
Denominations
Mints
Materials
```

It also contains:

- primary keys
- foreign keys
- unique constraints
- indexes
- stored procedures
- global seed scripts
- development seed data
- pre-deployment validation
- post-deployment synchronization

Repositories in `CoinsApp.DAL` use Dapper and the SQL Server client to access the database through the defined data-access procedures.

---

## Development Workflow

A practical workflow is:

```text
1. Open CoinsApp.slnx in Visual Studio
2. Restore/build the solution
3. Start CoinsApp.UI
4. Check Administration -> Database Status
5. Install Database when necessary
6. Test the relevant feature through the console menus
7. Use Reset Database for a clean development/test state
8. Repeat the feature test
```

For a database change:

```text
1. Modify CoinsApp.DB
2. Build the database project
3. Check the generated DACPAC/scripts
4. Start CoinsApp.UI
5. Install/update the database
6. Test the affected feature
7. Reset and repeat when a clean test state is required
```

---

# TODO / Roadmap

The roadmap is ordered by practical importance. The project should remain a console application for now.

## 1. Authentication and Account Security

This is the next major functional step.

- [ ] Implement password verification
- [ ] Implement login
- [ ] Implement logout
- [ ] Add current-user/session context
- [ ] Add user self-registration
- [ ] Decide whether registration is open or administrator-controlled
- [ ] Add authenticated-user password change
- [ ] Define account activation/deactivation behavior
- [ ] Introduce roles and permissions
- [ ] Protect Administration and User Management
- [ ] Implement authorization checks in the BLL
- [ ] Prevent unauthorized access to another user's data
- [ ] Add authentication and authorization tests
- [ ] Review password-reset/recovery requirements before implementing them

## 2. Ownership and Domain Rules

The database already contains user/ownership relationships. The application still needs explicit rules around them.

- [ ] Define authenticated-user ownership rules
- [ ] Define which collections a user may access
- [ ] Define which coins a user may modify
- [ ] Define how purchases and sales affect ownership
- [ ] Define how `OwnerId` / `InitialOwnerId` should behave
- [ ] Restrict modifications according to the ownership rules
- [ ] Add tests for ownership boundaries

## 3. Validation and Error Handling

- [ ] Review validation consistently across all services
- [ ] Standardize validation messages
- [ ] Improve handling of duplicate records
- [ ] Improve foreign-key error handling
- [ ] Review monetary and date validation
- [ ] Review nullable/optional fields
- [ ] Distinguish validation errors from database/infrastructure errors
- [ ] Add consistent top-level exception handling
- [ ] Add logging where it provides practical value

## 4. Console UI

- [ ] Finish the consistency review of all menus
- [ ] Keep list output consistently tabular
- [ ] Keep detail output consistent
- [ ] Standardize destructive-operation confirmation
- [ ] Standardize navigation and empty-result handling
- [ ] Review all input handling through `MenuInput`
- [ ] Add useful search/filter operations
- [ ] Add useful sorting operations

## 5. Collector Features

After authentication and core reliability are in place:

- [ ] Collection overview
- [ ] Collection statistics
- [ ] Coin counts by collection
- [ ] Purchase-cost totals
- [ ] Current collection value
- [ ] Profit/loss for sold coins
- [ ] Value by currency
- [ ] Value by country
- [ ] Value by material
- [ ] Better coin search/filtering
- [ ] Useful collection reports
- [ ] Optional CSV/Excel export

## 6. Database and Deployment

- [ ] Document the database installation requirements
- [ ] Define a clear database-version/deployment strategy as the project grows
- [ ] Review DACPAC deployment behavior against existing data
- [ ] Review backup/restore requirements
- [ ] Add database integrity checks where useful
- [ ] Continue reviewing indexes against actual query patterns
- [ ] Continue reviewing stored procedures against repository requirements

## 7. Testing

There is currently no separate test project in the solution.

- [ ] Add a BLL unit-test project
- [ ] Add repository/integration tests
- [ ] Add database deployment tests where practical
- [ ] Cover CRUD operations across the feature chains
- [ ] Cover validation rules
- [ ] Cover authentication
- [ ] Cover authorization
- [ ] Cover ownership rules
- [ ] Establish a repeatable clean-database test procedure

## 8. Documentation

- [ ] Document the three-layer architecture
- [ ] Document one complete feature chain from Menu to Repository to SQL
- [ ] Document DI registration
- [ ] Document database deployment/reset
- [ ] Document global vs. development seed data
- [ ] Document authentication/authorization after implementation
- [ ] Keep this README synchronized with the source

## 9. Optional Future GUI

A GUI is **not currently required**.

If the project later needs a graphical interface, it can be added as another presentation client while keeping the existing console application:

```text
                 +----------------------+
                 |    CoinsApp.UI       |
                 |      Console         |
                 +----------+-----------+
                            |
                            v
                    +---------------+
                    |  CoinsApp.BLL |
                    +-------+-------+
                            |
                            v
                    +---------------+
                    |  CoinsApp.DAL |
                    +-------+-------+
                            |
                            v
                       SQL Server


Later, if needed:

                 +----------------------+
                 |    CoinsApp.GUI     |
                 |        WPF          |
                 +----------+-----------+
                            |
                            v
                       CoinsApp.BLL
```

The GUI should be considered only after the current console application and its authentication, authorization, validation and testing foundations are mature.

---

## Current Status

**Stage:** functional local console application and database-architecture learning project.

The main CRUD feature chains, SQL database project, stored procedures, reference-data seeding, development reset workflow, database administration and user password hashing are implemented.

The next major functional milestone is:

```text
Authentication
    ->
User registration
    ->
Session/current user
    ->
Roles and authorization
    ->
Ownership enforcement
```

The application should currently be treated as a **local development/learning application**, not as a production-ready multi-user system.

---

## License

No license has been specified for the repository yet.
