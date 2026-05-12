# VideoGameManagerEF

A web application developed with **ASP.NET Core Razor Pages** to manage a video game catalog, evolving from file-based persistence to a relational database managed with **Entity Framework Core (EF Core)**.

This version implements a **Code First** approach, where the database schema is automatically generated from C# models, including entity relationships and complex LINQ queries.

## 🚀 Main Features

* **Relational Persistence:** Uses SQL Server to store game and developer data.
* **Relational Data Model (1:N):** Management of the `Developer` entity and its relationship with `Game`.
* **Advanced Statistics & Queries (LINQ):**
  * Combined search by title, genre, and minimum year.
  * Average score calculations per developer.
  * Game distribution by decades.
  * "Productive Developers" filtering based on a dynamic game count threshold.
* **Referential Integrity:** Blocks the deletion of developers that have associated games to maintain database consistency.
* **Integrated Creation:** Ability to create a new `Developer` directly from the `Game` creation form.

---

## 📂 Project Structure

The project follows a clean architecture where the `DbContext` acts as the central hub for SQL Server communication.

```text
VideoGameManagerEF/
├── Models/
│   ├── Game.cs                # Game entity with FK to Developer
│   └── Developer.cs           # Studio entity (1:N relationship with Games)
├── Data/
│   ├── GameStoreContext.cs    # Entity Framework Core DbContext
│   └── Migrations/            # Database schema history
├── Pages/
│   ├── Games/                 # Full CRUD for video games
│   ├── Developers/            # Studio management and associated games view
│   ├── Stats/                 # Advanced statistics and search dashboard
│   └── Shared/
│       └── _Layout.cshtml     # Main layout template
├── appsettings.json           # ConnectionString configuration
└── Program.cs                 # DbContext service and DI configuration
```

---

## ⚙️ Execution Instructions

### Prerequisites

* **[.NET 10.0 SDK](https://dotnet.microsoft.com/download)** (Required for current NuGet package compatibility).
* **Visual Studio 2026 Insider Edition** (The project requires the latest IDE features for deployment).
* **SQL Server** (LocalDB or Express) running locally.
* EF Core Global Tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Setup and Launch

1. **Configure the Database:**

   Open `appsettings.json` and adjust the `DefaultConnection` according to your local server instance:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=VideoGameManagerDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. **Apply Migrations:**

   Run the following command in the terminal to create the tables automatically:

   ```bash
   dotnet ef database update
   ```

3. **Run the Application:**

   Press **F5** in Visual Studio 2026 Insider or run via terminal:

   ```bash
   dotnet run
   ```

---

## 📊 Implemented LINQ Queries

The system leverages the LINQ to Entities engine to perform operations directly on the database.

### 1. Combined Search (Task 7.2)

Allows filtering games simultaneously by multiple criteria (case-insensitive):

```csharp
var query = _context.Games
    .Include(g => g.Developer)
    .AsQueryable();

if (!string.IsNullOrEmpty(titleFilter))
    query = query.Where(g => g.Title.Contains(titleFilter));

if (!string.IsNullOrEmpty(genreFilter))
    query = query.Where(g => g.Genre == genreFilter);
```

### 2. Performance Ranking (Task 7.1)

Dynamic calculation of the average quality score for each studio:

```csharp
var avgByDev = await _context.Developers
    .Where(d => d.Games.Any())
    .Select(d => new DeveloperStat {
        Name = d.Name,
        AvgScore = d.Games.Average(g => g.Score)
    })
    .ToListAsync();
```

### 3. Data Integrity (Task 8.3)

Before deleting a developer, the system verifies dependencies to prevent foreign key constraint errors:

```csharp
if (developer.Games.Any()) {
    ModelState.AddModelError("", "Cannot delete a studio that has associated games.");
}
```

---

## 🛠️ Technologies Used

* ASP.NET Core 10 Razor Pages - Presentation framework.
* Entity Framework Core - ORM for data management.
* LINQ - Query language for filtering and statistics.
* Bootstrap 5 - UI design and responsiveness.
* SQL Server - Relational database engine.