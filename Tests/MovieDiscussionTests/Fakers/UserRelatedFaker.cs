using Bogus;
using Grpc.Core;
using MovieGrpcClient;
using System.Security.Claims;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Fakers
{
    public static class UserRelatedFaker
    {
        public static Faker<User> GetUserFaker()
        {
            return new Faker<User>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Password, f => f.Internet.Password());
        }

        public static Faker<UserDto> GetUserDtoFaker()
        {
            return new Faker<UserDto>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email());
        }

        public static Faker<UserClaimsDto> GetUserClaimsDtoFaker()
        {
            return new Faker<UserClaimsDto>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Role, f => f.Random.Enum<Role>())
                .RuleFor(x => x.Email, f => f.Internet.Email());
        }

        public static ClaimsPrincipal GetUserPrincipal(string? email = null, Guid? id = null, string? userId = null, Role? role = null)
        {
            var claims = new List<Claim> {
                new Claim("Email", email ?? new Faker().Internet.Email()),
                new Claim("Id", id?.ToString() ?? Guid.NewGuid().ToString()),
                new Claim("UserId", userId ?? Guid.NewGuid().ToString()),
                new Claim("Role", role?.ToString() ?? Role.User.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimPrincipal = new ClaimsPrincipal(identity);

            return claimPrincipal;
        }

        public static AsyncUnaryCall<CreateProfileAndWatchlistReply> GetGrpcCreateReply()
        {
            return new AsyncUnaryCall<CreateProfileAndWatchlistReply>(
                Task.FromResult(new CreateProfileAndWatchlistReply()),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });
        }

        public static AsyncUnaryCall<DeleteProfileAndWatchlistReply> GetGrpcDeleteReply()
        {
            return new AsyncUnaryCall<DeleteProfileAndWatchlistReply>(
                Task.FromResult(new DeleteProfileAndWatchlistReply()),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });
        }

        public static Faker<LoginRequest> GetLoginRequestFaker()
        {
            return new Faker<LoginRequest>()
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Password, f => f.Internet.Password());
        }

        public static Faker<RegisterRequest> GetRegisterRequestFaker()
        {
            return new Faker<RegisterRequest>()
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Password, f => f.Internet.Password());
        }

        public static Faker<UpdateUserRequest> GetUpdateUserRequestFaker()
        {
            return new Faker<UpdateUserRequest>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.UserName, f => f.Name.FullName());
        }

        public static Faker<ConfirmEmailRequest> GetConfirmEmailFaker()
        {
            return new Faker<ConfirmEmailRequest>()
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Token, f => f.Random.Guid().ToString());
        }

        public static string GetEmail()
        {
            return new Faker().Internet.Email();
        }

        public static Faker<ResetPasswordRequest> GetResetPasswordFaker()
        {
            return new Faker<ResetPasswordRequest>()
                .RuleFor(x => x.NewPassword, f => f.Random.Guid().ToString())
                .RuleFor(x => x.Token, f => f.Random.Guid().ToString())
                .RuleFor(x => x.Email, f => f.Internet.Email());
        }
    }
}
