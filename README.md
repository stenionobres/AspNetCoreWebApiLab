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
* [Rest X Restful](#rest-x-restful)
    * [Maturity Model](#maturity-model)
    * [Resources](#resources)
    * [HTTP Verbs](#http-verbs)
    * [Hypermedia (HATEOAS)](#hypermedia-HATEOAS)
* [Versioning](#versioning)

## Prerequisites

What needs to be installed on the machine to extend and debug the project:

    Visual Studio Community 2019;
    Net Core SDK 3.1;
    SQL Server

## Getting Started

* Install and/or configure all the prerequisites mentioned above;
* Clone the repository on the local machine;
* Create the database used in the application: AspNetCoreWebApiLab;
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

## Rest X Restful

Representational state transfer (REST) is a standard defined by Roy Fielding for a software architecture of interactive applications that use Web services. A Web service that follows this standard is called RESTful. [Wikipedia]

### Maturity Model

A [model](https://martinfowler.com/articles/richardsonMaturityModel.html) developed by Leonard Richardson that breaks down the principal elements of a REST approach into three steps: resources, http verbs, and hypermedia controls.

### Resources

Usually, a resource is something that can be stored on a computer and represented as a stream of bits: a document, a row in a database, or the result of running an algorithm. 

So now rather than making all our requests to a singular service endpoint, we now start talking to individual resources. Examples of resources: customers, companies, clients and users. Many APIs design guides recommends **the use of nouns in plural** to name resources instead verbs.

### HTTP Verbs

Separate URIs are given for separate resources, while incorporating different HTTP verbs according to the CRUD usage of those resources. The mainly HTTP verbs are: POST, GET, PUT, PATCH and DELETE

| HTTP Verb | Action on resource |
|:---------:|:------------------:|
| POST      | Create             |
| GET       | Read               |
| PUT       | Update             |
| PATCH     | Partial Update     |
| DELETE    | Delete             |

The mainly response status codes are listed below. These status codes represents the behavior that occurred on the server.

| Code | Description    | 
|:----:|:---------------|
| 200  | OK             |
| 201  | Created        |
| 400  | Bad Request    |
| 401  | Not Authorized |
| 403  | Forbidden      |
| 404  | Not Found      |
| 500  | Internal Error |

For a good use of HTTP verbs with correct status codes the table below was made. It's very important to make the correct use of verbs and codes to correctly use the REST standard.

| HTTP Verb | Related status codes         |
|:---------:|:-----------------------------|
| POST      | 201, 400, 401, 403, 500      |
| GET       | 200, 400, 401, 403, 404, 500 |
| PUT       | 200, 400, 401, 403, 404, 500 |
| PATCH     | 200, 400, 401, 403, 404, 500 |
| DELETE    | 200, 400, 401, 403, 404, 500 |

This [controller](./AspNetCoreWebApiLab.Api/Controllers/Experiments/ResourcesController.cs) presents how to implements a basic resource that uses the main HTTP verbs with correct response status codes. 

### Hypermedia (HATEOAS)

HATEOAS (Hypermedia as the Engine of Application State) is a constraint of the REST application architecture. The term “hypermedia” refers to any content that contains links to other forms of media such as images, movies, and text.

REST architectural style lets us use the hypermedia links in the response contents. It allows the client can dynamically navigate to the appropriate resources by traversing the hypermedia links. Below is shown an example:

``` JSON
HTTP/1.1 200 OK
Content-Type: application/vnd.acme.account+json
Content-Length: ...

{
    "account": {
        "account_number": 12345,
        "balance": {
            "currency": "usd",
            "value": -25.00
        },
        "links": {
            "deposit": "/accounts/12345/deposit"
        }
    }
}
```

## Versioning

One of the most important things in API design is versioning. The business rules changes and the api users shouldn't identify issues.

The main types of versioning in API design are: query parameters, custom header and URI path.

With ASP.NET Core Web API is possible to version the controller or controller actions. For this is used the `ApiVersion`, `MapToApiVersion` attributes and the [Microsoft.AspNetCore.Mvc.Versioning](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning) package should be added on solution.

To enables versioning with the Microsoft package the code below should be added on [Startup](./AspNetCoreWebApiLab.Api/Startup.cs) class in the API project.

``` C#
services.AddApiVersioning();
```

The controllers [VersioningController](./AspNetCoreWebApiLab.Api/Controllers/Experiments/VersioningController.cs) and [Versioning20Controller](./AspNetCoreWebApiLab.Api/Controllers/Experiments/Versioning20Controller.cs) showns examples of a API that has three versions: 1.0, 1.1 and 2.0.

The versions 1.0 and 1.1 are in the same controller, however the version 2.0 is in the other controller. The api versioning type can be defined on startup configuration using the `ApiVersionReader` option like the examples below:

* Query parameters

``` C#
services.AddApiVersioning(versioningOptions => 
{
    versioningOptions.ApiVersionReader = new QueryStringApiVersionReader();
});
```

* Custom header

``` C#
services.AddApiVersioning(versioningOptions => 
{
    versioningOptions.ApiVersionReader = new HeaderApiVersionReader("X-Version");
});
```

* URI path

The parameter `{version:apiVersion}` must be inserted into the controller route. 

``` C#
services.AddApiVersioning(versioningOptions => 
{
    versioningOptions.ApiVersionReader = new UrlSegmentApiVersionReader();
});
```

To use many configurations the `Combine` method must be used for merge two or more configurations:

``` C#
services.AddApiVersioning(versioningOptions => 
{
    versioningOptions.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(),
                                                                  new HeaderApiVersionReader("X-Version"),new UrlSegmentApiVersionReader());
});
```

The controllers [VersioningController](./AspNetCoreWebApiLab.Api/Controllers/Experiments/VersioningController.cs) and [Versioning20Controller](./AspNetCoreWebApiLab.Api/Controllers/Experiments/Versioning20Controller.cs) use all three types of versioning: query parameters, custom header and URI path. For more details the [Startup](./AspNetCoreWebApiLab.Api/Startup.cs) class can be consulted.