# WealthSync

![home](https://github.com/user-attachments/assets/b8b4e4b0-37da-4808-a45f-54224a64dfd8)
![dashboard](https://github.com/user-attachments/assets/bbbee486-3b91-4798-976f-6bd34d560f50)
<img width="1680" alt="Screenshot 2025-04-29 at 08 56 41" src="https://github.com/user-attachments/assets/ce849175-ca4e-460b-8316-487670840a3c" />


**WealthSync** is a personal finance management application designed to help users track their budgets, savings, and stock investments. It features a secure backend built with ASP.NET Core, a responsive Angular frontend, and integrates with Financial Modeling Prep (FMP) for real-time stock data, including adjusted dividend yields. The app supports user authentication with JWT, budget management, and stock portfolio tracking with a limit of 3 stocks per user.

## Features

- **User Authentication**: Secure login, registration, and password reset using JWT and ASP.NET Core Identity.
- **Budget Management**: Create, view, and manage budgets with a clean Angular interface.
- **Stock Portfolio**: Add, view, and delete stocks (max 3 per user), with real-time prices and dividend yields from FMP.
- **Dividend Yield Calculation**: Uses adjusted dividends from FMP’s `/dividend` endpoint to account for stock splits.
- **Responsive UI**: Standalone Angular components with routing and authentication guards.
- **Data Persistence**: Entity Framework Core with Postgres for storing users, budgets, and stocks.

## Tech Stack

### Backend
- **ASP.NET Core 8**: RESTful API with controllers for authentication, stocks, and more.
- **Entity Framework Core**: ORM for database operations.
- **ASP.NET Core Identity**: User management and JWT authentication.
- **Financial Modeling Prep (FMP)**: External API for stock prices and dividend data.
- **IMemoryCache**: Caches stock data for 1 hour to reduce API calls.

### Frontend
- **Angular 17**: Standalone components, reactive forms, and HTTP client for API integration.
- **TypeScript**: Strongly-typed JavaScript for maintainability.
- **RxJS**: Observables for async operations.

### Database
- **Posgres**: Stores users, stocks, budgets, and password reset tokens.

## Prerequisites

- **.NET 8 SDK**: [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 20+ & npm**: [Download](https://nodejs.org/)
- **SQL Server**: Local or remote instance (e.g., SQL Server Express)
- **FMP API Key**: Sign up at [Financial Modeling Prep](https://financialmodelingprep.com/) (free tier: 250 requests/day)


## Setup Instructions

### Backend
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/wealthsync.git
   cd WealthSync/WealthSync.Backend
2. **Configure**:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WealthSync;Trusted_Connection=True;"
  },
  "Fmp": {
    "ApiKey": "YOUR_FMP_API_KEY"
  },
  "FrontendUrl": "http://localhost:5000",
  "Jwt": {
    "Issuer": "https://localhost:7001",
    "Audience": "http://localhost:5000",
    "Key": "your-secure-jwt-key-here"
  }
}
```
3. **Apply Migrations**:
  ```bash
  dotnet ef database update
  ```
### Frontend
1. **Install Dependencies**
```bash
npm install
```
2. ** Run the Frontend ** 
```bash
ng serve
```
