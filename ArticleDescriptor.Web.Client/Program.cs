using ArticleDescriptor.Application;
using ArticleDescriptor.Domain;
using ArticleDescriptor.Infrastructure;
using ArticleDescriptor.Infrastructure.Entities;
using ArticleDescriptor.Web.Client;
using ArticleDescriptor.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.UseBusinessApplication(builder.Configuration);

builder.Services.UseInteractiveApplication(builder.Configuration);

await builder.Build().RunAsync();
