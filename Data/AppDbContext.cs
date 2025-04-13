using Microsoft.EntityFrameworkCore;
using JbFinanceAPI.Models;

namespace JbFinanceAPI.Data
{
    // Está virando um gerenciador de banco de dados
    public class AppDbContext : DbContext
    {
        // Construtor para saber qual banco de dados iremos utilizar
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Pagamento> Pagamentos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
