# API Documentation

This document describes the available API endpoints, including request and response structures, status codes, authentication requirements, and validation rules.

## Base URL

```
https://localhost:8081/api/v1
```

## Authentication

Currently, the API does not enforce authentication.

## POST `/trades`

Creates a new trade entry.

### Request Body

```json
{
  "side": "Buy",
  "quantity": 10,
  "price": 123.45
}
```

### Validation Rules

* `side`: Required. Must be `Buy` or `Sell`
* `quantity`: Required. Must be an integer > 0
* `price`: Required. Must be a decimal > 0

### Response Body

```json
{
  "id": "94f2e917-ad4d-4f02-b799-d86775b6b554",
  "side": "Buy",
  "quantity": 10,
  "price": 123.45,
  "totalAmount": 1234.5,
  "executedAt": "2025-05-02T18:12:34.2440597Z"
}
```

### Status Codes

* `201 Created`: Trade successfully created
* `400 Bad Request`: Validation error
* `500 Internal Server Error`: Unexpected server error

## GET `/trades`

Retrieves a paginated list of trades.

### Query Parameters

* `pageNumber` (integer): Optional, defaults to 1
* `pageSize` (integer): Optional, defaults to 10. Maximum: 50

### Example

```
GET /trades?pageNumber=1&pageSize=10
```

### Response Body

```json
{
  "items": [
    {
      "id": "94f2e917-ad4d-4f02-b799-d86775b6b554",
      "side": "Buy",
      "quantity": 10,
      "price": 123.45,
      "totalAmount": 1234.5,
      "executedAt": "2025-05-02T18:12:34.244059Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 12,
  "totalPages": 2,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### Status Codes

* `200 OK`: Success
* `400 Bad Request`: Invalid pagination parameters
* `500 Internal Server Error`: Unexpected server error

## GET `/trades/{id}`

Retrieves a single trade by its unique identifier.

### Path Parameters

* `id` (GUID): Required. Must be a valid UUID.

### Example

```
GET /trades/2e1ac66f-49f4-4ea1-94ba-a817c5b725e2
```

### Response Body

```json
{
  "id": "2e1ac66f-49f4-4ea1-94ba-a817c5b725e2",
  "side": "Buy",
  "quantity": 100,
  "price": 75.5,
  "totalAmount": 7550,
  "executedAt": "2025-05-02T16:24:13.293787Z"
}
```

### Status Codes

* `200 OK`: Trade found
* `404 Not Found`: Trade with specified ID not found
* `400 Bad Request`: Invalid ID format
* `500 Internal Server Error`: Unexpected server error

## Error Handling

All error responses conform to [RFC 7807 - Problem Details for HTTP APIs](https://www.rfc-editor.org/rfc/rfc7807.html), a standard extension to [RFC 9110](https://www.rfc-editor.org/rfc/rfc9110.html).

The response uses the media type `application/problem+json` and has the following structure:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Resource not found",
  "status": 404,
  "detail": "Trade with ID 2e1ac66f-49f4-4ea1-94ba-a817c5b725e3 not found",
  "instance": "/api/v1/trades/2e1ac66f-49f4-4ea1-94ba-a817c5b725e3"
}
```

### Fields

- `type`: A URI reference that identifies the problem type.
- `title`: A short, human-readable summary of the problem.
- `status`: The HTTP status code.
- `detail`: A human-readable explanation specific to this occurrence of the problem.
- `instance`: A URI reference that identifies the specific occurrence.
- `errors`: An object containing field-specific validation messages (optional

## Notes

* All timestamps are in ISO 8601 format (UTC)
* The `totalAmount` is calculated as `quantity * price`
* API versioning is implemented using URL path segments (`/api/v1/`)
* JSON responses serialize enums as strings for better readability
