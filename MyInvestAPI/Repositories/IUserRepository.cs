using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(CreateUserViewModel userViewModel);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> GetUserWithAllPursesByIdAsync(int id);
    Task<User> GetUserWithAllPursesAndActivesByIdAsync(int id);
    void Update(int id, CreateUserViewModel userViewModel);
    void Delete(int id);
}
