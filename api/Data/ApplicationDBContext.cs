using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }

        //Grab sth out of database, return data in some form
        //Manipulating the whole entire stock table
        //By Migrations of Entity Frameworks, Dbset turn Stocks and Comments into object 
        //of tables(Stock and Comment) datas from database 
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}