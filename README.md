# CheesyMart

CheesyMart is a sample application with Angular front end and .NET 8 backend

## Installation

Change the Default connection string in the CheesyMart.Data project to a blank local Db / sql server and apply migrations.

```bash
dotnet ef database update
```

Run all the unit tests.

Change the Default connection string in the CheesyMart.API and launch it in Rider / Visual studio.

Navigate to \CheesyMart\CheesyMart.Portal\cheesy-mart and

```bash
npm install

npm run start
```

## App usage

Landing page displays cheese selection -Initially empty. Add some cheese from www.cheese.com (download images from there)

To view cheese product and edit click edit.

To add product to cart click on the cart icon in cheese selection page and to remove toggle it again.
To view cart click on the cart link in the banner, enter quantity and click calculate.

## Logging

Install serilog and navigate to the default port for example http://localhost:5341/#/events?range=1d

## Client build

If you need to change the API input and output models just run CakeBuild.cmd in the root folder
Note: Nswag newer version has an issue and you might need to make a change in the cheesy-client.service.ts to override the message string.
