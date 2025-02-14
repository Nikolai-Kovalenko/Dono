using Microsoft.AspNetCore.Builder;
using DonoControl.Services;
using DonoControl.Configuration;
using System.Diagnostics;
using DonoControl.Helpers;
using DonoControl.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Для локального запуска закомментируйте эту строку
// builder.Host.UseWindowsService();

string ipAddress = IpHelper.GetLocalIpAddress();

builder.WebHost.UseUrls($"http://{ipAddress}:5000");

builder.Services.AddDirectoryBrowser();
builder.Services.Configure<PresentationSettings>(
    builder.Configuration.GetSection(PresentationSettings.SectionName));
builder.Services.AddSingleton<IPresentationService, PresentationService>();

var app = builder.Build();


app.UseStaticFiles();


//app.MapGet("/", WebUI.GetMainPage);
app.MapGet("/api/presentation/slides", async (IPresentationService presentationService) =>
{
    return await presentationService.GetSlidesAsync();
});
app.MapGet("/slides", async (IPresentationService presentationService)
    => await presentationService.GetSlidesAsync());

//builder.WebHost.UseUrls("http://192.168.1.121:5000");

app.Run();