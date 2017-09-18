using Microsoft.EntityFrameworkCore;
using TheCircle.Models;

namespace TheCircle.Util
{
    public class MyDbContext : DbContext
    {
        public virtual DbSet<Remision> Remision { get; set; }
        public virtual DbSet<Apadrinado> Apadrinados { get; set; }
        public virtual DbSet<ItemFarmacia> ItemFarmacias { get; set; }
        public virtual DbSet<Foto> Fotos { get; set; }
        public virtual DbSet<Enfermedad> Enfermedades { get; set; }
        public virtual DbSet<Institucion> Instituciones { get; set; }
        public virtual DbSet<Atencion> Atenciones { get; set; }
        public virtual DbSet<Receta> Recetas { get; set; }
        public virtual DbSet<ItemReceta> ItemsReceta { get; set; }
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; }
        public virtual DbSet<ReporteEnfermedad> ReporteEnfermedad { get; set; }
        public virtual DbSet<ItemDespacho> ItemDespacho { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UserSafe> UserSafe { get; set; }
        public virtual DbSet<Compuesto> Compuesto { get; set; }
        public virtual DbSet<Transferencia> Transferencia { get; set; }        
        public virtual DbSet<PedidoInterno> PedidoInterno { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Data Source=GUYSRV08\\SOAAPPS;Initial Catalog=TheCircle;Integrated Security=False;User ID=thecircle_user;Password=Th3C1rCl3_cI;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
