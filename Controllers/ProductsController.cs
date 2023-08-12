using ManageProduct_Microservice.Models;
using ManageProduct_Microservice.Services;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManageProduct_Microservice.Controllers
{
    //[EnableCors("myCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Product> items = await productService.GetAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ProductsController>/5
        [HttpGet("{productName}")]
        [Authorize]
        public async Task<IActionResult> Get(string productName)
        {
            if (string.IsNullOrEmpty(productName)) { return BadRequest(); }
            try
            {
                Product item = await productService.GetByNameAsync(productName);
                if (item == null) { return NotFound(); }
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ProductsController>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Post([FromBody] Product newItem)
        {
            if (newItem == null) { return BadRequest(); }
            try
            {
                Product item = await productService.GetByNameAsync(newItem.Name);
                if (item != null) {
                    var data = new List<string>
                    {
                        newItem.Name,"Product already exist"
                    };
                    return BadRequest(data); }
                await productService.CreateAsync(newItem);
                return CreatedAtAction(nameof(Get), new {id = newItem.Id}, newItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Product item)
        {
            if (string.IsNullOrEmpty(id) || item == null) {
                return BadRequest();
            }
            try
            {
                await productService.UpdateAsync(id, item);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return BadRequest(); }

            try
            {
                Product product = await productService.GetByIdAsync(id);
                if (product == null) { return NotFound("Product not found"); }
                await productService.RemoveAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
