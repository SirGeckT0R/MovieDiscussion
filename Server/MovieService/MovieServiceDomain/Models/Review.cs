namespace MovieServiceDomain.Models
{
    public class Review : IdModel
    {
        public Guid MovieId { get; private set; }
        public Guid UserId { get; private set; }
        public int Value { get; private set; } = 0;

        public string Text { get; private set; } = string.Empty;

        public Review() { }

        public Review(Guid movieId, Guid userId, int value, string text)
        {
            MovieId = movieId;
            UserId = userId;
            Value = value;
            Text = text;
        }
    }
}
