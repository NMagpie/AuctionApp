﻿// <auto-generated />
using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AuctionAppDbContext))]
    partial class AuctionAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuctionApp.Domain.Models.Auction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("StatusId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.AuctionReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("ReviewText")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("UserId");

                    b.ToTable("AuctionReviews");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsWon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("LotId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LotId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Lot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuctionId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<decimal?>("InitialPrice")
                        .HasColumnType("money");

                    b.Property<int?>("LotOrder")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.ToTable("Lots");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("money")
                        .HasDefaultValue(0m);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.UserWatchlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuctionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("UserId", "AuctionId")
                        .IsUnique()
                        .HasFilter("[AuctionId] IS NOT NULL");

                    b.ToTable("UserWatchlists");
                });

            modelBuilder.Entity("CategoryLot", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("LotsId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesId", "LotsId");

                    b.HasIndex("LotsId");

                    b.ToTable("CategoryLot");
                });

            modelBuilder.Entity("EntityFramework.Domain.Models.AuctionStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuctionStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Status = "Created"
                        },
                        new
                        {
                            Id = 2,
                            Status = "Active"
                        },
                        new
                        {
                            Id = 3,
                            Status = "Finished"
                        });
                });

            modelBuilder.Entity("EntityFramework.Domain.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Auction", b =>
                {
                    b.HasOne("AuctionApp.Domain.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("EntityFramework.Domain.Models.AuctionStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.AuctionReview", b =>
                {
                    b.HasOne("AuctionApp.Domain.Models.Auction", "Auction")
                        .WithMany()
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuctionApp.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Auction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Bid", b =>
                {
                    b.HasOne("AuctionApp.Domain.Models.Lot", "Lot")
                        .WithMany("Bids")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lot");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Lot", b =>
                {
                    b.HasOne("AuctionApp.Domain.Models.Auction", "Auction")
                        .WithMany("Lots")
                        .HasForeignKey("AuctionId");

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.UserWatchlist", b =>
                {
                    b.HasOne("AuctionApp.Domain.Models.Auction", "Auction")
                        .WithMany()
                        .HasForeignKey("AuctionId");

                    b.HasOne("AuctionApp.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CategoryLot", b =>
                {
                    b.HasOne("EntityFramework.Domain.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuctionApp.Domain.Models.Lot", null)
                        .WithMany()
                        .HasForeignKey("LotsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Auction", b =>
                {
                    b.Navigation("Lots");
                });

            modelBuilder.Entity("AuctionApp.Domain.Models.Lot", b =>
                {
                    b.Navigation("Bids");
                });
#pragma warning restore 612, 618
        }
    }
}
