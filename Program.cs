using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WorkflowBuilder;
using WorkflowBuilder.Services;
using Blazor.Diagrams.Core;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Workflow services
builder.Services.AddScoped<WorkflowService>();
builder.Services.AddSingleton<WorkflowEventService>();

await builder.Build().RunAsync();
