using ApplicationCore.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CabsBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CabTypesController : ControllerBase
    {
        private readonly ICabTypesService _cabTypesService;
        public CabTypesController(ICabTypesService cabTypesService)
        {
            _cabTypesService = cabTypesService ;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddcabTypes(ApplicationCore.Models.Request.CabTypes cabTypes)
        {
            var result = await _cabTypesService.Add(cabTypes);
            return result != null ? Ok(result) : NotFound("Please input your info");
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeletecabTypes(int id)
        {
            await _cabTypesService.Delete(id);
            return Ok(new { token = "Deleted" }); ;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListAllcabTypes()
        {
            var result = await _cabTypesService.GetAllAsync();
            return result != null ? Ok(result) : NotFound("No cabTypes Found");
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdcabTypes(int id)
        {
            var result = await _cabTypesService.GetById(id);
            return result != null ? Ok(result) : NotFound("No cabTypes Found");
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> UpdatecabTypes(ApplicationCore.Models.Request.CabTypes cabTypes)
        {
            var result = await _cabTypesService.Edit(cabTypes);
            return result != null ? Ok(result) : NotFound("Please input your info");
        }
    }
}
