# WorkShop

Simple ERP application for WorkShops

## Features

This application tries to help people to have control over:

* Providers
* Provider Invoices
* Work Orders
* Inventory
* Clients

## Architecture

This application was build using the following frameworks:

* .Net Core 3.1
* SteelToe 3
* Blazor Server
* Entity Framework

Using MySQL 8 as RDBMS

## Build

```
dotnet restore
dotnet build
```

### Migrations

This application uses EF migrations since is still at development phase, in future a different tool to manage migrations will be considered.

In development EF tool from .net core can be used:

```
dotnet ef database update
```

