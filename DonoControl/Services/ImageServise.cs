using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonoControl.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DonoControl.Services
{
    public class ImageServise : IImageServise
    {

        private readonly IConfiguration _configuration;

        public ImageServise(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IResult> GetImageAsync(string category)
        {
            string? imagePath = _configuration[$"Image:{category}"];

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                return Results.NotFound("Image not found");
            }

            // Генерируем URL для загрузки изображения через API
            string imageUrl = $"/image-file/{category}";

            string htmlContent = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Full Screen Image</title>
                    <style>
                        * {{ margin: 0; padding: 0; }}
                        html, body {{ width: 100%; height: 100%; overflow: hidden; background-color: black; }}
                        img {{ width: 100vw; height: 100vh; object-fit: contain; display: block; margin: auto; }}
                    </style>
                </head>
                <body>
                    <img src='{imageUrl}' alt='Image'>
                </body>
                </html>";

            return Results.Content(htmlContent, "text/html");
        }

        public async Task<IResult> GetImageFileAsync(string category)
        {
            string? imagePath = _configuration[$"Image:{category}"];

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                return Results.NotFound("Image not found");
            }

            return Results.File(imagePath, "image/png");
        }
    }
}
