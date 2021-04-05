using Logic.Payments.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public class EfPaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Student> Students { get; set; }


        public EfPaymentDbContext(DbContextOptions<EfPaymentDbContext> contextOptions) : base(contextOptions) { }
    }
}
