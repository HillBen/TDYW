using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TDYW.Data;

namespace TDYW.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170614175411_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TDYW.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TDYW.Models.Complaint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DateCreated");

                    b.Property<string>("Description")
                        .HasMaxLength(50);

                    b.Property<int>("PickId");

                    b.Property<int>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("PickId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Complaints");
                });

            modelBuilder.Entity("TDYW.Models.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .HasMaxLength(255);

                    b.Property<bool>("OpenInvite");

                    b.Property<int>("PoolId");

                    b.Property<string>("Subject")
                        .HasMaxLength(78);

                    b.HasKey("Id");

                    b.HasIndex("PoolId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("TDYW.Models.Invitee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateJoined");

                    b.Property<DateTime?>("DateSent");

                    b.Property<string>("Email");

                    b.Property<int>("InvitationId");

                    b.HasKey("Id");

                    b.HasIndex("InvitationId");

                    b.ToTable("Invitees");
                });

            modelBuilder.Entity("TDYW.Models.Judgement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Agreed");

                    b.Property<int>("ComplaintId");

                    b.Property<string>("DateCreated");

                    b.Property<int>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("ComplaintId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Rulings");
                });

            modelBuilder.Entity("TDYW.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<DateTime?>("DeathDate");

                    b.Property<string>("Description")
                        .HasMaxLength(255);

                    b.Property<string>("ImageUrl");

                    b.Property<DateTime>("LastQuery")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("PageId");

                    b.Property<DateTime>("RevisionDate");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("TDYW.Models.Pick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("PersonId");

                    b.Property<int>("PlayerId");

                    b.Property<bool>("PointsAwarded");

                    b.Property<int>("PotentialValue");

                    b.Property<int>("Rank");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Picks");
                });

            modelBuilder.Entity("TDYW.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool>("HidePicksBeforeStart");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("PoolId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PoolId");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TDYW.Models.Pool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowPluralityVote");

                    b.Property<string>("Description")
                        .HasMaxLength(255);

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("FixedAgeBonus");

                    b.Property<int>("FixedAgeBonusMinuend");

                    b.Property<bool>("FixedRankBonus");

                    b.Property<int>("FixedRankBonusFactor");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("OpenEnrollment");

                    b.Property<int>("OversPerPlayer");

                    b.Property<int>("PicksPerPlayer");

                    b.Property<bool>("Private");

                    b.Property<bool>("RequireTwoThirdsVote");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("UserId");

                    b.Property<bool>("WeightedAgeBonus");

                    b.Property<int>("WeightedAgeBonusFactor");

                    b.Property<bool>("WeightedRankBonus");

                    b.Property<int>("WeightedRankBonusFactor");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Pools");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TDYW.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TDYW.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TDYW.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Complaint", b =>
                {
                    b.HasOne("TDYW.Models.Pick", "Pick")
                        .WithMany("Complaints")
                        .HasForeignKey("PickId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TDYW.Models.Player", "Player")
                        .WithMany("Complaints")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Invitation", b =>
                {
                    b.HasOne("TDYW.Models.Pool", "Pool")
                        .WithMany("Invitations")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Invitee", b =>
                {
                    b.HasOne("TDYW.Models.Invitation", "Invitation")
                        .WithMany("Invitees")
                        .HasForeignKey("InvitationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Judgement", b =>
                {
                    b.HasOne("TDYW.Models.Complaint", "Complaint")
                        .WithMany("Judgements")
                        .HasForeignKey("ComplaintId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TDYW.Models.Player", "Player")
                        .WithMany("Judgements")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Pick", b =>
                {
                    b.HasOne("TDYW.Models.Person", "Person")
                        .WithMany("Picks")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TDYW.Models.Player", "Player")
                        .WithMany("Picks")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TDYW.Models.Player", b =>
                {
                    b.HasOne("TDYW.Models.Pool", "Pool")
                        .WithMany("Players")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TDYW.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Players")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TDYW.Models.Pool", b =>
                {
                    b.HasOne("TDYW.Models.ApplicationUser", "Owner")
                        .WithMany("Pools")
                        .HasForeignKey("UserId");
                });
        }
    }
}
