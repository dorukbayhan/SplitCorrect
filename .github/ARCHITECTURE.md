# SplitCorrect - Project Summary

## Overview
A production-ready Expense Splitter backend built with C# .NET 8 following Clean Architecture principles. This is a Splitwise-like application that allows users to track shared expenses, calculate balances, and suggest optimal settlements.

## Project Statistics
- **Projects**: 5 (.NET projects)
- **Source Files**: 28 core files
- **Test Files**: 4 test classes
- **Tests**: 26 unit tests (100% passing)
- **Lines of Code**: ~2,000+ (estimated)

## Architecture

### Clean Architecture Layers

#### 1. Domain Layer (`SplitCorrect.Domain`)
**Purpose**: Core business logic and domain rules

**Files**:
- `Common/BaseEntity.cs` - Base class for all entities
- `Common/Money.cs` - Value object for monetary amounts with currency
- `Entities/Group.cs` - Expense group aggregate root
- `Entities/Member.cs` - Group member entity
- `Entities/Expense.cs` - Expense entity
- `Entities/ExpenseSplit.cs` - Individual split of an expense
- `Services/BalanceCalculator.cs` - Domain service for balance and settlement calculations

**Key Features**:
- Rich domain models with encapsulated business rules
- Money value object handles currency and rounding
- Balance calculator implements greedy algorithm for minimal settlements
- No external dependencies

#### 2. Application Layer (`SplitCorrect.Application`)
**Purpose**: Use cases and application logic

**Files**:
- `Services/GroupService.cs` - Group management use cases
- `Services/MemberService.cs` - Member management use cases  
- `Services/ExpenseService.cs` - Expense management and balance queries
- `DTOs/GroupDtos.cs` - Group request/response DTOs
- `DTOs/MemberDtos.cs` - Member request/response DTOs
- `DTOs/ExpenseDtos.cs` - Expense, balance, and settlement DTOs
- `Interfaces/IGroupRepository.cs` - Group repository contract
- `Interfaces/IMemberRepository.cs` - Member repository contract
- `Interfaces/IExpenseRepository.cs` - Expense repository contract

**Key Features**:
- Clean separation between domain and external concerns
- DTOs for API communication
- Repository interfaces for data access
- CancellationToken support throughout

#### 3. Infrastructure Layer (`SplitCorrect.Infrastructure`)
**Purpose**: External integrations and data persistence

**Files**:
- `Persistence/SplitCorrectDbContext.cs` - EF Core database context
- `Repositories/GroupRepository.cs` - Group repository implementation
- `Repositories/MemberRepository.cs` - Member repository implementation
- `Repositories/ExpenseRepository.cs` - Expense repository implementation

**Key Features**:
- Entity Framework Core 8.0 with PostgreSQL
- Fluent API configuration for entities
- Owned types for Money value object
- Repository pattern implementation

#### 4. API Layer (`SplitCorrect.Api`)
**Purpose**: HTTP endpoints and dependency injection

**Files**:
- `Controllers/GroupsController.cs` - Group CRUD endpoints
- `Controllers/MembersController.cs` - Member CRUD endpoints
- `Controllers/ExpensesController.cs` - Expense and balance endpoints
- `Program.cs` - Application startup and DI configuration
- `appsettings.json` - Configuration

**Key Features**:
- RESTful API design
- Swagger/OpenAPI documentation
- Dependency injection setup
- CORS configuration
- No business logic in controllers

#### 5. Tests (`SplitCorrect.Tests`)
**Purpose**: Unit tests for core domain logic

**Files**:
- `Domain/MoneyTests.cs` - Money value object tests (10 tests)
- `Domain/ExpenseTests.cs` - Expense entity tests (5 tests)
- `Domain/GroupTests.cs` - Group entity tests (5 tests)
- `Domain/BalanceCalculatorTests.cs` - Balance calculation tests (6 tests)

**Coverage**:
- Money operations (addition, subtraction, multiplication, division)
- Expense creation and equal splitting
- Group member management
- Balance calculation algorithms
- Settlement suggestion optimization

## API Endpoints

