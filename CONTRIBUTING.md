# Contributing to SplitCorrect

Thank you for your interest in contributing to SplitCorrect! We welcome contributions from everyone.

##  Getting Started

### 1. Fork and Clone
```bash
git clone https://github.com/<your-username>/SplitCorrect.git
cd SplitCorrect
```

### 2. Setup Development Environment
```bash
# Windows
setup.bat

# Linux/macOS
chmod +x setup.sh
./setup.sh
```

### 3. Create a Branch
```bash
git checkout -b feature/your-feature-name
```

## Development Workflow

### Making Changes

1. Make your changes in the appropriate layer (Domain, Application, Infrastructure, API)
2. Write or update tests
3. Run tests locally
4. Update documentation if needed

### Running Tests
```bash
cd src/SplitCorrect.Tests
dotnet test
```

### Database Migrations (if needed)
```bash
cd src/SplitCorrect.Infrastructure
dotnet ef migrations add YourMigrationName --startup-project ../SplitCorrect.Api
dotnet ef database update --startup-project ../SplitCorrect.Api
```

##  Coding Standards

### C# Backend
- Follow Clean Architecture principles
- Use async/await pattern
- Enable nullable reference types
- Write unit tests for business logic
- Use meaningful variable names

### TypeScript Frontend
- Use functional components with hooks
- Define proper TypeScript interfaces
- Use Tailwind CSS for styling
- Handle errors gracefully

### Commit Messages
Use [Conventional Commits](https://www.conventionalcommits.org/):

```
type(scope): description

[optional body]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting)
- `refactor`: Code refactoring
- `test`: Test additions/updates
- `chore`: Build process or tooling changes

**Examples:**
```
feat(expense): add category support
fix(balance): correct multi-currency calculation
docs(readme): update installation steps
test(group): add group deletion tests
```

## Testing Requirements

- All new features must include tests
- Maintain or improve code coverage
- All tests must pass before submitting PR

##  Submitting Changes

### 1. Push Your Branch
```bash
git push origin feature/your-feature-name
```

### 2. Create Pull Request

Use the PR template and include:
- Clear description of changes
- Related issue number
- Test results
- Screenshots (for UI changes)

### 3. Code Review

- Address review feedback
- Keep PR focused and manageable
- Respond to comments promptly

##  Resources

- [Project Architecture](.github/ARCHITECTURE.md) - Technical documentation
- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [EF Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [React TypeScript Guide](https://react-typescript-cheatsheet.netlify.app/)

##  Reporting Bugs

Use the [Bug Report Template](.github/ISSUE_TEMPLATE/bug_report.yml) and include:
- Clear description
- Steps to reproduce
- Expected vs actual behavior
- Environment details
- Error logs/screenshots

##  Feature Requests

Use the [Feature Request Template](.github/ISSUE_TEMPLATE/feature_request.yml) and describe:
- The problem it solves
- Proposed solution
- Alternative approaches considered

## Questions?

- Open a GitHub Discussion
- Comment on existing issues
- Review our documentation

---

Thank you for contributing! 🎉
