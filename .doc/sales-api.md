[Back to README](../README.md)

# Sales API

## Overview

The Sales API provides endpoints for managing sales records, including creating sales, updating item quantities, and retrieving sales with filtering and pagination capabilities.

## Business Rules

### Quantity-Based Discounts
* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

## Endpoints

### List Sales

```http
GET /api/sales
```

Retrieves a paginated list of sales with optional filtering and sorting.

#### Query Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| pageNumber | integer | Page number (default: 1) |
| pageSize | integer | Number of items per page (default: 10) |
| searchTerm | string | Optional search term to filter by sale number, customer name, or branch name |
| sortBy | string | Field to sort by (`date`, `number`, `customer`, `amount`) |
| sortDescending | boolean | Sort direction (default: false) |
| filters.startDate | string | Filter sales from this date (ISO 8601 format) |
| filters.endDate | string | Filter sales until this date (ISO 8601 format) |
| filters.minAmount | number | Filter sales with total amount greater than or equal to this value |
| filters.maxAmount | number | Filter sales with total amount less than or equal to this value |
| filters.isCancelled | boolean | Filter by cancellation status |

#### Response

```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "saleNumber": "SALE001",
      "saleDate": "2024-01-01T12:00:00Z",
      "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "customerName": "John Doe",
      "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "branchName": "Main Branch",
      "totalAmount": 100.00,
      "isCancelled": false,
      "items": [
        {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "productName": "Product A",
          "quantity": 5,
          "unitPrice": 20.00,
          "discount": 10.00,
          "totalAmount": 90.00,
          "isCancelled": false
        }
      ]
    }
  ],
  "pageNumber": 1,
  "totalPages": 10,
  "totalCount": 100
}
```

### Get Sale by ID

```http
GET /api/sales/{id}
```

Retrieves a specific sale by its ID.

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| id | guid | The unique identifier of the sale |

#### Response

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "saleNumber": "SALE001",
  "saleDate": "2024-01-01T12:00:00Z",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "John Doe",
  "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchName": "Main Branch",
  "totalAmount": 100.00,
  "isCancelled": false,
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "productName": "Product A",
      "quantity": 5,
      "unitPrice": 20.00,
      "discount": 10.00,
      "totalAmount": 90.00,
      "isCancelled": false
    }
  ]
}
```

### Create Sale

```http
POST /api/sales
```

Creates a new sale.

#### Request Body

```json
{
  "customerName": "John Doe",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchName": "Main Branch",
  "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "productName": "Product A",
      "quantity": 5,
      "unitPrice": 20.00
    }
  ]
}
```

#### Response

```
201 Created
Location: /api/sales/{id}
```

Returns the ID of the created sale in the response body.

### Update Item Quantity

```http
PUT /api/sales/{saleNumber}/items/{productId}/quantity
```

Updates the quantity of a specific item in a sale.

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| saleNumber | string | The sale number |
| productId | guid | The product ID |

#### Request Body

```json
{
  "newQuantity": 10
}
```

#### Response

```
204 No Content
```

### Cancel Sale

```http
POST /api/sales/{saleNumber}/cancel
```

Cancels a specific sale.

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| saleNumber | string | The sale number to cancel |

#### Response

```
204 No Content
```

## Domain Events

The following domain events are published:

* `SaleCreatedEvent`: When a new sale is created
* `SaleModifiedEvent`: When a sale is modified
* `SaleCancelledEvent`: When a sale is cancelled
* `SaleItemModifiedEvent`: When a sale item's quantity is updated
* `SaleItemCancelledEvent`: When a sale item is cancelled

## Error Responses

In addition to the standard API error responses, the Sales API may return the following specific errors:

### 400 Bad Request

```json
{
  "type": "ValidationError",
  "error": "Invalid quantity",
  "detail": "The quantity must be between 1 and 20"
}
```

### 404 Not Found

```json
{
  "type": "ResourceNotFound",
  "error": "Sale not found",
  "detail": "The sale with number SALE001 does not exist"
}
```

### 400 Bad Request

```json
{
  "type": "BusinessRuleViolation",
  "error": "Invalid operation",
  "detail": "Cannot update items in a cancelled sale"
}
```

<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./products-api.md">Previous: Products API</a>
  <a href="./carts-api.md">Next: Carts API</a>
</div> 