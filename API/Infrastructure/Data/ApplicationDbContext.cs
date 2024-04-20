using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Kupac> Kupacs { get; set; }
        public DbSet<Dres> Dress { get; set; }

        public DbSet<VelicinaDresa> VelicinaDresas { get; set; }
        public DbSet<Porudzbina> Porudzbinas { get; set; }
        public DbSet<StavkaPorudzbine> StavkaPorudzbines { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasIndex(e => e.AdminEmail)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(e => e.AdminKorisnickoIme)
                .IsUnique();

            modelBuilder.Entity<Kupac>()
                .HasIndex(e => e.KupacEmail)
                .IsUnique();

            modelBuilder.Entity<Kupac>()
                .HasIndex(e => e.KupacKorisnickoIme)
                .IsUnique();

            modelBuilder.Entity<Dres>()
                .ToTable(table =>
                {
                    table.HasCheckConstraint("CHK_Status", "Status IN ('na stanju', 'nedostupno')");
                });

            modelBuilder.Entity<Dres>()
                .HasOne(a => a.Admin)
                .WithMany(j => j.Dress)
                .HasForeignKey(a => a.AdminId);

            modelBuilder.Entity<VelicinaDresa>()
                .HasOne(j => j.Dres)
                .WithMany(s => s.VelicinaDresas)
                .HasForeignKey(j => j.DresId);

            modelBuilder.Entity<Porudzbina>()
                .HasOne(c => c.Kupac)
                .WithMany(o => o.Porudzbinas)
                .HasForeignKey(c => c.KupacId);

            modelBuilder.Entity<StavkaPorudzbine>()
                .HasOne(o => o.Porudzbina)
                .WithMany(i => i.StavkaPorudzbines)
                .HasForeignKey(o => o.PorudzbinaId);


            modelBuilder.Entity<VelicinaDresa>()
                .HasOne(oi => oi.StavkaPorudzbines)
                .WithOne(s => s.VelicinaDresa)
                .HasForeignKey<StavkaPorudzbine>(oi => oi.VelicinaDresaId);

            modelBuilder.Entity<StavkaPorudzbine>(entry =>
            {
                entry.ToTable("StavkaPorudzbines", tb => tb.HasTrigger("Triger1"));
            });


        }
    }
}
