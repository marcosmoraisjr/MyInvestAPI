using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.ViewModels;
using System;

namespace MyInvestAPI.Repositories
{
    public class ActiveRepository : IActiveRepository
    {
        public readonly MyInvestContext _context;
        public readonly ILogger<ActiveRepository> _logger;

        public ActiveRepository(MyInvestContext context, ILogger<ActiveRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Active> CreateAsync(CreateActiveViewModel activeViewModel)
        {
            var purse = await _context.Purses.FirstOrDefaultAsync(purse => purse.Purse_Id.Equals(activeViewModel.Purse_Id));

            if (purse is null)
                throw new HttpResponseException(404, $"The purse with id {activeViewModel.Purse_Id} not found!");

            Active active = activeViewModel.CreateActive(purse);

            try
            {
                _context.Actives.Add(active);
                await _context.SaveChangesAsync();
                return active;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An Error occured when tryning create active! err: {ex.Message}");
                throw new HttpResponseException(500, "An Error occured when tryning create active!");
            }
        }

        public async Task<IEnumerable<Active>> GetAllAsync()
        {
            return await _context.Actives
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Active>> GetAllWithPursesAsync()
        {
            return await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .ToListAsync();
        }

        public async Task<Active> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "The ID must be greater than 0!");

            var active = await _context.Actives
                .AsNoTracking()
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"The active with id {id} not found!");

            return active;
        }

        public async Task<Active> GetByIdWithPursesAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "The ID must be greater than 0!");

            var active = await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"The active with id {id} not found!");

            return active;
        }

        public void Update(int id, UpdateActiveViewModel updateActiveViewModel)
        {
            var activeVerify = _context.Actives.FirstOrDefault(active => active.Active_Id.Equals(id));

            if (activeVerify is null)
                throw new HttpResponseException(404, $"The active with id {id} not found!");

            var active = updateActiveViewModel.UpdateActive(activeVerify);

            try
            {
                _context.Entry(active).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured when tryning to update the user! err: {ex.Message}");
                throw new HttpResponseException(500, "An Erro occured when tryning update active!");;
            }
        }

        public void Delete(int id)
        {
            Active active = _context.Actives
                .FirstOrDefault(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"The active with id {id} not found!");

            try
            {
                _context.Actives.Remove(active);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured when tryning to delete the user! err: {ex.Message}");
                throw new HttpResponseException(500, "An Erro occured when tryning delete active!"); ;
            }
        }

        public async Task<ActiveReturn> SearchActiveAsync(string active, string dYDesiredPercentage)
        {
            try
            {
                return await YahooFinanceApiClient.GetActive(active, dYDesiredPercentage);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"The active {active} not found! err: {ex.Message}");
                throw new HttpResponseException(404, $"The active {active} not found!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un error occured when tryning search actives! err: {ex.Message}");
                throw new HttpResponseException(500, "Un error occured when tryning search actives");
            }
        }

        public async Task<Purse> GetActivesByPurseId(int purseId)
        {
            var Purse = await _context.Purses
                .Include(p => p.Actives)
                .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

            if (Purse is null || !Purse.Actives.Any())
                throw new HttpResponseException(404, $"The purse with id {purseId} not found!");

            return Purse;
        }
    }
}
