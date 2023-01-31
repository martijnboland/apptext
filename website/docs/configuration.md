---
id: configuration
title: Configuration
---

You add AppText to an existing ASP.NET Core application add application startup with the `services.AddAppText()` extension method. 
This method adds the core services with API and returns an `AppTextBuilder` object where all extensions are configured.

An example:
```csharp
services.AddAppText() // Adds AppText core services with API
    .AddNoDbStorage('my_storage_folder') // Adds the NoDb storage module
    .AddAdmin() // Adds admin interface
```

The different AppText packages all come with their own extensions and options which are all configured at application startup. This page contains a complete overview of all configuration methods and options.

## AppText

The main AppText NuGet package. Contains core services, REST and GraphQL API's and the NoDb storage implementation. 

### AddAppText()

Namespace: `AppText.Configuration`

This is the main configuration method that adds the core services with API. You can optionally pass an AppTextApiConfigurationOptions action.

#### Example

```csharp
services.AddAppText(options => 
{
    options.RoutePrefix = "cms";
    options.RequireAuthenticatedUser = true;
});
```

AddAppText() returns the `AppTextBuilder` object that can be used to chain all other configuration options.

```csharp
services.AddAppText(options => 
{
    /// ... options
})
    .AddOtherConfiguration()
    .AddMoreConfiguration()
```

#### AppTextApiConfigurationOptions

| option | description | default |
|--------|-------------|---------|
| RoutePrefix | The route prefix for the AppText API. This defines the base url of the API: https://_your_host_url_/_{RoutePrefix}_. | "apptext" |
| RequireAuthenticatedUser | Only authenticated users are allowed to access the API. | false |
| RequiredAuthorizationPolicy | The name of a pre-defined authorization policy that is applied to the API. | *null* |
| EnableGraphiql | Enable the interactive Graphql browser (located at /_{RoutePrefix}_/_{appId}_/graphql/graphiql). | false |
| RegisterClaimsPrincipal | Should HttpContext.User be registered as ClaimsPrincipal? | true |

### InitializeApp()

Namespace: `AppText.Features.Application`

Ensure that a specific AppText application always exists by configuring it from code.

#### Example

```csharp
services.AddAppText()
    .InitializeApp("my-app", "My Application", new[] { "en", "nl" }, "en");
```

#### Parameters

| parameter | description | required |
|-----------|-------------|----------|
| appId | The App identifier (only lowercase characters, numbers and underscores allowed). | yes |
| displayName | Display name of the App. | yes |
| languages | Array with available languages. | yes |
| defaultLanguage | Default language. | yes |
| isSystem | Indicates if an App is a system App (for example, the AppText Admin app). | no, default false |

### AddNoDbStorage()

Namespace: `AppText.Storage.NoDb`