### Groups (`/api/groups`)
- `GET /api/groups` - List all groups
- `GET /api/groups/{id}` - Get group details
- `POST /api/groups` - Create new group
- `PUT /api/groups/{id}` - Update group
- `DELETE /api/groups/{id}` - Delete group
- `POST /api/groups/{groupId}/members/{memberId}` - Add member to group
- `DELETE /api/groups/{groupId}/members/{memberId}` - Remove member from group

### Members (`/api/members`)
- `GET /api/members` - List all members
- `GET /api/members/{id}` - Get member details
- `POST /api/members` - Create new member
- `PUT /api/members/{id}` - Update member
- `DELETE /api/members/{id}` - Delete member

### Expenses (`/api/expenses`)
- `GET /api/expenses/{id}` - Get expense details
- `GET /api/expenses/group/{groupId}` - List group expenses
- `POST /api/expenses` - Create expense with equal split
- `DELETE /api/expenses/{id}` - Delete expense
- `GET /api/expenses/group/{groupId}/details` - Get group with balances and settlements

## Key Algorithms

### Equal Split Calculation
```
Split Amount = Total Expense / Number of Participants
Each participant owes: Split Amount
Payer's net balance: Total - Split Amount
```

### Balance Calculation
```
For each member:
  Balance = Sum(Amounts Paid) - Sum(Amounts Owed)
  
Positive balance = owed money (creditor)
Negative balance = owes money (debtor)
```

### Settlement Suggestion (Greedy Algorithm)
```
1. Separate members into creditors (positive) and debtors (negative)
2. Sort creditors descending, debtors ascending
3. Match largest debt with largest credit
4. Create settlement for min(debt, credit)
5. Update remaining balances
6. Repeat until all settled

Result: At most N-1 transactions for N people
```

## Technology Stack

- **.NET 8.0** - Latest LTS version
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 8.0** - ORM
- **Npgsql** - PostgreSQL provider
- **xUnit** - Testing framework
- **Swashbuckle** - Swagger/OpenAPI
- **PostgreSQL** - Database

## Design Patterns & Principles

### SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Extensible through interfaces
- **Liskov Substitution**: Proper use of inheritance
- **Interface Segregation**: Focused repository interfaces
- **Dependency Inversion**: Dependencies on abstractions

### Patterns Used
- **Repository Pattern**: Data access abstraction
- **Domain-Driven Design**: Rich domain models
- **Value Objects**: Money with validation
- **Aggregate Root**: Group manages members and expenses
- **Dependency Injection**: Loose coupling

### Code Quality
✅ No business logic in controllers  
✅ Small, focused methods (typically < 20 lines)  
✅ Descriptive naming  
✅ CancellationToken support for async operations  
✅ Proper exception handling  
✅ Input validation in domain layer  
✅ Comprehensive unit tests  

## Database Schema

### Tables
- **Groups** - Expense groups
- **Members** - Group members
- **GroupMembers** - Many-to-many join table
- **Expenses** - Expense records
- **ExpenseSplits** - Individual expense splits

### Relationships
- Group → Members (many-to-many)
- Group → Expenses (one-to-many, cascade delete)
- Expense → PaidBy Member (many-to-one)
- Expense → Splits (one-to-many, cascade delete)
- ExpenseSplit → Member (many-to-one)

## Setup & Deployment

### Development Setup
1. Clone repository
2. Run `docker-compose up -d` for PostgreSQL
3. Run `dotnet tool restore` for EF tools
4. Create migration: `dotnet ef migrations add InitialCreate`
5. Update database: `dotnet ef database update`
6. Run API: `dotnet run --project src/SplitCorrect.Api`

### Testing
```bash
dotnet test
```

### Build
```bash
dotnet build
```

## Future Enhancements (Beyond MVP)

### Potential Features
- Unequal splits (percentage, exact amounts, shares)
- Multiple currencies with exchange rates
- Expense categories and tags
- Receipt image upload
- Group activity feed
- Email notifications
- User authentication & authorization
- Simplify debts optimization
- Recurring expenses
- Export to CSV/PDF
- Mobile app integration

### Technical Improvements
- CQRS pattern for read/write separation
- Event sourcing for audit trail
- Redis caching for performance
- GraphQL API option
- Background job processing
- Rate limiting
- API versioning
- Comprehensive integration tests
- Load testing

## License
MIT

## Author
Built with Clean Architecture principles and SOLID design patterns.
