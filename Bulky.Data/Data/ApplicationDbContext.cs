using Bulky.Models;
using BUlky.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BUlky.DataAccess
{
    public class ApplicationDbContext :DbContext  
    {                                                                       
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext>options):base(options) 
        {
            
        }
        //this line creates table in DataBase V
        public DbSet<Category> Categories { get; set; }

        //to create,insert into categories
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci-fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 },
                new Category { Id = 4, Name = "hi", DisplayOrder = 3 }


                );
        }
    }
}
