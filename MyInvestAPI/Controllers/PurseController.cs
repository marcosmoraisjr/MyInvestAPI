using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurseController : ControllerBase
    {
        public readonly MyInvestContext _context;

        public PurseController(MyInvestContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Purse>> Create(CreatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for create purse must not be null.");

            Purse purse = purseViewModel.CreatePurse();

            try
            {
                await _context.Purses.AddAsync(purse);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("GetPurse", new { id = purse.Purse_Id }, purse);
            }
            catch (Exception)
            {
                return BadRequest("An error ocurred when tryning to create the user.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAll()
        {
            return await _context.Purses
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("actives")]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAllWithActives()
        {
            return await _context.Purses
                .Include(purse => purse.Actives)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("{id:int}", Name = "GetPurse")]
        public async Task<ActionResult<Purse>> getById(int id)
        {
            var purse = await _context.Purses
                .AsNoTracking()
                .FirstOrDefaultAsync(purse => purse.Purse_Id == id);

            if (purse is null)
                return NotFound($"Purse with id {id} not found.");

            return Ok(purse);
        }

        [HttpGet("{id:int}/actives")]
        public async Task<ActionResult<Purse>> getByIdWithActives(int id)
        {
            var purse = await _context.Purses
                .AsNoTracking()
                .Include(purse => purse.Actives)
                .FirstOrDefaultAsync(purse => purse.Purse_Id == id);

            if (purse is null)
                return NotFound($"Purse with id {id} not found.");

            return Ok(purse);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for update purse must not be null.");

            var purseVerify = _context.Purses.FirstOrDefault(purse => purse.Purse_Id == id);

            if (purseVerify is null)
                return NotFound("Purse not found.");

            Purse purse = purseViewModel.UpdatePurse(purseVerify);

            try
            {
                _context.Entry(purse).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("An error occured when tryning to update the purse.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var purseVerify = await _context.Purses.FirstOrDefaultAsync(purse => purse.Purse_Id.Equals(id));

            if (purseVerify is null)
                return BadRequest("Purse not found.");

            try
            {
                _context.Purses.Remove(purseVerify);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("An error occured when tryning to delete the user");
            }
        }
    }
}
