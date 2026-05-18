using Microsoft.AspNetCore.Mvc;
using ReviewService.Models;
using ReviewService.Services;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewRepository _reviewService;
        public ReviewsController(ReviewRepository reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<List<Review>> Get()
        {
            return await _reviewService.GetAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Review review)
        {
            await _reviewService.CreateAsync(review);

            return Ok(review);
        }

        [HttpGet("product/{productId}")]
        public async Task<List<Review>> GetByProduct(string productId)
        {
            return await _reviewService.GetByProductIdAsync(productId);
        }

        [HttpGet("product/{productId}/rating")]
        public async Task<IActionResult> GetAverageRating(string productId)
        {
            var average = await _reviewService.GetAverageRatingAsync(productId);
            return Ok(new { productId, averageRating = average });
        }
    }
}