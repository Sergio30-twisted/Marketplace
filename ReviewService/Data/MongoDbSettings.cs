namespace ReviewService.Data
{
	public class MongoDbSettings
	{
		public string ConnectionString { get; set; } = null!;

		public string DatabaseName { get; set; } = null!;

		public string ReviewsCollectionName { get; set; } = null!;
	}
}