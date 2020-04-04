# Reverse My Budget
This web application takes past user transactions, categorises them and produces a budget based on this data. 

## Initialise
1. Open a command prompt in the ClientApp directory and run: yarn install (npm has proven to be unreliable in this project, so use yarn instead)
2. If you want to use something other than SQL Express localdb for the database, then set the DefaultConnection in: appsettings.Development.json
2. In Visual Studio, open Package Console Manager and run: Update-Database

## To run in Development mode
1. Open a command prompt in the ClientApp directory and run: yarn start
2. Open a command prompt in the VulnDotNetCore directory (where the .csproj file is) and run: dotnet watch run
3. In a browser go to: localhost:5000

## License
This work is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. http://creativecommons.org/licenses/by-nc-sa/4.0/