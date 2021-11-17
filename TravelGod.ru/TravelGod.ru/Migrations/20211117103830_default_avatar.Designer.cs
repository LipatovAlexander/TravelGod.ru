﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelGod.ru.Models;

namespace TravelGod.ru.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20211117103830_default_avatar")]
    partial class default_avatar
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("ChatUser", b =>
                {
                    b.Property<int>("JoinedChatsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("JoinedChatsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("userchat");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("InitiatorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InitiatorId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<int?>("TripId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("TravelGod.ru.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Files");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "default-avatar.png",
                            Path = "CustomFiles/Avatars/default-avatar.png"
                        });
                });

            modelBuilder.Entity("TravelGod.ru.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ChatId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<int?>("TripId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("RememberMe")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ChatId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("InitiatorId")
                        .HasColumnType("int");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("UsersCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("InitiatorId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TravelGod.ru.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("LastName")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.Property<int>("JoinedTripsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("JoinedTripsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("usertrip");
                });

            modelBuilder.Entity("ChatUser", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Chat", null)
                        .WithMany()
                        .HasForeignKey("JoinedChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelGod.ru.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TravelGod.ru.Models.Chat", b =>
                {
                    b.HasOne("TravelGod.ru.Models.User", "Initiator")
                        .WithMany("OwnedChats")
                        .HasForeignKey("InitiatorId");

                    b.Navigation("Initiator");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Comment", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Trip", "Trip")
                        .WithMany("Comments")
                        .HasForeignKey("TripId");

                    b.HasOne("TravelGod.ru.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Message", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId");

                    b.HasOne("TravelGod.ru.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Rating", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Trip", "Trip")
                        .WithMany("Ratings")
                        .HasForeignKey("TripId");

                    b.HasOne("TravelGod.ru.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Session", b =>
                {
                    b.HasOne("TravelGod.ru.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Trip", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId");

                    b.HasOne("TravelGod.ru.Models.User", "Initiator")
                        .WithMany("OwnedTrips")
                        .HasForeignKey("InitiatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Initiator");
                });

            modelBuilder.Entity("TravelGod.ru.Models.User", b =>
                {
                    b.HasOne("TravelGod.ru.Models.File", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.HasOne("TravelGod.ru.Models.Trip", null)
                        .WithMany()
                        .HasForeignKey("JoinedTripsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelGod.ru.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TravelGod.ru.Models.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("TravelGod.ru.Models.Trip", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("TravelGod.ru.Models.User", b =>
                {
                    b.Navigation("OwnedChats");

                    b.Navigation("OwnedTrips");
                });
#pragma warning restore 612, 618
        }
    }
}
