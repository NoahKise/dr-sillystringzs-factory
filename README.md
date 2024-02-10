# Dr. Sillystringz's Factory

#### By Noah Kise

#### A C# web application to allow a fictional factory owner to keep track of their engineers and machines.

## Technologies Used

* C#
* .NET 6 SDK
* Entity Framework Core
* MySQL
* CSS
* Razor Pages
* Media Queries

## Description

This is a simple web application to help a fictional factory owner manage their machines and engineers. Full CRUD functionality is provided for both engineers and machines. The user can manage which machines each engineer is licensed for, and can create/view/update/delete those relationships from both the machine details view and the engineer details view.

## Setup/Installation Requirements

* .NET must be installed. Latest version can be found [here](https://dotnet.microsoft.com/en-us/).
* To run locally on your computer, clone the main branch of this [repository](https://github.com/NoahKise/dr-sillystringzs-factory).
* In your terminal, navigate to the root folder of this project and run `dotnet restore`.
* Open MySQL Workbench. Latest version can be downloaded [here](https://dev.mysql.com/downloads/workbench/).
* Create a new file in the "Factory" directory called appsettings.json. NOTE: If you plan to use this project as a jumping off point for further development, you must ensure that appsettings.json is added to your .gitignore file and committed prior to creating this file.
* In `appsettings.json`, enter the following, replacing `USERNAME` and `PASSWORD` to match the settings of your local MySQL server. Replace `DATABASE-NAME` with whatever you would like to name your database.
  
```
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=DATABASE-NAME;uid=USERNAME;pwd=PASSWORD;"
    }
}
```
* In your terminal, navigate to the "Factory" directory and run `dotnet ef database update` to create a local database schema.
* To view the project in a web browser, navigate to the "Factory" directory and run `dotnet watch run`.

## Known Bugs

* Entering numbers or special characters into the name fields when you are creating an engineer or machine won't break the application, but the font will display a placeholder character and will look sub-optimal.

## License

Code licensed under [GPL](LICENSE.txt)

Any suggestions for ways I can improve this app? Reach out to me at noah@kisefamily.com

Copyright (c) February 9 2024 Noah Kise