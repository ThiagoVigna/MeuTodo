using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ClientsController : ControllerBase
    {
        [HttpGet]
        [Route("client")]
        public async Task<IActionResult> GetAsync(
           [FromServices] AppDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var data = await context
                 .Clients
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
        [Route("client/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var client = await context
                .Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return client == null
                ? NotFound()
                : Ok(client);
        }

        [HttpPost("client")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDataContext context,
            [FromBody] CreateClientViewModel model)

        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clients = new Clients
            {
                SocialReason = model.SocialReason,
                Cnpj = model.Cnpj,
            };

            try
            {
                await context.Clients.AddAsync(clients);
                await context.SaveChangesAsync();
                return Created($"v1/client/{clients.Id}", clients);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("client/{id}")]
        public async Task<IActionResult> PutAsync(
           [FromServices] AppDataContext context,
           [FromBody] CreateClientViewModel model,
           [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await context.Clients.FirstOrDefaultAsync(x => x.Id == Id);

            if (data == null)
                return NotFound();


            try
            {
                data.SocialReason = model.SocialReason;
                data.Cnpj = model.Cnpj;

                context.Clients.Update(data);
                await context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete("client/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var users = await context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Clients.Remove(users);
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
