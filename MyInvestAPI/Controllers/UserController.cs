using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly MyInvestContext _context;

        public UserController(MyInvestContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserViewModel userViewModel)
        {
            if (userViewModel is null)
                return BadRequest("The User body must not be null.");

            User user = userViewModel.CreateUser();

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("GetUser", new { id = user.User_Id }, user);
            }
            catch(Exception)
            {
                return StatusCode(500, "An error occured when tryning to create the user");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("purses")]
        public async Task<IEnumerable<User>> GetAllUsersWithPurses()
        {
            return await _context.Users
                .Include(user => user.Purses)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("purses/actives")]
        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActives()
        {
            return await _context.Users
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("{id:int}", Name ="GetUser")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.User_Id == id);

            if (user is null)
                return BadRequest("User not found.");

            return Ok(user);
        }

        [HttpGet("{id:int}/purses")]
        public async Task<ActionResult<User>> GetUserWithAllPurses(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                .FirstOrDefaultAsync(user => user.User_Id.Equals(id));

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpGet("{id:int}/purses/actives")]
        public async Task<ActionResult<User>> GetUserWithAllPursesAndActives(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                .FirstOrDefaultAsync(user => user.User_Id.Equals(id));

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateUserViewModel userViewModel)
        {
            if (userViewModel is null)
                return BadRequest("The data for update must not be null.");

            var userVerify = _context.Users.FirstOrDefault(user => user.User_Id == id);

            if (userVerify is null)
                return NotFound("User not found.");

            User user = userViewModel.UpdateUser(userVerify);

            try
            { 
                _context.Entry(user).State = EntityState.Modified;
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
            var user = _context.Users.FirstOrDefault(user => user.User_Id == id);

            if (user is null)
                return NotFound($"User with id {id} not found.");

            try
            {
                _context.Users.Remove(user);
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
