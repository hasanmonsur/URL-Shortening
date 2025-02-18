using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenUrlController : ControllerBase
    {
        private readonly UrlShortenerService _urlShortenerService;

        public ShortenUrlController(UrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortUrlRequest request)
        {
            var shortCode = await _urlShortenerService.ShortenUrl(request.OriginalUrl);
            var shortUrl = $"{Request.Scheme}://{Request.Host}/{shortCode}";
            return Ok(new ShortUrlResponse { ShortUrl = shortUrl });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectUrl(string shortCode)
        {
            var originalUrl = await _urlShortenerService.GetOriginalUrl(shortCode);
            if (string.IsNullOrEmpty(originalUrl))
                return NotFound();

            return Redirect(originalUrl);
        }
    }
}
