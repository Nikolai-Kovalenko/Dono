using DonoControl.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonoControl.UI
{
    public static class WebUI
    {
        public static IResult GetMainPage()
        {
            return Results.Text(""" 
            <!DOCTYPE html>
            <html>
            <head>
                <title>DonoControl - Presentation Viewer</title>
                <style>
                    body { 
                        margin: 0; 
                        background: #000; 
                        color: white; 
                        font-family: Arial; 
                    }
                    img { 
                        max-width: 100%; 
                        max-height: 100vh; 
                        display: block; 
                        margin: auto; 
                    }
                    .controls { 
                        position: fixed; 
                        bottom: 20px; 
                        left: 50%; 
                        transform: translateX(-50%);
                        background: rgba(0,0,0,0.5);
                        padding: 10px;
                        border-radius: 5px;
                    }
                    button { 
                        margin: 0 10px; 
                        padding: 10px 20px;
                        background: #444;
                        color: white;
                        border: none;
                        border-radius: 3px;
                        cursor: pointer;
                    }
                    button:hover { 
                        background: #666; 
                    }
                </style>
            </head>
            <body>
                <div id='slideContainer'></div>
                <div class='controls'>
                    <button onclick='prevSlide()'><</button>
                    <button onclick='toggleAutoplay()'>Play/Pause</button>
                    <button onclick='nextSlide()'>></button>
                </div>
                <script>
                    let currentSlide = 0;
                    let slides = [];
                    let autoplayInterval = null;

                    fetch('/slides')  // Запрос слайдов с сервера
                        .then(r => r.json())
                        .then(data => {
                            slides = data;
                            showSlide(0);
                        })
                        .catch(error => {
                            console.error('Error loading slides:', error);
                            alert('Error loading presentation. Please check the console for details.');
                        });

                    function showSlide(index) {
                        if (slides.length === 0) return;
                        currentSlide = (index + slides.length) % slides.length;
                        document.getElementById('slideContainer').innerHTML = 
                            `<img src='${slides[currentSlide]}' />`; // Путь к изображению слайда
                    }

                    function nextSlide() { showSlide(currentSlide + 1); }
                    function prevSlide() { showSlide(currentSlide - 1); }
                    function toggleAutoplay() {
                        if (autoplayInterval) {
                            clearInterval(autoplayInterval);
                            autoplayInterval = null;
                        } else {
                            autoplayInterval = setInterval(nextSlide, 5000); // Каждые 5 секунд
                        }
                    }

                    // Управление с клавиатуры
                    document.addEventListener('keydown', (e) => {
                        if (e.key === 'ArrowRight') nextSlide();
                        if (e.key === 'ArrowLeft') prevSlide();
                        if (e.key === ' ') toggleAutoplay();
                    });
                </script>
            </body>
            </html>
        """, "text/html");
        }
    }

}
