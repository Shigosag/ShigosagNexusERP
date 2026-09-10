# Shigosag Nexus ERP - Architecture & System Design

## 1. Architectural Pattern
- **Core Pattern:** MVVM (Model-View-ViewModel) backed by Microsoft Dependency Injection (`Microsoft.Extensions.DependencyInjection`) and CommunityToolkit.Mvvm.
- **Presentation Layer:** WPF with XAML data-binding and hardware-accelerated Bezier Path geometry.
- **Domain & Persistence:** Entity Framework Core 8.0, configured to run against PostgreSQL / cloud PostgreSQL instances (e.g. Neon, Supabase) with zero-configuration fallback to encrypted SQLite.

## 2. Security & Authentication Architecture
- **JWT Pipeline:** Implements HMAC-SHA256 token issuance via `JwtTokenService` utilizing `System.IdentityModel.Tokens.Jwt`.
- **Password Security:** Multi-round BCrypt salting and verification (`BCrypt.Net-Next`).
- **Claim Extraction:** User roles (`Admin`, `Manager`, `User`) and identifiers are bound to bearer claims and validated on every operational transaction.

## 3. UI/UX Interaction Standards
- Native system modal interruptions (`MessageBox.Show`) are isolated strictly to application boot sequence errors.
- Real-time application event notifications are handled via an observable `INotificationService` toast system with data-driven status color palettes.
