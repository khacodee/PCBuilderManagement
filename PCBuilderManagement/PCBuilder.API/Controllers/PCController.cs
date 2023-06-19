using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Repository.Model;
using PCBuilder.Services.DTO;
using PCBuilder.Services.Service;

namespace PCBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCController : ControllerBase
    {
        private readonly IPCServices _IPCServices;
        public PCController(IPCServices IPCServices)
        {
            _IPCServices = IPCServices;
        }
        [HttpGet("GetListByCustomer")]
        public async Task<IActionResult> GetPCListByCustomer()
        {
            var PCs = await _IPCServices.GetPCListByCustomer();
            if(PCs == null)
            {
                return NotFound();
            }
            return Ok(PCs);
        }
        [HttpGet("GetListByAdmin")]
        public async Task<IActionResult> GetPCListByAdmin()
        {
            var PCs = await _IPCServices.GetPCListByAdmin();
            if (PCs == null)
            {
                return NotFound();
            }
            return Ok(PCs);
        }
        [HttpGet("PCWithComponent")]
        public async Task<IActionResult> GetPCComponent()
        {
            var response = await _IPCServices.GetPCComponent();

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("PCWithComponent/{PcId}")]
        public async Task<IActionResult> GetPcComponentById(int PcId)
        {
            var response = await _IPCServices.GetPCComponentByID(PcId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }



        [HttpGet("{PcId}")]
        public async Task<IActionResult> GetPcByIdList(int PcId)
        {
            var pc = await _IPCServices.GetPCByID(PcId);

            if (pc == null)
            {
                return NotFound();
            }

            return Ok(pc);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePC([FromBody] PcDTO pcCreateDTO)
        {
            var response = await _IPCServices.CreatePC(pcCreateDTO);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePC(int id, [FromBody] PcDTO pcUpdateDTO)
        {
            var response = await _IPCServices.UpdatePC(id, pcUpdateDTO);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePC(int id)
        {
            var response = await _IPCServices.DeletePC(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpPost("{PcId}/components")]
        public async Task<IActionResult> AddComponentsToPC(int PcId,List<int> componentIds)
        {
            var response = await _IPCServices.AddComponentsToPC(PcId, componentIds);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
