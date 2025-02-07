namespace DiscussionServiceApplication.Dto
{
    public record DiscussionDto(Guid Id, 
                                string Title, 
                                string Description, 
                                DateTime StartAt, 
                                Guid CreatedBy, 
                                bool IsActive, 
                                ICollection<Guid> Subscribers)
    {
        public DiscussionDto() : this(Guid.Empty, 
                                      "", 
                                      "", 
                                      DateTime.UtcNow, 
                                      Guid.Empty, 
                                      false, 
                                      new List<Guid>()
                                      ) 
        { 

        }
    }
}
