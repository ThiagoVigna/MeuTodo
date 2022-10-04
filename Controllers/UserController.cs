using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class UserController : ControllerBase
    {
        private AppDataContext _context;
        public UserController(AppDataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult<IEquatable<Users>>> GetAsync()
        {
            var data = await _context
                .Users
                .AsNoTracking()
                .ToListAsync();

                return data is null
                    ? NotFound()
                    : Ok(data);
        }

        [HttpGet]
        [Route("Users/{id}")]
        public async Task<ActionResult<Users>> GetByIdAsync([FromRoute] int id)
        {
            Users users = await _context.Users.FindAsync(id);

            if(users is null)
                return NotFound();

            return users;
        }

        [HttpPost("users")]
        public async Task<ActionResult<Users>> PostAsync([FromBody] CreateUserViewModel model)

        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new Users
            {
                Name = model.Name,
                Cpf = model.Cpf,
                Login = model.Login,
                Password = model.Password
            };

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Created($"v1/users/{user.Id}", user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult> PutAsync([FromBody] CreateUserViewModel model,[FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (data is null)
                return NotFound();
          
            try
            {
                data.Name= model.Name;
                data.Cpf= model.Cpf;
                data.Login= model.Login;  
                data.Password= model.Password;

                _context.Users.Update(data);
                await _context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Users.Remove(users);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
