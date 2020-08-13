---
id: installation
title: Installation
---

AppText requires an ASP.NET Core 3.1+ host application to run on and is installed via [NuGet packages](https://www.nuget.org/packages?q=apptext).

You can choose to create a brand new host application with the dotnet CLI or Visual Studio or simply add AppText to an existing ASP.NET Core application.

## Installation with NuGet

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

## Add the Admin interface

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

## Your first App

To use AppText for your own application, start with creating an App in AppText.

You can create a new AppText App in two ways: via the Admin interface by clicking the '+' icon in the top left of the screen next to 'AppText Admin App', or via code in Startup.cs:

```csharp
services.AddAppText()
    .AddNoDbStorage(dataPath)
    .AddAdmin()
    .InitializeApp("my_app", "My App", new string[] { "en", "nl" }, "en")
```

An App has an ID, a Display Name, a list of available languages and the default language.

*Note that the App ID may only contain lowercase characters, numbers and underscores. This is because it's being used in API url's and GraphQL identifiers.*

Once the App is created, you can create collections and content items with the simple buit-in Translation content type or create your custom complex content types that can contain all sorts of data.

By now, you have a fully operational Content Management System for your app. Next step is to actually display this content in your own app.

- [Use AppText content in ASP.NET Core applications with AppText.Localization](gettingstarted-aspnetcore)
- [Use AppText content in JavaScript applications with i18Next](gettingstarted-js)

## One word of warning

AppText is security-agnostic, which means that you have to define authentication and authorization in the ASP.NET Core host application and AppText can use that to secure the API endpoints and the Admin interface.

However, out of the box, no security is in place and anonymous users can probably access AppText. Therefore it's wise to always secure your AppText installation directly after installation.