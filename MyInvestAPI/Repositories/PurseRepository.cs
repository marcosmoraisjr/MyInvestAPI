using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories;

public class PurseRepository : IPurseRepository
{
    public readonly MyInvestContext _context;
    public readonly ILogger<PurseRepository> _logger;

    public PurseRepository(MyInvestContext context, ILogger<PurseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel)
    {
        var userVerify = await _context.Users.FirstOrDefaultAsync(u => u.User_Id == purseViewModel.User_Id);

        if (userVerify is null)
            throw new HttpResponseException(404, $"The user with ID {purseViewModel.User_Id} not found!");

        Purse purse = purseViewModel.CreatePurse();

        try
        {
            await _context.Purses.AddAsync(purse);
            await _context.SaveChangesAsync();
            return purse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when tryning create user! ex: {ex.Message}");
            throw new HttpResponseException(500, "An error occured when tryning create purse");
        }
    }


    public async Task<IEnumerable<Purse>> GetAllAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Purse>> GetAllWithActivesAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Include(p => p.Actives)
            .ToListAsync();
    }

    public async Task<Purse> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new HttpResponseException(400, "The ID must be greater than 0!");

        var purse = await _context.Purses
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        return purse;
    }

    public async Task<Purse> GetByIdWithActivesAsync(int id)
    {
        if (id <= 0)
            throw new HttpResponseException(400, "The ID must be greater than 0!");

        var purse = await _context.Purses
            .Include(p => p.Actives)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        return purse;
    }

    public void Update(int id, UpdatePurseViewModel updatePurseViewModel)
    {
        var purseVerify = _context.Purses.FirstOrDefault(p => p.Purse_Id == id);

        if (purseVerify is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        Purse purse = updatePurseViewModel.UpdatePurse(purseVerify);

        try
        {
            _context.Entry(purse).State = EntityState.Modified;
            _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning update purse! err: {ex.Message}");
            throw new HttpResponseException(500, "An Erro occured when tryning update purse!");
        }
    }

    public void Delete(int id)
    {
        var purse = _context.Purses.FirstOrDefault(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        try
        {
            _context.Purses.Remove(purse);
            _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning delete purse! err: {ex.Message}");
            throw new HttpResponseException(500, "An Erro occured when tryning delete purse!");
        }
    }
}
