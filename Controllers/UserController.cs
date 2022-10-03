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
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetAsync(
           [FromServices] AppDataContext context,
           [FromQuery]CreateUserListViewModel list)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
               var data = await context
                .Users
                .AsNoTracking()
                .ToListAsync();


                return data == null
                    ? NotFound()
                    : Ok(data);

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("Users/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var users = await context
                .Users             
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return users == null
                ? NotFound()
                : Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDataContext context,
            [FromBody] CreateUserViewModel model)

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
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Created($"v1/users/{user.Id}", user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> PutAsync(
           [FromServices] AppDataContext context,
           [FromBody] CreateUserViewModel model,
           [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (data == null)
                return NotFound();

          
            try
            {
                data.Name= model.Name;
                data.Cpf= model.Cpf;
                data.Login= model.Login;  
                data.Password= model.Password;

                context.Users.Update(data);
                await context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var users = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Users.Remove(users);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
