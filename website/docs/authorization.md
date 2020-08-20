---
id: authorization
title: Authorization
---

AppText has two authorization modes:
- **full access** to API, Admin interface and all Apps based on the ASP.NET Core host application configuration in Startup.cs;
- **read-only access** to the REST and GraphQL API's based on an [API key](api-keys) that is created with the Admin interface for a single app.

That's it. No users, no roles, no groups.

This page covers full-access authorization via the host application. See [API key authorization](api-keys) how to configure read-only API access.

## Authorization via ASP.NET Core host application

AppText itself is completely security-agnostic. It depends on the configuration of the host application. This way all ASP.NET Core authentication methods are supported out of the box. ASP.NET Identity with local accounts, AzureAD, IdentityServer, everything is possible. 

There are two [configuration settings](configuration.md#apptextapiconfigurationoptions) for AppText that link it to the Authentication and Authorization configuration of the host application: `RequireAuthenticatedUser` and `RequiredAuthorizationPolicy`.

### RequireAuthenticatedUser

This is a simple configuration option that simply says: 'anonymous access to AppText is not allowed, but any authenticated user can do everything'. 

#### Example with ASP.NET Identity

```csharp
services.AddDbContext<ApplicationDbContext>();

services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddAuthentication()

services.AddAppText(options =>
{
    options.RequireAuthenticatedUser = true;
});
```

:::note
Use this only when you don't have any extra requirements about who can access AppText. Most of the times you'd probably be better off with a custom authorization policy and link that to AppText with the `RequiredAuthorizationPolicy` option. 
:::

### RequiredAuthorizationPolicy

[Authorization policies in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1) are a very powerful and flexible way to secure applications. When setting the RequiredAuthorizationPolicy option, you're applying a single authorization policy to all of AppText.

#### Example with Azure AD authentication 

```csharp
services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
    .AddAzureAD(options => Configuration.Bind("AzureAd", options));

services.AddAuthorization(options =>
{
    options.AddPolicy("apptext-admin-only", policyBuilder =>
    {
        policyBuilder
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AzureADDefaults.AuthenticationScheme)
            .RequireClaim("group", "Azure AD group ID of AppText Administrators")
            .Build();
    });
});

services.AddAppText(options =>
{
    options.RequiredAuthorizationPolicy = "apptext-admin-only";
});
```

#### Example with ASP.NET Identity 

```csharp
services.AddDbContext<ApplicationDbContext>();

services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddAuthentication()

services.AddAuthorization(options =>
{
    options.AddPolicy("apptext-admin-only", policyBuilder =>
    {
        policyBuilder
            .RequireAuthenticatedUser()
            .RequireRole("AppTextAdministrators")
            .Build();
    });
});

services.AddAppText(options =>
{
    options.RequiredAuthorizationPolicy = "apptext-admin-only";
});
```