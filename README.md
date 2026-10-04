# Claims Management API

Backend API for an insurance claims management system built with ASP.NET Core.

The system supports Claimant and Claims Officer workflows including policy management, claim submission, review, assessment, settlement, and dashboard reporting.

## Overview

The backend follows a layered architecture with separation between:

- API / Controllers
- Application Services
- Domain Models and Business Rules
- Repositories
- Unit of Work
- Entity Framework Core persistence
- Authentication and Authorization

### Request Flow

```text
HTTP Request
     |
     v
Controller
     |
     v
Application Service
     |
     v
Repository / Unit of Work
     |
     v
Entity Framework Core
     |
     v
PostgreSQL
```

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core | REST API |
| .NET | Application runtime |
| Entity Framework Core | ORM / database access |
| PostgreSQL | Database |
| JWT Bearer | Authentication |
| ASP.NET Core Authorization | Role-based access control |
| Swagger / OpenAPI | API documentation |
| xUnit | Unit testing |

## Project Structure

```text
Claims.Api/
│
├── Exceptions/
│   └── NotFoundException.cs
│
├── Infrastructure/
│   └── Persistence/
│       ├── AppDbContext.cs
│       └── UnitOfWork.cs
│
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs
│
├── Migrations/
│
├── Modules/
│   └── Claims/
│       ├── Controllers/
│       │   ├── ClaimantsController.cs
│       │   ├── ClaimsController.cs
│       │   ├── ClaimsOfficerController.cs
│       │   └── PolicyController.cs
│       │
│       ├── DTOs/
│       │
│       ├── Entities/
│       │
│       ├── Enum/
│       │
│       ├── Interface/
│       │   ├── Repository/
│       │   └── Service/
│       │
│       ├── Repositories/
│       │
│       └── Services/
│
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

The exact folder structure may vary slightly depending on the implementation, but responsibilities are kept separated by layer.

## Authentication

The API uses JWT Bearer Authentication.

Authenticated requests must include:

```http
Authorization: Bearer <token>
```

### Roles

#### Claimant

Claimants can:

- View their policies
- Create policies
- Submit claims
- View their claims
- Track claim status

#### Claims Officer

Claims Officers can:

- View claims assigned to them
- Review claims
- Request additional information
- Perform assessments
- Process settlement-related workflows

Role-based access is enforced using ASP.NET Core authorization.

Example:

```csharp
[Authorize(Roles = "Claimant")]
```

## Development Authentication

For local development, the API provides a development authentication endpoint for generating a Claimant JWT.

```http
GET /api/dev-auth/token/claimant?userId={userId}
```

The returned token can be used in Postman or the Angular frontend:

```http
Authorization: Bearer <token>
```

> The development authentication endpoint is intended for local development/testing only and is not a production authentication mechanism.

## Database

The application uses PostgreSQL with Entity Framework Core.

Configure the database connection in `appsettings.Development.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=claimsdb;Username=postgres;Password=your_password"
  }
}
```

Do not commit real database credentials or production secrets.

## Entity Framework Core Migrations

Apply the database schema with:

```bash
dotnet ef database update
```

To create a new migration after changing the entity model:

```bash
dotnet ef migrations add <MigrationName>
```

Then apply it:

```bash
dotnet ef database update
```

## Running the API

### 1. Clone the repository

```bash
git clone <repository-url>
cd claims-backend
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure PostgreSQL

Update the connection string in `appsettings.Development.json`.

### 4. Apply migrations

```bash
dotnet ef database update
```

### 5. Run the API

```bash
dotnet run
```

The API is available at:

```text
http://localhost:5058
```

Swagger:

```text
http://localhost:5058/swagger
```

## API Endpoints

### Claims

#### Get Claims

```http
GET /api/claims
```

Returns claims available to the authenticated user.

#### Get Claim

```http
GET /api/claims/{claimId}
```

#### Create Claim

```http
POST /api/claims
```

Example request:

```json
{
  "claimNumber": "PSA-000003",
  "policyId": "1a4d655f-8498-43a3-a803-4e45a4df19aa",
  "incidentDate": "2026-09-30",
  "incidentDescription": "Rear-end collision",
  "estimatedLiability": 34500
}
```

The Claimant identity is obtained from the authenticated JWT rather than accepting a `claimantId` from the client.

## Policies

### Get Claimant Policies

```http
GET /api/policies/{claimantId}
```

Example response:

```json
[
  {
    "id": "1a4d655f-8498-43a3-a803-4e45a4df19aa",
    "claimantId": "48fa1912-dceb-49e1-8a5b-4e036fcfd798",
    "policyNumber": "POL-MY-MTR-0003",
    "policyType": "Motor",
    "market": "Malaysia"
  }
]
```

