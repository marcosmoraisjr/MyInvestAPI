using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly MyInvestContext _context;
        public readonly ILogger<UserRepository> _logger;

        public UserRepository(MyInvestContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> CreateAsync(CreateUserViewModel userViewModel)
        {
            User user = userViewModel.CreateUser();

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            } 
            catch(Exception ex)
            {
                _logger.LogError($"An error occured when tryning to create the user! err: {ex.Message}");
                throw new HttpResponseException(500, "An error occured when tryning to create the user");
            }
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAsync()
        {
            return await _context.Users
                .Include(user => user.Purses)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync()
        {
            return await _context.Users
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "The ID must be greater than 0!");

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.User_Id == id);

            if (user is null)
                throw new HttpResponseException(404, $"The user with id {id} not found!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "The ID must be greater than 0!");

            var user = await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                .FirstOrDefaultAsync(user => user.User_Id == id);

            if (user is null)
                throw new HttpResponseException(404, $"The user with id {id} not found!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesAndActivesByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "The ID must be greater than 0!");

            var user = await _context.Users
                .Include(user => user.Purses)
                .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.User_Id == id);

            if (user is null)
                throw new HttpResponseException(404, $"The user with id {id} not found!");

            return user;
        }

        public void Update(int id, CreateUserViewModel userViewModel)
        {
            User userVerify = _context.Users.FirstOrDefault(user => user.User_Id == id);

            if (userVerify == null)
                throw new HttpResponseException(404, $"The user with ID {id} not found!");

            User user = userViewModel.UpdateUser(userVerify);

            try
            {
                _context.Entry(userVerify).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar atualizar o usuário! err: {err.Message}");
                throw new HttpResponseException(500, "An error occured when tryning to update the user");
            }
        }

        public void Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(user => user.User_Id == id);

            if (user == null)
                throw new HttpResponseException(404, $"The user with ID {id} not found!");

            try
            {
                _context.Users.Remove(user);
                _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar deletar o usuário! err: {ex.Message}");
                throw new HttpResponseException(500, "An error occured when tryning to delete the user");
            }
        }
    }
}
