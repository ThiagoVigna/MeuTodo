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
        public async Task<IActionResult> GetAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest();

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
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var users = await _context
                .Users             
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return users is null
                ? NotFound()
                : Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserViewModel model)

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
        public async Task<IActionResult> PutAsync([FromBody] CreateUserViewModel model,[FromRoute] int Id)
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
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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
