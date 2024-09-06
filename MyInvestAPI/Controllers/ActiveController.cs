using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActiveController : ControllerBase
    {
        public readonly MyInvestContext _context;

        public ActiveController(MyInvestContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Action>> Create(CreateActiveViewModel activeViewModel)
        {
            if (activeViewModel is null)
                return BadRequest("The body for create a new active must not be null.");

            Purse purse = _context.Purses.FirstOrDefault(purse => purse.Purse_Id.Equals(activeViewModel.Purse_Id));

            if (purse is null)
                return NotFound("Purse not found.");

            Active active = activeViewModel.CreateActive(purse);

            try
            {
                _context.Actives.Add(active);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("GetActive", new { id = active.Active_Id }, active);
            }
            catch(Exception)
            {
                return BadRequest("An error occured when tryning to create the user");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Active>>> GetAll()
        {
            return await _context.Actives
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("purses")]
        public async Task<ActionResult<IEnumerable<Active>>> GetAllWithPurses()
        {
            return await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .ToListAsync();
        }

        [HttpGet("{id}", Name ="GetActive")]
        public async Task<ActionResult<Action>> GetById(int id)
        {
            var ActiveVerify = await _context.Actives
                .AsNoTracking()
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpGet("{id}/purses")]
        public async Task<ActionResult<Action>> GetByIdWithPurses(int id)
        {
            var ActiveVerify = await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateActiveViewModel activeViewModel)
        {
            var activeVerify = await _context.Actives.FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (activeVerify is null)
                return NotFound("Active not found.");

            var active = activeViewModel.UpdateActive(activeVerify);

            try
            {
                _context.Entry(active).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception)
            {
                return BadRequest("An error occured when tryning to update the user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var active = await _context.Actives
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                return NotFound("Active not found.");

            try
            {
                _context.Actives.Remove(active);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception)
            {
                return BadRequest("An error occured when tryning to delete the user");
            }
        }

        [HttpGet("/search-active/{active}")]
        public async Task<ActionResult<ActiveReturn>> GetActive(string active)
        {
            try
            {
                return await YahooFinanceApiClient.GetActive(active);
            }
            catch(Exception)
            {
                return NotFound("The active not found!");
            }
        }

        [HttpGet("/get-actives/{purseId}")]
        public async Task<ActionResult> GetActivesByPurseId(int purseId)
        {
            var actives = await _context.Purses
                .Include(p => p.Actives)
                .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

            if (actives is null || !actives.Actives.Any())
            {
                return NotFound("No actives found for the given purse ID.");
            }

            return Ok(actives);
        }
    }
}
