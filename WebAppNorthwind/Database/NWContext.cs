using Microsoft.EntityFrameworkCore;

namespace WebAppNorthwind.Database
{
    public class NWContext :DbContext
    {
        public DbSet<Customers> Customers => Set<Customers>();
        private readonly IConfiguration _configuration;

        public NWContext(DbContextOptions<NWContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
       
    }
}
