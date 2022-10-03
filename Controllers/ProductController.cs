﻿using MeuTodo.Data;
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
        [HttpGet]
        [Route("product")]
        public async Task<IActionResult> GetAsync(
           [FromServices] AppDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var data = await context
                 .Products
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
        [Route("product/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var data = await context
                .Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return data == null
                ? NotFound()
                : Ok(data);
        }

        [HttpPost("product")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDataContext context,
            [FromBody] CreateProductViewModel model)

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
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                return Created($"v1/product/{product.Id}", product);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("product/{id}")]
        public async Task<IActionResult> PutAsync(
           [FromServices] AppDataContext context,
           [FromBody] CreateProductViewModel model,
           [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);

            if (data == null)
                return NotFound();


            try
            {
                data.NameProduct = model.NameProduct;
                data.Price = model.Price;

                context.Products.Update(data);
                await context.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDataContext context,
            [FromRoute] int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Products.Remove(product);
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

