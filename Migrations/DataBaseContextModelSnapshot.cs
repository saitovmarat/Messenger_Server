﻿// <auto-generated />
using System;
using MessangerBack.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MessangerBack.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MessangerBack.Models.ChatModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<string>("ChatName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LastMessageId")
                        .HasColumnType("uuid");

                    b.Property<Guid[]>("Users")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("LastMessageId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("MessangerBack.Models.MessageModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("MessageSentTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MessangerBack.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChatModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatModelId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MessangerBack.Models.ChatModel", b =>
                {
                    b.HasOne("MessangerBack.Models.UserModel", "Admin")
                        .WithMany("ChatWhereIsTheAdmin")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MessangerBack.Models.MessageModel", "LastMessage")
                        .WithMany()
                        .HasForeignKey("LastMessageId");

                    b.Navigation("Admin");

                    b.Navigation("LastMessage");
                });

            modelBuilder.Entity("MessangerBack.Models.MessageModel", b =>
                {
                    b.HasOne("MessangerBack.Models.ChatModel", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MessangerBack.Models.UserModel", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MessangerBack.Models.UserModel", b =>
                {
                    b.HasOne("MessangerBack.Models.ChatModel", null)
                        .WithMany("ModelsOfUsers")
                        .HasForeignKey("ChatModelId");
                });

            modelBuilder.Entity("MessangerBack.Models.ChatModel", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("ModelsOfUsers");
                });

            modelBuilder.Entity("MessangerBack.Models.UserModel", b =>
                {
                    b.Navigation("ChatWhereIsTheAdmin");
                });
#pragma warning restore 612, 618
        }
    }
}
