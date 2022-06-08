# Weather
C# backend and React frontend simple weather forecast by city name web application

How to run the app:

1. Please have Visual Studio 2019 Community and node npm installed


2. Visual Studio 2019 Developer PowerShell command: install for Entity Framework and JsonConverter

C:\Weather>dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.1

C:\Weather>dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.1

C:\Weather>dotnet tool install --global dotnet-ef --version 3.1.1

C:\Weather>dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.1.1


3. Please ensure SQL Server Express 2016 LocalB is installed with Visual Studio 2019


4. You might not need this command but if you want to rebuild the database schema then delete the Migrations folder:

C:\Weather>dotnet ef migrations add Initial


5. Please use this command to create the database named Skys with sample data

C:\Weather>dotnet ef database update


6. If you need to reset the database, please use this command and then run command in step 5 again

C:\Weather>dotnet ef database drop --force


7. Command to install React related libraries

C:\Weather\ClientApp> npm install


8. Please use Visual Studio 2019 to open the solution file at: C:\Weather>Weather.sln


9. In Visual Studio 2019, click Debug and then select Start Debugging


10. The page https://localhost:44326/ will automatically show in a web browser window


11. Type a city name into the City input box and a number into the Days input box
