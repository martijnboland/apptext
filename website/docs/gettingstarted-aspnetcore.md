---
id: gettingstarted-aspnetcore
title: AppText for ASP.NET Core applications
---

This page continues where [Installation](installation#install-into-an-aspnet-core-host-application) finished: a working AppText installation in an ASP.NET Core host application. 

In this example, the host application itself is going to be localized with AppText.

## ASP.NET Core localization

ASP.NET Core has built-in components for localization of user interfaces, `IStringLocalizer` and `IViewLocalizer`. Please check [the official documentation about .NET Core globalization and localization](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) if you're not familiar with these components.

AppText has a NuGet package that enables standard .NET Core localization while using AppText to manage localized content.

## AppText.Localization package

Add the AppText.Localization package if you want to use AppText to manage localized content for your ASP.NET Core application:

```
dotnet add package AppText.Localization
```

Then, enable ASP.NET Core localization with the AppText integration (note the `.AddAppTextLocalization(options => ...)` call):

```csharp
// Register ASP.NET MVC components and enable localization
services.AddControllersWithViews()
    .AddViewLocalization();

// Add AppText components

// Get physical directory for storage 
var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");

// Add AppText API and storage components
services.AddAppText()
    .AddNoDbStorage(dataPath)
    .AddAdmin()
    .AddAppTextLocalization(options =>
    {
        // Create empty items in AppText for all keys that are not found
        options.CreateItemsWhenNotFound = true;
    });
```

With the above configuration, all localized content that is displayed with IStringLocalizer or IViewLocalizer comes from AppText and you manage it on the fly with the AppText Admin interface.

## Request localization

In ASP.NET Core, request localization (which language is used for a specific request) is configured at startup (either in `Startup.cs` or `Program.cs`):

```csharp
app.UseRequestLocalization();
```

Normally you'd configure this with some options that specify the available cultures and the default culture, but you can also hook this up with AppText, so that the available languages and default language defined in the AppText App will become the available cultures and default culture for request localization:

```csharp
services.AddAppTextLocalization(options =>
{
    // Create empty items in AppText for all keys that are not found
    options.CreateItemsWhenNotFound = true;
    // Hook up request localization with AppText
    options.ConfigureRequestLocalizationOptions = true;
});
```

You can easily check request localization by adding `?culture={apptext_supported_language}` as query string where {apptext_supported_languange} is one of the available languages in the AppText App.

## Example application

You can find a working example of an ASP.NET Core 6.0 MVC application that uses AppText for localization at https://github.com/martijnboland/apptext/tree/main/examples/AspNetCoreMvcExample.