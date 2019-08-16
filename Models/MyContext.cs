using Microsoft.EntityFrameworkCore;

namespace LoginReggy.Models
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<UserReg> UserRegs {get; set;}
    }
}