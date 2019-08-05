using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Persistence.Classes
{
    public class AddContext : DbContext
    {
        IOptions<DataBase> _options;
        public AddContext(IOptions<DataBase> options)
        {
            _options = options;
        }
        public DbSet<Contract> Contracts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_options.Value.ConnectionString);


    }
}
