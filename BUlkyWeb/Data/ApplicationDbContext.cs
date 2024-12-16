using Microsoft.EntityFrameworkCore;

namespace BUlkyWeb.Data
{
    public class ApplicationDbContext :DbContext
    {                                                                       //base=> we want to pass that to the dbcontext(baseclass) so in c# base()
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext>options):base(options) 
        {
            
        }
    }
}
