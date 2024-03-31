﻿// <auto-generated />
using System;
using ApplicationIT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationIT.Migrations
{
    [DbContext(typeof(RequestContext))]
    partial class RequestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationIT.Models.Request", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("activity")
                        .HasColumnType("text");

                    b.Property<Guid?>("author")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("outline")
                        .HasColumnType("text");

                    b.Property<bool>("submit")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("submitDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}