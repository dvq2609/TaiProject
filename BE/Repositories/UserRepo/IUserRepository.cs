using BE.Models;

namespace BE.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByEmailConfirmationTokenAsync(string token);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
