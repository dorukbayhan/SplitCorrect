# Security Policy

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |

## Reporting a Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report them via email to the project maintainers.

You should receive a response within 48 hours. If for some reason you do not, please follow up via email to ensure we received your original message.

Please include the requested information listed below (as much as you can provide) to help us better understand the nature and scope of the possible issue:

* Type of issue (e.g. SQL injection, authentication bypass, etc.)
* Full paths of source file(s) related to the manifestation of the issue
* The location of the affected source code (tag/branch/commit or direct URL)
* Any special configuration required to reproduce the issue
* Step-by-step instructions to reproduce the issue
* Proof-of-concept or exploit code (if possible)
* Impact of the issue, including how an attacker might exploit the issue

This information will help us triage your report more quickly.

## Preferred Languages

We prefer all communications to be in English or Turkish.

## Safe Harbor

We support safe harbor for security researchers who:

* Make a good faith effort to avoid privacy violations, destruction of data, and interruption or degradation of our services
* Only interact with accounts you own or with explicit permission of the account holder
* Do not exploit a security issue you discover for any reason
* Report any vulnerability you've discovered promptly
* In general, please treat our users' data with care and respect

We will not pursue legal action against or suspend or ban you from our services for reports that are submitted in good faith.

## Security Best Practices

### For Developers

When contributing to this project:

1. **Never commit secrets**: Use environment variables and `appsettings` files
2. **Validate all inputs**: Both frontend and backend
3. **Use parameterized queries**: EF Core does this by default, but be careful with raw SQL
4. **Keep dependencies updated**: Regularly run `dotnet outdated` and `npm audit`
5. **Follow OWASP guidelines**: Especially for authentication and authorization

### For Users

When deploying this application:

1. **Change default passwords**: Update all passwords in `docker-compose.yml` and `appsettings`
2. **Use HTTPS in production**: Configure SSL certificates
3. **Restrict database access**: Don't expose PostgreSQL port publicly
4. **Regular backups**: Implement backup strategy for database
5. **Update regularly**: Keep .NET, Node.js, and Docker images updated

## Known Security Considerations

### Development Environment

This project's default configuration uses simple passwords (`postgres/postgres`) for **local development only**. 

**DO NOT use these in production!**

### Production Deployment

Before deploying to production:

- [ ] Change all database passwords
- [ ] Configure HTTPS/SSL
- [ ] Set up proper CORS policies
- [ ] Implement rate limiting
- [ ] Add authentication/authorization
- [ ] Configure proper logging (without exposing sensitive data)
- [ ] Set `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Use secret management (Azure Key Vault, AWS Secrets Manager, etc.)

## Disclosure Policy

When we receive a security bug report, we will:

1. Confirm the problem and determine the affected versions
2. Audit code to find any potential similar problems
3. Prepare fixes for all supported versions
4. Release new security fix versions as soon as possible

## Comments on this Policy

If you have suggestions on how this process could be improved, please submit a pull request.
