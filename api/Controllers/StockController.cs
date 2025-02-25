using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
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
            var stocks = _context.Stocks.ToList();    
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

            return Ok(stock);
        }
    }
}