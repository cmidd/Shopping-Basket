# Shopping-Basket
A simple shopping basket simulator using .NET 7 and Entity Framework

The following tools are prerequisites to developing the website:

- Visual Studio 2022
- [.Net Framework 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Getting started

- Open the solution in Visual Studio
- Add the correct "Shopping" connectionString in appSettings.development.json
- Run the app

## Using the app

- Add products to your basket
- Go to the Basket page to view your total
- Apply vouchers to adjust the total

## How it works

- A basket is created upon first visit, and the ID stored as a cookie
- API calls are made to modify the basket
- The app uses Entity Framework to communicate with db tables that store the following:
  - Baskets
  - BasketItems
  - Products
  - vouchers

## Vouchers

- Enter "1" for a 10% discount
- Enter "2" for a 50% discount

## Notes

- API endpoints are available via /swagger