Configure AppText to use [NoDb](https://github.com/cloudscribe/NoDb) as file-based storage implementation.

#### Example

```csharp
var dataPath = Path.Combine(Env.ContentRootPath, "App_Data");

services.AddAppText()
    .AddNoDbStorage(dataPath);
```

#### Parameters

| parameter | description | required |
|-----------|-------------|----------|
| baseFolder | Physical folder where AppText content files will be stored. | yes |

## AppText.Storage.LiteDb

The AppText.Storage.LiteDb NuGet package contains an AppText storage implementation that uses [LiteDB](https://www.litedb.org/) as database. 

### AddLiteDbStorage()

Configure AppText to use LiteDB for storage.

#### Example

```csharp
var connectionString = $"FileName={Path.Combine(Env.ContentRootPath, "App_Data", "AppText.db")};Mode=Exclusive";

services.AddAppText()
    .AddLiteDbStorage(connectionString);
```

#### Parameters

| parameter | description | required |
|-----------|-------------|----------|
| connectionString | The LiteDB [connection string](https://www.litedb.org/docs/connection-string/). | yes |

## AppText.Translations

The AppText.Translations NuGet package contains a standard Content Type, `Translation` for translations and extensions for the AppText REST API that make it easy to use AppText with external translation libraries. The package comes with support for RESX, GNU gettext .po and JSON formats.   

### AddTranslations()

Namespace: `AppText.Translations.Configuration`

Adds the Translation Content Type and the REST API endpoints  for translations. You can optionally pass an AppTextTranslationConfigurationOptions action.

:::note
When no options are specified, AppText checks if options with the same name are set in [AppTextApiConfigurationOptions](#apptextapiconfigurationoptions) and applies these.
:::

#### Example

```csharp
services.AddAppText()
    .AddTranslations(options => 
    {
        options.RequiredAuthorizationPolicy = "my-policy"
    });
```

#### AppTextTranslationConfigurationOptions

| option | description | default |
|--------|-------------|---------|
| RequireAuthenticatedUser | Only authenticated users are allowed to access the Translations API. | false |
| RequiredAuthorizationPolicy | The name of a pre-defined authorization policy that is applied to the Translations API. | *null* |

## AppText.AdminApp

The AppText.AdminApp NuGet package contains the Admin interface for the AppText REST API.

:::note
AppText.AdminApp uses the AppText.Translations package so you don't have to add that package anymore when using the AppText.AdminApp package.
:::

### AddAdmin()

Namespace: `AppText.AdminApp.Configuration`

Adds the Admin interface to AppText. You can optionally pass an AppTextAdminConfigurationOptions action.

:::note
When no options are specified, AppText checks if options with the same name are set in [AppTextApiConfigurationOptions](#apptextapiconfigurationoptions) and applies these.
:::

#### Example

```csharp
services.AddAppText()
    .AddAdmin(options => 
    {
        options.RoutePrefix = "admin";
        options.ApiBaseUrl = "/apptext";
        options.RequiredAuthorizationPolicy = "apptext-administrator-only";
    });
```

#### AppTextAdminConfigurationOptions

| option | description | default |
|--------|-------------|---------|
| RoutePrefix | The Admin interface url prefix (Admin url becomes https://_your_host_url_/_{RoutePrefix}_). | inherits RoutePrefix of the [AppTextApiConfigurationOptions](#apptextapiconfigurationoptions)
| ApiBaseUrl | The base URL of the AppText REST API | Same base url as Admin interface |
| RequireAuthenticatedUser | Only authenticated users are allowed to access the Admin interface. | false |
| RequiredAuthorizationPolicy | The name of a pre-defined authorization policy that allows users to access the Admin interface. | *null* |
| AuthType |  The type of authentication to use for the Admin interface and accessing the API. Options are `AppTextAdminAuthType.DefaultCookie` or `AppTextAdminAuthType.Oidc`. | AppTextAdminAuthType.DefaultCookie |
| OidcSettings | Optional OpenID Connect settings (only applies when AuthType is set to AppTextAdminAuthType.Oidc). | *empty* |
| EmbeddedViewsDisabled | Don't use the embedded views (only for development). | false |

## AppText.Localization

The AppText.Localization NuGet package enables .NET Core apps to use AppText dynamic resources with the standard .NET Core localization API.

### AddAppTextLocalization()

Namespace: `AppText.Localization`

Enables AppText for localization of the .NET Core host application. You can optionally pass an AppTextLocalizationOptions action.

#### Example

```csharp
services.AddAppText()
    .AddAppTextLocalization(options => 
    {
        options.AppId = "my_host_app";
        options.CreateItemsWhenNotFound = true;
        options.ConfigureRequestLocalizationOptions = true;
    });
```

#### AppTextLocalizationOptions

| option | description | default |
|--------|-------------|---------|
| CollectionName | The collection name where the content items are stored. | "Resources" |
| AppId | AppText App id for which the content items should be stored. | The Assembly name of the entry assembly (lowercased). |
| CreateItemsWhenNotFound | Create new empty content item with a key when the key is not found. | false |
| ConfigureRequestLocalizationOptions | Base the ASP.NET Core request localization options (supported languages, default language) on the AppText app | false |
| PrefixContentKeys | Prefix localization keys with type or paths. When set to false, localization keys will be shared. | true |
| PrefixSeparator | Separator between content key prefix and content key. | "." |
| DefaultLanguage | The default language of the AppText App | *null* |
| RecycleHostAppAfterSavingApp | When set to true, the host application is terminated after changing the AppText app. This is required to immediately see the effect of adding new languages or changing the default language. WARNING: this only works when running behind a server like IIS that automatically restarts the process when it is terminated. Do not set this option to true when running standalone. It will kill your host application. | false |