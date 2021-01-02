
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TemplateNBL.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }


        public System.Data.Entity.DbSet<Models.Audit> Audits { get; set; }


        public System.Data.Entity.DbSet<Models.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<Models.Permission> Permissions { get; set; }


        public System.Data.Entity.DbSet<Models.Rol> Rols { get; set; }



        public System.Data.Entity.DbSet<Models.Usuario> Usuarios { get; set; }


        public System.Data.Entity.DbSet<Models.Window> Windows { get; set; }

        public System.Data.Entity.DbSet<Models.Status> Status { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Act>()
            //.HasMany(c => c.)
            //.WithOptional()
            //.Map(m => m.MapKey("ClaimId"));


            //modelBuilder.Entity<Order>()
            //.HasMany<OrderProduct>(c => c.OrderProducts)
            //.WithOptional(x => x.Order)
            //.WillCascadeOnDelete(true);

        }


       



        public System.Data.Entity.DbSet<Models.Setting> Settings { get; set; }
        public System.Data.Entity.DbSet<Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<Models.ProductType> ProductTypes { get; set; }
        public System.Data.Entity.DbSet<Models.Center> Centers { get; set; }
        public System.Data.Entity.DbSet<Models.SupplieMedical> SupplieMedicals { get; set; }
        public System.Data.Entity.DbSet<Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<Models.OrderProduct> OrderProducts { get; set; }

        public System.Data.Entity.DbSet<Models.CenterProduct> CenterProducts { get; set; }

        public System.Data.Entity.DbSet<Models.OrderTracking> OrderTrackings { get; set; }



    }
}