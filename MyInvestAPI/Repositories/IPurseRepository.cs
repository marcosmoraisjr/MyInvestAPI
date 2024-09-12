using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;
using System.Diagnostics.Contracts;

namespace MyInvestAPI.Repositories;

public interface IPurseRepository
{
    Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel);
    Task<IEnumerable<Purse>> GetAllAsync();
    Task<IEnumerable<Purse>> GetAllWithActivesAsync();
    Task<Purse> GetByIdAsync(int id);
    Task<Purse> GetByIdWithActivesAsync(int id);
    void Update(int id, UpdatePurseViewModel updatePurseViewModel);
    void Delete(int id); 
}
