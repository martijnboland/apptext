# AppText
AppText is a Content Management System for Applications. A hybrid between a headless Content Management System and a Translation Management System.

AppText is built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) and [React](https:reactjs.org). It is installed via [NuGet packages](https://www.nuget.org/packages?q=apptext).

![.NET Core Build](https://github.com/martijnboland/apptext/workflows/.NET%20Core%20Build/badge.svg)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/apptext)

## Why?
Many custom software applications (web, mobile, native) are shipped with embedded content. Think of labels, tooltips or even complete pages with information. This content often needs to be localized in multiple languages.

Once an application is released, updating the embedded content can become a bit of burden. Even the smallest textual change often requires a new version that needs to be deployed by the developer of the application. The same when adding a new language or adding missing translations.

The primary goal of AppText is to enable content updates in applications without having to go through the entire process of deploying a new version of the application.

## Who should use it?
AppText is intended for .NET application developers who want an easy way of managing content for their applications and being able to delegate content management to non-developers.

## Getting started
AppText needs an ASP.NET Core 3.1 (or newer) host application to run on. For this example we'll create a new MVC application with ASP.NET Identity authentication (you can always add it to your own .NET Core applications as well):

```
dotnet new mvc --auth individual
```

### Adding the AppText Admin interface

Add the AppText API and Admin app NuGet packages (the version is required because we only have pre-releases so far):

```
dotnet add package AppText.AdminApp --version 0.3.0-alpha1
```

Register AppText components in the ConfigureServices method of Startup.cs:

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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                Configuration.GetConnectionString("DefaultConnection")));
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddControllersWithViews();
        services.AddRazorPages();

        // Add AppText components

        // Get physical directory for storage
        var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");

        services.AddAppText() // Adds AppText API to /apptext
            .AddNoDbStorage(dataPath) // Adds file-based storage
            .AddAdmin(); // Add admin UI, by default it is accessible at /apptext
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Unchanged...
    }
}
```

The AppText Admin interface is now available at: https://localhost:5001/apptext

[![Screenshot AppText dashboard](media/screenshots/apptext-dashboard-480.png?raw=true "Edit translations")](media/screenshots/apptext-dashboard.png?raw=true)

You'll notice that there already is some content in there. That's because AppText uses itself for the localization of the Admin interface :-). Go and try to change some content. This will be reflected immediately in the Admin interface.

### Secure the Admin interface
Until now, unauthenticated users can simply access the Admin interface. AppText itself doesn't know anything about authentication, but you can secure it with two configuration options. The first one, RequireAuthenticatedUser simply tells that only authenticated users can access AppText (ConfigureServices in Startup.cs):

```csharp
services.AddAppText(options => 
{
    options.RequireAuthenticatedUser = true;
});
```

Alternatively, and this is the better option, it's possible to define an authorization Policy that AppText uses:

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AppText", policy => policy
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)); // Local accounts
});

services.AddAppText(options => 
{
    options.RequiredAuthorizationPolicy = "AppText";
})
```

This way, it's entirely up to you how AppText is secured. Local accounts, Azure AD, OpenID Connect, everything that is supported by ASP.NET Core can be used.

## Development

### Prerequisites

AppText is a .NET Core app and uses React for the Admin app. You should be able to run it on any platform that supports .NET Core (Windows, MacOS, Linux). To build it, you'll need the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) and a reasonably recent version of [node.js](https://nodejs.org), for example, the latest LTS version. 

### Get the source code

```
git clone https://github.com/martijnboland/apptext.git
```
Navigate to the folder where you cloned the sources. You'll see a /src folder and in that folder the following components:
- AppText - the core logic with the REST and Graphql api's and a [file-based storage engine](https://github.com/cloudscribe/NoDb);
- AppText.AdminApp - the management application;
- AppText.Localization - enables .NET Core apps to use AppText dynamic resources with the standard .NET Core localization API;
- AppText.Storage.LiteDB - storage engine based on [LiteDB](https://www.litedb.org/);
- AppText.Translations - module that adds a global 'Translation' content type and provides a REST endpoint to retrieve content as JSON, .NET resx or GNU gettext PO files.
- HostAppExample - ASP.NET Core template app with authentication that hosts AppText as embedded application. This one is also configured by default to use the LiteDB storage engine.

### Build and run API and Admin app
Follow the steps below to build and run the AppText API and Admin App.

Open a terminal window and navigate to the /src/AppText.Admin/ClientApp folder, then build the React Admin app with
```
npm install
npm run build
```
Open a second terminal, navigate to the /src/AppText.AdminApp folder and execute
```
dotnet run
```
The admin interface will become available at https://localhost:5101/apptext. It is set up that it will initialize itself as app in the AppText storage at startup.

When developing the Admin app, you can run the webpack development server (with Hot Module Reloading) from the /src/AppText.Admin/ClientApp folder with
```
npm start
```
The Admin app will then be available at http://localhost:8080.

### Build and run the Host App example

An alternative way of running AppText from source is by building AppText and AppText.Admin first, but then running the HostAppExample. This showcases how AppText can be integrated in any existing ASP.NET Core web application.

Open a terminal, navigate to the /src/AppText.AdminApp/ClientApp folder and execute
```
npm install
npm run prod
```
Then navigate back to the sources root folder (where AppText.sln is located) and execute
```
dotnet build
```
Finally go to the src/HostAppExample folder and execute
```
dotnet run
```
The host app is available at https://localhost:5001 and the AppText admin app is at https://localhost:5001/apptext. Note that you have to create an account first and log in before you can access the admin app.

## AppText Concepts

### App
The App is the top level object in AppText. It defines which languages are available, which one is the default and has a list of ContentTypes. You can not do anything unless you have at least one App object.

### ContentType
A ContentType defines the structure of a specific piece of content. It contains field definitions such as 'Title' or 'Publication Date'.

### Collection
A collection is a container for Content and is linked to a specific ContentType. All content in the collection can only contain fields that are specified in the ContentType. 

### Content
Content is linked to a Collection and consists of a unique key (within the Collection) with field values for the fields that are specified in the ContentType of the Collection.

### Example object structure:
- AppTextAdmin (App)
  - Languages
    - en
    - nl
    - de
  - SimpleText (ContentType)
    - Text (Field)
  - HelpPage (ContentType)
    - Title (Field)
    - Body (Field)
  - Labels (Collection with SimpleText as ContentType)
    - Label1 (Content)
      - Text (FieldValue)
        - en: 'Text for Label1'
        - nl: 'Tekst voor Label1'
        - de: 'Text für Label1'
    - Label2 (Content)
      - Text (FieldValue)
        - en: 'Text for Label2'
        - nl: 'Tekst voor Label2'
        - de: 'Text für Label2'
  - Tooltips (Collection with SimpleText as ContentType)
    - Tooltip1 (Content)
      - Text (FieldValue)
        - en: 'Text for Tooltip1'
        - nl: 'Tekst voor Tooltip1'
        - de: 'Text für Tooltip1'
  - HelpPages (Collection with HelpPage as ContentType)
    - HelpPage1 (Content)
      - Title (FieldValue)
        - en: 'Title for HelpPage1'
        - nl: 'Titel voor HelpPage1'
        - de: 'Titel für HelpPage1'
      - Body (FieldValue)
        - en: 'Contents of HelpPage1'
        - nl: 'Inhoud van HelpPage1'
        - de: 'Inhalt von HelpPage1'
