using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services.Interfaces;
using ProductApi.Domain.Common;

namespace ProductApi.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Lista os produtos de acordo com o filtro
        /// </summary>
        /// <param name="filter">Objeto JSON usado para especificar os critérios de filtragem.<br /> 
        /// Caso a string com o filtro esteja vazia, retornará todos produtos.<br /> 
        /// Só é possivel um filtro por vez devido ao uso do InMemory como banco de dados assim ñão consigo usar fromsqlraw com inMemory<br />
        /// Todos campos do json são opcionais. Ex: {"order": "name", "ascending":false} ou {"id": 1} ou  {"name": "notebook"}<br /></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> Get(string? filter)
        {
            var products = await _productService.ListAsync(string.IsNullOrEmpty(filter) ? null : JsonConvert.DeserializeObject<Filter>(filter));
            if (products == null)
                return NotFound("Products not found");
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO ProductDto)
        {
            if (ProductDto == null)
                return BadRequest("Invalid Data");

            var result = await _productService.AddAsync(ProductDto);
            if (!result) 
                return new ConflictObjectResult("Product already exists");
            return new CreatedAtRouteResult("GetProduct", new { id = ProductDto.Id });
        }

        [HttpPut]

        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO ProductDto)
        {
            if (ProductDto == null)
                return BadRequest("Invalid Data");

            if (id != ProductDto.Id)
                return BadRequest("Invalid Id");            

            var result = await _productService.UpdateAsync(ProductDto);
            if (!result)
                return new NotFoundObjectResult("Product not found");
            return Ok(ProductDto);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return new NotFoundObjectResult("Product not found");
            return new OkResult();
        }

    }
}
