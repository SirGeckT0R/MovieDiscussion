namespace UserServiceApplication.Dto
{
    public record UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public UpdateUserRequest() { }
        public UpdateUserRequest(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
