<h1 align="center">
    OnlineStore - ASP.NET Core
</h1>

<p align="center">
  <img alt="GitHub language count" src="https://img.shields.io/github/languages/count/JV-Amorim/OnlineStore-ASP.NET-Core">

  <img alt="Repository size" src="https://img.shields.io/github/repo-size/JV-Amorim/OnlineStore-ASP.NET-Core">
  
  <a href="https://github.com/JV-Amorim/OnlineStore-ASP.NET-Core/commits/master">
    <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/JV-Amorim/OnlineStore-ASP.NET-Core">
  </a>

  <img alt="License" src="https://img.shields.io/badge/license-MIT-brightgreen">
</p>

<p align="center">
  <a href="#project">Project</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#tools">Tools</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#how-to-use">How To Use</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#how-to-contribute">How to Contribute</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#license">License</a>
</p>

<br>

## Project

Online store created with ASP.NET Core.

## Tools

- [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/) - .NET Core is an open-source, general-purpose development framework for building cross-platform apps.
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) - ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected apps.
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - EF Core can serve as an object-relational mapper (O/RM), enabling .NET developers to work with a database using .NET objects.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2019).

## How to Use

To run this project, follow the below steps:

1- Before running this project, will MUST install .NET Core, ASP.NET Core, Entity Framework and SQL Server. Create a database in SQL Server with "OnlineStore" name.

2- Clone or download this project;

3- Create a XML file in the path: `<Project-Folder>/Private/PrivateData.xml`. Write the following template inside of the XML file and fill with your data:

```
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<root>
    <!-- Email account to receive messages of the Contact page. -->
    <Contact_Email></Contact_Email>
    <Contact_Password></Contact_Password>
    
    <!-- The connection string of your SQL Server database. -->
    <Database_Connection_String></Database_Connection_String>
</root>
```
In the email account data, your email account must have permission to access less secure apps.

4- Open the folder in your terminal;

5- Run `dotnet restore`;

6- Run `dotnet run`;

7- Open the browser and go to `localhost:5000`.

That's all!

## How to Contribute

To contribute with this project:

- Do a fork of this repository;
- Create a branch with your feature: `git checkout -b my-feature`;
- Commit your changes: `git commit -m 'feat: 'My feature details'`.
- Push the commits to your branch `git push origin my-feature`.

After the merge of your pull request has been made, you can delete your branch.

## License

This project is licensed under the MIT License. See the [license](https://opensource.org/licenses/MIT) page for details.