namespace ChronotrackService.Application
{
    public interface IAuthService
    {
        Task<(string, UserEntity)> AuthenticateAsync(string email, string password);
        Task<UserEntity> AuthenticateAsync(string email);
    }
}
