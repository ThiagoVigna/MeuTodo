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
        private AppDataContext _context;
        public ClientsController(AppDataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("client")]
        public async Task<IActionResult> GetAsync()
        {
            var data = await _context
                 .Clients
                 .AsNoTracking()
                 .ToListAsync();

                return data is null
                    ? NotFound()
                    : Ok(data);
        }

        [HttpGet]
        [Route("client/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var client = await _context
                .Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return client is null
                ? NotFound()
                : Ok(client);
        }

        [HttpPost("client")]
        public async Task<IActionResult> PostAsync([FromBody] CreateClientViewModel model)
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
                await _context.Clients.AddAsync(clients);
                await _context.SaveChangesAsync();
                return Created($"v1/client/{clients.Id}", clients);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("client/{id}")]
        public async Task<IActionResult> PutAsync([FromBody] CreateClientViewModel model, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await _context.Clients.FirstOrDefaultAsync(x => x.Id == Id);

            if (data is null)
                return NotFound();

            try
            {
                data.SocialReason = model.SocialReason;
                data.Cnpj = model.Cnpj;

                _context.Clients.Update(data);
                await _context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("client/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var users = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Clients.Remove(users);
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
