﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SpendingTracker.Infrastructure;

#nullable disable

namespace SpendingTracker.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategoriesLink", b =>
                {
                    b.Property<Guid>("ChildId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid");

                    b.HasKey("ChildId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("CategoriesLink", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredSpendingCategoryLink", b =>
                {
                    b.Property<Guid>("SpendingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("SpendingId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("SpendingCategoryLink", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredCurrency", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("CountryIcon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Currency", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredCurrencyRateByDay", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Base")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Coefficient")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Target")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CurrencyRateByDay", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredSpending", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("ActionSource")
                        .HasColumnType("integer");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Spending", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUser", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TelegramUser", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUserCurrentButtonGroup", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("UserId", "GroupId")
                        .IsUnique();

                    b.ToTable("TelegramUserCurrentButtonGroup", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings.StoredUserSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DefaultValueAsString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSetting", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings.StoredUserSettingValue", b =>
                {
                    b.Property<Guid>("SettingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ValueAsString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SettingId", "UserId");

                    b.ToTable("UserSettingValue", (string)null);
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategoriesLink", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategory", "Child")
                        .WithMany()
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredSpendingCategoryLink", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories.StoredCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredSpending", "Spending")
                        .WithMany()
                        .HasForeignKey("SpendingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Spending");
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredSpending", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredCurrency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUser", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUserCurrentButtonGroup", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUser", null)
                        .WithOne()
                        .HasForeignKey("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredTelegramUserCurrentButtonGroup", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredUser", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.StoredCurrency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings.StoredUserSettingValue", b =>
                {
                    b.HasOne("SpendingTracker.Infrastructure.Abstractions.Models.Stored.UserSettings.StoredUserSetting", "Setting")
                        .WithMany()
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Setting");
                });
#pragma warning restore 612, 618
        }
    }
}
