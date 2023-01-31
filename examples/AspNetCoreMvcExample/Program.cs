using AppText.AdminApp.Configuration;
using AppText.Configuration;
using AppText.Features.Application;
using AppText.Localization;
using AppText.Storage.NoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add AppText
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data");

builder.Services.AddAppText()
    // Use simple JSON file storage
    .AddNoDbStorage(dataPath)
    // Admin api is available at /apptext
    .AddAdmin()
    // Initialize an app with languages and default language in AppText where the content is stored
    .InitializeApp("example_app", "ASP.NET Core example", new string[] { "en", "nl", "fr" }, "en")
    // Add AppText implementation for ASP.NET Core localization 
    .AddAppTextLocalization(options =>
    {
        // The ID of the app that we created just before
        options.AppId = "example_app";
        // Create empty items in AppText for all keys that are not found
        options.CreateItemsWhenNotFound = true;
        // Use AppText app settings of example_app for ASP.NET Core request localization
        // SupportedCultures becomes 'en', 'nl' and 'fr' and the DefaultCulture is 'en'
        options.ConfigureRequestLocalizationOptions = true;
    });

// ASP.NET Core MVC
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();