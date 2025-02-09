namespace ChronotrackService.Application
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserEntity> _users = new List<UserEntity>
        {
            new UserEntity { Id = 1, Email = "test@example.com", Password = "password123" }
        };

        public Task<UserEntity> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }
    }
}
