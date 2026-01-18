# SplitCorrect - Expense Splitting Application

A modern expense splitting application built with C# .NET 8 and React. Split expenses with friends, track balances, and settle debts easily.

## Features

- **Group Management**: Create groups for different friend circles or events
- **Member Management**: Add members to groups with email tracking
- **Expense Tracking**: Record expenses and split them equally among members
- **Balance Calculation**: Automatically calculate who owes whom
- **Settlement Suggestions**: Smart debt simplification
- **Modern UI**: Clean, responsive interface built with React and Tailwind CSS

##  Architecture
Clean Architecture implementation with:
- **Domain Layer**: Entities, Value Objects, Business Logic
- **Application Layer**: Services, DTOs, Use Cases
- **Infrastructure Layer**: Database, Repositories, EF Core
- **API Layer**: REST API Controllers, Swagger
- **Tests**: Unit tests with xUnit, Moq, FluentAssertions

##  Prerequisites

- **.NET 8 SDK**: [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+**: [Download](https://nodejs.org/)
- **Docker Desktop**: [Download](https://www.docker.com/products/docker-desktop)

## Installation

### 1. Clone the Repository
```bash
git clone <your-repo-url>
cd SplitCorrect
```

### 2. Start Docker Services
```bash
docker compose up -d
```

This will start:
- PostgreSQL on `localhost:5432`
- pgAdmin on `http://localhost:5050`

### 3. Setup Backend

#### Restore Dependencies
```bash
cd src/SplitCorrect.Infrastructure
dotnet restore
```

#### Apply Database Migrations
```bash
dotnet ef database update --startup-project ../SplitCorrect.Api
```

### 4. Setup Frontend

#### Install Dependencies
```bash
cd frontend
npm install
```

#### Configure Environment (Optional)
Copy `.env.example` to `.env` and modify if needed:
```bash
cp .env.example .env
```

Default configuration works out of the box.

##  Running the Application

### Start Backend (Terminal 1)
```bash
cd src/SplitCorrect.Api
dotnet run
```
API will be available at:
- **API**: http://localhost:5016
- **Swagger UI**: http://localhost:5016/swagger

### Start Frontend (Terminal 2)
```bash
cd frontend
npm run dev
```
Frontend will be available at:
- **App**: http://localhost:3000

## 🧪 Running Tests

```bash
cd src/SplitCorrect.Tests
dotnet test
```

### With Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Database Management

### Access pgAdmin
1. Navigate to http://localhost:5050
2. Login with:
   - **Email**: admin@admin.com
   - **Password**: admin
3. Add server connection:
   - **Host**: postgres (Docker service name) or host.docker.internal
   - **Port**: 5432
   - **Database**: splitcorrect
   - **Username**: postgres
   - **Password**: postgres

### Create New Migration
```bash
cd src/SplitCorrect.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../SplitCorrect.Api
```

### Reset Database
```bash
cd src/SplitCorrect.Infrastructure
dotnet ef database drop --force --startup-project ../SplitCorrect.Api
dotnet ef database update --startup-project ../SplitCorrect.Api
```

## 🔧 Configuration

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=splitcorrect;Username=postgres;Password=postgres"
  }
}
```

### Frontend (.env)
```env
VITE_API_BASE_URL=http://localhost:5016/api
```

## 📁 Project Structure

```
SplitCorrect/
├── src/
│   ├── SplitCorrect.Domain/          # Entities, Value Objects
│   ├── SplitCorrect.Application/     # Services, DTOs
│   ├── SplitCorrect.Infrastructure/  # Database, Repositories
│   ├── SplitCorrect.Api/             # REST API, Controllers
│   └── SplitCorrect.Tests/           # Unit Tests
├── frontend/                          # React TypeScript App
│   ├── src/
│   │   ├── components/               # UI Components
│   │   ├── api.ts                    # API Client
│   │   └── App.tsx                   # Main App
│   └── package.json
├── docker-compose.yml                 # Docker Services
└── README.md
```

## API Endpoints

### Groups
- `GET /api/groups` - List all groups
- `POST /api/groups` - Create group
- `GET /api/groups/{id}` - Get group details
- `DELETE /api/groups/{id}` - Delete group

### Members
- `GET /api/members/group/{groupId}` - List members in group
- `POST /api/members` - Add member to group
- `DELETE /api/members/{id}` - Remove member

### Expenses
- `GET /api/expenses/group/{groupId}` - List expenses in group
- `POST /api/expenses` - Create expense
- `DELETE /api/expenses/{id}` - Delete expense

### Balances
- `GET /api/balances/group/{groupId}` - Calculate group balances
- `GET /api/balances/settlements/{groupId}` - Get settlement suggestions

## Docker Commands

```bash
# Start services
docker compose up -d

# Stop services
docker compose down

# View logs
docker compose logs -f postgres

# Rebuild services
docker compose up -d --build
```

## Troubleshooting

### Port Already in Use
If ports 5016, 3000, 5432, or 5050 are in use:

**Backend**: Change port in [Properties/launchSettings.json](src/SplitCorrect.Api/Properties/launchSettings.json)

**Frontend**: Change port in [vite.config.ts](frontend/vite.config.ts)

**Docker**: Change ports in [docker-compose.yml](docker-compose.yml)

### Migration Errors
```bash
# Drop and recreate database
cd src/SplitCorrect.Infrastructure
dotnet ef database drop --force --startup-project ../SplitCorrect.Api
dotnet ef database update --startup-project ../SplitCorrect.Api
```

### Connection Refused
Ensure Docker services are running:
```bash
docker compose ps
```

##  Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

##  License

This project is licensed under the MIT License.

## Tech Stack

**Backend:**
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- xUnit, Moq, FluentAssertions

**Frontend:**
- React 18
- TypeScript
- Vite
- Tailwind CSS 3
- Lucide Icons

**DevOps:**
- Docker & Docker Compose
- pgAdmin 4

## Support

For issues and questions, please open an issue on GitHub.

---

Made with using Clean Architecture principles
