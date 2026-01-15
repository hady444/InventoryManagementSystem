# Inventory Tracking System

## Overview

This system manages **warehouses, products, and stock transactions**. It tracks stock levels accurately while supporting operations like **adding, editing, and reversing transactions**.  

It is implemented with **ASP.NET Core**, **EF Core**, and a layered architecture: **Controller → Service → Repository → Database**.  

---

## Key Features

1. **Warehouse Management**
   - Add, edit, delete warehouses
   - Unique warehouse names enforced
   - Soft delete supported

2. **Product Management**
   - Add, edit, delete products
   - Each product has a `Name`, `SKU` (unique), and optional `Description`
   - Soft delete supported

3. **Stock Transactions**
   - Add, edit, delete (reverse) transactions
   - Supports `In` and `Out` transaction types
   - Positive quantity for IN, negative for OUT
   - Prevents stock from going below zero
   - Maintains stock consistency across all warehouses
   - Transaction edits update **transaction date** to preserve chronological consistency

4. **Stock Summary**
   - Summary shows totals for products, warehouses, and current stock
   - Quick stats for total IN/OUT and available stock
   - Supports warehouse- and product-specific filtering

5. **Interactive UI**
   - +/- buttons next to stock quantity in transaction form
   - Filters via dropdowns (warehouse/product) without page reload
   - Soft delete confirmation modal with background blur
   - Pagination with query string preservation

---

## Business Rules / Decisions

- **Soft Delete**  
  - Transactions, products, and warehouses are soft-deleted
  - Keeps historical data intact and allows reversals

- **Transaction Edits**
  - Quantity or type changes update the transaction date

- **Reverse/Delete Transaction**
  - Stock is adjusted by reversing the transaction
  - Ensures warehouse stock never drops below zero
  - Prevents invalid historical stock levels

- **Concurrent Edits**
  - System does not lock UI; two users can view stock simultaneously  
  - Server-side validation prevents negative stock
  - UI shows current stock dynamically to reduce conflicts

- **Validation**
  - Quantity must be > 0
  - Product/Warehouse IDs cannot be changed after creation
  - Stock calculation ensures consistent totals

---

## Technologies Used

- **ASP.NET Core MVC**  
- **Entity Framework Core** (In-memory / SQL Server)  
- **AutoMapper** for VM ↔ Model mapping  
- **jQuery** for dynamic UI updates  
- **Bootstrap 5** for styling

---

## How to Use

1. **Add Warehouses** via Warehouse page.
2. **Add Products** via Product page.
3. **Create Transactions**  
   - Select product and warehouse  
   - Specify transaction type (IN/OUT) and quantity  
   - Stock is validated automatically
4. **Edit Transactions**  
   - Changes update stock and transaction date
5. **Reverse Transactions**  
   - Reverse adjusts stock and soft-deletes the transaction
6. **Filter and Search**  
   - Use dropdown filters for warehouse/product
   - Pagination preserves filters
7. **Summary**  
   - Quick view of total stock, transactions, and warehouse stats
