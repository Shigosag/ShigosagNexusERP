# Shigosag Nexus ERP - Data Contracts & Service Contracts

## 1. Authentication Service (`IAuthService`)
- `Task<AuthResult> LoginAsync(string username, string password)`
  - Authenticates user against BCrypt hash.
  - On success, issues a signed JWT token valid for 480 minutes.
- `void Logout()`: Invalidates session tokens and resets security principals.

## 2. Token Service (`IJwtTokenService`)
- `string GenerateToken(User user)`: Emits RFC 7519 JSON Web Token containing identity claims.
- `ClaimsPrincipal? ValidateToken(string token)`: Validates signature, audience, issuer, and expiration.

## 3. Notification Engine (`INotificationService`)
- `void Notify(string message, NotificationType type)`: Emits non-blocking notification events consumed by `MainViewModel`.
