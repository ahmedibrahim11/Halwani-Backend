﻿using Halwani.Data.Entities.AssoicationTikcet;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.location;
using Halwani.Data.Entities.Message;
using Halwani.Data.Entities.Notification;
using Halwani.Data.Entities.ProductCategories;
using Halwani.Data.Entities.ResolutionCategories;
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
        public virtual DbSet<TicketMessage> TicketMessages { get; set; }
        #endregion

        #region SLA
        public virtual DbSet<SLA> SLAs { get; set; }
        #endregion
        
        #region Tickets
        public virtual DbSet<SLmMeasurement> SLmMeasurements { get; set; }
        #endregion

        #region Team
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<UserTeams> UserTeams { get; set; }
        #endregion

        #region Notification
        public virtual DbSet<Notification> Notifications { get; set; }
        #endregion
        
        #region Users
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        #endregion

        #region [ProductCategories]
        public DbSet<ProductCategory> ProductCategories { get; set; }
        #endregion

        #region [ResolutionCategories]
        public DbSet<ResolutionCategory> ResolutionCategories { get; set; }
        #endregion
        public DbSet<Location> Locations { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=tcp:ticketsyslive.database.windows.net,1433;Initial Catalog=TSLive;Persist Security Info=False;User ID=TS;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=.;Database=Halwani;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_100_CI_AI");

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_ProductCategories_ProductCategories_ParentId");
            });
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
                
                entity.Property(e => e.TeamName)
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
