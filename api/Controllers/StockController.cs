using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    //Route to navigate using controller
    [Route("api/stock")]
    [ApiController]
    //ControllerBase first, Attributes later
    public class StockController : ControllerBase
    {
        //private, readonly attribute
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {   
            _context = context;
        }
        
        //Get method
        [HttpGet]
        //Interface
        //Create List
        public IActionResult GetAll()
        {
            //ToList -> return a list like object
            //Make SQL go out to the database - deferred execution
            var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());    // Select StockDto. Select same as map
            return Ok(stocks);
        }
        
        //Create Detail - take "id" attribute from list of stocks in GetAll()
        [HttpGet("{id}")]
        //Create an API endpoint that will only return one actual item
        //Turn string of id into int, pass into stock;
        public IActionResult GetById([FromRoute] int id)
        {
            //Search by id
            //Return object (detail) that has the specific id.
            //Find from the list of stocks
            var stock = _context.Stocks.Find(id);
            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto()); //Select StockDto
        }

        [HttpPost]
        //body of json version of the object
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            //id = id of the new stock object at save changesl, return object in form stockDto type
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")] //specify ID
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if(stockModel == null) return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if(stockModel == null) return NotFound();
            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();
            
            //NoContent() is the good thumbs up (green one) for succeeded deleting
            return NoContent();
        }
    }
}