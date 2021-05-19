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
    public class BookingsHistoryController : ControllerBase
    {
        private readonly IBookingsHistoryService _bookingsService;
        public BookingsHistoryController(IBookingsHistoryService bookingsService)
        {
            _bookingsService = bookingsService;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddPlace(ApplicationCore.Models.Request.BookingsHistory bookingsHistory)
        {
            var result = await _bookingsService.Add(bookingsHistory);
            return result != null ? Ok(result) : NotFound("Please input your info");
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            await _bookingsService.Delete(id);
            return Ok(new { token = "Deleted" }); ;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListAllPlace()
        {
            var result = await _bookingsService.GetAllAsync();
            return result != null ? Ok(result) : NotFound("No bookings Found");
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdPlace(int id)
        {
            var result = await _bookingsService.GetById(id);
            return result != null ? Ok(result) : NotFound("No bookings Found");
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> UpdatePlace(ApplicationCore.Models.Request.BookingsHistory bookings)
        {
            var result = await _bookingsService.Edit(bookings);
            return result != null ? Ok(result) : NotFound("Please input your info");
        }
    }
}
