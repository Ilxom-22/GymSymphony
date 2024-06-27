﻿// <auto-generated />
using System;
using Gymphony.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Gymphony.Domain.Entities.AccessToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.MembershipPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly?>("ActivationDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<byte>("DurationCount")
                        .HasColumnType("smallint");

                    b.Property<int>("DurationUnit")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MembershipPlans");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.NotificationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NotificationMethod")
                        .HasColumnType("integer");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("SentTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("NotificationHistories");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.NotificationTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("NotificationTemplates");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthDataHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AuthenticationProvider")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator<int>("Role");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.VerificationToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("VerificationTokens");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.Admin", b =>
                {
                    b.HasBaseType("Gymphony.Domain.Entities.User");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("TemporaryPasswordChanged")
                        .HasColumnType("boolean");

                    b.HasIndex("CreatedByUserId");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.Member", b =>
                {
                    b.HasBaseType("Gymphony.Domain.Entities.User");

                    b.Property<DateTimeOffset?>("BirthDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("StripeCustomerId")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.AccessToken", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.User", "User")
                        .WithOne("AccessToken")
                        .HasForeignKey("Gymphony.Domain.Entities.AccessToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.MembershipPlan", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.Admin", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Gymphony.Domain.Entities.Admin", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Gymphony.Domain.Entities.StripeDetails", "StripeDetails", b1 =>
                        {
                            b1.Property<Guid>("MembershipPlanId")
                                .HasColumnType("uuid");

                            b1.Property<string>("PriceId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("ProductId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("MembershipPlanId");

                            b1.ToTable("MembershipPlans");

                            b1.WithOwner()
                                .HasForeignKey("MembershipPlanId");
                        });

                    b.Navigation("CreatedBy");

                    b.Navigation("ModifiedBy");

                    b.Navigation("StripeDetails");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.NotificationHistory", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.NotificationTemplate", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("Gymphony.Domain.Entities.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.VerificationToken", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.User", "User")
                        .WithOne("VerificationToken")
                        .HasForeignKey("Gymphony.Domain.Entities.VerificationToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.Admin", b =>
                {
                    b.HasOne("Gymphony.Domain.Entities.Admin", null)
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Gymphony.Domain.Entities.User", b =>
                {
                    b.Navigation("AccessToken");

                    b.Navigation("RefreshToken");

                    b.Navigation("VerificationToken");
                });
#pragma warning restore 612, 618
        }
    }
}
