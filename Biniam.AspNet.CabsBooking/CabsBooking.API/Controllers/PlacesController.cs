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
    public class PlacesController : ControllerBase
    {
        private readonly IPlacesService _placesService;
        public PlacesController(IPlacesService placesService)
        {
            _placesService = placesService;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddPlace(ApplicationCore.Models.Request.Places places) 
        {
            var result =await _placesService.Add(places);
            return result != null? Ok(result) : NotFound("Please input your info");
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            await _placesService.Delete(id);
            return Ok(new { token = "Deleted" }); ;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListAllPlace()
        {
            var result = await _placesService.GetAllAsync();
            return result != null ? Ok(result) : NotFound("No Places Found");
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdPlace(int id)
        {
            var result = await _placesService.GetById(id);
            return result != null ? Ok(result) : NotFound("No Places Found");
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> UpdatePlace(ApplicationCore.Models.Request.Places places)
        {
            var result = await _placesService.Edit(places);
            return result != null ? Ok(result) : NotFound("Please input your info");
        }

    }
}
