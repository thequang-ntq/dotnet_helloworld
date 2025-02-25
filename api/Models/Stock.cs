using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Stock
    {
        //ai - auto increment
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        // Add data, when input decimal, make sure it's only a monetary amount
        // Force SQL database to limit it to 18 digit, 2 decimal places
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        //Invest in stock => It pays a dividend
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        //Market cap is the whole entire value of the company
        //Value can up to trillion (one hundred billion)
        public long MarketCap { get; set; }
        //One to many relationship - Comment. A stock can have multiple comments
        public List<Comment> Comment { get; set; } = new List<Comment>();
    }
}