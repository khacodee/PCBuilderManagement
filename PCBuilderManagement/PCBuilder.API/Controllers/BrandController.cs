using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Services.DTO;
using PCBuilder.Services.Service;

namespace PCBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices _brandServices;

        public BrandController(IBrandServices brandServices)
        {
            _brandServices = brandServices;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandServices.GetBrandsAsync();
            return Ok(brands);
        }

        // GET: api/Brand/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandServices.GetBrandByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        // POST: api/Brand
        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandDTO brandDTO)
        {
            var response = await _brandServices.CreateBrandAsync(brandDTO);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetBrandById), new { id = response.Data.Id }, response.Data);
            }

            return BadRequest(response.Message);
        }

        // PUT: api/Brand/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, BrandDTO brandDTO)
        {
            if (id != brandDTO.Id)
            {
                return BadRequest("Invalid brand ID");
            }

            var response = await _brandServices.UpdateBrandAsync(id, brandDTO);

            if (response.Success)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Message);
        }

        // DELETE: api/Brand/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var response = await _brandServices.DeleteBrandAsync(id);

            if (response.Success)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }
    }

}
