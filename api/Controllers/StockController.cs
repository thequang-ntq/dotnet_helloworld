using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    //Route to navigate using controller
    [Route("api/stock")]
    [ApiController]
    //ControllerBase first, Attributes later
    public class StockController : ControllerBase
    {
        //private, readonly attribute
        //use DI - Dependency injection
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        //constructor injection with variable stockRepo
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {   
            _context = context;
            _stockRepo = stockRepo;
        }
        
        //Get method. Make async to be faster. Async hard to debug
        [HttpGet]
        //Interface
        //Create List
        public async Task<IActionResult> GetAll()
        {
            //ToList -> return a list like object
            //Make SQL go out to the database - deferred execution
            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());    // Select StockDto. Select same as map
            return Ok(stocks);
        }
        
        //Create Detail - take "id" attribute from list of stocks in GetAll()
        [HttpGet("{id}")]
        //Create an API endpoint that will only return one actual item
        //Turn string of id into int, pass into stock;
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //Search by id
            //Return object (detail) that has the specific id.
            //Find from the list of stocks
            var stock = await _context.Stocks.FindAsync(id);
            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto()); //Select StockDto
        }

        [HttpPost]
        //body of json version of the object
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            //id = id of the new stock object at save changesl, return object in form stockDto type
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")] //specify ID
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null) return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null) return NotFound();
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            
            //NoContent() is the good thumbs up (green one) for succeeded deleting
            return NoContent();
        }
    }
}