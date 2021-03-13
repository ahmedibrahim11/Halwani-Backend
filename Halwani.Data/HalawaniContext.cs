using Halwani.Data.Entities.AssoicationTikcet;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Data.Entities.SLM_Measurement;
using Halwani.Data.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data
{
    public partial class HalawaniContext : DbContext
    {
        public HalawaniContext()
        {
        }

        public HalawaniContext(DbContextOptions<HalawaniContext> options)
            : base(options)
        {
        }

        #region DbSets

        #region Assoication
        public virtual DbSet<Assoication> Assoications { get; set; }
        #endregion

        #region Tickets
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketHistory> TicketHistories { get; set; }
        #endregion
        
        #region Tickets
        public virtual DbSet<SLA> SLAs { get; set; }
        #endregion
        
        #region Tickets
        public virtual DbSet<SLmMeasurement> SLmMeasurements { get; set; }
        #endregion

        #region Users
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        #endregion

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();    //UseLazyLoadingProxies

            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SCOTApp;Integrated Security=True");

                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=db.expertapps.com.sa\\MSSQL14,1444;Database=SCOTApp;User ID=sa;Password=ExpApps-14;Trusted_Connection=False;MultipleActiveResultSets=true;ApplicationIntent=ReadWrite");
                //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=db.expertapps.com.sa\\MSSQL14,1444;Database=SCOTAppzTest;User ID=sa;Password=ExpApps-14;Trusted_Connection=False;MultipleActiveResultSets=true;ApplicationIntent=ReadWrite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_100_CI_AI");

            modelBuilder.Entity<Assoication>(entity =>
            {
                entity.Property(e => e.Sumbitter).IsRequired().HasMaxLength(150);

                entity.Property(e => e.FormName).IsRequired().HasMaxLength(150);

                entity.Property(e => e.DependandForm)
                    .IsRequired()
                    .HasMaxLength(150);
                
                entity.Property(e => e.DependandRequestId)
                    .IsRequired()
                    .HasMaxLength(150);

                //entity.HasOne(d => d.Ticket)
                //    .WithMany(p => p.TicketNo)
                //    .HasForeignKey(d => d.RegionID)
                //    .HasConstraintName("FK_City_Region");
            });
            
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.TicketName).IsRequired().HasMaxLength(500);

                entity.Property(e => e.SubmitterTeam).IsRequired().HasMaxLength(150);

                entity.Property(e => e.SubmitterEmail)
                    .IsRequired()
                    .HasMaxLength(150);
                
                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(150);
                
                entity.Property(e => e.ReportedSource)
                    .IsRequired()
                    .HasMaxLength(150);

                //entity.HasOne(d => d.Ticket)
                //    .WithMany(p => p.TicketNo)
                //    .HasForeignKey(d => d.RegionID)
                //    .HasConstraintName("FK_City_Region");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
