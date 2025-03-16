﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjektGrupowy.API.Data;

#nullable disable

namespace ProjektGrupowy.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LabelerProject", b =>
                {
                    b.Property<int>("ProjectLabelersId")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectLabelersId1")
                        .HasColumnType("integer");

                    b.HasKey("ProjectLabelersId", "ProjectLabelersId1");

                    b.HasIndex("ProjectLabelersId1");

                    b.ToTable("LabelerProject");
                });

            modelBuilder.Entity("LabelerSubjectVideoGroupAssignment", b =>
                {
                    b.Property<int>("LabelersId")
                        .HasColumnType("integer");

                    b.Property<int>("SubjectVideoGroupsId")
                        .HasColumnType("integer");

                    b.HasKey("LabelersId", "SubjectVideoGroupsId");

                    b.HasIndex("SubjectVideoGroupsId");

                    b.ToTable("LabelerSubjectVideoGroupAssignment");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.AssignedLabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("End")
                        .HasColumnType("interval");

                    b.Property<int>("LabelId")
                        .HasColumnType("integer");

                    b.Property<int>("LabelerId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("interval");

                    b.Property<int>("SubjectVideoGroupAssignmentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.HasIndex("LabelerId");

                    b.HasIndex("SubjectVideoGroupAssignmentId");

                    b.ToTable("AssignedLabels");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ColorHex")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<char?>("Shortcut")
                        .HasColumnType("character(1)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Labeler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Labelers");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("ModificationDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ScientistId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ScientistId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.ProjectAccessCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpiresAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("IX_ProjectAccessCode_Code");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectAccessCodes");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Scientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Scientists");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.SubjectVideoGroupAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("ModificationDate")
                        .HasColumnType("date");

                    b.Property<int>("SubjectId")
                        .HasColumnType("integer");

                    b.Property<int>("VideoGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("VideoGroupId");

                    b.ToTable("SubjectVideoGroupAssignments");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("PositionInQueue")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("VideoGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VideoGroupId", "PositionInQueue");

                    b.ToTable("Vidoes");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.VideoGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("VideoGroups");
                });

            modelBuilder.Entity("LabelerProject", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Labeler", null)
                        .WithMany()
                        .HasForeignKey("ProjectLabelersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectLabelersId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LabelerSubjectVideoGroupAssignment", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Labeler", null)
                        .WithMany()
                        .HasForeignKey("LabelersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.SubjectVideoGroupAssignment", null)
                        .WithMany()
                        .HasForeignKey("SubjectVideoGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.AssignedLabel", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Label", "Label")
                        .WithMany("AssignedLabels")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.Labeler", "Labeler")
                        .WithMany("AssignedLabels")
                        .HasForeignKey("LabelerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.SubjectVideoGroupAssignment", "SubjectVideoGroupAssignment")
                        .WithMany("AssignedLabels")
                        .HasForeignKey("SubjectVideoGroupAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");

                    b.Navigation("Labeler");

                    b.Navigation("SubjectVideoGroupAssignment");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Label", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Subject", "Subject")
                        .WithMany("Labels")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Labeler", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.User", "User")
                        .WithOne("Labeler")
                        .HasForeignKey("ProjektGrupowy.API.Models.Labeler", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Project", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Scientist", "Scientist")
                        .WithMany("Projects")
                        .HasForeignKey("ScientistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.ProjectAccessCode", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Project", "Project")
                        .WithMany("AccessCodes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Scientist", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.User", "User")
                        .WithOne("Scientist")
                        .HasForeignKey("ProjektGrupowy.API.Models.Scientist", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Subject", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Project", "Project")
                        .WithMany("Subjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.SubjectVideoGroupAssignment", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Subject", "Subject")
                        .WithMany("SubjectVideoGroupAssignments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektGrupowy.API.Models.VideoGroup", "VideoGroup")
                        .WithMany("SubjectVideoGroupAssignments")
                        .HasForeignKey("VideoGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("VideoGroup");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Video", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.VideoGroup", "VideoGroup")
                        .WithMany("Videos")
                        .HasForeignKey("VideoGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VideoGroup");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.VideoGroup", b =>
                {
                    b.HasOne("ProjektGrupowy.API.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Label", b =>
                {
                    b.Navigation("AssignedLabels");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Labeler", b =>
                {
                    b.Navigation("AssignedLabels");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Project", b =>
                {
                    b.Navigation("AccessCodes");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Scientist", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.Subject", b =>
                {
                    b.Navigation("Labels");

                    b.Navigation("SubjectVideoGroupAssignments");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.SubjectVideoGroupAssignment", b =>
                {
                    b.Navigation("AssignedLabels");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.User", b =>
                {
                    b.Navigation("Labeler");

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("ProjektGrupowy.API.Models.VideoGroup", b =>
                {
                    b.Navigation("SubjectVideoGroupAssignments");

                    b.Navigation("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
