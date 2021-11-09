﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OstaniStudent.Database.Models;

#nullable disable

namespace OstaniStudent.Database
{
    public partial class ServiceDb : DbContext
    {
        public ServiceDb()
        {
        }

        public ServiceDb(DbContextOptions<ServiceDb> options)
            : base(options)
        {
        }

        public virtual DbSet<Korisnici> Korisnicis { get; set; }
        public virtual DbSet<KorisniciPredmeti> KorisniciPredmetis { get; set; }
        public virtual DbSet<KorisniciUloge> KorisniciUloges { get; set; }
        public virtual DbSet<KorisnikZeljeniModul> KorisnikZeljeniModuls { get; set; }
        public virtual DbSet<Moduli> Modulis { get; set; }
        public virtual DbSet<Predmeti> Predmetis { get; set; }
        public virtual DbSet<Sifrarnik> Sifrarniks { get; set; }
        public virtual DbSet<Uloge> Uloges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Korisnici>(entity =>
            {
                entity.ToTable("Korisnici");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Jmbag)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("JMBAG");

                entity.Property(e => e.Prezime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KorisniciPredmeti>(entity =>
            {
                entity.ToTable("KorisniciPredmeti");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.KorisniciPredmetis)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisniciPredmeti_Korisnici");

                entity.HasOne(d => d.IdPredmetNavigation)
                    .WithMany(p => p.KorisniciPredmetis)
                    .HasForeignKey(d => d.IdPredmet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisniciPredmeti_Predmeti");
            });

            modelBuilder.Entity<KorisniciUloge>(entity =>
            {
                entity.ToTable("KorisniciUloge");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.KorisniciUloges)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisniciUloge_Korisnici");

                entity.HasOne(d => d.IdUlogeNavigation)
                    .WithMany(p => p.KorisniciUloges)
                    .HasForeignKey(d => d.IdUloge)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisniciUloge_Uloge");
            });

            modelBuilder.Entity<KorisnikZeljeniModul>(entity =>
            {
                entity.ToTable("KorisnikZeljeniModul");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.KorisnikZeljeniModuls)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisnikZeljeniModul_Korisnici");

                entity.HasOne(d => d.IdModulNavigation)
                    .WithMany(p => p.KorisnikZeljeniModuls)
                    .HasForeignKey(d => d.IdModul)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisnikZeljeniModul_Moduli");
            });

            modelBuilder.Entity<Moduli>(entity =>
            {
                entity.ToTable("Moduli");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Predmeti>(entity =>
            {
                entity.ToTable("Predmeti");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdModulNavigation)
                    .WithMany(p => p.Predmetis)
                    .HasForeignKey(d => d.IdModul)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Predmeti_Moduli");

                entity.HasOne(d => d.IdSifrarnikNavigation)
                    .WithMany(p => p.Predmetis)
                    .HasForeignKey(d => d.IdSifrarnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Predmeti_Sifrarnik");
            });

            modelBuilder.Entity<Sifrarnik>(entity =>
            {
                entity.ToTable("Sifrarnik");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Uloge>(entity =>
            {
                entity.ToTable("Uloge");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}