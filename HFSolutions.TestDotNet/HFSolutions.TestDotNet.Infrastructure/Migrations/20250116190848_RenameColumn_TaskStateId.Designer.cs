﻿// <auto-generated />
using System;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HFSolutions.TestDotNet.Infrastructure.Migrations
{
    [DbContext(typeof(UserTasksContext))]
    [Migration("20250116190848_RenameColumn_TaskStateId")]
    partial class RenameColumn_TaskStateId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HFSolutions.TestDotNet.Domain.Entities.TaskState", b =>
                {
                    b.Property<int>("TaskStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskStateId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskStateId");

                    b.ToTable("TaskState");
                });

            modelBuilder.Entity("HFSolutions.TestDotNet.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HFSolutions.TestDotNet.Domain.Entities.UserTask", b =>
                {
                    b.Property<int>("UserTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserTaskId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskStateId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserTaskId");

                    b.HasIndex("TaskStateId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTask");
                });

            modelBuilder.Entity("HFSolutions.TestDotNet.Domain.Entities.UserTask", b =>
                {
                    b.HasOne("HFSolutions.TestDotNet.Domain.Entities.TaskState", "TaskState")
                        .WithMany()
                        .HasForeignKey("TaskStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HFSolutions.TestDotNet.Domain.Entities.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HFSolutions.TestDotNet.Domain.Entities.User", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}