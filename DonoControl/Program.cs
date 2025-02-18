using Microsoft.AspNetCore.Builder;
using DonoControl.Services;
using DonoControl.Configuration;
using System.Diagnostics;
using DonoControl.Helpers;
using DonoControl.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DonoControl.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWindowsService();

string ipAddress = IpHelper.GetLocalIpAddress();

builder.WebHost.UseUrls($"http://{ipAddress}:5001");

builder.Services.AddDirectoryBrowser();
builder.Services.Configure<PresentationSettings>(
    builder.Configuration.GetSection(PresentationSettings.SectionName));
builder.Services.AddSingleton<IPresentationService, PresentationService>();
builder.Services.AddSingleton<IImageServise, ImageServise>();

var app = builder.Build();


app.UseStaticFiles();


//app.MapGet("/", WebUI.GetMainPage);
app.MapGet("/api/presentation/slides", async (IPresentationService presentationService) =>
{
    return await presentationService.GetSlidesAsync();
});
app.MapGet("/image/{category}", async (string category, IImageServise ImageService) =>
{
    return await ImageService.GetImageAsync(category);
});
app.MapGet("/image-file/{category}", async (string category, IImageServise imageService) =>
{
    return await imageService.GetImageFileAsync(category);
});
app.MapGet("/slides", async (IPresentationService presentationService)
    => await presentationService.GetSlidesAsync());

app.Run();