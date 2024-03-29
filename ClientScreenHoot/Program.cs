using ClientScreenHoot;
using ClientScreenHoot.Components;
using ClientScreenHoot.Components.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build(); 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapPost("/send_image", async (IFormFile image) =>
{

    if (image == null || image.Length == 0)
    {
        return 0;
    }

    using (var memoryStream = new MemoryStream())
    {
        await image.CopyToAsync(memoryStream);
        Image.ByteImage = memoryStream;
    }
    return image.Length;
}).DisableAntiforgery();

app.MapGet("/get_card", () =>
{
    return JsonSerializer.Serialize(Answer.CurrentAnswer);
}).DisableAntiforgery();

app.Run();
