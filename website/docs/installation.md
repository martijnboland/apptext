---
id: installation
title: Installation
---

There are two different ways you can run AppText: as a standalone application from a [Docker container](#install-with-docker) or embedded in an [ASP.NET Core application](#install-into-an-aspnet-core-host-application).

## Install with Docker

The AppText [Docker container image](https://hub.docker.com/r/apptextio/apptext) comes pre-configured with a LiteDB embedded database and has a single admin user that is created at first time use.

Run it with the following command:

```
docker run --name apptext -p 5000:80 apptextio/apptext
```

Visit http://localhost:5000 in your browser, create the admin user and login with that user. AppText will ask you to [create an App first](#your-first-app).

### Store data outside Docker container

By default, the AppText data is stored in the /DATA volume. When you want to store that AppText data in a different location, you have to run it with a custom volume and set the AppText DATAFOLDER environment variable to that custom volume:

```
docker run -d --name apptext -p 5000:80 -v /tmp:/apptextdata -e DATAFOLDER=/apptextdata apptextio/apptext
```

In the example above, the /tmp folder is mapped to the /apptextdata volume in the container, so the AppText data is stored in the /tmp folder.

### Environment variables for Docker container

#### DATAFOLDER

Specifies the folder in the container where the data is stored. Default value: **/DATA**.

#### DISABLE_AUTH

Turns off authentication when set to **true**.

#### ADMINUSER

Username for the AppText admin account. Default **admin** when not set.

#### ADMINPASSWORD

Password for the AppText admin account. Default empty and you have to enter the password at first use. 

## Install into an ASP.NET Core host application

AppText embedded in an ASP.NET Core application requires an ASP.NET Core 6+ host application to run on and is installed via [NuGet packages](https://www.nuget.org/packages?q=apptext).

You can choose to create a brand new host application with the dotnet CLI or Visual Studio or simply add AppText to an existing ASP.NET Core application.

### Install from NuGet

Add the AppText NuGet package. This package contains the REST API, the GraphQL API and a storage implementation that stores content in JSON text files.

```
dotnet add package AppText
```

To enable the AppText API's and storage you'll need to a add few lines in the `ConfigureServices()` method of the `Startup.cs` class:

```csharp
public class Startup
{
    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        Configuration = configuration;
        Env = env; // Added IHostEnvironment via DI. This is used later to obtain the physical directory for storage.
    }

    public IConfiguration Configuration { get; }
    public IHostEnvironment Env { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Register ASP.NET MVC components
        services.AddControllersWithViews();

        // Add AppText components

        // Get physical directory for storage
        var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");

        // Add AppText API and storage components
        services.AddAppText()
            .AddNoDbStorage(dataPath);
    }
}
```

You can verify if the API is working by browsing to `{your_host_url}/apptext/apps` or `{your_host_url}/apptext/me`.

### Add the Admin interface from NuGet

The Admin interface comes in a separate NuGet package:

```
dotnet add package AppText.AdminApp
```

Enable it in `Startup.cs` by extending the existing AppText registration:

```csharp
services.AddAppText()
    .AddNoDbStorage(dataPath)
    .AddAdmin()
```
Adding the Admin interface automatically adds the first App in AppText with all labels, messages and errors of the Admin interface (apptext_admin).

Now, when browsing to `{your_host_url}/apptext`, you see the Admin interface where you can start playing around with managing content for the Admin interface itself!

### One word of warning

AppText embedded in an ASP.NET Core application is security-agnostic, which means that you have to define authentication and authorization in the ASP.NET Core host application and AppText can use that to secure the API endpoints and the Admin interface.

However, out of the box, no security is in place and anonymous users can probably access AppText. Therefore it's wise to always secure your AppText installation directly after installation.

Don't forget to check [the authorization docs](authorization) for more information.

## Your first App

After installation, AppText present you with a form to create a new App. An App represents the application where you want to manage content for.

An App has an ID, a Display Name, a list of available languages and the default language.

*Note that the App ID may only contain lowercase characters, numbers and underscores. This is because it's being used in API url's and GraphQL identifiers.*

Once the App is created, you can create collections and content items with the simple buit-in Translation content type or create your custom complex content types that can contain all sorts of data.

By now, you have a fully operational Content Management System for your app. Next step is to actually display this content in your own app.

- [Use AppText content in ASP.NET Core applications with AppText.Localization](gettingstarted-aspnetcore)
- [Use AppText content in JavaScript applications with i18Next](gettingstarted-js)