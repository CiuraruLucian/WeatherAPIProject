# Weather API

## Overview

This project is a simple ASP.NET Core Web API that retrieves weather information from the Visual Crossing Weather API and caches responses using Redis.

Instead of storing weather data locally, the application fetches weather data from a third-party service and stores the result in Redis for 12 hours. This reduces the number of external API requests and improves performance.

## Features

* Get weather information by city name
* Uses the Visual Crossing Weather API
* Redis caching with a 12-hour expiration time
* Rate limiting to prevent abuse
* Environment-based configuration
* Basic error handling

## Technologies Used

* ASP.NET Core Web API
* Visual Crossing Weather API
* Redis
* StackExchange.Redis
* HttpClient
* ASP.NET Rate Limiting

## API Endpoint

### Get Weather by City

```http
GET /api/weather/{city}
```

Example:

```http
GET /api/weather/london
```

### Response

Returns weather data from Visual Crossing.

## How It Works

1. User sends a request with a city name.
2. The application checks Redis for cached weather data.
3. If cached data exists, it is returned immediately.
4. If no cached data exists:

   * The application calls the Visual Crossing API.
   * The response is stored in Redis with a 12-hour expiration.
   * The weather data is returned to the user.

## Configuration

The application uses values stored in appsettings.json.

Required settings:

{
  "VisualCrossing": {
    "ApiKey": "YOUR_API_KEY"
  },
  "Redis": {
    "ConnectionString": "YOUR_REDIS_CONNECTION_STRING"
  }
}
Visual Crossing API Key

Create a free account with Visual Crossing and generate an API key. Replace YOUR_API_KEY with your own key.

Redis Connection String

If Redis is running locally, the default connection string is:

localhost:6379

Update this value if Redis is hosted elsewhere.

## Running the Project

1. Clone the repository.
2. Install and start Redis.
3. Add your Visual Crossing API key.
4. Run the application:

```bash
dotnet run
```

5. Test the endpoint using a browser, Postman, or Swagger.

## Error Handling

The API returns a 500 status code if:

* The external weather service is unavailable.
* An unexpected error occurs during processing.

## Author

Gheorghe Lucian Ciuraru
