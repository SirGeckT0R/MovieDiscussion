namespace MovieServiceDomain.Models
{
    public class Review : IdModel
    {
        public Guid MovieId { get; private set; }
        public Guid ProfileId { get;  set; }
        public int Value { get; private set; } = 0;

        public string Text { get; private set; } = string.Empty;

        public Review() { }

        public Review(Guid movieId, Guid profileId, int value, string text)
        {
            MovieId = movieId;
            ProfileId = profileId;
            Value = value;
            Text = text;
        }
    }
}
