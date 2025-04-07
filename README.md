# BookStoreAPI

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET SDK (for development)

### Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/mickaelnambs/BookStoreAPI
   cd BookStoreAPI
   ```

2. Start the containers:
   ```bash
   docker compose up -d
   ```

3. The API will be available at:
   - API Endpoints: http://localhost:5000/api
   - Swagger Documentation: http://localhost:5000/swagger/index.html

## Development

### Project Structure

- **API**: Contains controllers, extension methods, and application configuration
- **Core**: Contains entities, interfaces, and business logic
- **Infrastructure**: Contains data access, repositories, and service implementations

### Seed Data

The application comes with seed data for:
- Books
- Delivery methods
- Admin user (admin@test.com / Pa$$w0rd)

## Payment Integration

The application is integrated with Stripe for payment processing. To test payment functionality:

### Configure Stripe Settings

Add these values to your environment variables or update them in `appsettings.json`:

```json
"StripeSettings": {
  "PublishableKey": "pk_test_YOUR_TEST_KEY",
  "SecretKey": "sk_test_YOUR_TEST_KEY",
  "WhSecret": "whsec_YOUR_WEBHOOK_SECRET"
}
```

### Test Cards

For testing payments in test mode, you can use these Stripe test cards:
- Success: 4242 4242 4242 4242
- Requires Authentication: 4000 0025 0000 3155
- Declined: 4000 0000 0000 0002

### Webhooks

For local webhook testing, you can use the Stripe CLI to forward events to your local environment.