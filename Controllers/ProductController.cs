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
    public class ProductController : ControllerBase
    {
        private AppDataContext _context;
        public ProductController(AppDataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("product")]
        public async Task<IActionResult> GetAsync()
        {
            var data = await _context
             .Products
             .AsNoTracking()
             .ToListAsync();

            return data is null
                ? NotFound()
                : Ok(data);
        }

        [HttpGet]
        [Route("product/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var data = await _context
                .Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return data is null
                ? NotFound()
                : Ok(data);
        }

        [HttpPost("product")]
        public async Task<IActionResult> PostAsync([FromBody] CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = new Product
            {
                NameProduct = model.NameProduct,
                Price = model.Price,
            };

            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return Created($"v1/product/{product.Id}", product);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("product/{id}")]
        public async Task<IActionResult> PutAsync([FromBody] CreateProductViewModel model,[FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);

            if (data is null)
                return NotFound();

            try
            {
                data.NameProduct = model.NameProduct;
                data.Price = model.Price;

                _context.Products.Update(data);
                await _context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Products.Remove(product);
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

