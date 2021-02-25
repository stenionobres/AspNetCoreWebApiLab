# ASP.NET Core Web API Lab

Read this documentation in other languages: [Portuguese (pt-BR)](README-pt-BR.md)

Application created with the main objective of exploring the features and characteristics of the ASP.NET Core Web API.

In this application, several real usage scenarios were tested based on a mini application.

After the case studies, the main conclusions were documented in this file and serve as a reference for use and source of consultation.

## Table of contents

* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)
* [Solution Structure](#solution-structure)
    * [Used Packages](#used-packages)
    * [AspNetCoreWebApiLab-Api](#AspNetCoreWebApiLab-Api)
    * [AspNetCoreWebApiLab-ApiClient](#AspNetCoreWebApiLab-ApiClient)
    * [AspNetCoreWebApiLab-Persistence](#AspNetCoreWebApiLab-Persistence)

## Prerequisites

What needs to be installed on the machine to extend and debug the project:

    Visual Studio Community 2019;
    Net Core SDK 3.1;
    SQL Server

## Getting Started

* Install and/or configure all the prerequisites mentioned above;
* Clone the repository on the local machine;
* Create the databases used in the application: AspNetCoreWebApiLab;
* Download Nuget dependencies for the solution in Visual Studio;
* Run the migrations to the desired database with the command: Update-Database -Context [ClassName of context];
* Execute the AspNetCoreWebApiLab.Api project;

## Solution Structure

The solution `AspNetCoreWebApiLab` is divided into three projects: `AspNetCoreWebApiLab.Api`, `AspNetCoreWebApiLab.ApiClient` and `AspNetCoreWebApiLab.Persistence`. In the next sections the projects are detailed.

### Used Packages

>Net Core 3.1

>[VisualStudio Web CodeGeneration Design 3.1.5](https://www.nuget.org/packages/Microsoft.VisualStudio.Web.CodeGeneration.Design/3.1.5)

>[Microsoft.AspNetCore.Mvc.Versioning 4.2.0](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning/4.2.0)

### AspNetCoreWebApiLab-Api

### AspNetCoreWebApiLab-ApiClient

### AspNetCoreWebApiLab-Persistence