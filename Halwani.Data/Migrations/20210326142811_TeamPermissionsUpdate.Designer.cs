﻿// <auto-generated />
using System;
using Halwani.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Halwani.Data.Migrations
{
    [DbContext(typeof(HalawaniContext))]
    [Migration("20210326142811_TeamPermissionsUpdate")]
    partial class TeamPermissionsUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Arabic_100_CI_AI")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Halwani.Data.Entities.AssoicationTikcet.Assoication", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DependandForm")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("DependandRequestId")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("FinalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Sumbitter")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Assoications");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.RequestType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<int>("TicketType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("RequestType");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.RequestTypeGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("RequestTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RequestTypeId");

                    b.ToTable("RequestTypeGroups");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.Ticket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attachement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("ProductCategoryName1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCategoryName2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportedSource")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("RequestTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ResolvedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("Source")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubmitterEmail")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("SubmitterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmitterTeam")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("TicketName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("TicketNo")
                        .HasColumnType("int");

                    b.Property<int?>("TicketSeverity")
                        .HasColumnType("int");

                    b.Property<int?>("TicketStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestTypeId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.TicketHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FromTeam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NewStatus")
                        .HasColumnType("int");

                    b.Property<int?>("OldStatus")
                        .HasColumnType("int");

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.Property<string>("ToTeam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketHistories");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Message.TicketMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MessageText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Submitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketMessages");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ProductCategories.ProductCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ResolutionCategories.ResolutionCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentCategoryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("ResolutionCategories");
                });

            modelBuilder.Entity("Halwani.Data.Entities.SLA.SLA", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("ProductCategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SLADuration")
                        .HasColumnType("float");

                    b.Property<string>("SLAName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SLAType")
                        .HasColumnType("int");

                    b.Property<string>("Team")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WorkingDays")
                        .HasColumnType("float");

                    b.Property<double>("WorkingHours")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("SLAs");
                });

            modelBuilder.Entity("Halwani.Data.Entities.SLM_Measurement.SLmMeasurement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("SLAId")
                        .HasColumnType("bigint");

                    b.Property<string>("SLAStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SLAId");

                    b.HasIndex("TicketId");

                    b.ToTable("SLmMeasurements");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Team.Team", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModfiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceLine")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Team.TeamPermissions", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AllowedTeams")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAllTeams")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<long?>("RoleId1")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId1");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamPermissions");
                });

            modelBuilder.Entity("Halwani.Data.Entities.User.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Permissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Halwani.Data.Entities.User.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TeamsId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("TeamsId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Halwani.Data.Entities.AssoicationTikcet.Assoication", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Incident.Ticket", "Ticket")
                        .WithMany("Assoication")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.RequestType", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Team.Team", "DefaultTeam")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DefaultTeam");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.RequestTypeGroups", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Incident.Group", "Group")
                        .WithMany("RequestTypeGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Halwani.Data.Entities.Incident.RequestType", "RequestType")
                        .WithMany("RequestTypeGroups")
                        .HasForeignKey("RequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("RequestType");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.Ticket", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Incident.RequestType", "RequestType")
                        .WithMany()
                        .HasForeignKey("RequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequestType");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.TicketHistory", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Incident.Ticket", "Ticket")
                        .WithMany("TicketHistories")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Message.TicketMessage", b =>
                {
                    b.HasOne("Halwani.Data.Entities.Incident.Ticket", "Ticket")
                        .WithMany("TicketMessage")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ProductCategories.ProductCategory", b =>
                {
                    b.HasOne("Halwani.Data.Entities.ProductCategories.ProductCategory", "Parent")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ParentCategoryId")
                        .HasConstraintName("FK_ProductCategories_ProductCategories_ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ResolutionCategories.ResolutionCategory", b =>
                {
                    b.HasOne("Halwani.Data.Entities.ResolutionCategories.ResolutionCategory", "Parent")
                        .WithMany("ResolutionCategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Halwani.Data.Entities.SLM_Measurement.SLmMeasurement", b =>
                {
                    b.HasOne("Halwani.Data.Entities.SLA.SLA", null)
                        .WithMany("SLmMeasurements")
                        .HasForeignKey("SLAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Halwani.Data.Entities.Incident.Ticket", null)
                        .WithMany("SLmMeasurements")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Halwani.Data.Entities.Team.TeamPermissions", b =>
                {
                    b.HasOne("Halwani.Data.Entities.User.Role", "Role")
                        .WithMany("TeamPermissions")
                        .HasForeignKey("RoleId1");

                    b.HasOne("Halwani.Data.Entities.Team.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Halwani.Data.Entities.User.User", b =>
                {
                    b.HasOne("Halwani.Data.Entities.User.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Halwani.Data.Entities.Team.Team", "Teams")
                        .WithMany("Users")
                        .HasForeignKey("TeamsId");

                    b.Navigation("Role");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.Group", b =>
                {
                    b.Navigation("RequestTypeGroups");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.RequestType", b =>
                {
                    b.Navigation("RequestTypeGroups");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Incident.Ticket", b =>
                {
                    b.Navigation("Assoication");

                    b.Navigation("SLmMeasurements");

                    b.Navigation("TicketHistories");

                    b.Navigation("TicketMessage");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ProductCategories.ProductCategory", b =>
                {
                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("Halwani.Data.Entities.ResolutionCategories.ResolutionCategory", b =>
                {
                    b.Navigation("ResolutionCategories");
                });

            modelBuilder.Entity("Halwani.Data.Entities.SLA.SLA", b =>
                {
                    b.Navigation("SLmMeasurements");
                });

            modelBuilder.Entity("Halwani.Data.Entities.Team.Team", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Halwani.Data.Entities.User.Role", b =>
                {
                    b.Navigation("TeamPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
