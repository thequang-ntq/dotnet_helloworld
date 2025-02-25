using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        // Link List<Comment> in Stock.cs to Comment.cs
        // Convention
        // StockId - actual key to from relationship within the database
        public int? StockId { get; set; } // Foreign Key
        // Relationship
        // Stock? -> Navigation property allow us to access into class Stock
        // Navigate within our models
        public Stock? Stock { get; set; }
    }
}