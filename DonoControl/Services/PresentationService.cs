using Microsoft.Extensions.Options;
using Spire.Presentation;
using DonoControl.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Runtime;

namespace DonoControl.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly PresentationSettings _settings;
        private readonly ILogger<PresentationService> _logger;
        private readonly string _wwwrootPath;


        public PresentationService(
            IOptions<PresentationSettings> settings,
            ILogger<PresentationService> logger,
            IWebHostEnvironment environment)
        {
            _settings = settings.Value;
            _logger = logger;
            _wwwrootPath = environment.WebRootPath;

            if (string.IsNullOrEmpty(_settings.FilePath) || !File.Exists(_settings.FilePath))
            {
                _logger.LogError("Presentation file not found at {FilePath}", _settings.FilePath);
                throw new ArgumentNullException(nameof(_settings.FilePath), "Presentation file path is not set or file does not exist.");
            }

            var slidesPath = Path.Combine(_wwwrootPath, "slides");
            if (!Directory.Exists(slidesPath))
            {
                Directory.CreateDirectory(slidesPath);
            }
        }

        public async Task<IResult> GetSlidesAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_settings.FilePath) || !File.Exists(_settings.FilePath))
                {
                    _logger.LogError("Presentation file not found at {FilePath}", _settings.FilePath);
                    return Results.BadRequest("Presentation file not found");
                }

                DeleteSlides();

                var presentation = new Presentation();
                _logger.LogInformation("Loading presentation from {FilePath}", _settings.FilePath);
                presentation.LoadFromFile(_settings.FilePath);

                presentation.Masters[0].SlideBackground.Fill.FillType = Spire.Presentation.Drawing.FillFormatType.None;

                var slides = new List<string>();
                for (int i = 0; i < presentation.Slides.Count; i++)
                {
                    var imagePath = Path.Combine("slides", $"slide_{i}.png");
                    var physicalPath = Path.Combine(_wwwrootPath, imagePath);

                    int width = 1920;
                    int height = 1080;

                    using (Image slideImage = presentation.Slides[i].SaveAsImage(width, height))
                    {
                        slideImage.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    slides.Add($"/{imagePath}");
                }

                return Results.Ok(slides);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing presentation");
                return Results.Problem("Internal server error while processing presentation", statusCode: 500);
            }
        }

        private void DeleteSlides()
        {
            string slidesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slides");

            if (Directory.Exists(slidesDirectory))
            {
                foreach (string file in Directory.GetFiles(slidesDirectory))
                {
                    File.Delete(file);
                }
            }
        }

    }
}