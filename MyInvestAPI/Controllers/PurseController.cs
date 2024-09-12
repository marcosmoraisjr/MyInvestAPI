using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurseController : ControllerBase
    {
        public readonly IPurseRepository _repository;

        public PurseController(IPurseRepository IPurseRepository)
        {
            _repository = IPurseRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Purse>> Create(CreatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for create purse must not be null.");

            var purseCreated = await _repository.CreateAsync(purseViewModel);
                
            return new CreatedAtRouteResult("GetPurse", new { id = purseCreated.Purse_Id }, purseCreated);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("actives")]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAllWithActives()
        {
            return Ok(_repository.GetAllWithActivesAsync());
        }

        [HttpGet("{id:int}", Name = "GetPurse")]
        public async Task<ActionResult<Purse>> getById(int id)
        {
            return Ok(await _repository.GetByIdAsync(id));
        }

        [HttpGet("{id:int}/actives")]
        public async Task<ActionResult<Purse>> getByIdWithActives(int id)
        {
            return Ok(await _repository.GetByIdWithActivesAsync(id));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for update purse must not be null.");

            _repository.Update(id, purseViewModel);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
