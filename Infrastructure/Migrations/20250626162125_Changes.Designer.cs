﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AbsanteeContext))]
    [Migration("20250626162125_Changes")]
    partial class Changes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.DataModel.CollaboratorDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("Infrastructure.DataModel.UserDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("UserIds");
                });

            modelBuilder.Entity("Infrastructure.DataModel.CollaboratorDataModel", b =>
                {
                    b.OwnsOne("Domain.Models.PeriodDateTime", "PeriodDateTime", b1 =>
                        {
                            b1.Property<Guid>("CollaboratorDataModelId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("_finalDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("_initDate")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("CollaboratorDataModelId");

                            b1.ToTable("Collaborators");

                            b1.WithOwner()
                                .HasForeignKey("CollaboratorDataModelId");
                        });

                    b.Navigation("PeriodDateTime")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
