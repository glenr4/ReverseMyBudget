# Reverse My Budget
A web app that reverse engineers your budget from past bank transactions. 

Most budgeting exercises go the other way by nominating fixed amounts into each category but this never seems to line up with your actual expenses. There are weekly, fortnightly, monthly, quarterly, annual and ad-hoc transactions that occur in you bank account and it is quite difficult to align this with your pay and budget. Therefore, starting off with where you are at ie your actual expenses and working back from there to create a budget, makes much more sense.

Once you have your budget, you can identify where you are wasting money and cut back on those expenses. Then continue to upload bank transactions over time to see how the changes you have made are tracking.

## Purpose
To be useful for anyone who has been struggling to get a hold on their finances. Monitoring with a manually constructed budget is a painful process that takes a lot of effort. And if it is annoying to maintain, then it is easy to avoid doing it, which makes the whole budgeting exercise is a big, frustrating waste of time.

There are other budgeting apps out there but of the ones I have looked at in the past, they require you to give them your bank logon details, which gives them full control over your accounts! This app does not require bank logon credentials, just download transactions as a CSV file and then upload into ReverseMyBudget.

## Current Status
This is a work in progress. Currently CSV files from National Australia Bank (NAB) can be uploaded, duplicates ignored and displayed in a table. The user interface is still pretty basic and needs a lot of work. It currently uses the UI of the Identity Server starter project.

Authentication is via Identity Server: https://identityserver4.readthedocs.io/en/latest/. However, this will be changed in future updates to Azure B2C. This will outsource the auth management to Azure and result in a more secure application with much less maintenance required.

## Initialise
1. Install Entity Framework Core CLI Tools: `dotnet tool install --global dotnet-ef`
1. Open a command prompt in the ReverseMyBudget/ClientApp directory and run: `yarn install` (npm has proven to be unreliable in this project, so use yarn instead)
1. If you want to use something other than SQL Express localdb for the database, then set the DefaultConnection in: appsettings.Development.json
1. In ReverseMyBudget folder run: 

    `dotnet ef database update --context ReverseMyBudgetDbContext`

    `dotnet ef database update --context ApplicationUserDbContext`

## To run in Development mode
1. Open a command prompt in the ReverseMyBudget/ClientApp directory and run: `yarn start`
1. Open a command prompt in the ReverseMyBudget directory (where the .csproj file is) and run: `dotnet watch run`
	(or run from Visual Studio)
1. In a browser go to: https://localhost:5001

## License
This work is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. http://creativecommons.org/licenses/by-nc-sa/4.0/