# Technical Flows Documentation

# Docker Setup Guide

## Prerequisites
- Docker Desktop installed
- Git installed

## Quick Start

1. Clone the repository:
```bash
git clone https://github.com/your-repo/developer-evaluation.git

### Build the Docker images:
docker-compose build
### Start the containers:
docker-compose up -d
### View application logs:
docker logs developer-evaluation-api

### Database Migrations
- Migrations will run automatically on container startup.

### API Documentation
- Once running, access Swagger documentation at: http://localhost:5000/swagger

## Product Management Flow

### 1. Create Product
- Endpoint: POST /api/products
- Flow:
  1. Request received by ProductsController
  2. Request mapped to CreateProductCommand
  3. Command validated by CreateProductCommandValidator
  4. CreateProductHandler processes command
  5. Product entity created and saved via ProductRepository
  6. Response mapped and returned with success status

### 2. Get Product
- Endpoint: GET /api/products/{id}
- Flow:
  1. Request received with ID parameter
  2. Mapped to GetProductCommand
  3. GetProductHandler retrieves product via repository
  4. Product mapped to response DTO
  5. Returns product details or 404 if not found

### 3. Update Product
- Endpoint: PUT /api/products/{id}
- Flow:
  1. Request with ID and updated data received
  2. Mapped to UpdateProductCommand
  3. UpdateProductHandler validates and processes update
  4. Product updated via repository
  5. Returns updated product details

### 4. Delete Product
- Endpoint: DELETE /api/products/{id}
- Flow:
  1. Delete request with ID received
  2. Mapped to DeleteProductCommand
  3. DeleteProductHandler processes deletion
  4. Product removed via repository
  5. Returns success response

## Sales Management Flow

### 1. Create Sale
- Endpoint: POST /api/sales
- Flow:
  1. Sale creation request received
  2. Mapped to CreateSaleCommand
  3. Validates product quantities (max 20 per item)
  4. Creates sale record with items
  5. Publishes SaleCreatedEvent via EventPublisher
  6. Returns created sale details

### 2. Get Sale
- Endpoint: GET /api/sales/{id}
- Flow:
  1. Request processed by SalesController
  2. Sale retrieved with all related items
  3. Returns complete sale information

### 3. Update Sale
- Endpoint: PUT /api/sales/{id}
- Flow:
  1. Update request processed
  2. Validates updated quantities
  3. Updates sale and items
  4. Publishes SaleUpdatedEvent via EventPublisher
  5. Returns updated sale details

### 4. Delete Sale
- Endpoint: DELETE /api/sales/{id}
- Flow:
  1. Delete request processed
  2. Sale and related items removed
  3. Publishes SaleDeletedEvent via EventPublisher
  4. Returns success confirmation

## Common Features
- All endpoints implement proper error handling.
- Validation occurs at both request and domain levels.
- AutoMapper is used for DTO mappings.
- Mediator pattern implements command/handler pattern.
- Repository pattern manages data access.
- Event Publication:
- In operations related to Sales (Cart), corresponding events (SaleCreatedEvent, SaleUpdatedEvent, SaleDeletedEvent)                 are published via the EventPublisher to allow asynchronous processing and integration with other systems.
- API response formatting and logging are implemented consistently.

# How this development was structured

/Domain/Entities/
- Product.cs - Core product entity with properties and validation
- Sale.cs - Core sale entity with items and business rules
- BaseEntity.cs - Common entity properties and validation logic

/Application/Products/
- CreateProduct/
- GetProduct/
- UpdateProduct/
- DeleteProduct/

/Application/Sales/
- CreateSale/
- GetSale/
- UpdateSale/
- DeleteSale/

/Features/Products/
- ProductsController.cs - RESTful endpoints for product operations
- Request/Response DTOs for each operation

/Features/Sales/
- SalesController.cs - RESTful endpoints for sales management
- Request/Response DTOs for each operation

/ORM/
- DefaultContext.cs - EF Core DbContext
- Repositories/ - Implementation of domain repositories

/INFRASTRUCTURE/
- EventPublisher.cs - Event publishing logic

/tests/Unit/
- Handler unit tests
