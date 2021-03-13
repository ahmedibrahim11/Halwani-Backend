﻿using Halwani.Data.Entities.AssoicationTikcet;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Data.Entities.SLM_Measurement;
using Halwani.Data.Entities.Team;
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
        
        #region SLA
        public virtual DbSet<SLA> SLAs { get; set; }
        #endregion
        
        #region Tickets
        public virtual DbSet<SLmMeasurement> SLmMeasurements { get; set; }
        #endregion

        #region Team
        public virtual DbSet<Team> Teams { get; set; }
        #endregion
        
        #region Users
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        #endregion

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=.;Database=Halwani;Trusted_Connection=True;MultipleActiveResultSets=true");
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
