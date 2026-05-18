using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReviewService.Data;
using ReviewService.Models;

namespace ReviewService.Services
{
    public class ReviewRepository
    {
        private readonly IMongoCollection<Review> _reviewsCollection;

        public ReviewRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(
                mongoDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbSettings.Value.DatabaseName);

            _reviewsCollection = mongoDatabase.GetCollection<Review>(
                mongoDbSettings.Value.ReviewsCollectionName);
        }

        public async Task<List<Review>> GetAsync() =>
            await _reviewsCollection.Find(_ => true).ToListAsync();

        public async Task<List<Review>> GetByProductIdAsync(string productId) =>
            await _reviewsCollection
                .Find(r => r.ProductId == productId)
                .ToListAsync();

        public async Task<double> GetAverageRatingAsync(string productId)
        {
            var reviews = await _reviewsCollection
                .Find(r => r.ProductId == productId)
                .ToListAsync();

            if (reviews.Count == 0) return 0;

            return Math.Round(reviews.Average(r => r.Rating), 2);
        }

        public async Task CreateAsync(Review review) =>
            await _reviewsCollection.InsertOneAsync(review);
    }
}