### Get Policy

```http
GET /api/policies/{id}
```

### Create Policy

```http
POST /api/policies
```

Example request:

```json
{
  "policyNumber": "POL-MY-MTR-0003",
  "policyType": "Motor",
  "market": "Malaysia"
}
```

The Claimant ID is derived from the authenticated JWT.

## Claim Assessment

An assessment can be created once the claim reaches an allowed assessment state.

```http
POST /api/claims/{claimId}/assessment
```

Example request:

```json
{
  "assessmentNotes": "Damage is consistent with reported incident.",
  "estimatedLoss": 34500
}
```

The backend validates the claim status before allowing the assessment operation.

## Claim Status Workflow

The claim lifecycle includes states such as:

```text
Submitted
    |
    v
UnderReview
    |
    +----> Rejected
    |
    v
PendingAssessment
    |
    v
Approved
    |
    v
PendingSettlement
    |
    v
Settled
```

Status transitions are controlled by backend business rules rather than allowing the client to arbitrarily set a claim status.

## Dashboard

The dashboard provides a summary of claim activity and operational workload.

```http
GET /api/claims/dashboard
```

Example response:

```json
{
  "summary": {
    "totalClaims": 3,
    "submitted": 0,
    "underReview": 1,
    "pendingSettlement": 0,
    "approved": 0,
    "rejected": 2
  },
  "financial": {
    "totalEstimatedLoss": 64500,
    "totalApprovedAmount": 0,
    "totalSettlementAmount": 10000,
    "outstandingLiability": -10000
  },
  "officerWorkload": {
    "unassignedClaims": 1,
    "myActiveClaims": 0,
    "pendingAssessment": 0,
    "pendingSettlement": 0
  },
  "recentClaims": []
}
```

## Error Handling

The API uses standard HTTP status codes together with business validation responses.

| Status | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | Resource created successfully |
| `400 Bad Request` | Invalid request |
| `401 Unauthorized` | Missing or invalid authentication |
| `403 Forbidden` | Authenticated user does not have permission |
| `404 Not Found` | Resource does not exist |
| `409 Conflict` | Business rule or state conflict |
| `500 Internal Server Error` | Unexpected server error |

Example:

```json
{
  "message": "Claim Status Must be UnderReview"
}
```

## Business Rules

Important business rules are enforced by the backend.

Examples:

- Claimants can only access their own claims.
- Claimants can only create policies for their own account.
- Claim status transitions are validated.
- Assessment creation is only allowed when the claim is in an appropriate state.
- Protected endpoints require a valid JWT.
- Role-specific endpoints require the correct role.
- Identity information should be taken from authenticated claims rather than trusted from the request body.

Example:

```csharp
var claimantIdValue =
    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
```

## Repository and Unit of Work

Persistence is abstracted through repository and Unit of Work layers.

```text
Controller
    |
    v
Application Service
    |
    v
Repository
    |
    v
Unit of Work
    |
    v
Entity Framework Core
    |
    v
PostgreSQL
```

This keeps database concerns separated from business logic and improves maintainability and testability.

## API Testing

The API can be tested using:

- Swagger
- Postman
- Angular frontend

Typical local workflow:

```text
1. Generate development Claimant token
          |
          v
2. Store/use JWT
          |
          v
3. Send Authorization: Bearer <token>
          |
          v
4. View policies
          |
          v
5. Create claim
          |
          v
6. Review claim
          |
          v
7. Perform assessment
          |
          v
8. Process settlement
```

## Frontend Integration

The Angular frontend communicates with the API over HTTP.

```text
Angular
   |
   | Authorization: Bearer <JWT>
   v
Claims API
   |
   v
Application Services
   |
   v
PostgreSQL
```

The frontend does not access the database directly.

Authentication tokens are attached to API requests through the Angular HTTP authentication/interceptor layer.

## Configuration and Security

Do not commit sensitive production configuration.

The following should be stored securely:

- Database passwords
- JWT signing keys
- API keys
- Production credentials
- Other application secrets

For production deployments, use environment variables or a dedicated secret-management solution.

## Future Improvements

Potential improvements include:

- Production identity provider integration
- Refresh token support
- Centralized exception handling
- Structured logging
- API versioning
- Pagination and filtering
- Automated integration tests
- Docker support
- CI/CD pipeline
- Production secret management
- Audit logging
- More granular authorization policies

## License

This project is intended for demonstration and development purposes.
