using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CircuitAdventures.Client;
using CircuitAdventures.Client.Services;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Add MudBlazor services
builder.Services.AddMudServices();

// Register application services as Scoped (per user session)
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LevelService>();
builder.Services.AddScoped<CertificateService>();
builder.Services.AddScoped<ProgressService>();

await builder.Build().RunAsync();
