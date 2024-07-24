﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SITracker.Data;

#nullable disable

namespace SITracker.Migrations
{
    [DbContext(typeof(TrackerDbContext))]
    [Migration("20240724183104_AddLastPasswordChangeToUsers")]
    partial class AddLastPasswordChangeToUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SITracker.Models.Adversary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("adversary_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("flag");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Pathname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("pathname");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Pathname")
                        .IsUnique();

                    b.ToTable("adversaries");
                });

            modelBuilder.Entity("SITracker.Models.GameSession", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("game_session_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AdversaryId")
                        .HasColumnType("bigint")
                        .HasColumnName("adversary_id");

                    b.Property<string>("Board")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("board");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_completed");

                    b.Property<DateTime?>("PlayedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("played_on");

                    b.Property<string>("Result")
                        .HasColumnType("text")
                        .HasColumnName("result");

                    b.Property<string>("SessionName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("session_name");

                    b.Property<long>("SpiritId")
                        .HasColumnType("bigint")
                        .HasColumnName("spirit_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("AdversaryId");

                    b.HasIndex("SpiritId");

                    b.HasIndex("UserId");

                    b.ToTable("game_sessions");
                });

            modelBuilder.Entity("SITracker.Models.Spirit", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("spirit_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Pathname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("pathname");

                    b.HasKey("id");

                    b.HasIndex("Name", "Pathname")
                        .IsUnique();

                    b.ToTable("spirits");
                });

            modelBuilder.Entity("SITracker.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime?>("LastPasswordChange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registration_date");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("Email", "Username")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("SITracker.Models.GameSession", b =>
                {
                    b.HasOne("SITracker.Models.Adversary", "Adversary")
                        .WithMany()
                        .HasForeignKey("AdversaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SITracker.Models.Spirit", "Spirit")
                        .WithMany()
                        .HasForeignKey("SpiritId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SITracker.Models.User", "User")
                        .WithMany("GameSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adversary");

                    b.Navigation("Spirit");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SITracker.Models.User", b =>
                {
                    b.Navigation("GameSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
