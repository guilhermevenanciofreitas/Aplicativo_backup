using Aplicativo.Utils.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aplicativo.Server
{
    public class Context : DbContext
    {

        private string ConnectionString { get; set; } = @"Server=localhost\AtlantaSistemas;Database=Dev;User Id=sa;Password=@Rped94ft";

        public Context()
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<EmpresaEndereco> EmpresaEndereco { get; set; }

        public DbSet<Endereco> Endereco { get; set; }

        public DbSet<Estado> Estado { get; set; }

        public DbSet<Municipio> Municipio { get; set; }

        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}